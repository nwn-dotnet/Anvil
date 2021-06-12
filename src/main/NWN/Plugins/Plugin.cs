using System;
using System.IO;
using System.Reflection;
using Anvil.Internal;

namespace NWN.Plugins
{
  internal sealed class Plugin : IDisposable
  {
    public readonly string PluginPath;
    public readonly string ResourcePath;
    public readonly bool HasResourceDirectory;
    public readonly AssemblyName AssemblyName;

    private PluginLoadContext pluginLoadContext;

    public bool Loading { get; private set; }

    public bool IsLoaded
    {
      get => Assembly != null;
    }

    public Assembly Assembly;

    public Plugin(string pluginPath, string resourcePath, PluginLoadContext pluginLoadContext)
    {
      PluginPath = pluginPath;
      ResourcePath = resourcePath;
      AssemblyName = AssemblyName.GetAssemblyName(pluginPath);
      HasResourceDirectory = Directory.Exists(resourcePath);
      this.pluginLoadContext = pluginLoadContext;
    }

    public void Load()
    {
      Loading = true;

      try
      {
        Assembly = pluginLoadContext.LoadFromAssemblyName(AssemblyName);
      }
      finally
      {
        Loading = false;
      }
    }

    public bool IsMatchingPlugin(AssemblyName assemblyName)
    {
      return AssemblyName.Name == assemblyName.Name;
    }

    public void Dispose()
    {
      Assembly = null;

      if (EnvironmentConfig.ReloadEnabled)
      {
        pluginLoadContext.Unload();
      }

      pluginLoadContext = null;
    }
  }
}
