using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Anvil.Internal;
using Anvil.Services;
using NLog;

namespace Anvil.Plugins
{
  /// <summary>
  /// Loads all available plugins and their types for service initialisation.
  /// </summary>
  [ServiceBindingOptions(BindingOrder.Core)]
  internal sealed class PluginLoader : ITypeLoader
  {
    private const string PluginResourceDir = "resources";

    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    public IReadOnlyCollection<Type> LoadedTypes { get; private set; }

    public IReadOnlyCollection<string> ResourcePaths { get; private set; }

    private readonly HashSet<Assembly> loadedAssemblies = new HashSet<Assembly>();
    private readonly List<Plugin> plugins = new List<Plugin>();

    public void Init()
    {
      LoadCore();
      BootstrapPlugins();
      LoadPlugins();

      LoadedTypes = GetLoadedTypes();
      ResourcePaths = GetResourcePaths();
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
        throw new Exception("No plugins are available to load, and NWM_PREVENT_START_NO_PLUGIN is enabled.\n" +
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
          Log.Warn($"Cannot find plugin assembly \"{pluginPath}\". Does your plugin assembly match the name of the directory?");
          continue;
        }

        Plugin plugin = CreatePlugin(pluginPath, Path.Combine(pluginRoot, PluginResourceDir), pluginName);
        plugins.Add(plugin);
      }
    }

    private Plugin CreatePlugin(string pluginPath, string resourcePath, string pluginName)
    {
      PluginLoadContext pluginLoadContext = new PluginLoadContext(this, pluginPath, pluginName);
      return new Plugin(pluginPath, resourcePath, pluginLoadContext);
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
      Assembly assembly = ResolveDependencyFromAnvil(pluginName, dependencyName);
      if (assembly == null)
      {
        assembly = ResolveDependencyFromPlugins(pluginName, dependencyName);
      }

      return assembly;
    }

    private Assembly ResolveDependencyFromAnvil(string pluginName, AssemblyName dependencyName)
    {
      foreach (Assembly assembly in Assemblies.AllAssemblies)
      {
        if (IsValidDependency(pluginName, dependencyName, assembly.GetName()))
        {
          return assembly;
        }
      }

      return null;
    }

    private Assembly ResolveDependencyFromPlugins(string pluginName, AssemblyName dependencyName)
    {
      foreach (Plugin plugin in plugins)
      {
        if (!IsValidDependency(pluginName, dependencyName, plugin.AssemblyName))
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

    private bool IsValidDependency(string plugin, AssemblyName requested, AssemblyName resolved)
    {
      if (requested.Name != resolved.Name)
      {
        return false;
      }

      if (requested.Version != resolved.Version)
      {
        Log.Warn($"DotNET Plugin {plugin} references {requested.Name}, v{requested.Version} but the server is running v{resolved.Version}! You may encounter compatibility issues.");
      }

      return true;
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

    private IReadOnlyCollection<string> GetResourcePaths()
    {
      List<string> resourcePaths = new List<string>();
      foreach (Plugin plugin in plugins)
      {
        if (plugin.HasResourceDirectory)
        {
          resourcePaths.Add(plugin.ResourcePath);
        }
      }

      return resourcePaths.AsReadOnly();
    }

    void ITypeLoader.Dispose()
    {
      loadedAssemblies.Clear();
      LoadedTypes = null;
      ResourcePaths = null;

      Log.Info("Unloading plugins...");
      foreach (Plugin plugin in plugins)
      {
        plugin.Dispose();
        Log.Info($"Unloaded DotNET plugin ({plugin.AssemblyName.Name}) - {plugin.PluginPath}");
      }

      plugins.Clear();
    }
  }
}
