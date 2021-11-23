using NWN.Core;

namespace Anvil.API.Events
{
  public sealed class EffectRunScriptEvent : IEvent
  {
    public NwObject EffectTarget { get; }

    public Effect Effect { get; }

    public EffectRunScriptType EventType { get; }

    NwObject IEvent.Context
    {
      get => EffectTarget;
    }

    public EffectRunScriptEvent()
    {
      EffectTarget = NWScript.OBJECT_SELF.ToNwObject();
      Effect = NWScript.GetLastRunScriptEffect();
      EventType = (EffectRunScriptType)NWScript.GetLastRunScriptEffectScriptType();
    }
  }
}
