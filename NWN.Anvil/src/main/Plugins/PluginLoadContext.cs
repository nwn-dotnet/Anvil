using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Loader;
using Anvil.Internal;

namespace Anvil.Plugins
{
  internal sealed class PluginLoadContext : AssemblyLoadContext
  {
    private static readonly string[] NativeLibPrefixes = { "lib" };
    private readonly Dictionary<string, WeakReference<Assembly>> assemblyCache = new Dictionary<string, WeakReference<Assembly>>();
    private readonly Plugin plugin;

    private readonly PluginManager pluginManager;

    private readonly AssemblyDependencyResolver resolver;

    public PluginLoadContext(PluginManager pluginManager, Plugin plugin) : base(EnvironmentConfig.ReloadEnabled)
    {
      this.pluginManager = pluginManager;
      this.plugin = plugin;

      resolver = new AssemblyDependencyResolver(plugin.Path);
    }

    protected override Assembly Load(AssemblyName assemblyName)
    {
      if (!assemblyCache.TryGetValue(assemblyName.FullName, out WeakReference<Assembly> assemblyRef) || !assemblyRef.TryGetTarget(out Assembly assembly))
      {
        assembly = GetAssembly(assemblyName);
        assemblyRef = new WeakReference<Assembly>(assembly);
        assemblyCache[assemblyName.FullName] = assemblyRef;
      }

      return assembly;
    }

    protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
    {
      string libraryPath = resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
      if (libraryPath != null)
      {
        return LoadUnmanagedDllFromPath(libraryPath);
      }

      libraryPath = ResolveUnamangedFromAdditionalPaths(unmanagedDllName);
      if (libraryPath != null)
      {
        return LoadUnmanagedDllFromPath(libraryPath);
      }

      return IntPtr.Zero;
    }

    private Assembly GetAssembly(AssemblyName assemblyName)
    {
      // Resolve this plugin's assembly locally.
      if (assemblyName.Name == plugin.Name.Name)
      {
        return ResolveLocal(assemblyName);
      }

      // Resolve the dependency with the bundled assemblies (NWN.Core/Anvil), then check if other plugins can provide the dependency.
      Assembly assembly = pluginManager.ResolveDependency(plugin.Name.Name, assemblyName);
      if (assembly != null)
      {
        return assembly;
      }

      // Then try resolving the dependency locally by checking the plugin folder.
      assembly = ResolveLocal(assemblyName);
      if (assembly != null)
      {
        return assembly;
      }

      // Then try resolving from any specified linked roots.
      return ResolveFromAdditionalPaths(assemblyName);
    }

    private Assembly ResolveFromAdditionalPaths(AssemblyName assemblyName)
    {
      if (plugin.AdditionalAssemblyPaths != null && plugin.AdditionalAssemblyPaths.TryGetValue(assemblyName.Name!, out string assemblyPath))
      {
        return LoadFromAssemblyPath(assemblyPath);
      }

      return null;
    }

    private Assembly ResolveLocal(AssemblyName assemblyName)
    {
      string assemblyPath = resolver.ResolveAssemblyToPath(assemblyName);
      return assemblyPath != null ? LoadFromAssemblyPath(assemblyPath) : null;
    }

    private string ResolveUnamangedFromAdditionalPaths(string unmanagedDllName)
    {
      if (plugin.UnmanagedAssemblyPaths == null)
      {
        return null;
      }

      if (plugin.UnmanagedAssemblyPaths.TryGetValue(unmanagedDllName, out string path))
      {
        return path;
      }

      foreach (string nativeLibPrefix in NativeLibPrefixes)
      {
        if (plugin.UnmanagedAssemblyPaths.TryGetValue(nativeLibPrefix + unmanagedDllName, out path))
        {
          return path;
        }
      }

      return null;
    }
  }
}
