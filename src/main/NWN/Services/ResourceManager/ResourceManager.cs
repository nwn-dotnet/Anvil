using System;
using System.IO;
using System.Linq;
using Anvil.Internal;
using NLog;
using NWN.API;
using NWN.Native.API;
using NWN.Plugins;

namespace NWN.Services
{
  [ServiceBinding(typeof(ResourceManager))]
  public sealed class ResourceManager : IDisposable
  {
    public const int MaxNameLength = 16;

    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private const string AliasBaseName = "ANVILRES";
    private const string AliasSuffix = ":";
    private const uint BasePriority = 70500000;

    private static readonly CExoBase ExoBase = NWNXLib.ExoBase();
    private static readonly CExoResMan ResMan = NWNXLib.ExoResMan();

    private readonly CExoString tempAlias;

    private uint currentIndex;

    public ResourceManager(ITypeLoader typeLoader)
    {
      if (Directory.Exists(EnvironmentConfig.ResourcePath))
      {
        Directory.Delete(EnvironmentConfig.ResourcePath, true);
      }

      tempAlias = CreateResourceDirectory(EnvironmentConfig.ResourcePath).ToExoString();

      foreach (string resourcePath in typeLoader.ResourcePaths)
      {
        CreateResourceDirectory(resourcePath);
      }
    }

    public void WriteTempResource(string resourceName, byte[] data)
    {
      string nameWithoutExtension = Path.GetFileNameWithoutExtension(resourceName);

      if (nameWithoutExtension.Length > MaxNameLength)
      {
        throw new ArgumentOutOfRangeException(nameof(resourceName), $"Resource name (excl. extension) must be less than {MaxNameLength} characters.");
      }

      if (nameWithoutExtension.Any(c => !char.IsLetterOrDigit(c) && c != '_'))
      {
        throw new ArgumentOutOfRangeException(nameof(resourceName), $"Resource name must only contain alphanumeric characters, or underscores.");
      }

      File.WriteAllBytes(Path.Combine(EnvironmentConfig.ResourcePath, resourceName), data);
      ResMan.UpdateResourceDirectory(tempAlias);
    }

    private string CreateResourceDirectory(string path)
    {
      if (string.IsNullOrEmpty(path))
      {
        throw new ArgumentOutOfRangeException(nameof(path), "Path must not be empty or null.");
      }

      string alias = AliasBaseName + currentIndex + AliasSuffix;
      CExoString exoAlias = alias.ToExoString();

      ExoBase.m_pcExoAliasList.Add(exoAlias, path.ToExoString());
      ResMan.CreateDirectory(exoAlias);
      ResMan.AddResourceDirectory(exoAlias, BasePriority + currentIndex, true.ToInt());

      currentIndex++;

      return alias;
    }

    void IDisposable.Dispose()
    {
      Directory.Delete(EnvironmentConfig.ResourcePath, true);
    }
  }
}
