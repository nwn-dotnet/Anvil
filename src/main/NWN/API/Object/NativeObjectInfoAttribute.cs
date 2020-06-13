using System;
using NWN.API.Constants;
using NWNX.API.Constants;

namespace NWN.API
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