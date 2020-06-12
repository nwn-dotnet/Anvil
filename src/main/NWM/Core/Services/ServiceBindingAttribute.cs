using System;
using JetBrains.Annotations;

namespace NWM.Core
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