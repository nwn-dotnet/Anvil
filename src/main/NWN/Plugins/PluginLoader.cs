using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NLog;

namespace NWN.Plugins
{
  /// <summary>
  /// Loads all available plugins and their types for service initialisation.
  /// </summary>
  internal class PluginLoader : ITypeLoader
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    public IReadOnlyCollection<Type> LoadedTypes { get; }

    private readonly HashSet<Assembly> loadedAssemblies = new HashSet<Assembly>();

    public PluginLoader()
    {
      LoadCore();
      LoadPlugins();
      LoadedTypes = GetLoadedTypes();
    }

    private void LoadCore()
    {
      foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
      {
        if (IsValidAssembly(assembly))
        {
          loadedAssemblies.Add(assembly);
        }
      }
    }

    private void LoadPlugins()
    {
      string pluginRoot = ResolvePluginsRoot();

      foreach (string assemblyName in Directory.GetFiles(pluginRoot, "*.dll", SearchOption.AllDirectories))
      {
        Assembly assembly = LoadPluginAssembly(assemblyName);
        if (assembly != null && IsValidAssembly(assembly))
        {
          loadedAssemblies.Add(assembly);
          Log.Info($"Loaded DotNET plugin ({assembly.GetName().Name}).");
        }
      }
    }

    private static string ResolvePluginsRoot()
    {
      string pluginPath = EnvironmentConfig.PluginsPath;
      if (string.IsNullOrEmpty(pluginPath))
      {
        pluginPath = $"{AssemblyConstants.AssemblyDir}/Plugins";
      }

      return pluginPath;
    }

    private Assembly LoadPluginAssembly(string assemblyPath)
    {
      try
      {
        PluginLoadContext loadContext = new PluginLoadContext(assemblyPath);
        return loadContext.LoadFromAssemblyName(AssemblyName.GetAssemblyName(assemblyPath));
      }
      catch (BadImageFormatException) {}

      return null;
    }

    private static bool IsValidAssembly(Assembly assembly)
      => assembly == AssemblyConstants.NWMAssembly || assembly.GetReferencedAssemblies().Any(name => name.ToString() == AssemblyConstants.NWMName.FullName);

    private IReadOnlyCollection<Type> GetLoadedTypes()
    {
      List<Type> loadedTypes = new List<Type>();
      foreach (Assembly assembly in loadedAssemblies)
      {
        loadedTypes.AddRange(assembly.GetTypes());
      }

      return loadedTypes.AsReadOnly();
    }
  }
}