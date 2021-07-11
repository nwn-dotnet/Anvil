using System;
using System.Collections.Generic;
using System.Reflection;
using LightInject;
using NWN.Plugins;

namespace NWN.Services
{
  [ServiceBinding(typeof(InjectionService))]
  [ServiceBindingOptions(BindingOrder.API)]
  public sealed class InjectionService
  {
    private readonly IServiceContainer container;

    public InjectionService(IServiceContainer container, ITypeLoader typeLoader)
    {
      this.container = container;
      InjectStaticProperties(typeLoader.LoadedTypes);
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

      container.InjectProperties(instance);
      return instance;
    }

    private void InjectStaticProperties(IEnumerable<Type> types)
    {
      InjectPropertySelector propertySelector = new InjectPropertySelector(InjectPropertyTypes.StaticOnly);

      foreach (Type type in types)
      {
        List<PropertyInfo> injectableTypes = (List<PropertyInfo>)propertySelector.Execute(type);

        foreach (PropertyInfo propertyInfo in injectableTypes)
        {
          object value = container.TryGetInstance(propertyInfo.PropertyType);
          propertyInfo.SetValue(null, value);
        }
      }
    }
  }
}
