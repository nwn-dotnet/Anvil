using NWN.Core;

namespace NWN.API.Constants
{
  public enum EventType
  {
    Heartbeat = NWScript.EVENT_HEARTBEAT,
    Perceive = NWScript.EVENT_PERCEIVE,
    EndCombatRound = NWScript.EVENT_END_COMBAT_ROUND,
    Dialogue = NWScript.EVENT_DIALOGUE,
    Attacked = NWScript.EVENT_ATTACKED,
    Damaged = NWScript.EVENT_DAMAGED,
    Disturbed = NWScript.EVENT_DISTURBED,
    SpellCastAt = NWScript.EVENT_SPELL_CAST_AT
  }
}