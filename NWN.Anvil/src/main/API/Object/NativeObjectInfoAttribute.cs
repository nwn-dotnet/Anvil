using System;
using NWN.Native.API;

namespace Anvil.API
{
  [AttributeUsage(AttributeTargets.Class)]
  internal sealed class NativeObjectInfoAttribute : Attribute
  {
    public readonly ObjectType NativeObjectType;
    public readonly ObjectTypes ObjectType;

    public NativeObjectInfoAttribute(ObjectTypes objectType, ObjectType nativeObjectType)
    {
      ObjectType = objectType;
      NativeObjectType = nativeObjectType;
    }
  }
}
