using System;

namespace Anvil.API
{
  [AttributeUsage(AttributeTargets.Class, Inherited = false)]
  internal sealed class ObjectFilterAttribute : Attribute
  {
    public readonly ObjectTypes ObjectFilter;

    public ObjectFilterAttribute(ObjectTypes objectFilter)
    {
      ObjectFilter = objectFilter;
    }
  }
}
