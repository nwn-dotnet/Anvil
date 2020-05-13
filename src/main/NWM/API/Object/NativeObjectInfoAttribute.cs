using System;
using NWM.API.Constants;
using NWMX.API.Constants;

namespace NWM.API
{
  [AttributeUsage(AttributeTargets.Class)]
  internal class NativeObjectInfoAttribute : Attribute
  {
    public readonly ObjectType ObjectType;
    public readonly InternalObjectType InternalObjectType;

    public NativeObjectInfoAttribute(ObjectType objectType, InternalObjectType internalObjectType)
    {
      this.ObjectType = objectType;
      this.InternalObjectType = internalObjectType;
    }
  }
}