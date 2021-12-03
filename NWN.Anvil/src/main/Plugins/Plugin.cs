using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Anvil.Internal;

namespace Anvil.Plugins
{
  internal sealed class Plugin : IDisposable
  {
    private readonly PluginManager pluginManager;

    private PluginLoadContext pluginLoadContext;

    public Plugin(PluginManager pluginManager, string path)
    {
      this.pluginManager = pluginManager;
      Path = path;
      Name = AssemblyName.GetAssemblyName(path);
    }

    public Dictionary<string, string> AdditionalAssemblyPaths { get; init; }

    public Assembly Assembly { get; private set; }

    public bool HasResourceDirectory
    {
      get => ResourcePath != null && Directory.Exists(ResourcePath);
    }

    public bool IsLoaded
    {
      get => Assembly != null;
    }

    public bool Loading { get; private set; }

    public AssemblyName Name { get; }

    public string Path { get; }

    public string ResourcePath { get; init; }

    public Dictionary<string, string> UnmanagedAssemblyPaths { get; init; }

    public void Dispose()
    {
      Assembly = null;

      pluginLoadContext?.Dispose();
      WeakReference unloadHandle = new WeakReference(pluginLoadContext);
      pluginLoadContext = null;

      if (EnvironmentConfig.ReloadEnabled)
      {
        while (unloadHandle.IsAlive)
        {
          GC.Collect();
          GC.WaitForPendingFinalizers();
        }
      }
    }

    public void Load()
    {
      pluginLoadContext = new PluginLoadContext(pluginManager, this);
      Loading = true;

      try
      {
        Assembly = pluginLoadContext.LoadFromAssemblyName(Name);
      }
      finally
      {
        Loading = false;
      }
    }
  }
}
