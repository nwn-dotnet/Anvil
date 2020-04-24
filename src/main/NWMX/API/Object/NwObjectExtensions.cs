using NWM.API;
using NWNX;

namespace NWMX.API
{
  public static class NwObjectExtensions
  {
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