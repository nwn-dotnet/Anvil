using NWN.API.Constants;
using NWN.Core;

// TODO Populate event data.
namespace NWN.API.Events
{
  /// <summary>
  /// Events for <see cref="NwEncounter"/> triggers.
  /// </summary>
  public static class EncounterEvents
  {
    [GameEvent(EventScriptType.EncounterOnObjectEnter)]
    public sealed class OnEnter : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwEncounter"/> that was entered.
      /// </summary>
      public NwEncounter Encounter { get; } = NWScript.OBJECT_SELF.ToNwObject<NwEncounter>();

      NwObject IEvent.Context
      {
        get => Encounter;
      }
    }

    [GameEvent(EventScriptType.EncounterOnObjectExit)]
    public sealed class OnExit : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwEncounter"/> that was exited.
      /// </summary>
      public NwEncounter Encounter { get; } = NWScript.OBJECT_SELF.ToNwObject<NwEncounter>();

      NwObject IEvent.Context
      {
        get => Encounter;
      }
    }

    [GameEvent(EventScriptType.EncounterOnHeartbeat)]
    public sealed class OnHeartbeat : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwEncounter"/> associated with this heartbeat event.
      /// </summary>
      public NwEncounter Encounter { get; } = NWScript.OBJECT_SELF.ToNwObject<NwEncounter>();

      NwObject IEvent.Context
      {
        get => Encounter;
      }
    }

    [GameEvent(EventScriptType.EncounterOnEncounterExhausted)]
    public sealed class OnExhausted : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwEncounter"/> that was exhausted.
      /// </summary>
      public NwEncounter Encounter { get; } = NWScript.OBJECT_SELF.ToNwObject<NwEncounter>();

      NwObject IEvent.Context
      {
        get => Encounter;
      }
    }

    [GameEvent(EventScriptType.EncounterOnUserDefinedEvent)]
    public sealed class OnUserDefined : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwEncounter"/> associated with this user defined event.
      /// </summary>
      public NwEncounter Encounter { get; } = NWScript.OBJECT_SELF.ToNwObject<NwEncounter>();

      NwObject IEvent.Context
      {
        get => Encounter;
      }

      public static void Signal(NwEncounter encounter, int eventId)
      {
        Event nwEvent = NWScript.EventUserDefined(eventId);
        NWScript.SignalEvent(encounter, nwEvent);
      }
    }
  }
}
