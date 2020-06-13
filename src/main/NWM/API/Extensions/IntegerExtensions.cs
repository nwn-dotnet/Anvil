using System;
using NWN.Core;

namespace NWM.API
{
  public static class IntegerExtensions
  {
    public static bool ToBool(this int value)
    {
      return value == NWScript.TRUE;
    }

    public static int ToInt(this bool value)
    {
      return value ? NWScript.TRUE : NWScript.FALSE;
    }

    public static T ToNwObjectSafe<T>(this uint objectId) where T : NwObject
    {
      return NwObjectFactory.CreateInternal(objectId) as T;
    }

    public static Lazy<NwObject> ToNwObjectLazy(this uint objectId)
    {
      return new Lazy<NwObject>(() => ToNwObject(objectId));
    }

    public static T ToNwObject<T>(this uint objectId) where T : NwObject
    {
      return (T) NwObjectFactory.CreateInternal(objectId);
    }

    public static NwObject ToNwObject(this uint objectId)
    {
      return NwObjectFactory.CreateInternal(objectId);
    }
  }
}