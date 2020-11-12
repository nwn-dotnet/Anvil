using NWN.API.Constants;

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
      public NwTrigger Trigger { get; private set; }

      protected override void PrepareEvent(NwTrigger objSelf)
      {
        Trigger = objSelf;
      }
    }

    [NativeEvent(EventScriptType.TriggerOnObjectEnter)]
    public sealed class OnEnter : NativeEvent<NwTrigger, OnEnter>
    {
      public NwTrigger Trigger { get; private set; }

      protected override void PrepareEvent(NwTrigger objSelf)
      {
        Trigger = objSelf;
      }
    }

    [NativeEvent(EventScriptType.TriggerOnObjectExit)]
    public sealed class OnExit : NativeEvent<NwTrigger, OnExit>
    {
      public NwTrigger Trigger { get; private set; }

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
      public NwTrigger Trigger { get; private set; }

      protected override void PrepareEvent(NwTrigger objSelf)
      {
        Trigger = objSelf;
      }
    }

    [NativeEvent(EventScriptType.TriggerOnDisarmed)]
    public sealed class OnDisarmed : NativeEvent<NwTrigger, OnDisarmed>
    {
      public NwTrigger Trigger { get; private set; }

      protected override void PrepareEvent(NwTrigger objSelf)
      {
        Trigger = objSelf;
      }
    }

    [NativeEvent(EventScriptType.TriggerOnClicked)]
    public sealed class OnClicked : NativeEvent<NwTrigger, OnClicked>
    {
      public NwTrigger Trigger { get; private set; }

      protected override void PrepareEvent(NwTrigger objSelf)
      {
        Trigger = objSelf;
      }
    }
  }
}
