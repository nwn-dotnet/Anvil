using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LightInject;

namespace NWN.Services
{
  /// <summary>
  /// An <see cref="IPropertyDependencySelector"/> that uses the <see cref="InjectAttribute"/> to determine which properties to inject service dependencies.
  /// </summary>
  internal sealed class InjectPropertyDependencySelector : PropertyDependencySelector
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="InjectPropertyDependencySelector"/> class.
    /// </summary>
    /// <param name="propertySelector">The <see cref="InjectPropertySelector"/> that is responsible for selecting a list of injectable properties.</param>
    public InjectPropertyDependencySelector(InjectPropertySelector propertySelector) : base(propertySelector) {}

    /// <summary>
    /// Selects the property dependencies for the given <paramref name="type"/>
    /// that is annotated with the <see cref="InjectAttribute"/>.
    /// </summary>
    /// <param name="type">The <see cref="Type"/> for which to select the property dependencies.</param>
    /// <returns>A list of <see cref="PropertyDependency"/> instances that represents the property dependencies for the given <paramref name="type"/>.</returns>
    public override IEnumerable<PropertyDependency> Execute(Type type)
    {
      PropertyInfo[] properties = PropertySelector.Execute(type).ToArray();
      foreach (PropertyInfo propertyInfo in properties)
      {
        InjectAttribute injectAttribute = propertyInfo.GetCustomAttribute<InjectAttribute>(true);

        if (injectAttribute != null)
        {
          yield return new PropertyDependency
          {
            Property = propertyInfo,
            ServiceName = injectAttribute.ServiceName,
            ServiceType = propertyInfo.PropertyType,
            IsRequired = true,
          };
        }
      }
    }
  }
}
