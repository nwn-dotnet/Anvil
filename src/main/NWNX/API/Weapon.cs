using NWN.Core.NWNX;

namespace NWNX.API
{
  // TODO implement plugin functions
  public static class Weapon
  {
    static Weapon()
    {
      PluginUtils.AssertPluginExists<WeaponPlugin>();
    }
  }
}