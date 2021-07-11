using System;
using NWN.Native.API;

namespace NWN.API
{
  public static class NativeObjectExtensions
  {
    public static T ToNwObjectSafe<T>(this ICGameObject gameObject) where T : NwObject
    {
      return NwObject.CreateInternal(gameObject) as T;
    }

    public static Lazy<NwObject> ToNwObjectLazy(this ICGameObject gameObject)
    {
      return new Lazy<NwObject>(() => ToNwObject(gameObject));
    }

    public static T ToNwObject<T>(this ICGameObject gameObject) where T : NwObject
    {
      return (T)NwObject.CreateInternal(gameObject);
    }

    public static NwObject ToNwObject(this ICGameObject gameObject)
    {
      return NwObject.CreateInternal(gameObject);
    }

    public static NwPlayer ToNwPlayer(this CNWSPlayer player)
    {
      return player != null && player.Pointer != IntPtr.Zero ? new NwPlayer(player) : null;
    }
  }
}
