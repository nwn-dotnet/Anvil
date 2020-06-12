using System.Numerics;
using NWM.API;
using NWM.API.Constants;
using NWNX;

namespace NWMX.API
{
  public static class Object
  {
    static Object()
    {
      PluginUtils.AssertPluginExists<ObjectPlugin>();
    }

    public static void AddToArea(this NwGameObject gameObject, Location location)
    {
      ObjectPlugin.AddToArea(gameObject, location.Area, location.Position);
    }

    public static void AddToArea(this NwGameObject gameObject, NwArea area, Vector3 position)
    {
      ObjectPlugin.AddToArea(gameObject, area, position);
    }

    public static void AcquireItem(this NwGameObject gameObject, NwItem item)
    {
      ObjectPlugin.AcquireItem(gameObject, item);
    }

    public static bool CheckFit(this NwGameObject gameObject, NwItem item)
    {
      return CheckFit(gameObject, item.BaseItemType);
    }

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

    public static bool GetIsStatic(this NwPlaceable placeable)
    {
      return ObjectPlugin.GetPlaceableIsStatic(placeable).ToBool();
    }

    public static void SetIsStatic(this NwPlaceable placeable, bool value)
    {
      ObjectPlugin.SetPlaceableIsStatic(placeable, value.ToInt());
    }
  }
}