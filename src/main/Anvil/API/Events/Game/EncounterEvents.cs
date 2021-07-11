using System;
using NWN.API.Constants;
using NWN.API.Events;
using NWN.Core;

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

namespace NWN.API
{
  public sealed partial class NwEncounter
  {
    /// <inheritdoc cref="NWN.API.Events.EncounterEvents.OnEnter"/>
    public event Action<EncounterEvents.OnEnter> OnEnter
    {
      add => EventService.Subscribe<EncounterEvents.OnEnter, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<EncounterEvents.OnEnter, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.EncounterEvents.OnExit"/>
    public event Action<EncounterEvents.OnExit> OnExit
    {
      add => EventService.Subscribe<EncounterEvents.OnExit, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<EncounterEvents.OnExit, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.EncounterEvents.OnHeartbeat"/>
    public event Action<EncounterEvents.OnHeartbeat> OnHeartbeat
    {
      add => EventService.Subscribe<EncounterEvents.OnHeartbeat, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<EncounterEvents.OnHeartbeat, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.EncounterEvents.OnExhausted"/>
    public event Action<EncounterEvents.OnExhausted> OnExhausted
    {
      add => EventService.Subscribe<EncounterEvents.OnExhausted, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<EncounterEvents.OnExhausted, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.EncounterEvents.OnUserDefined"/>
    public event Action<EncounterEvents.OnUserDefined> OnUserDefined
    {
      add => EventService.Subscribe<EncounterEvents.OnUserDefined, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<EncounterEvents.OnUserDefined, GameEventFactory>(this, value);
    }
  }
}
