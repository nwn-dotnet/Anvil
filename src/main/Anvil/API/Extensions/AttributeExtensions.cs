using System;

namespace Anvil.API
{
  public static class AttributeExtensions
  {
    public static T[] GetCustomAttributes<T>(this Type type, bool inherit = true) where T : Attribute
    {
      return (T[])type.GetCustomAttributes(typeof(T), inherit);
    }
  }
}
