using System;

namespace Anvil.API
{
  [AttributeUsage(AttributeTargets.Class, Inherited = false)]
  internal sealed class ObjectFilterAttribute(ObjectTypes objectFilter) : Attribute
  {
    public readonly ObjectTypes ObjectFilter = objectFilter;
  }
}
