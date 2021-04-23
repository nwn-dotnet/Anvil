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
      /// Gets the <see cref="NwEncounter"/> on entered.
      /// </summary>
      public NwEncounter Encounter { get; } = NWScript.OBJECT_SELF.ToNwObject<NwEncounter>();

      NwObject IEvent.Context => Encounter;
    }

    [GameEvent(EventScriptType.EncounterOnObjectExit)]
    public sealed class OnExit : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwEncounter"/> on exited.
      /// </summary>
      public NwEncounter Encounter { get; } = NWScript.OBJECT_SELF.ToNwObject<NwEncounter>();

      NwObject IEvent.Context => Encounter;
    }

    [GameEvent(EventScriptType.EncounterOnHeartbeat)]
    public sealed class OnHeartbeat : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwEncounter"/> on heartbeat.
      /// </summary>
      public NwEncounter Encounter { get; } = NWScript.OBJECT_SELF.ToNwObject<NwEncounter>();

      NwObject IEvent.Context => Encounter;
    }

    [GameEvent(EventScriptType.EncounterOnEncounterExhausted)]
    public sealed class OnExhausted : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwEncounter"/> on exhausted.
      /// </summary>
      public NwEncounter Encounter { get; } = NWScript.OBJECT_SELF.ToNwObject<NwEncounter>();

      NwObject IEvent.Context => Encounter;
    }

    [GameEvent(EventScriptType.EncounterOnUserDefinedEvent)]
    public sealed class OnUserDefined : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwEncounter"/> on user defined.
      /// </summary>
      public NwEncounter Encounter { get; } = NWScript.OBJECT_SELF.ToNwObject<NwEncounter>();

      NwObject IEvent.Context => Encounter;

      public static void Signal(NwEncounter encounter, int eventId)
      {
        Event nwEvent = NWScript.EventUserDefined(eventId);
        NWScript.SignalEvent(encounter, nwEvent);
      }
    }
  }
}
