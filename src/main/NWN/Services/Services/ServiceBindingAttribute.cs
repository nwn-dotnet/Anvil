using System;
using JetBrains.Annotations;

namespace NWN.Services
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
  [MeansImplicitUse]
  public class ServiceBindingAttribute : Attribute
  {
    public readonly BindingType BindingType;
    public readonly Type BindFrom;

    public ServiceBindingAttribute(Type bindFrom, BindingType bindingType = BindingType.Singleton)
    {
      BindFrom = bindFrom;
      BindingType = bindingType;
    }
  }
}