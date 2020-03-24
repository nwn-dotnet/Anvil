namespace NWM.API
{
  public enum AreaEventType
  {
    [DefaultScriptSuffix("ent")] Enter,
    [DefaultScriptSuffix("exi")] Exit,
    [DefaultScriptSuffix("hea")] Heartbeat,
    [DefaultScriptSuffix("use")] UserDefined
  }
}