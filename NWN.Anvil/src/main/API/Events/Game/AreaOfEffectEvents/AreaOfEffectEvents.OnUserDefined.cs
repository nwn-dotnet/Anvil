using System;
using Anvil.API.Events;
using NWN.Core;

namespace Anvil.API.Events
{
  /// <summary>
  /// Events for effects created with <see cref="Effect.AreaOfEffect(Anvil.API.PersistentVfxType,Anvil.Services.ScriptCallbackHandle,Anvil.Services.ScriptCallbackHandle,Anvil.Services.ScriptCallbackHandle)"/>.
  /// </summary>
  public static partial class AreaOfEffectEvents
  {
    [GameEvent(EventScriptType.AreaOfEffectOnUserDefinedEvent)]
    public sealed class OnUserDefined : IEvent
    {
      public NwAreaOfEffect Effect { get; } = NWScript.OBJECT_SELF.ToNwObject<NwAreaOfEffect>();

      NwObject IEvent.Context
      {
        get => Effect;
      }

      public static void Signal(NwAreaOfEffect areaOfEffect, int eventId)
      {
        Event nwEvent = NWScript.EventUserDefined(eventId);
        NWScript.SignalEvent(areaOfEffect, nwEvent);
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwAreaOfEffect
  {
    /// <inheritdoc cref="AreaOfEffectEvents.OnUserDefined"/>
    public event Action<AreaOfEffectEvents.OnUserDefined> OnUserDefined
    {
      add => EventService.Subscribe<AreaOfEffectEvents.OnUserDefined, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<AreaOfEffectEvents.OnUserDefined, GameEventFactory>(this, value);
    }
  }
}
