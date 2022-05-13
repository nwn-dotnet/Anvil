using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using Anvil.Internal;
using Anvil.Services;
using NLog;

namespace Anvil.Plugins
{
  /// <summary>
  /// Loads all available plugins and their types for service initialisation.
  /// </summary>
  [ServiceBindingOptions(InternalBindingPriority.AboveNormal)]
  public sealed class PluginManager : ICoreService
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private const int PluginUnloadAttempts = 10;
    private const int PluginUnloadSleepMs = 5000;

    private readonly HashSet<Assembly> loadedAssemblies = new HashSet<Assembly>();
    private readonly List<Plugin> plugins = new List<Plugin>();

    internal IReadOnlyCollection<Type> LoadedTypes { get; private set; } = null!;

    internal IReadOnlyCollection<string> ResourcePaths { get; private set; } = null!;

    /// <summary>
    /// Gets the install directory of the specified plugin.
    /// </summary>
    /// <param name="pluginAssembly">The assembly of the plugin, e.g. typeof(MyService).Assembly</param>
    /// <returns>The install directory for the specified plugin.</returns>
    /// <exception cref="ArgumentException">Thrown if the specified assembly is not a plugin.</exception>
    public string? GetPluginDirectory(Assembly pluginAssembly)
    {
      if (IsPluginAssembly(pluginAssembly))
      {
        return Path.GetDirectoryName(pluginAssembly.Location);
      }

      throw new ArgumentException("Specified assembly is not a loaded plugin assembly.", nameof(pluginAssembly));
    }

    /// <summary>
    /// Gets if the specified assembly is the primary assembly for a plugin.
    /// </summary>
    /// <param name="assembly">The assembly to query.</param>
    /// <returns>True if the assembly is a plugin, otherwise false.</returns>
    public bool IsPluginAssembly(Assembly assembly)
    {
      return plugins.Any(plugin => plugin.Assembly == assembly);
    }

    public bool IsPluginLoaded(string pluginName)
    {
      return plugins.Any(plugin => plugin.Name.Name == pluginName);
    }

    public Assembly? ResolveDependency(string pluginName, AssemblyName dependencyName)
    {
      Assembly? assembly = ResolveDependencyFromAnvil(pluginName, dependencyName);
      if (assembly == null)
      {
        assembly = ResolveDependencyFromPlugins(pluginName, dependencyName);
      }

      return assembly;
    }

    void ICoreService.Init() {}

    void ICoreService.Load()
    {
      LoadCore();
      BootstrapPlugins();
      LoadPlugins();

      LoadedTypes = GetLoadedTypes();
      ResourcePaths = GetResourcePaths();
    }

    void ICoreService.Shutdown() {}

    void ICoreService.Start() {}

    void ICoreService.Unload()
    {
      loadedAssemblies.Clear();
      LoadedTypes = null!;
      ResourcePaths = null!;

      Log.Info("Unloading plugins...");
      Dictionary<WeakReference, string> pendingUnloads = new Dictionary<WeakReference, string>();
      foreach (Plugin plugin in plugins)
      {
        Log.Info("Unloading DotNET plugin {PluginName} - {PluginPath}", plugin.Name.Name, plugin.Path);
        pendingUnloads.Add(plugin.Unload(), plugin.Name.Name!);
      }

      plugins.Clear();
      plugins.TrimExcess();

      if (EnvironmentConfig.ReloadEnabled)
      {
        for (int unloadAttempt = 1; !IsUnloadComplete(pendingUnloads, unloadAttempt); unloadAttempt++)
        {
          GC.Collect();
          GC.WaitForPendingFinalizers();

          if (unloadAttempt > PluginUnloadAttempts)
          {
            Thread.Sleep(PluginUnloadSleepMs);
          }
        }
      }
    }

    private void BootstrapPlugins()
    {
      IPluginSource[] pluginSources =
      {
        new PaketPluginSource(this),
        new LocalPluginSource(this),
      };

      foreach (IPluginSource pluginSource in pluginSources)
      {
        plugins.AddRange(pluginSource.Bootstrap());
      }

      if (EnvironmentConfig.PreventStartNoPlugin && plugins.Count == 0)
      {
        throw new Exception("No plugins are available to load, and NWM_PREVENT_START_NO_PLUGIN is enabled.\n" +
          $"Check your plugins are available at {HomeStorage.Plugins}, or update NWM_PLUGIN_PATH to the correct location.");
      }
    }

