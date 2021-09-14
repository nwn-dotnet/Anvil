using NWN.Core;

namespace Anvil.API
{
  public enum EffectRunScriptType
  {
    OnApplied = NWScript.RUNSCRIPT_EFFECT_SCRIPT_TYPE_ON_APPLIED,
    OnRemoved = NWScript.RUNSCRIPT_EFFECT_SCRIPT_TYPE_ON_REMOVED,
    OnInterval = NWScript.RUNSCRIPT_EFFECT_SCRIPT_TYPE_ON_INTERVAL,
  }
}
