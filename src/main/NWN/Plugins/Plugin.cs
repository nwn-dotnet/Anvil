using System;
using System.Reflection;

namespace NWN.Plugins
{
  internal sealed class Plugin : IDisposable
  {
    public readonly string PluginPath;
    public readonly AssemblyName AssemblyName;

    private PluginLoadContext pluginLoadContext;

    public bool Loading { get; private set; }

    public bool IsLoaded
    {
      get => Assembly != null;
    }

    public Assembly Assembly;

    public Plugin(string pluginPath, PluginLoadContext pluginLoadContext)
    {
      PluginPath = pluginPath;
      AssemblyName = AssemblyName.GetAssemblyName(pluginPath);
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
      return AssemblyName == assemblyName;
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
