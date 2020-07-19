using NWN.Core.NWNX;

namespace NWNX.API
{
  public static class Reveal
  {
    static Reveal()
    {
      PluginUtils.AssertPluginExists<RevealPlugin>();
    }
  }
}