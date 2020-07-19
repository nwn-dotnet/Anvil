using NWN.Core.NWNX;

namespace NWNX.API
{
  public static class Appearance
  {
    static Appearance()
    {
      PluginUtils.AssertPluginExists<AppearancePlugin>();
    }
  }
}