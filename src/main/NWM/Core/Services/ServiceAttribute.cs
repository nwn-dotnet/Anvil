using System;
using JetBrains.Annotations;

namespace NWM.Core
{
  [AttributeUsage(AttributeTargets.Class)]
  [MeansImplicitUse]
  public class ServiceAttribute : Attribute
  {
    public bool BindSelf = true;
    public bool IsCollection = false;

    public BindingType BindingType = BindingType.Singleton;
    public Type[] BindFrom;

    public ServiceAttribute(params Type[] bindFrom)
    {
      BindFrom = bindFrom;
    }
  }
}