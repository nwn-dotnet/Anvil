namespace NWM.API
{
  public enum CreatureEventType
  {
    [DefaultScriptSuffix("blo")] Blocked,
    [DefaultScriptSuffix("com")] CombatRoundEnd,
    [DefaultScriptSuffix("con")] Conversation,
    [DefaultScriptSuffix("dam")] Damaged,
    [DefaultScriptSuffix("dea")] Death,
    [DefaultScriptSuffix("dis")] Disturbed,
    [DefaultScriptSuffix("hea")] Heartbeat,
    [DefaultScriptSuffix("per")] Perception,
    [DefaultScriptSuffix("phy")] PhysicalAttacked,
    [DefaultScriptSuffix("res")] Rested,
    [DefaultScriptSuffix("spa")] Spawn,
    [DefaultScriptSuffix("spe")] SpellCastAt,
    [DefaultScriptSuffix("use")] UserDefined
  }
}