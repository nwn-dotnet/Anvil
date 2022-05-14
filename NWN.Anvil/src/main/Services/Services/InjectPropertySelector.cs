using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Anvil.API;
using LightInject;
using NLog;

namespace Anvil.Services
{
  internal sealed class InjectPropertySelector : PropertySelector
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();
    private readonly InjectPropertyTypes propertyTypes;

    public InjectPropertySelector(InjectPropertyTypes propertyTypes)
    {
      this.propertyTypes = propertyTypes;
    }

    /// <summary>
    /// Determines if the <paramref name="propertyInfo"/> represents an injectable property.
    /// </summary>
    /// <param name="propertyInfo">The <see cref="PropertyInfo"/> that describes the target property.</param>
    /// <returns><b>true</b> if the property is injectable, otherwise <b>false</b>.</returns>
    protected override bool IsInjectable(PropertyInfo propertyInfo)
    {
      InjectAttribute? injectAttribute = propertyInfo.SafeGetCustomAttribute<InjectAttribute>();
      if (injectAttribute == null)
      {
        return false;
      }

      try
      {
        MethodInfo? setMethod = propertyInfo.SetMethod;

        if (setMethod != null && propertyInfo.GetIndexParameters().Length == 0)
        {
          return IsValidPropertyType(setMethod);
        }

        throw new ArgumentException("Cannot inject property as it does not have set/init accessor defined, or is an unsupported property type", propertyInfo.GetFullName());
      }
      catch (FileNotFoundException e)
      {
        AssemblyName? assemblyName = e.FileName != null ? new AssemblyName(e.FileName) : null;
        bool optional = injectAttribute.Optional || assemblyName != null && propertyInfo.ReflectedType?.GetCustomAttribute<ServiceBindingOptionsAttribute>()?.PluginDependencies?.Contains(assemblyName.Name) == true;

        if (optional)
        {
          Log.Debug(e, "Cannot inject optional property {Property} as the referenced assembly could not be loaded", propertyInfo.GetFullName());
          return false;
        }

        throw new FileNotFoundException("Cannot inject property as the referenced assembly could not be loaded", e);
      }
    }

    private bool IsValidPropertyType(MethodBase setMethod)
    {
      return propertyTypes == InjectPropertyTypes.InstanceOnly && !setMethod.IsStatic || propertyTypes == InjectPropertyTypes.StaticOnly && setMethod.IsStatic;
    }
  }
}
