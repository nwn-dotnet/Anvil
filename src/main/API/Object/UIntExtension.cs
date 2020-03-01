using System;

namespace NWM.API
{
  public static class UIntExtension
  {
    public static T ToNwObjectSafe<T>(this uint objectId) where T : NwObject
    {
      return NwObject.CreateInternal(objectId) as T;
    }

    public static Lazy<NwObject> ToNwObjectLazy(this uint objectId)
    {
      return new Lazy<NwObject>(() => ToNwObject(objectId));
    }

    public static T ToNwObject<T>(this uint objectId) where T : NwObject
    {
      return (T) NwObject.CreateInternal(objectId);
    }

    public static NwObject ToNwObject(this uint objectId)
    {
      return NwObject.CreateInternal(objectId);
    }
  }
}