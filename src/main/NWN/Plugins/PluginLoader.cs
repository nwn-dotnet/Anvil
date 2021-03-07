using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NLog;
using NWN.Services;

namespace NWN.Plugins
{
  /// <summary>
  /// Loads all available plugins and their types for service initialisation.
  /// </summary>
  [BindingOrder(BindingOrder.Core)]
  internal class PluginLoader : ITypeLoader
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    public IReadOnlyCollection<Type> LoadedTypes { get; private set; }

    private readonly HashSet<Assembly> loadedAssemblies = new HashSet<Assembly>();
    private readonly List<Plugin> plugins = new List<Plugin>();

    public void Init()
    {
      LoadCore();
      BootstrapPlugins();
      LoadPlugins();
      LoadedTypes = GetLoadedTypes();
    }

    private void LoadCore()
    {
      foreach (Assembly assembly in Assemblies.AllAssemblies)
      {
        loadedAssemblies.Add(assembly);
      }
    }

    private void BootstrapPlugins()
    {
      string[] pluginPaths = Directory.GetDirectories(EnvironmentConfig.PluginsPath);

      Log.Info($"Loading {pluginPaths.Length} DotNET plugin/s from: {EnvironmentConfig.PluginsPath}");

      if (EnvironmentConfig.PreventStartNoPlugin && pluginPaths.Length == 0)
      {
        throw new Exception($"No plugins are available to load, and NWM_PREVENT_START_NO_PLUGIN is enabled.\n" +
          $"Check your plugins are available at {EnvironmentConfig.PluginsPath}, or update NWM_PLUGIN_PATH to the correct location.");
      }

      foreach (string pluginRoot in pluginPaths)
      {
        string pluginName = new DirectoryInfo(pluginRoot).Name;

        if (PluginNameIsReserved(pluginName))
        {
          Log.Warn($"Skipping plugin \"{pluginName}\" as it uses a reserved name.");
          continue;
        }

        string pluginPath = Path.Combine(pluginRoot, $"{pluginName}.dll");
        if (!File.Exists(pluginPath))
        {
          continue;
        }

        Plugin plugin = CreatePlugin(pluginPath, pluginName);
        plugins.Add(plugin);
      }
    }

    private Plugin CreatePlugin(string pluginPath, string pluginName)
    {
      PluginLoadContext pluginLoadContext = new PluginLoadContext(this, pluginPath, pluginName);
      return new Plugin(pluginPath, pluginLoadContext);
    }

    private static bool PluginNameIsReserved(string pluginName)
    {
      return Assemblies.ReservedNames.Contains(pluginName);
    }

    private void LoadPlugins()
    {
      foreach (Plugin plugin in plugins)
      {
        if (plugin.IsLoaded)
        {
          continue;
        }

        LoadPlugin(plugin);
      }
    }

    private void LoadPlugin(Plugin plugin)
    {
      Log.Info($"Loading DotNET plugin ({plugin.AssemblyName.Name}) - {plugin.PluginPath}");
      plugin.Load();

      if (plugin.Assembly == null)
      {
        Log.Error($"Failed to load DotNET plugin ({plugin.AssemblyName.Name}) - {plugin.PluginPath}");
        return;
      }

      loadedAssemblies.Add(plugin.Assembly);
      Log.Info($"Loaded DotNET plugin ({plugin.AssemblyName.Name}) - {plugin.PluginPath}");
    }

    public Assembly ResolveDependency(string pluginName, AssemblyName dependencyName)
    {
      Assembly assembly = ResolveDependencyFromManaged(pluginName, dependencyName);
      if (assembly == null)
      {
        assembly = ResolveDependencyFromPlugins(pluginName, dependencyName);
      }

      return assembly;
    }

    private Assembly ResolveDependencyFromManaged(string pluginName, AssemblyName dependencyName)
    {
      foreach (Assembly assembly in Assemblies.AllAssemblies)
      {
        AssemblyName assemblyName = assembly.GetName();
        if (assemblyName.Name != dependencyName.Name)
        {
          continue;
        }

        if (dependencyName.Version != assemblyName.Version)
        {
          Log.Warn($"DotNET Plugin {pluginName} references {dependencyName.Name}, v{dependencyName.Version} but the server is running v{assemblyName.Version}! You may encounter compatibility issues.");
        }

        return assembly;
      }

      return null;
    }

    private Assembly ResolveDependencyFromPlugins(string pluginName, AssemblyName dependencyName)
    {
      foreach (Plugin plugin in plugins)
      {
        if (!plugin.IsMatchingPlugin(dependencyName))
        {
          continue;
        }

        if (plugin.Loading)
        {
          Log.Error($"DotNET plugins {pluginName} <--> {plugin.AssemblyName.Name} cannot be loaded as they have circular dependencies.");
          return null;
        }

        if (!plugin.IsLoaded)
        {
          LoadPlugin(plugin);
        }

        return plugin.Assembly;
      }

      return null;
    }

    private IReadOnlyCollection<Type> GetLoadedTypes()
    {
      List<Type> loadedTypes = new List<Type>();
      foreach (Assembly assembly in loadedAssemblies)
      {
        loadedTypes.AddRange(assembly.GetTypes());
      }

      return loadedTypes.AsReadOnly();
    }

    public void Dispose()
    {
      loadedAssemblies.Clear();
      LoadedTypes = null;

      foreach (Plugin plugin in plugins)
      {
        plugin.Dispose();
        Log.Info($"Unloaded DotNET plugin ({plugin.AssemblyName.Name}) - {plugin.PluginPath}");
      }

      plugins.Clear();
    }
  }
}
