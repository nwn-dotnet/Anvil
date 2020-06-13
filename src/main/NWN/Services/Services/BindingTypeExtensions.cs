using System;
using SimpleInjector;

namespace NWN.Services
{
  public static class BindingTypeExtensions
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