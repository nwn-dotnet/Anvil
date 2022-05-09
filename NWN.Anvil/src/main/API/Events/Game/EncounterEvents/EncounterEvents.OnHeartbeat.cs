using System;
using Anvil.API.Events;
using NWN.Core;

namespace Anvil.API.Events
{
  /// <summary>
  /// Events for <see cref="NwEncounter"/> triggers.
  /// </summary>
  public static partial class EncounterEvents
  {
    [GameEvent(EventScriptType.EncounterOnHeartbeat)]
    public sealed class OnHeartbeat : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwEncounter"/> associated with this heartbeat event.
      /// </summary>
      public NwEncounter Encounter { get; } = NWScript.OBJECT_SELF.ToNwObject<NwEncounter>()!;

      NwObject? IEvent.Context => Encounter;
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwEncounter
  {
    /// <inheritdoc cref="EncounterEvents.OnHeartbeat"/>
    public event Action<EncounterEvents.OnHeartbeat> OnHeartbeat
    {
      add => EventService.Subscribe<EncounterEvents.OnHeartbeat, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<EncounterEvents.OnHeartbeat, GameEventFactory>(this, value);
    }
  }
}
