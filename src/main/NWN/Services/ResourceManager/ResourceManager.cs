using System;
using System.IO;
using NWN.API;
using NWN.Native.API;

namespace NWN.Services.ResourceManager
{
  [ServiceBinding(typeof(ResourceManager))]
  public sealed class ResourceManager : IDisposable
  {
    private const string AliasName = "NMAN_TEMP";

    private static readonly CExoBase exoBase = NWNXLib.ExoBase();
    private static readonly CExoResMan resMan = NWNXLib.ExoResMan();

    public ResourceManager()
    {
      Directory.Delete(EnvironmentConfig.ResourcePath, true);
      CreateResourceDirectory(AliasName, EnvironmentConfig.ResourcePath, 100000);
    }

    private void CreateResourceDirectory(string aliasName, string path, uint priority)
    {
      if (string.IsNullOrEmpty(aliasName))
      {
        throw new ArgumentOutOfRangeException(nameof(aliasName), "Alias name must not be empty or null.");
      }

      if (!exoBase.m_pcExoAliasList.GetAliasPath(aliasName.ToExoString()).IsEmpty().ToBool())
      {
        throw new Exception($"Alias name {aliasName} already exists. Please use nwn.ini to redefine base game resource directories.");
      }

      exoBase.m_pcExoAliasList.Add(aliasName.ToExoString(), path.ToExoString());
      resMan.CreateDirectory(aliasName.ToExoString());
      resMan.AddResourceDirectory(aliasName.ToExoString(), priority, true.ToInt());
    }

    void IDisposable.Dispose()
    {
      Directory.Delete(EnvironmentConfig.ResourcePath, true);
    }
  }
}
