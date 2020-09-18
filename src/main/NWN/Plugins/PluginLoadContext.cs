using System;
using System.Reflection;
using System.Runtime.Loader;

namespace NWN.Plugins
{
  internal class PluginLoadContext : AssemblyLoadContext
  {
    private readonly AssemblyDependencyResolver resolver;

    public PluginLoadContext(string pluginPath)
    {
      resolver = new AssemblyDependencyResolver(pluginPath);
    }

    protected override Assembly Load(AssemblyName assemblyName)
    {
      // Try resolving locally from the plugin folder.
      string assemblyPath = resolver.ResolveAssemblyToPath(assemblyName);
      if (assemblyPath != null)
      {
        return LoadFromAssemblyPath(assemblyPath);
      }

      // Try resolving from NWN.Managed.
      foreach (Assembly assembly in AssemblyConstants.NWMLoadContext.Assemblies)
      {
        if (assembly.FullName == assemblyName.FullName)
        {
          return assembly;
        }
      }

      // Try resolving a different version
      foreach (Assembly assembly in AssemblyConstants.NWMLoadContext.Assemblies)
      {
        if (assembly.GetName().Name == assemblyName.Name)
        {
          return assembly;
        }
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