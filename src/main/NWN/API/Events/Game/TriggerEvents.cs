using NWN.API.Constants;
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
