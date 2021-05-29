using System;
using NWN.API.Constants;
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
      this.ObjectType = objectType;
      this.NativeObjectType = nativeObjectType;
    }
  }
}
