using System;
using System.Collections.Generic;
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

    public AssemblyName Name { get; }

    public string Path { get; }

    public PluginInfoAttribute PluginInfo { get; }

    public string? ResourcePath { get; init; }

    public Assembly? Assembly { get; private set; }

    public IReadOnlyList<Type>? PluginTypes { get; private set; }

    public bool Loading { get; private set; }

    public bool IsLoaded => Assembly != null;

    internal IServiceContainer? Container { get; private set; }

    internal Dictionary<string, string>? AdditionalAssemblyPaths { get; init; }

    internal Dictionary<string, string>? UnmanagedAssemblyPaths { get; init; }

    internal Plugin(string path)
    {
      Path = path;
      Name = AssemblyName.GetAssemblyName(path);
      PluginInfo = LoadPluginInfo() ?? new PluginInfoAttribute();
    }

    internal void Load()
    {
      pluginLoadContext = InjectionService.Inject(new PluginLoadContext(this, PluginInfo.Isolated || EnvironmentConfig.ReloadEnabled));
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
      if (EnvironmentConfig.ReloadEnabled)
      {
        pluginLoadContext?.Dispose();
      }

      pluginLoadContext = null;
      return unloadHandle;
    }

    private PluginInfoAttribute? LoadPluginInfo()
    {
      try
      {
        List<string> assemblyPaths = new List<string>(Assemblies.RuntimeAssemblies)
        {
          Path,
          Assemblies.Anvil.Location,
        };

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
        Container = ServiceManager.CreateIsolatedPluginContainer(PluginTypes!);
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
      Container?.Dispose();
      Container = null;
    }
  }
}
