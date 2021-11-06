using System;

namespace Anvil.API
{
  public static class AttributeExtensions
  {
    [Obsolete("Use ReflectionExtensions.GetCustomAttributes instead.")]
    public static T[] GetCustomAttributes<T>(Type type, bool inherit = true) where T : Attribute
    {
      return (T[])type.GetCustomAttributes(typeof(T), inherit);
    }
  }
}
