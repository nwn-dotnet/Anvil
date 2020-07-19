using NWN.Core.NWNX;

namespace NWNX.API
{
  public static class Visibility
  {
    static Visibility()
    {
      PluginUtils.AssertPluginExists<VisibilityPlugin>();
    }
  }
}