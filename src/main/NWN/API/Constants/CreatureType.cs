using NWN.Core;

namespace NWN.API.Constants
{
  public enum CreatureType
  {
    None = -1,
    RacialType = NWScript.CREATURE_TYPE_RACIAL_TYPE,
    PlayerChar = NWScript.CREATURE_TYPE_PLAYER_CHAR,
    Class = NWScript.CREATURE_TYPE_CLASS,
    Reputation = NWScript.CREATURE_TYPE_REPUTATION,
    IsAlive = NWScript.CREATURE_TYPE_IS_ALIVE,
    HasSpellEffect = NWScript.CREATURE_TYPE_HAS_SPELL_EFFECT,
    DoesNotHaveSpellEffect = NWScript.CREATURE_TYPE_DOES_NOT_HAVE_SPELL_EFFECT,
    Perception = NWScript.CREATURE_TYPE_PERCEPTION
  }
}