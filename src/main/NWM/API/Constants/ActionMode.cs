using NWN.Core;

namespace NWM.API.Constants
{
  public enum ActionMode
  {
    Detect = NWScript.ACTION_MODE_DETECT,
    Stealth = NWScript.ACTION_MODE_STEALTH,
    Parry = NWScript.ACTION_MODE_PARRY,
    PowerAttack = NWScript.ACTION_MODE_POWER_ATTACK,
    ImprovedPowerAttack = NWScript.ACTION_MODE_IMPROVED_POWER_ATTACK,
    CounterSpell = NWScript.ACTION_MODE_COUNTERSPELL,
    FlurryOfBlows = NWScript.ACTION_MODE_FLURRY_OF_BLOWS,
    RapidShot = NWScript.ACTION_MODE_RAPID_SHOT,
    Expertise = NWScript.ACTION_MODE_EXPERTISE,
    ImprovedExpertise = NWScript.ACTION_MODE_IMPROVED_EXPERTISE,
    DefensiveCast = NWScript.ACTION_MODE_DEFENSIVE_CAST,
    DirtyFighting = NWScript.ACTION_MODE_DIRTY_FIGHTING
  }
}