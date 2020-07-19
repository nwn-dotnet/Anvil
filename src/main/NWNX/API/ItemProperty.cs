using NWN.Core.NWNX;

namespace NWNX.API
{
  public static class ItemProperty
  {
    static ItemProperty()
    {
      PluginUtils.AssertPluginExists<ItempropPlugin>();
    }
  }
}