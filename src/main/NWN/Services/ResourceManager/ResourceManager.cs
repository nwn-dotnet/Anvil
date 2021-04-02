using System;
using System.IO;
using NWN.API;
using NWN.Native.API;
using NWN.Plugins;

namespace NWN.Services.ResourceManager
{
  [ServiceBinding(typeof(ResourceManager))]
  public sealed class ResourceManager : IDisposable
  {
    private const string AliasBaseName = "NMAN";
    private const uint BasePriority = 70000000;

    private static readonly CExoBase exoBase = NWNXLib.ExoBase();
    private static readonly CExoResMan resMan = NWNXLib.ExoResMan();

    private uint currentIndex;

    public ResourceManager(ITypeLoader typeLoader)
    {
      Directory.Delete(EnvironmentConfig.ResourcePath, true);
      CreateResourceDirectory(EnvironmentConfig.ResourcePath);

      foreach (string resourcePath in typeLoader.ResourcePaths)
      {
        CreateResourceDirectory(resourcePath);
      }
    }

    private void CreateResourceDirectory(string path)
    {
      if (string.IsNullOrEmpty(path))
      {
        throw new ArgumentOutOfRangeException(nameof(path), "Path must not be empty or null.");
      }

      string aliasName = AliasBaseName + currentIndex;

      exoBase.m_pcExoAliasList.Add(aliasName.ToExoString(), path.ToExoString());
      resMan.CreateDirectory(aliasName.ToExoString());
      resMan.AddResourceDirectory(aliasName.ToExoString(), BasePriority + currentIndex, true.ToInt());

      currentIndex++;
    }

    void IDisposable.Dispose()
    {
      Directory.Delete(EnvironmentConfig.ResourcePath, true);
    }
  }
}