    private IReadOnlyCollection<Type> GetLoadedTypes()
    {
      List<Type> loadedTypes = new List<Type>();
      foreach (Assembly assembly in loadedAssemblies)
      {
        loadedTypes.AddRange(GetTypesFromAssembly(assembly));
      }

      return loadedTypes;
    }

    private IReadOnlyCollection<string> GetResourcePaths()
    {
      List<string> resourcePaths = new List<string>();
      foreach (Plugin plugin in plugins)
      {
        if (plugin.HasResourceDirectory)
        {
          resourcePaths.Add(plugin.ResourcePath!);
        }
      }

      return resourcePaths;
    }

    private IEnumerable<Type> GetTypesFromAssembly(Assembly assembly)
    {
      IEnumerable<Type> assemblyTypes;
      try
      {
        assemblyTypes = assembly.GetTypes();
      }
      catch (ReflectionTypeLoadException e)
      {
        PluginInfoAttribute? pluginInfoAttribute = assembly.GetCustomAttribute<PluginInfoAttribute>();
        if (pluginInfoAttribute?.OptionalDependencies == null)
        {
          throw;
        }

        foreach (Exception? exception in e.LoaderExceptions)
        {
          if (exception is FileNotFoundException fileNotFoundException)
          {
            AssemblyName assemblyName = new AssemblyName(fileNotFoundException.FileName!);
            if (pluginInfoAttribute.OptionalDependencies.Contains(assemblyName.Name))
            {
              continue;
            }
          }

          throw;
        }

        assemblyTypes = e.Types.Where(type => type != null)!;
      }

      return assemblyTypes;
    }

    private bool IsUnloadComplete(Dictionary<WeakReference, string> pendingUnloads, int attempt)
    {
      bool retVal = true;
      foreach ((WeakReference? assemblyReference, string? pluginName) in pendingUnloads)
      {
        if (assemblyReference.IsAlive)
        {
          if (attempt > PluginUnloadAttempts)
          {
            Log.Warn("Plugin {PluginName} is preventing unload", pluginName);
          }

          retVal = false;
        }
      }

      return retVal;
    }

    private bool IsValidDependency(string plugin, AssemblyName requested, AssemblyName resolved)
    {
      if (requested.Name != resolved.Name)
      {
        return false;
      }

      if (requested.Version != resolved.Version)
      {
        Log.Warn("DotNET Plugin {Plugin} references {ReferenceName}, v{ReferenceVersion} but the server is running v{ServerVersion}! You may encounter compatibility issues",
          plugin,
          requested.Name,
          requested.Version,
          resolved.Version);
      }

      return true;
    }

    private void LoadCore()
    {
      foreach (Assembly assembly in Assemblies.AllAssemblies)
      {
        loadedAssemblies.Add(assembly);
      }
    }

    private void LoadPlugin(Plugin plugin)
    {
      Log.Info("Loading DotNET plugin {Plugin} - {PluginPath}", plugin.Name.Name, plugin.Path);
      plugin.Load();

      if (plugin.Assembly == null)
      {
        Log.Error("Failed to load DotNET plugin {Plugin} - {PluginPath}", plugin.Name.Name, plugin.Path);
        return;
      }

      loadedAssemblies.Add(plugin.Assembly);
      Log.Info("Loaded DotNET plugin {Plugin} - {PluginPath}", plugin.Name.Name, plugin.Path);
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

    private Assembly? ResolveDependencyFromAnvil(string pluginName, AssemblyName dependencyName)
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

    private Assembly? ResolveDependencyFromPlugins(string pluginName, AssemblyName dependencyName)
    {
      foreach (Plugin plugin in plugins)
      {
        if (!IsValidDependency(pluginName, dependencyName, plugin.Name))
        {
          continue;
        }

        if (plugin.Loading)
        {
          Log.Error("DotNET plugins {PluginA} <--> {PluginB} cannot be loaded as they have circular dependencies", pluginName, plugin.Name.Name);
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
  }
}
