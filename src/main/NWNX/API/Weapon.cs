using NWN.Core.NWNX;

namespace NWNX.API
{
  public static class Weapon
  {
    static Weapon()
    {
      PluginUtils.AssertPluginExists<WeaponPlugin>();
    }
  }
}