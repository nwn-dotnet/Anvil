namespace NWM.Internal
{
  internal class NWMInterop
  {
    private const string PLUGIN_NAME = "NWNX_NWMInterop";

    public static ObjectType GetObjectType(uint objectId)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "GetObjectType");
      NWN.Internal.NativeFunctions.nwnxPushObject(objectId);
      NWN.Internal.NativeFunctions.nwnxCallFunction();

      return (ObjectType) NWN.Internal.NativeFunctions.nwnxPopInt();
    }

    public static string GetUserDirectory()
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "GetUserDirectory");
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return NWN.Internal.NativeFunctions.nwnxPopString();
    }
  }

  public enum ObjectType
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