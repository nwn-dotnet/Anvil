using System;

namespace Anvil.API
{
  [AttributeUsage(AttributeTargets.Class)]
  internal sealed class ObjectTypeAttribute(ObjectTypes objectType) : Attribute
  {
    public readonly ObjectTypes ObjectType = objectType;
  }
}
