using NWN.Core.NWNX;

namespace NWNX.API
{
  // TODO implement plugin functions
  public static class Damage
  {
    static Damage()
    {
      PluginUtils.AssertPluginExists<DamagePlugin>();
    }
  }
}
