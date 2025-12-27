using System;
using Anvil.API.Events;
using NWN.Core;

namespace Anvil.API.Events
{
  /// <summary>
  /// Built-in events associated with a specific encounter.
  /// </summary>
  public static partial class EncounterEvents
  {
    /// <summary>
    /// Triggered for an encounter when the associated <see cref="Signal"/> method is called.
    /// </summary>
    /// <seealso cref="Signal"/>
    [GameEvent(EventScriptType.EncounterOnUserDefinedEvent)]
    public sealed class OnUserDefined : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwEncounter"/> associated with this user defined event.
      /// </summary>
      public NwEncounter Encounter { get; } = NWScript.OBJECT_SELF.ToNwObject<NwEncounter>()!;

      NwObject IEvent.Context => Encounter;

      /// <summary>
      /// Signals a user-defined event with the given ID to an encounter.
      /// </summary>
      /// <param name="encounter">The encounter to receive the event.</param>
      /// <param name="eventId">The integer event identifier to deliver.</param>
      public static void Signal(NwEncounter encounter, int eventId)
      {
        Event nwEvent = NWScript.EventUserDefined(eventId)!;
        NWScript.SignalEvent(encounter, nwEvent);
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwEncounter
  {
    /// <inheritdoc cref="EncounterEvents.OnUserDefined"/>
    public event Action<EncounterEvents.OnUserDefined> OnUserDefined
    {
      add => EventService.Subscribe<EncounterEvents.OnUserDefined, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<EncounterEvents.OnUserDefined, GameEventFactory>(this, value);
    }
  }
}
