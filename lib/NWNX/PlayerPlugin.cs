using NWN;

namespace NWNX
{
  [NWNXPlugin(PLUGIN_NAME)]
  public class PlayerPlugin
  {
    public const string PLUGIN_NAME = "NWNX_Player";

    // Force opens the target object's inventory for the player.
    // A few notes about this function:
    // - If the placeable is in a different area than the player, the portrait will not be shown
    // - The placeable's open/close animations will be played
    // - Clicking the 'close' button will cause the player to walk to the placeable;
    //     If the placeable is in a different area, the player will just walk to the edge
    //     of the current area and stop. This action can be cancelled manually.
    // - Walking will close the placeable automatically.
    public static void ForcePlaceableInventoryWindow(uint player, uint placeable)
    {
      Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "ForcePlaceableInventoryWindow");
      Internal.NativeFunctions.nwnxPushObject(placeable);
      Internal.NativeFunctions.nwnxPushObject(player);
      Internal.NativeFunctions.nwnxCallFunction();
    }

    // Override the name of placeable for player only
    // "" to clear the override
    public static void SetPlaceableNameOverride(uint player, uint placeable, string name)
    {
      Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "SetPlaceableNameOverride");
      Internal.NativeFunctions.nwnxPushString(name);
      Internal.NativeFunctions.nwnxPushObject(placeable);
      Internal.NativeFunctions.nwnxPushObject(player);
      Internal.NativeFunctions.nwnxCallFunction();
    }
  }
}