using NWN.Core;

namespace Anvil.API.Events
{
  public sealed class EffectRunScriptEvent : IEvent
  {
    public EffectRunScriptEvent()
    {
      EffectTarget = NWScript.OBJECT_SELF.ToNwObject();
      Effect = NWScript.GetLastRunScriptEffect();
      EventType = (EffectRunScriptType)NWScript.GetLastRunScriptEffectScriptType();
    }

    public Effect? Effect { get; }
    public NwObject? EffectTarget { get; }

    public EffectRunScriptType EventType { get; }

    NwObject? IEvent.Context => EffectTarget;
  }
}
