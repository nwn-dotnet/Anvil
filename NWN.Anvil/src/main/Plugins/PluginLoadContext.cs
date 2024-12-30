using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Loader;
using Anvil.Services;

namespace Anvil.Plugins
{
  internal sealed class PluginLoadContext(Plugin plugin, bool isCollectible) : AssemblyLoadContext(isCollectible), IDisposable
  {
    private static readonly string[] NativeLibPrefixes = ["lib"];

    [Inject]
    private PluginManager PluginManager { get; init; } = null!;

    private Plugin? plugin = plugin;
    private AssemblyDependencyResolver resolver = new AssemblyDependencyResolver(plugin.Path);
    private readonly Dictionary<string, WeakReference<Assembly>> assemblyCache = new Dictionary<string, WeakReference<Assembly>>();

    public void Dispose()
    {
      resolver = null!;
      plugin = null;
      assemblyCache.Clear();
      Unload();
    }

    protected override Assembly? Load(AssemblyName assemblyName)
    {
      if (!assemblyCache.TryGetValue(assemblyName.FullName, out WeakReference<Assembly>? assemblyRef) || !assemblyRef.TryGetTarget(out Assembly? assembly))
      {
        assembly = GetAssembly(assemblyName);
        if (assembly == null)
        {
          return assembly;
        }

        assemblyRef = new WeakReference<Assembly>(assembly);
        assemblyCache[assemblyName.FullName] = assemblyRef;
      }

      return assembly;
    }

    protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
    {
      if (plugin == null)
      {
        return IntPtr.Zero;
      }

      string? libraryPath = resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
      if (libraryPath != null)
      {
        return LoadUnmanagedDllFromPath(libraryPath);
      }

      libraryPath = ResolveUnmanagedFromAdditionalPaths(unmanagedDllName);
      if (libraryPath != null)
      {
        return LoadUnmanagedDllFromPath(libraryPath);
      }

      return IntPtr.Zero;
    }

    private Assembly? GetAssembly(AssemblyName assemblyName)
    {
      if (plugin?.Name.Name == null)
      {
        return null;
      }

      // Resolve the plugin's own assembly from our known path
      if (assemblyName.Name == plugin.Name.Name)
      {
        return LoadFromAssemblyPath(plugin.Path);
      }

      // Resolve the dependency with the bundled assemblies (NWN.Core/Anvil), then check if other plugins can provide the dependency.
      Assembly? assembly = PluginManager.ResolveDependency(plugin.Name.Name, assemblyName);
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

    private Assembly? ResolveFromAdditionalPaths(AssemblyName assemblyName)
    {
      if (plugin?.AdditionalAssemblyPaths != null && plugin.AdditionalAssemblyPaths.TryGetValue(assemblyName.Name!, out string? assemblyPath))
      {
        return LoadFromAssemblyPath(assemblyPath);
      }

      return null;
    }

    private Assembly? ResolveLocal(AssemblyName assemblyName)
    {
      string? assemblyPath = resolver.ResolveAssemblyToPath(assemblyName);
      return assemblyPath != null ? LoadFromAssemblyPath(assemblyPath) : null;
    }

    private string? ResolveUnmanagedFromAdditionalPaths(string unmanagedDllName)
    {
      if (plugin?.UnmanagedAssemblyPaths == null)
      {
        return null;
      }

      if (plugin.UnmanagedAssemblyPaths.TryGetValue(unmanagedDllName, out string? path))
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
