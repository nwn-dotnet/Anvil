using System;
using Anvil.API;
using NWN.API.Events;
using NWN.Core;

namespace NWN.API.Events
{
  /// <summary>
  /// Events for drawn, world-placed triggers.
  /// </summary>
  public static class TriggerEvents
  {
    [GameEvent(EventScriptType.TriggerOnHeartbeat)]
    public sealed class OnHeartbeat : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwTrigger"/> associated with this heartbeat event.
      /// </summary>
      public NwTrigger Trigger { get; } = NWScript.OBJECT_SELF.ToNwObject<NwTrigger>();

      NwObject IEvent.Context
      {
        get => Trigger;
      }
    }

    [GameEvent(EventScriptType.TriggerOnObjectEnter)]
    public sealed class OnEnter : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwTrigger"/> that was entered.
      /// </summary>
      public NwTrigger Trigger { get; } = NWScript.OBJECT_SELF.ToNwObject<NwTrigger>();

      /// <summary>
      /// Gets the <see cref="NwGameObject"/> that entered this <see cref="NwTrigger"/>.
      /// </summary>
      public NwGameObject EnteringObject { get; } = NWScript.GetEnteringObject().ToNwObject<NwGameObject>();

      NwObject IEvent.Context
      {
        get => Trigger;
      }
    }

    [GameEvent(EventScriptType.TriggerOnObjectExit)]
    public sealed class OnExit : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwTrigger"/> that was left.
      /// </summary>
      public NwTrigger Trigger { get; } = NWScript.OBJECT_SELF.ToNwObject<NwTrigger>();

      /// <summary>
      /// Gets the <see cref="NwGameObject"/> that left this <see cref="NwTrigger"/>.
      /// </summary>
      public NwGameObject ExitingObject { get; } = NWScript.GetExitingObject().ToNwObject<NwGameObject>();

      NwObject IEvent.Context
      {
        get => Trigger;
      }
    }

    [GameEvent(EventScriptType.TriggerOnUserDefinedEvent)]
    public sealed class OnUserDefined : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwTrigger"/> that is running a user defined event.
      /// </summary>
      public NwTrigger Trigger { get; } = NWScript.OBJECT_SELF.ToNwObject<NwTrigger>();

      /// <summary>
      /// Gets the specific event number used to trigger this user-defined event.
      /// </summary>
      public int EventNumber { get; } = NWScript.GetUserDefinedEventNumber();

      NwObject IEvent.Context
      {
        get => Trigger;
      }

      public static void Signal(NwTrigger trigger, int eventId)
      {
        Event nwEvent = NWScript.EventUserDefined(eventId);
        NWScript.SignalEvent(trigger, nwEvent);
      }
    }

    [GameEvent(EventScriptType.TriggerOnTrapTriggered)]
    public sealed class OnTrapTriggered : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwTrigger"/> that was triggered.
      /// </summary>
      public NwTrigger Trigger { get; } = NWScript.OBJECT_SELF.ToNwObject<NwTrigger>();

      /// <summary>
      /// Gets the <see cref="NwGameObject"/> that triggered this <see cref="NwTrigger"/>.
      /// </summary>
      public NwGameObject TriggeredBy { get; } = NWScript.GetEnteringObject().ToNwObject<NwGameObject>();

      NwObject IEvent.Context
      {
        get => Trigger;
      }
    }

    [GameEvent(EventScriptType.TriggerOnDisarmed)]
    public sealed class OnDisarmed : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwTrigger"/> that was disarmed.
      /// </summary>
      public NwTrigger Trigger { get; } = NWScript.OBJECT_SELF.ToNwObject<NwTrigger>();

      /// <summary>
      /// Gets the <see cref="NwCreature"/> who disarmed this trigger.
      /// </summary>
      public NwCreature DisarmedBy { get; } = NWScript.GetLastDisarmed().ToNwObject<NwCreature>();

      NwObject IEvent.Context
      {
        get => Trigger;
      }
    }

    [GameEvent(EventScriptType.TriggerOnClicked)]
    public sealed class OnClicked : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwTrigger"/> that was clicked.
      /// </summary>
      public NwTrigger Trigger { get; } = NWScript.OBJECT_SELF.ToNwObject<NwTrigger>();

      /// <summary>
      /// Gets the <see cref="NwCreature"/> that clicked this <see cref="NwTrigger"/>.
      /// </summary>
      public NwCreature ClickedBy { get; } = NWScript.GetClickingObject().ToNwObject<NwCreature>();

      NwObject IEvent.Context
      {
        get => Trigger;
      }
    }
  }
}

namespace NWN.API
{
  public sealed partial class NwTrigger
  {
    /// <inheritdoc cref="NWN.API.Events.TriggerEvents.OnHeartbeat"/>
    public event Action<TriggerEvents.OnHeartbeat> OnHeartbeat
    {
      add => EventService.Subscribe<TriggerEvents.OnHeartbeat, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<TriggerEvents.OnHeartbeat, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.TriggerEvents.OnEnter"/>
    public event Action<TriggerEvents.OnEnter> OnEnter
    {
      add => EventService.Subscribe<TriggerEvents.OnEnter, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<TriggerEvents.OnEnter, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.TriggerEvents.OnExit"/>
    public event Action<TriggerEvents.OnExit> OnExit
    {
      add => EventService.Subscribe<TriggerEvents.OnExit, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<TriggerEvents.OnExit, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.TriggerEvents.OnUserDefined"/>
    public event Action<TriggerEvents.OnUserDefined> OnUserDefined
    {
      add => EventService.Subscribe<TriggerEvents.OnUserDefined, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<TriggerEvents.OnUserDefined, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.TriggerEvents.OnTrapTriggered"/>
    public event Action<TriggerEvents.OnTrapTriggered> OnTrapTriggered
    {
      add => EventService.Subscribe<TriggerEvents.OnTrapTriggered, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<TriggerEvents.OnTrapTriggered, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.TriggerEvents.OnDisarmed"/>
    public event Action<TriggerEvents.OnDisarmed> OnDisarmed
    {
      add => EventService.Subscribe<TriggerEvents.OnDisarmed, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<TriggerEvents.OnDisarmed, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.TriggerEvents.OnClicked"/>
    public event Action<TriggerEvents.OnClicked> OnClicked
    {
      add => EventService.Subscribe<TriggerEvents.OnClicked, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<TriggerEvents.OnClicked, GameEventFactory>(this, value);
    }
  }
}
