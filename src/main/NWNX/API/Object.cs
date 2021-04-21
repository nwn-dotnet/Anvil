using System;
using System.Numerics;
using NWN.API;
using NWN.API.Constants;
using NWN.Core.NWNX;

namespace NWNX.API
{
  public static class Object
  {
    static Object()
    {
      PluginUtils.AssertPluginExists<ObjectPlugin>();
    }

    [Obsolete("Use GameObject.Location instead.")]
    public static void AddToArea(this NwGameObject gameObject, Location location)
    {
      ObjectPlugin.AddToArea(gameObject, location.Area, location.Position);
    }

    [Obsolete("Use GameObject.Location instead.")]
    public static void AddToArea(this NwGameObject gameObject, NwArea area, Vector3 position)
    {
      ObjectPlugin.AddToArea(gameObject, area, position);
    }

    [Obsolete("Use Inventory.CheckFit instead.")]
    public static bool CheckFit(this NwGameObject gameObject, NwItem item)
    {
      return CheckFit(gameObject, item.BaseItemType);
    }

    [Obsolete("Use Inventory.CheckFit instead.")]
    public static bool CheckFit(this NwGameObject gameObject, BaseItemType baseItemType)
    {
      return ObjectPlugin.CheckFit(gameObject, (int) baseItemType).ToBool();
    }

    public static string GetPersistentString(this NwObject obj, string key)
    {
      return ObjectPlugin.GetString(obj, key);
    }

    public static void SetPersistentString(this NwObject obj, string key, string value)
    {
      ObjectPlugin.SetString(obj, key, value, true.ToInt());
    }

    [Obsolete("Use NwPlaceable.IsStatic instead.")]
    public static bool GetIsStatic(this NwPlaceable placeable)
    {
      return ObjectPlugin.GetPlaceableIsStatic(placeable).ToBool();
    }

    [Obsolete("Use NwPlaceable.IsStatic instead.")]
    public static void SetIsStatic(this NwPlaceable placeable, bool value)
    {
      ObjectPlugin.SetPlaceableIsStatic(placeable, value.ToInt());
    }

    [Obsolete("Use NwObject.PeekUUID instead.")]
    public static string PeekUUID(this NwObject obj)
    {
      return ObjectPlugin.PeekUUID(obj);
    }

    [Obsolete("Use NwXXX.Serialize instead.")]
    public static string Serialize(this NwObject obj)
    {
      return ObjectPlugin.Serialize(obj);
    }
  }
}
