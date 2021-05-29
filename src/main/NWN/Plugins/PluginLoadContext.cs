using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using Anvil.Internal;

namespace NWN.Plugins
{
  internal sealed class PluginLoadContext : AssemblyLoadContext
  {
    private readonly PluginLoader pluginLoader;
    private readonly string pluginName;

    private readonly AssemblyDependencyResolver resolver;

    public PluginLoadContext(PluginLoader pluginLoader, string pluginPath, string pluginName) : base(EnvironmentConfig.ReloadEnabled)
    {
      this.pluginLoader = pluginLoader;
      this.pluginName = pluginName;
      resolver = new AssemblyDependencyResolver(pluginPath);
    }

    protected override Assembly Load(AssemblyName assemblyName)
    {
      if (!Anvil.Internal.Assemblies.ReservedNames.Contains(assemblyName.Name))
      {
        // Try resolving locally from the plugin folder.
        string assemblyPath = resolver.ResolveAssemblyToPath(assemblyName);
        if (assemblyPath != null)
        {
          return LoadAssemblyAtPath(assemblyPath);
        }
      }

      // Resolve from the plugin loader.
      return pluginLoader.ResolveDependency(pluginName, assemblyName);
    }

    private Assembly LoadAssemblyAtPath(string assemblyPath)
    {
      using MemoryStream memoryStream = new MemoryStream(File.ReadAllBytes(assemblyPath));
      return LoadFromStream(memoryStream);
    }

    protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
    {
      string libraryPath = resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
      if (libraryPath != null)
      {
        return LoadUnmanagedDllFromPath(libraryPath);
      }

      return IntPtr.Zero;
    }
  }
}
