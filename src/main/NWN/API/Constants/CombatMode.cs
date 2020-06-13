using NWN.Core;

namespace NWN.API.Constants
{
  public enum CombatMode
  {
    Invalid = NWScript.COMBAT_MODE_INVALID,
    Parry = NWScript.COMBAT_MODE_PARRY,
    PowerAttack = NWScript.COMBAT_MODE_POWER_ATTACK,
    ImprovedPowerAttack = NWScript.COMBAT_MODE_IMPROVED_POWER_ATTACK,
    FlurryOfBlows = NWScript.COMBAT_MODE_FLURRY_OF_BLOWS,
    RapidShot = NWScript.COMBAT_MODE_RAPID_SHOT,
    Expertise = NWScript.COMBAT_MODE_EXPERTISE,
    ImprovedExpertise = NWScript.COMBAT_MODE_IMPROVED_EXPERTISE,
    DefensiveCasting = NWScript.COMBAT_MODE_DEFENSIVE_CASTING,
    DirtyFighting = NWScript.COMBAT_MODE_DIRTY_FIGHTING,
    DefensiveStance = NWScript.COMBAT_MODE_DEFENSIVE_STANCE
  }
}