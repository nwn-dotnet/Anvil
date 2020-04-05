using System;

namespace NWM.API
{
  public static class GuidExtension
  {
    public static T ToNwObjectSafe<T>(this Guid objectId) where T : NwObject
    {
      return NwObject.CreateInternal(objectId) as T;
    }

    public static Lazy<NwObject> ToNwObjectLazy(this Guid objectId)
    {
      return new Lazy<NwObject>(() => ToNwObject(objectId));
    }

    public static T ToNwObject<T>(this Guid objectId) where T : NwObject
    {
      return (T) NwObject.CreateInternal(objectId);
    }

    public static NwObject ToNwObject(this Guid objectId)
    {
      return NwObject.CreateInternal(objectId);
    }

    public static string ToUUIDString(this Guid guid)
    {
      return guid.ToString("D");
    }
  }
}