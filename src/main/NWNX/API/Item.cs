using System;
using NWN.API;
using NWN.Core.NWNX;

namespace NWNX.API
{
  // TODO implement plugin functions
  public static class Item
  {
    static Item()
    {
      PluginUtils.AssertPluginExists<ItemPlugin>();
    }

    [Obsolete("Use NWItem.Weight instead.")]
    public static void SetWeight(this NwItem item, decimal weight)
      => ItemPlugin.SetWeight(item, (int) Math.Round(weight * 10.0m, MidpointRounding.ToZero));
  }
}
