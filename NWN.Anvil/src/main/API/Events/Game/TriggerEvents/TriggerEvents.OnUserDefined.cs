using System;
using Anvil.API.Events;
using NWN.Core;

namespace Anvil.API.Events
{
  /// <summary>
  /// Events for drawn, world-placed triggers.
  /// </summary>
  public static partial class TriggerEvents
  {
    [GameEvent(EventScriptType.TriggerOnUserDefinedEvent)]
    public sealed class OnUserDefined : IEvent
    {
      /// <summary>
      /// Gets the specific event number used to trigger this user-defined event.
      /// </summary>
      public int EventNumber { get; } = NWScript.GetUserDefinedEventNumber();

      /// <summary>
      /// Gets the <see cref="NwTrigger"/> that is running a user defined event.
      /// </summary>
      public NwTrigger Trigger { get; } = NWScript.OBJECT_SELF.ToNwObject<NwTrigger>()!;

      NwObject? IEvent.Context => Trigger;

      public static void Signal(NwTrigger trigger, int eventId)
      {
        Event nwEvent = NWScript.EventUserDefined(eventId)!;
        NWScript.SignalEvent(trigger, nwEvent);
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwTrigger
  {
    /// <inheritdoc cref="TriggerEvents.OnUserDefined"/>
    public event Action<TriggerEvents.OnUserDefined> OnUserDefined
    {
      add => EventService.Subscribe<TriggerEvents.OnUserDefined, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<TriggerEvents.OnUserDefined, GameEventFactory>(this, value);
    }
  }
}
