using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Anvil.API;
using Anvil.Internal;
using Anvil.Services;
using NLog;

namespace Anvil.Plugins
{
  internal sealed class Plugin
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    [Inject]
    private ResourceManager ResourceManager { get; init; } = null!;

    [Inject]
    private InjectionService InjectionService { get; init; } = null!;

    private string? resourcePathAlias;
    private PluginLoadContext? pluginLoadContext;

    public AssemblyName Name { get; }

    public string Path { get; }

    public PluginInfoAttribute? PluginInfo { get; }

    public string? ResourcePath { get; init; }

    public Assembly? Assembly { get; private set; }

    public IReadOnlyList<Type>? PluginTypes { get; private set; }

    public bool Loading { get; private set; }

    public bool IsLoaded => Assembly != null;

    internal Dictionary<string, string>? AdditionalAssemblyPaths { get; init; }

    internal Dictionary<string, string>? UnmanagedAssemblyPaths { get; init; }

    public Plugin(string path)
    {
      Path = path;
      Name = AssemblyName.GetAssemblyName(path);
    }

    public void Load()
    {
      pluginLoadContext = InjectionService.Inject(new PluginLoadContext(this));
      Loading = true;

      try
      {
        Assembly = pluginLoadContext.LoadFromAssemblyName(Name);
        PluginTypes = GetPluginTypes();
        AddResourceDirectory();
      }
      finally
      {
        Loading = false;
      }
    }

    public WeakReference Unload()
    {
      RemoveResourceDirectory();

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

    private IReadOnlyList<Type> GetPluginTypes()
    {
      IReadOnlyList<Type> assemblyTypes;
      try
      {
        assemblyTypes = Assembly!.GetTypes();
      }
      catch (ReflectionTypeLoadException e)
      {
        if (PluginInfo?.OptionalDependencies == null)
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

    private void AddResourceDirectory()
    {
      if (!Directory.Exists(ResourcePath))
      {
        return;
      }

      resourcePathAlias = ResourceManager.CreateResourceDirectory(ResourcePath);
    }

    private void RemoveResourceDirectory()
    {
      if (resourcePathAlias != null)
      {
        ResourceManager.RemoveResourceDirectory(resourcePathAlias);
        resourcePathAlias = null;
      }
    }
  }
}
