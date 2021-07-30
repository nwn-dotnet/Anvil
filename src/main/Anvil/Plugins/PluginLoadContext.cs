using System;
using System.Reflection;
using System.Runtime.Loader;
using Anvil.Internal;

namespace Anvil.Plugins
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
      // Resolve this plugin's assembly locally.
      if (assemblyName.Name == pluginName)
      {
        return ResolveLocal(assemblyName);
      }

      // Resolve the dependency with the bundled assemblies (NWN.Core/Anvil), then check if other plugins can provide the dependency.
      Assembly assembly = pluginLoader.ResolveDependency(pluginName, assemblyName);

      if (assembly != null)
      {
        return assembly;
      }

      // The try resolving the dependency locally by checking the plugin folder.
      return ResolveLocal(assemblyName);
    }

    private Assembly ResolveLocal(AssemblyName assemblyName)
    {
      string assemblyPath = resolver.ResolveAssemblyToPath(assemblyName);
      if (assemblyPath != null)
      {
        return LoadFromAssemblyPath(assemblyPath);
      }

      return null;
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
