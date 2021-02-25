using NWN.API.Constants;
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

    /// <summary>
    /// Set baseItem to be a Monk weapon.
    /// </summary>
    /// <param name="baseItem">BaseItemType for monk weapon.</param>
    public static void SetWeaponIsMonkWeapon(this BaseItemType baseItem) => WeaponPlugin.SetWeaponIsMonkWeapon((int)baseItem);

    /// <summary>
    /// Set weapon base item to be considered as unarmed for weapon finesse feat.
    /// </summary>
    /// <param name="baseItem">BaseItemType to be applied.</param>
    public static void SetWeaponUnarmed(this BaseItemType baseItem) => WeaponPlugin.SetWeaponUnarmed((int)baseItem);
  }
}
