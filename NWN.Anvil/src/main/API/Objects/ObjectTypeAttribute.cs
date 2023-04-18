using System;

namespace Anvil.API
{
  [AttributeUsage(AttributeTargets.Class)]
  internal sealed class ObjectTypeAttribute : Attribute
  {
    public readonly ObjectTypes ObjectType;

    public ObjectTypeAttribute(ObjectTypes objectType)
    {
      ObjectType = objectType;
    }
  }
}
