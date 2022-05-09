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
    [GameEvent(EventScriptType.TriggerOnHeartbeat)]
    public sealed class OnHeartbeat : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwTrigger"/> associated with this heartbeat event.
      /// </summary>
      public NwTrigger Trigger { get; } = NWScript.OBJECT_SELF.ToNwObject<NwTrigger>()!;

      NwObject? IEvent.Context => Trigger;
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwTrigger
  {
    /// <inheritdoc cref="TriggerEvents.OnHeartbeat"/>
    public event Action<TriggerEvents.OnHeartbeat> OnHeartbeat
    {
      add => EventService.Subscribe<TriggerEvents.OnHeartbeat, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<TriggerEvents.OnHeartbeat, GameEventFactory>(this, value);
    }
  }
}
