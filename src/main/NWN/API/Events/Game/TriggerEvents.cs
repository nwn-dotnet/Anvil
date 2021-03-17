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
    [NativeEvent(EventScriptType.TriggerOnHeartbeat)]
    public sealed class OnHeartbeat : IEvent
    {
      /// <summary>
      /// Gets the trigger associated with this heartbeat event.
      /// </summary>
      public NwTrigger Trigger { get; private set; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Trigger;

      public OnHeartbeat()
      {
        Trigger = NWScript.OBJECT_SELF.ToNwObject<NwTrigger>();
      }
    }

    [NativeEvent(EventScriptType.TriggerOnObjectEnter)]
    public sealed class OnEnter : IEvent
    {
      /// <summary>
      /// Gets the trigger that was entered.
      /// </summary>
      public NwTrigger Trigger { get; private set; }

      /// <summary>
      /// Gets the object that entered this trigger.
      /// </summary>
      public NwGameObject EnteringObject { get; private set; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Trigger;

      public OnEnter()
      {
        Trigger = NWScript.OBJECT_SELF.ToNwObject<NwTrigger>();
        EnteringObject = NWScript.GetEnteringObject().ToNwObject<NwGameObject>();
      }
    }

    [NativeEvent(EventScriptType.TriggerOnObjectExit)]
    public sealed class OnExit : IEvent
    {
      /// <summary>
      /// Gets the trigger that was left.
      /// </summary>
      public NwTrigger Trigger { get; private set; }

      /// <summary>
      /// Gets the object that left this trigger.
      /// </summary>
      public NwGameObject ExitingObject { get; private set; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Trigger;

      public OnExit()
      {
        Trigger = NWScript.OBJECT_SELF.ToNwObject<NwTrigger>();
      }
    }

    [NativeEvent(EventScriptType.TriggerOnUserDefinedEvent)]
    public sealed class OnUserDefined : IEvent
    {
      public NwTrigger Trigger { get; private set; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Trigger;

      public OnUserDefined()
      {
        Trigger = NWScript.OBJECT_SELF.ToNwObject<NwTrigger>();
      }
    }

    [NativeEvent(EventScriptType.TriggerOnTrapTriggered)]
    public sealed class OnTrapTriggered : IEvent
    {
      /// <summary>
      /// Gets the trigger associated with this trap.
      /// </summary>
      public NwTrigger Trigger { get; private set; }

      /// <summary>
      /// Gets the object that triggered this trap.
      /// </summary>
      public NwGameObject TriggeredBy { get; private set; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Trigger;

      public OnTrapTriggered()
      {
        Trigger = NWScript.OBJECT_SELF.ToNwObject<NwTrigger>();
        TriggeredBy = NWScript.GetEnteringObject().ToNwObject<NwGameObject>();
      }
    }

    [NativeEvent(EventScriptType.TriggerOnDisarmed)]
    public sealed class OnDisarmed : IEvent
    {
      /// <summary>
      /// Gets the trigger that was disarmed.
      /// </summary>
      public NwTrigger Trigger { get; private set; }

      /// <summary>
      /// Gets the creature who disarmed this trigger.
      /// </summary>
      public NwCreature DisarmedBy { get; private set; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Trigger;

      public OnDisarmed()
      {
        Trigger = NWScript.OBJECT_SELF.ToNwObject<NwTrigger>();
        DisarmedBy = NWScript.GetLastDisarmed().ToNwObject<NwCreature>();
      }
    }

    [NativeEvent(EventScriptType.TriggerOnClicked)]
    public sealed class OnClicked : IEvent
    {
      /// <summary>
      /// Gets the trigger that was clicked.
      /// </summary>
      public NwTrigger Trigger { get; private set; }

      /// <summary>
      /// Gets the creature that clicked this trigger.
      /// </summary>
      public NwCreature ClickedBy { get; private set; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Trigger;

      public OnClicked()
      {
        Trigger = NWScript.OBJECT_SELF.ToNwObject<NwTrigger>();
        ClickedBy = NWScript.GetClickingObject().ToNwObject<NwCreature>();
      }
    }
  }
}
