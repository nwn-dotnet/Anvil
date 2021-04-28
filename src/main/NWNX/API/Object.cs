using NWN.API;
using NWN.Core.NWNX;

namespace NWNX.API
{
  public static class Object
  {
    static Object()
    {
      PluginUtils.AssertPluginExists<ObjectPlugin>();
    }

    public static string GetPersistentString(this NwObject obj, string key)
    {
      return ObjectPlugin.GetString(obj, key);
    }

    public static void SetPersistentString(this NwObject obj, string key, string value)
    {
      ObjectPlugin.SetString(obj, key, value, true.ToInt());
    }
  }
}
