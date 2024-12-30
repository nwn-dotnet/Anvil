using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Anvil.Internal;
using Anvil.Plugins;
using LightInject;

namespace Anvil.Services
{
  public sealed class InjectionService : ICoreService
  {
    private readonly List<PropertyInfo> injectedStaticProperties = [];
    private readonly Dictionary<Plugin, List<PropertyInfo>> pluginInjectedProperties = new Dictionary<Plugin, List<PropertyInfo>>();

    [Inject]
    private IServiceManager ServiceManager { get; init; } = null!;

    [Inject]
    private Lazy<PluginManager> PluginManager { get; init; } = null!;

    /// <summary>
    /// Injects all properties with <see cref="InjectAttribute"/> in the specified object.
    /// </summary>
    /// <param name="instance">The instance to inject.</param>
    /// <typeparam name="T">The instance type.</typeparam>
    /// <returns>The instance with injected dependencies.</returns>
    public T Inject<T>(T instance)
    {
      Plugin? plugin = PluginManager.Value.GetPlugin(typeof(T).Assembly);
      if (plugin?.Container != null)
      {
        plugin.Container.InjectProperties(instance);
      }
      else
      {
        ServiceManager.InjectProperties(instance);
      }

      return instance;
    }

    void ICoreService.Init()
    {
      ServiceManager.OnContainerCreate += OnContainerCreate;
      ServiceManager.OnContainerPostDispose += OnContainerPostDispose;
    }

    private void OnContainerCreate(IServiceContainer container, Plugin? plugin)
    {
      if (plugin?.PluginTypes == null)
      {
        return;
      }

      pluginInjectedProperties.TryGetValue(plugin, out List<PropertyInfo>? propertyList);
      propertyList ??= [];
      pluginInjectedProperties[plugin] = propertyList;
      InjectStaticProperties(container, propertyList, plugin.PluginTypes);
    }

    private void OnContainerPostDispose(IServiceContainer container, Plugin? plugin)
    {
      if (plugin == null)
      {
        return;
      }

      if (pluginInjectedProperties.Remove(plugin, out List<PropertyInfo>? propertyList))
      {
        foreach (PropertyInfo propertyInfo in propertyList)
        {
          propertyInfo.SetValue(null, default);
        }
      }
    }

    void ICoreService.Load() {}

    void ICoreService.Shutdown() {}

    void ICoreService.Start()
    {
      InjectStaticProperties(ServiceManager.AnvilServiceContainer, injectedStaticProperties, Assemblies.AllTypes);
      foreach (Plugin plugin in PluginManager.Value.Plugins)
      {
        if (plugin.PluginTypes == null)
        {
          continue;
        }

        if (plugin.Container == null)
        {
          InjectStaticProperties(ServiceManager.AnvilServiceContainer, injectedStaticProperties, plugin.PluginTypes);
        }
        else
        {
          pluginInjectedProperties.TryGetValue(plugin, out List<PropertyInfo>? propertyList);
          propertyList ??= [];
          pluginInjectedProperties[plugin] = propertyList;

          InjectStaticProperties(plugin.Container, propertyList, plugin.PluginTypes);
        }
      }
    }

    // We clear injected properties as they can hold invalid references when reloading Anvil.
    void ICoreService.Unload()
    {
      foreach (PropertyInfo propertyInfo in injectedStaticProperties)
      {
        propertyInfo.SetValue(null, default);
      }

      foreach (PropertyInfo propertyInfo in pluginInjectedProperties.SelectMany(pair => pair.Value))
      {
        propertyInfo.SetValue(null, default);
      }

      injectedStaticProperties.Clear();
      pluginInjectedProperties.Clear();
    }

    private void InjectStaticProperties(IServiceContainer container, List<PropertyInfo> propertyList, IEnumerable<Type> types)
    {
      InjectPropertySelector propertySelector = new InjectPropertySelector(InjectPropertyTypes.StaticOnly);
      foreach (Type type in types)
      {
        List<PropertyInfo> injectableTypes = (List<PropertyInfo>)propertySelector.Execute(type);

        foreach (PropertyInfo propertyInfo in injectableTypes)
        {
          object value = container.TryGetInstance(propertyInfo.PropertyType);
          propertyInfo.SetValue(null, value);
          propertyList.Add(propertyInfo);
        }
      }
    }
  }
}
