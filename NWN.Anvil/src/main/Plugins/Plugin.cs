using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using Anvil.API;
using Anvil.Internal;
using Anvil.Services;
using LightInject;
using NLog;

namespace Anvil.Plugins
{
  /// <summary>
  /// Represents an anvil plugin. Plugins are loaded during startup, or on-demand if configured with an isolated context.
  /// </summary>
  public sealed class Plugin
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    [Inject]
    private ResourceManager ResourceManager { get; init; } = null!;

    [Inject]
    private InjectionService InjectionService { get; init; } = null!;

    [Inject]
    private IServiceManager ServiceManager { get; init; } = null!;

    private string? resourcePathAlias;
    private PluginLoadContext? pluginLoadContext;

    /// <summary>
    /// Gets the name/id of this plugin.
    /// </summary>
    public AssemblyName Name { get; }

    /// <summary>
    /// Gets the full path of the plugin assembly.
    /// </summary>
    public string Path { get; }

    /// <summary>
    /// Gets additional metadata information about this plugin.
    /// </summary>
    public PluginInfoAttribute PluginInfo { get; private set; } = null!;

    /// <summary>
    /// Gets the path in the plugin containing additional game resources.
    /// </summary>
    public string? ResourcePath { get; init; }

    /// <summary>
    /// If the plugin is loaded (<see cref="IsLoaded"/>), returns the loaded assembly.
    /// </summary>
    public Assembly? Assembly { get; private set; }

    /// <summary>
    /// If the plugin is loaded (<see cref="IsLoaded"/>), returns a list containing all recognised types in the plugin assembly.
    /// </summary>
    public IReadOnlyList<Type>? PluginTypes { get; private set; }

    /// <summary>
    /// Gets if the plugin is currently loading.
    /// </summary>
    public bool Loading { get; private set; }

    /// <summary>
    /// Gets if this plugin has been loaded by anvil and is currently active.
    /// </summary>
    [MemberNotNullWhen(true, nameof(PluginTypes))]
    [MemberNotNullWhen(true, nameof(Assembly))]
    public bool IsLoaded => Assembly != null;

    internal IServiceContainer? Container { get; private set; }

    internal Dictionary<string, string>? AdditionalAssemblyPaths { get; init; }

    internal Dictionary<string, string>? UnmanagedAssemblyPaths { get; init; }

    internal Plugin(string path)
    {
      Path = path;
      Name = AssemblyName.GetAssemblyName(path);
    }

    internal void Init(List<string> assemblyPaths)
    {
      PluginInfo = LoadPluginInfo(assemblyPaths) ?? new PluginInfoAttribute();
    }

    internal void Load()
    {
      pluginLoadContext = InjectionService.Inject(new PluginLoadContext(this, EnvironmentConfig.ReloadEnabled || PluginInfo.Isolated));
      Loading = true;

      try
      {
        Assembly = pluginLoadContext.LoadFromAssemblyName(Name);
        PluginTypes = GetPluginTypes();
        SetupResourceDirectory();
        SetupIsolatedContainer();
      }
      finally
      {
        Loading = false;
      }
    }

    internal WeakReference Unload()
    {
      RemoveResourceDirectory();
      RemoveIsolatedContainer();

      Assembly = null;
      PluginTypes = null;

      WeakReference unloadHandle = new WeakReference(pluginLoadContext, true);
      if (EnvironmentConfig.ReloadEnabled || PluginInfo.Isolated)
      {
        pluginLoadContext?.Dispose();
      }

      pluginLoadContext = null;
      return unloadHandle;
    }

    private PluginInfoAttribute? LoadPluginInfo(List<string> assemblyPaths)
    {
      try
      {
        PathAssemblyResolver resolver = new PathAssemblyResolver(assemblyPaths);
        using MetadataLoadContext context = new MetadataLoadContext(resolver);
        Assembly assemblyMeta = context.LoadFromAssemblyPath(Path);

        return assemblyMeta.GetCustomAttributeFromMetadata<PluginInfoAttribute>();
      }
      catch (Exception e)
      {
        Log.Error(e, $"Loading metadata for plugin '{Name.Name}' failed.");
        return null;
      }
    }

    private IReadOnlyList<Type> GetPluginTypes()
    {
      IReadOnlyList<Type> assemblyTypes;
      try
      {
        assemblyTypes = Assembly!.GetTypes();
      }
      catch (ReflectionTypeLoadException e)
      {
        if (PluginInfo.OptionalDependencies == null)
        {
          throw;
        }

        foreach (Exception? exception in e.LoaderExceptions)
        {
          if (exception is FileNotFoundException fileNotFoundException)
          {
            AssemblyName assemblyName = new AssemblyName(fileNotFoundException.FileName!);
            if (PluginInfo.OptionalDependencies.Contains(assemblyName.Name))
            {
              continue;
            }
          }

          throw;
        }

        assemblyTypes = e.Types.Where(type => type != null).ToList()!;
      }

      return assemblyTypes;
    }

    private void SetupResourceDirectory()
    {
      if (!Directory.Exists(ResourcePath))
      {
        return;
      }

      resourcePathAlias = ResourceManager.CreateResourceDirectory(ResourcePath);
    }

    private void SetupIsolatedContainer()
    {
      if (PluginInfo.Isolated)
      {
        Container = ServiceManager.CreatePluginContainer(PluginTypes!);
      }
    }

    private void RemoveResourceDirectory()
    {
      if (resourcePathAlias != null)
      {
        ResourceManager.RemoveResourceDirectory(resourcePathAlias);
        resourcePathAlias = null;
      }
    }

    private void RemoveIsolatedContainer()
    {
      if (Container != null)
      {
        ServiceManager.DisposePluginContainer(Container);
        Container = null;
      }
    }
  }
}
