using NWM.API;
using NWN;

namespace NWNX
{
  [NWNXPlugin(PLUGIN_NAME)]
  internal class NWMInteropPlugin
  {
    public const string PLUGIN_NAME = "NWNX_NWMInterop";

    // This plugin is required.
    static NWMInteropPlugin() => PluginUtils.AssertPluginExists<NWMInteropPlugin>();

    public static InternalObjectType GetObjectType(uint objectId)
    {
      Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "GetObjectType");
      Internal.NativeFunctions.nwnxPushObject(objectId);
      Internal.NativeFunctions.nwnxCallFunction();
      return (InternalObjectType) Internal.NativeFunctions.nwnxPopInt();
    }

    public static string GetUserDirectory()
    {
      Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "GetUserDirectory");
      Internal.NativeFunctions.nwnxCallFunction();
      return Internal.NativeFunctions.nwnxPopString();
    }

    public static bool PluginExists(string pluginName)
    {
      Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "PluginExists");
      Internal.NativeFunctions.nwnxPushString(pluginName);
      Internal.NativeFunctions.nwnxCallFunction();
      return Internal.NativeFunctions.nwnxPopInt().ToBool();
    }
  }

  internal enum InternalObjectType
  {
    Invalid = -1,
    GUI = 1,
    Tile = 2,
    Module = 3,
    Area = 4,
    Creature = 5,
    Item = 6,
    Trigger = 7,
    Projectile = 8,
    Placeable = 9,
    Door = 10,
    AreaOfEffect = 11,
    Waypoint = 12,
    Encounter = 13,
    Store = 14,
    Portal = 15,
    Sound = 16,
  }
}