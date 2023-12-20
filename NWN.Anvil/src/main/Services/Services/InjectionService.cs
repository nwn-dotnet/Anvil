using System;
using System.Collections.Generic;
using System.Reflection;
using Anvil.Internal;
using Anvil.Plugins;

namespace Anvil.Services
{
  [ServiceBindingOptions(InternalBindingPriority.Highest)]
  public sealed class InjectionService : ICoreService
  {
    private readonly List<PropertyInfo> injectedStaticProperties = new List<PropertyInfo>();

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

    void ICoreService.Init() {}

    void ICoreService.Load() {}

    void ICoreService.Shutdown() {}

    void ICoreService.Start()
    {
      InjectStaticProperties(Assemblies.AllTypes);
      foreach (Plugin plugin in PluginManager.Value.Plugins)
      {
        if (plugin.PluginTypes != null)
        {
          InjectStaticProperties(plugin.PluginTypes);
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

      injectedStaticProperties.Clear();
    }

    private void InjectStaticProperties(IEnumerable<Type> types)
    {
      InjectPropertySelector propertySelector = new InjectPropertySelector(InjectPropertyTypes.StaticOnly);
      foreach (Type type in types)
      {
        List<PropertyInfo> injectableTypes = (List<PropertyInfo>)propertySelector.Execute(type);

        foreach (PropertyInfo propertyInfo in injectableTypes)
        {
          object value = ServiceManager.AnvilServiceContainer.TryGetInstance(propertyInfo.PropertyType);
          propertyInfo.SetValue(null, value);
          injectedStaticProperties.Add(propertyInfo);
        }
      }
    }
  }
}
