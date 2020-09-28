using System;
using JetBrains.Annotations;

namespace NWN.Services
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
  [MeansImplicitUse]
  public class ServiceBindingAttribute : Attribute
  {
    public readonly Type BindFrom;
    public readonly BindingType BindingType;
    internal readonly BindingContext BindingContext;

    public ServiceBindingAttribute(Type bindFrom, BindingType bindingType = BindingType.Singleton)
      : this(bindFrom, BindingContext.Service, bindingType) {}

    internal ServiceBindingAttribute(Type bindFrom, BindingContext bindingContext)
    {
      this.BindFrom = bindFrom;
      this.BindingContext = bindingContext;
      this.BindingType = BindingType.Singleton;
    }

    private ServiceBindingAttribute(Type bindFrom, BindingContext bindingContext, BindingType bindingType = BindingType.Singleton)
    {
      this.BindFrom = bindFrom;
      this.BindingContext = bindingContext;
      this.BindingType = bindingType;
    }
  }
}
