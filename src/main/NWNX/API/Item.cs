using NWN.Core.NWNX;

namespace NWNX.API
{
  public static class Item
  {
    static Item()
    {
      PluginUtils.AssertPluginExists<ItemPlugin>();
    }
  }
}