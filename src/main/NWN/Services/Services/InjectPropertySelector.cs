using System.Reflection;
using LightInject;
using NLog;
using NWN.API;

namespace NWN.Services
{
  public sealed class InjectPropertySelector : PropertySelector
  {
    private readonly InjectPropertyTypes propertyTypes;
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

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
      MethodInfo setMethod = propertyInfo.SetMethod;
      bool isValid = setMethod != null && propertyInfo.GetIndexParameters().Length == 0;
      bool hasAttribute = propertyInfo.IsDefined(typeof(InjectAttribute), true);

      if (!isValid && hasAttribute)
      {
        Log.Error($"Cannot inject property \"{propertyInfo.GetFullName()}\" as it does not have set/init defined, or is an unsupported property type.");
      }

      return isValid && hasAttribute && IsValidPropertyType(setMethod);
    }

    private bool IsValidPropertyType(MethodBase setMethod)
    {
      return propertyTypes == InjectPropertyTypes.InstanceOnly && !setMethod.IsStatic || propertyTypes == InjectPropertyTypes.StaticOnly && setMethod.IsStatic;
    }
  }
}
