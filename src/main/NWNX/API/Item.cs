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
  }
}