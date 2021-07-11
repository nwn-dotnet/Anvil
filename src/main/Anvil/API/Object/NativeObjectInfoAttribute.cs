using System;
using Anvil.API;
using NWN.Native.API;

namespace NWN.API
{
  [AttributeUsage(AttributeTargets.Class)]
  internal sealed class NativeObjectInfoAttribute : Attribute
  {
    public readonly ObjectTypes ObjectType;
    public readonly ObjectType NativeObjectType;

    public NativeObjectInfoAttribute(ObjectTypes objectType, ObjectType nativeObjectType)
    {
      ObjectType = objectType;
      NativeObjectType = nativeObjectType;
    }
  }
}
