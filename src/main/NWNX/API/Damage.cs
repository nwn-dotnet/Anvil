using NWN.Core.NWNX;

namespace NWNX.API
{
  public static class Damage
  {
    static Damage()
    {
      PluginUtils.AssertPluginExists<DamagePlugin>();
    }
  }
}