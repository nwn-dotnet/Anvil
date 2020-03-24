namespace NWM.API
{
  public enum ModuleEventType
  {
    [DefaultScriptSuffix("acq_ite")] AcquireItem,
    [DefaultScriptSuffix("act_ite")] ActivateItem,
    [DefaultScriptSuffix("cli_ent")] ClientEnter,
    [DefaultScriptSuffix("cli_lea")] ClientLeave,
    [DefaultScriptSuffix("cut_abo")] CutsceneAbort,
    [DefaultScriptSuffix("mod_hea")] Heartbeat,
    [DefaultScriptSuffix("mod_loa")] ModuleLoad,
    [DefaultScriptSuffix("pla_cha")] PlayerChat,
    [DefaultScriptSuffix("pla_dea")] PlayerDeath,
    [DefaultScriptSuffix("pla_dyi")] PlayerDying,
    [DefaultScriptSuffix("pla_equ")] PlayerEquipItem,
    [DefaultScriptSuffix("pla_lev")] PlayerLevelUp,
    [DefaultScriptSuffix("pla_resp")] PlayerRespawn,
    [DefaultScriptSuffix("pla_rest")] PlayerRest,
    [DefaultScriptSuffix("pla_uneq")] PlayerUnEquipItem,
    [DefaultScriptSuffix("una_ite")] UnAcquireItem,
    [DefaultScriptSuffix("use")] UserDefined
  }
}