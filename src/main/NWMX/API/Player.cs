using NWM;
using NWM.API;
using NWNX;

namespace NWMX.API
{
  public static class Player
  {
    static Player()
    {
      PluginUtils.AssertPluginExists<PlayerPlugin>();
    }

    public static void SetQuickBarSlot(this NwPlayer player, int slot, QuickBarSlot data)
    {
      PlayerPlugin.SetQuickBarSlot(player, slot, data);
    }

    public static QuickBarSlot GetQuickBarSlot(this NwPlayer player, int slot)
    {
      return PlayerPlugin.GetQuickBarSlot(player, slot);
    }

    public static void ForceOpenInventory(this NwPlayer player, NwPlaceable target)
    {
      PlayerPlugin.ForcePlaceableInventoryWindow(player, target);
    }

    public static void SetPlaceableNameOverride(this NwPlayer player, NwPlaceable placeable, string name)
    {
      PlayerPlugin.SetPlaceableNameOverride(player, placeable, name);
    }

    public static void SetRestDuration(this NwPlayer player, int durationMs)
    {
      PlayerPlugin.SetRestDuration(player, durationMs);
    }
  }
}