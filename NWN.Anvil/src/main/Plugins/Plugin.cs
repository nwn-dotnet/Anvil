using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Anvil.Internal;

namespace Anvil.Plugins
{
  internal sealed class Plugin(PluginManager pluginManager, string path)
  {
    private PluginLoadContext? pluginLoadContext;

    public Dictionary<string, string>? AdditionalAssemblyPaths { get; init; }

    public Assembly? Assembly { get; private set; }

    public bool HasResourceDirectory => ResourcePath != null && Directory.Exists(ResourcePath);

    public bool IsLoaded => Assembly != null;

    public bool Loading { get; private set; }

    public AssemblyName Name { get; } = AssemblyName.GetAssemblyName(path);

    public string Path { get; } = path;

    public string? ResourcePath { get; init; }

    public Dictionary<string, string>? UnmanagedAssemblyPaths { get; init; }

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

    public WeakReference Unload()
    {
      Assembly = null;
      WeakReference unloadHandle = new WeakReference(pluginLoadContext, true);
      if (EnvironmentConfig.ReloadEnabled)
      {
        pluginLoadContext?.Dispose();
      }

      pluginLoadContext = null;
      return unloadHandle;
    }
  }
}
