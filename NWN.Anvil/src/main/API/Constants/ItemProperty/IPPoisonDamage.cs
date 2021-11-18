using NWN.Core;

namespace Anvil.API
{
  public enum IPPoisonDamage
  {
    Strength1d2 = NWScript.IP_CONST_POISON_1D2_STRDAMAGE,
    Dexterity1d2 = NWScript.IP_CONST_POISON_1D2_DEXDAMAGE,
    Constitution1d2 = NWScript.IP_CONST_POISON_1D2_CONDAMAGE,
    Intelligence1d2 = NWScript.IP_CONST_POISON_1D2_INTDAMAGE,
    Wisdom1d2 = NWScript.IP_CONST_POISON_1D2_WISDAMAGE,
    Charisma1d2 = NWScript.IP_CONST_POISON_1D2_CHADAMAGE,
  }
}
