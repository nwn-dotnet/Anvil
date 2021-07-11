using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Anvil.Internal;
using NLog;
using NWN.API;
using NWN.Native.API;
using NWN.Plugins;
using ResRefType = NWN.API.Constants.ResRefType;

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
        throw new ArgumentOutOfRangeException(nameof(resourceName), "Resource name must only contain alphanumeric characters, or underscores.");
      }

      File.WriteAllBytes(Path.Combine(EnvironmentConfig.ResourcePath, resourceName), data);
      ResMan.UpdateResourceDirectory(tempAlias);
    }

    /// <summary>
    /// Gets all resource names for the specified type.
    /// </summary>
    /// <param name="type">A resource type.</param>
    /// <param name="moduleOnly">If true, only bundled module resources will be returned.</param>
    /// <returns>Any matching ResRef names, otherwise an empty enumeration.</returns>
    public IEnumerable<string> FindResourcesOfType(ResRefType type, bool moduleOnly = true)
    {
      CExoStringList resourceList = ResMan.GetResOfType((ushort)type, moduleOnly.ToInt());
      for (int i = 0; i < resourceList.m_nCount; i++)
      {
        yield return resourceList._OpIndex(i).ToString();
      }
    }

    /// <summary>
    /// Determines if the supplied resource exists and is of the specified type.
    /// </summary>
    /// <param name="name">The resource name to check.</param>
    /// <param name="type">The type of this resource.</param>
    /// <returns>true if the supplied resource exists and is of the specified type, otherwise false.</returns>
    public unsafe bool IsValidResource(string name, ResRefType type = ResRefType.UTC)
    {
      return ResMan.Exists(new CResRef(name), (ushort)type, null).ToBool();
    }

    /// <summary>
    /// Gets the raw data of the specified resource.
    /// </summary>
    /// <param name="name">The resource name to retrieve.</param>
    /// <param name="type">The type of resource to retrieve.</param>
    /// <returns>The raw data of the associated resource, otherwise null if the resource does not exist.</returns>
    public byte[] GetResourceData(string name, ResRefType type)
    {
      switch (type)
      {
        case ResRefType.NSS:
          string source = GetNSSContents(name.ToExoString());
          return source != null ? StringHelper.Cp1252Encoding.GetBytes(source) : null;
        case ResRefType.NCS:
          return null;
        default:
          return GetStandardResourceData(name, (ushort)type);
      }
    }

    /// <summary>
    /// Gets the contents of a .nss script file as a string.
    /// </summary>
    /// <param name="scriptName">The name of the script to get the contents of.</param>
    /// <returns>The script file contents or "" on error.</returns>
    public unsafe string GetNSSContents(CExoString scriptName)
    {
      CScriptSourceFile scriptSourceFile = new CScriptSourceFile();
      byte* data;
      uint size = 0;

      if (scriptSourceFile.LoadScript(scriptName, &data, &size) == 0)
      {
        string retVal = StringHelper.ReadFixedLengthString(data, (int)size);
        scriptSourceFile.UnloadScript();
        return retVal;
      }

      return null;
    }

    private unsafe byte[] GetStandardResourceData(string name, ushort type)
    {
      CResRef resRef = new CResRef(name);
      if (!ResMan.Exists(resRef, type).ToBool())
      {
        return null;
      }

      CRes res = ResMan.GetResObject(resRef, type);
      if (res == null)
      {
        return null;
      }

      void* data = res.GetData();
      int size = res.GetSize();

      byte[] retVal = new byte[res.m_nSize];
      Marshal.Copy((IntPtr)data, retVal, 0, size);
      return retVal;
    }

    private string CreateResourceDirectory(string path)
    {
      if (string.IsNullOrEmpty(path))
      {
        throw new ArgumentOutOfRangeException(nameof(path), "Path must not be empty or null.");
      }

      string alias = AliasBaseName + currentIndex + AliasSuffix;
      uint priority = BasePriority + currentIndex;
      CExoString exoAlias = alias.ToExoString();

      Log.Info($"Setting up resource directory: {alias}:{path} (Priority: {priority})");

      ExoBase.m_pcExoAliasList.Add(exoAlias, path.ToExoString());
      ResMan.CreateDirectory(exoAlias);
      ResMan.AddResourceDirectory(exoAlias, priority, false.ToInt());
      ResMan.UpdateResourceDirectory(exoAlias);

      currentIndex++;

      return alias;
    }

    void IDisposable.Dispose()
    {
      if (Directory.Exists(EnvironmentConfig.ResourcePath))
      {
        Directory.Delete(EnvironmentConfig.ResourcePath, true);
      }
    }
  }
}
