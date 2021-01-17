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
    public sealed class OnHeartbeat : NativeEvent<NwTrigger, OnHeartbeat>
    {
      /// <summary>
      /// Gets the trigger associated with this heartbeat event.
      /// </summary>
      public NwTrigger Trigger { get; private set; }

      protected override void PrepareEvent(NwTrigger objSelf)
      {
        Trigger = objSelf;
      }
    }

    [NativeEvent(EventScriptType.TriggerOnObjectEnter)]
    public sealed class OnEnter : NativeEvent<NwTrigger, OnEnter>
    {
      /// <summary>
      /// Gets the trigger that was entered.
      /// </summary>
      public NwTrigger Trigger { get; private set; }

      /// <summary>
      /// Gets the object that entered this trigger.
      /// </summary>
      public NwGameObject EnteringObject { get; private set; }

      protected override void PrepareEvent(NwTrigger objSelf)
      {
        Trigger = objSelf;
        EnteringObject = NWScript.GetEnteringObject().ToNwObject<NwGameObject>();
      }
    }

    [NativeEvent(EventScriptType.TriggerOnObjectExit)]
    public sealed class OnExit : NativeEvent<NwTrigger, OnExit>
    {
      /// <summary>
      /// Gets the trigger that was left.
      /// </summary>
      public NwTrigger Trigger { get; private set; }

      /// <summary>
      /// Gets the object that left this trigger.
      /// </summary>
      public NwGameObject ExitingObject { get; private set; }

      protected override void PrepareEvent(NwTrigger objSelf)
      {
        Trigger = objSelf;
      }
    }

    [NativeEvent(EventScriptType.TriggerOnUserDefinedEvent)]
    public sealed class OnUserDefined : NativeEvent<NwTrigger, OnUserDefined>
    {
      public NwTrigger Trigger { get; private set; }

      protected override void PrepareEvent(NwTrigger objSelf)
      {
        Trigger = objSelf;
      }
    }

    [NativeEvent(EventScriptType.TriggerOnTrapTriggered)]
    public sealed class OnTrapTriggered : NativeEvent<NwTrigger, OnTrapTriggered>
    {
      /// <summary>
      /// Gets the trigger associated with this trap.
      /// </summary>
      public NwTrigger Trigger { get; private set; }

      /// <summary>
      /// Gets the object that triggered this trap.
      /// </summary>
      public NwGameObject TriggeredBy { get; private set; }

      protected override void PrepareEvent(NwTrigger objSelf)
      {
        Trigger = objSelf;
        TriggeredBy = NWScript.GetEnteringObject().ToNwObject<NwGameObject>();
      }
    }

    [NativeEvent(EventScriptType.TriggerOnDisarmed)]
    public sealed class OnDisarmed : NativeEvent<NwTrigger, OnDisarmed>
    {
      /// <summary>
      /// Gets the trigger that was disarmed.
      /// </summary>
      public NwTrigger Trigger { get; private set; }

      /// <summary>
      /// Gets the creature who disarmed this trigger.
      /// </summary>
      public NwCreature DisarmedBy { get; private set; }

      protected override void PrepareEvent(NwTrigger objSelf)
      {
        Trigger = objSelf;
        DisarmedBy = NWScript.GetLastDisarmed().ToNwObject<NwCreature>();
      }
    }

    [NativeEvent(EventScriptType.TriggerOnClicked)]
    public sealed class OnClicked : NativeEvent<NwTrigger, OnClicked>
    {
      /// <summary>
      /// Gets the trigger that was clicked.
      /// </summary>
      public NwTrigger Trigger { get; private set; }

      /// <summary>
      /// Gets the creature that clicked this trigger.
      /// </summary>
      public NwCreature ClickedBy { get; private set; }

      protected override void PrepareEvent(NwTrigger objSelf)
      {
        Trigger = objSelf;
        ClickedBy = NWScript.GetClickingObject().ToNwObject<NwCreature>();
      }
    }
  }
}
