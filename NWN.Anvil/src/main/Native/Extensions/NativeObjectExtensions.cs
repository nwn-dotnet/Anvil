using System;
using Anvil.API;
using NWN.Native.API;
using ItemProperty = Anvil.API.ItemProperty;

namespace Anvil.Native
{
  internal static class NativeObjectExtensions
  {
    public static Effect? ToEffect(this CGameEffect? effect, bool memoryOwn)
    {
      Effect? retVal = effect != null && effect.Pointer != IntPtr.Zero ? new Effect(effect, memoryOwn) : null;
      return retVal;
    }

    public static ItemProperty? ToItemProperty(this CGameEffect? ipEffect, bool memoryOwn)
    {
      return ipEffect != null && ipEffect.Pointer != IntPtr.Zero ? new ItemProperty(ipEffect, memoryOwn) : null;
    }

    public static T? ToNwObject<T>(this ICGameObject gameObject) where T : NwObject
    {
      return (T?)NwObject.CreateInternal(gameObject);
    }

    public static NwObject? ToNwObject(this ICGameObject gameObject)
    {
      return NwObject.CreateInternal(gameObject);
    }

    public static Lazy<NwObject?> ToNwObjectLazy(this ICGameObject gameObject)
    {
      return new Lazy<NwObject?>(() => ToNwObject(gameObject));
    }

    public static T? ToNwObjectSafe<T>(this ICGameObject gameObject) where T : NwObject
    {
      return NwObject.CreateInternal(gameObject) as T;
    }

    public static NwPlayer? ToNwPlayer(this CNWSPlayer? player)
    {
      return player != null && player.Pointer != IntPtr.Zero ? new NwPlayer(player) : null;
    }
  }
}
