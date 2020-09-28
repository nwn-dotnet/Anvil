using System;
using SimpleInjector;

namespace NWN.Services
{
  internal static class BindingTypeExtensions
  {
    public static Lifestyle ToLifestyle(this BindingType bindingType)
    {
      return bindingType switch
      {
        BindingType.Singleton => Lifestyle.Singleton,
        BindingType.Transient => Lifestyle.Transient,
        _ => throw new ArgumentOutOfRangeException()
      };
    }
  }
}
