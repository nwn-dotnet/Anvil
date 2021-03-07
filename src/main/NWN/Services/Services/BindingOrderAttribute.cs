using System;

namespace NWN.Services
{
  [AttributeUsage(AttributeTargets.Class)]
  internal class BindingOrderAttribute : Attribute
  {
    public readonly BindingOrder Order;

    public BindingOrderAttribute(BindingOrder order)
    {
      this.Order = order;
    }
  }
}
