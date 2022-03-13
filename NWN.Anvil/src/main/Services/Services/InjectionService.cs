using System;
using System.Collections.Generic;
using System.Reflection;
using Anvil.Plugins;

namespace Anvil.Services
{
  [ServiceBindingOptions(InternalBindingPriority.AboveNormal)]
  public sealed class InjectionService : ICoreService
  {
    private readonly List<PropertyInfo> injectedStaticProperties = new List<PropertyInfo>();
    private readonly PluginManager pluginManager;
    private readonly IServiceManager serviceManager;

    public InjectionService(IServiceManager serviceManager, PluginManager pluginManager)
    {
      this.serviceManager = serviceManager;
      this.pluginManager = pluginManager;
    }

    /// <summary>
    /// Injects all properties with <see cref="InjectAttribute"/> in the specified object.
    /// </summary>
    /// <param name="instance">The instance to inject.</param>
    /// <typeparam name="T">The instance type.</typeparam>
    /// <returns>The instance with injected dependencies.</returns>
    public T Inject<T>(T instance)
    {
      if (EqualityComparer<T>.Default.Equals(instance, default))
      {
        return default;
      }

      serviceManager.AnvilServiceContainer.InjectProperties(instance);
      return instance;
    }

    void ICoreService.Init() {}

    void ICoreService.Load() {}

    void ICoreService.Shutdown() {}

    void ICoreService.Start()
    {
      InjectStaticProperties(pluginManager.LoadedTypes);
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
          object value = serviceManager.AnvilServiceContainer.TryGetInstance(propertyInfo.PropertyType);
          propertyInfo.SetValue(null, value);
          injectedStaticProperties.Add(propertyInfo);
        }
      }
    }
  }
}
