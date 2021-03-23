using NWN.API.Constants;

using NWN.Core;

// TODO Populate event data.
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
      /// Gets the trigger associated with this heartbeat event.
      /// </summary>
      public NwTrigger Trigger { get; } = NWScript.OBJECT_SELF.ToNwObject<NwTrigger>();

      NwObject IEvent.Context => Trigger;
    }

    [GameEvent(EventScriptType.TriggerOnObjectEnter)]
    public sealed class OnEnter : IEvent
    {
      /// <summary>
      /// Gets the trigger that was entered.
      /// </summary>
      public NwTrigger Trigger { get; } = NWScript.OBJECT_SELF.ToNwObject<NwTrigger>();

      /// <summary>
      /// Gets the object that entered this trigger.
      /// </summary>
      public NwGameObject EnteringObject { get; } = NWScript.GetEnteringObject().ToNwObject<NwGameObject>();

      NwObject IEvent.Context => Trigger;
    }

    [GameEvent(EventScriptType.TriggerOnObjectExit)]
    public sealed class OnExit : IEvent
    {
      /// <summary>
      /// Gets the trigger that was left.
      /// </summary>
      public NwTrigger Trigger { get; } = NWScript.OBJECT_SELF.ToNwObject<NwTrigger>();

      /// <summary>
      /// Gets the object that left this trigger.
      /// </summary>
      public NwGameObject ExitingObject { get; private set; }

      NwObject IEvent.Context => Trigger;
    }

    [GameEvent(EventScriptType.TriggerOnUserDefinedEvent)]
    public sealed class OnUserDefined : IEvent
    {
      public NwTrigger Trigger { get; } = NWScript.OBJECT_SELF.ToNwObject<NwTrigger>();

      NwObject IEvent.Context => Trigger;

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
      /// Gets the trigger associated with this trap.
      /// </summary>
      public NwTrigger Trigger { get; } = NWScript.OBJECT_SELF.ToNwObject<NwTrigger>();

      /// <summary>
      /// Gets the object that triggered this trap.
      /// </summary>
      public NwGameObject TriggeredBy { get; } = NWScript.GetEnteringObject().ToNwObject<NwGameObject>();

      NwObject IEvent.Context => Trigger;
    }

    [GameEvent(EventScriptType.TriggerOnDisarmed)]
    public sealed class OnDisarmed : IEvent
    {
      /// <summary>
      /// Gets the trigger that was disarmed.
      /// </summary>
      public NwTrigger Trigger { get; } = NWScript.OBJECT_SELF.ToNwObject<NwTrigger>();

      /// <summary>
      /// Gets the creature who disarmed this trigger.
      /// </summary>
      public NwCreature DisarmedBy { get; } = NWScript.GetLastDisarmed().ToNwObject<NwCreature>();

      NwObject IEvent.Context => Trigger;
    }

    [GameEvent(EventScriptType.TriggerOnClicked)]
    public sealed class OnClicked : IEvent
    {
      /// <summary>
      /// Gets the trigger that was clicked.
      /// </summary>
      public NwTrigger Trigger { get; } = NWScript.OBJECT_SELF.ToNwObject<NwTrigger>();

      /// <summary>
      /// Gets the creature that clicked this trigger.
      /// </summary>
      public NwCreature ClickedBy { get; } = NWScript.GetClickingObject().ToNwObject<NwCreature>();

      NwObject IEvent.Context => Trigger;
    }
  }
}
