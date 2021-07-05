using System.Reflection;
using LightInject;
using NLog;
using NWN.API;

namespace NWN.Services
{
  public class InjectPropertySelector : PropertySelector
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Determines if the <paramref name="propertyInfo"/> represents an injectable property.
    /// </summary>
    /// <param name="propertyInfo">The <see cref="PropertyInfo"/> that describes the target property.</param>
    /// <returns><b>true</b> if the property is injectable, otherwise <b>false</b>.</returns>
    protected override bool IsInjectable(PropertyInfo propertyInfo)
    {
      bool retVal = propertyInfo.SetMethod != null && propertyInfo.GetIndexParameters().Length == 0;
      bool hasAttribute = propertyInfo.IsDefined(typeof(InjectAttribute), true);

      if (!retVal && hasAttribute)
      {
        Log.Error($"Cannot inject property \"{propertyInfo.GetFullName()}\" as it does not have set/init defined, or is an unsupported property type.");
      }

      return retVal && hasAttribute;
    }
  }
}
