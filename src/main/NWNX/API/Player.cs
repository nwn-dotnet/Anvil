using NWN.API;
using NWN.Core.NWNX;

namespace NWNX.API
{
  // TODO implement plugin functions
  public static class Player
  {
    static Player()
    {
      PluginUtils.AssertPluginExists<PlayerPlugin>();
    }

    public static void SetPlaceableNameOverride(this NwPlayer player, NwPlaceable placeable, string name)
    {
      PlayerPlugin.SetPlaceableNameOverride(player.ControlledCreature, placeable, name);
    }

    public static void SetRestDuration(this NwPlayer player, int durationMs)
    {
      PlayerPlugin.SetRestDuration(player.ControlledCreature, durationMs);
    }
  }
}
