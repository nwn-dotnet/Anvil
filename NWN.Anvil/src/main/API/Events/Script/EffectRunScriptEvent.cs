using NWN.Core;

namespace Anvil.API.Events
{
  public sealed class EffectRunScriptEvent : IEvent
  {
    public Effect? Effect { get; } = NWScript.GetLastRunScriptEffect();
    public NwObject? EffectTarget { get; } = NWScript.OBJECT_SELF.ToNwObject();

    public EffectRunScriptType EventType { get; } = (EffectRunScriptType)NWScript.GetLastRunScriptEffectScriptType();

    NwObject? IEvent.Context => EffectTarget;
  }
}
