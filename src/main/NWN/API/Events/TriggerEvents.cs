using NWN.API.Constants;

namespace NWN.API.Events
{
  public static class TriggerEvents
  {
    [ScriptEvent(EventScriptType.TriggerOnHeartbeat)]
    public sealed class OnHeartbeat : Event<NwTrigger, OnHeartbeat>
    {
      public NwTrigger Trigger { get; private set; }

      protected override void PrepareEvent(NwTrigger objSelf)
      {
        Trigger = objSelf;
      }
    }

    [ScriptEvent(EventScriptType.TriggerOnObjectEnter)]
    public sealed class OnEnter : Event<NwTrigger, OnEnter>
    {
      public NwTrigger Trigger { get; private set; }

      protected override void PrepareEvent(NwTrigger objSelf)
      {
        Trigger = objSelf;
      }
    }

    [ScriptEvent(EventScriptType.TriggerOnObjectExit)]
    public sealed class OnExit : Event<NwTrigger, OnExit>
    {
      public NwTrigger Trigger { get; private set; }

      protected override void PrepareEvent(NwTrigger objSelf)
      {
        Trigger = objSelf;
      }
    }

    [ScriptEvent(EventScriptType.TriggerOnUserDefinedEvent)]
    public sealed class OnUserDefined : Event<NwTrigger, OnUserDefined>
    {
      public NwTrigger Trigger { get; private set; }

      protected override void PrepareEvent(NwTrigger objSelf)
      {
        Trigger = objSelf;
      }
    }

    [ScriptEvent(EventScriptType.TriggerOnTrapTriggered)]
    public sealed class OnTrapTriggered : Event<NwTrigger, OnTrapTriggered>
    {
      public NwTrigger Trigger { get; private set; }

      protected override void PrepareEvent(NwTrigger objSelf)
      {
        Trigger = objSelf;
      }
    }

    [ScriptEvent(EventScriptType.TriggerOnDisarmed)]
    public sealed class OnDisarmed : Event<NwTrigger, OnDisarmed>
    {
      public NwTrigger Trigger { get; private set; }

      protected override void PrepareEvent(NwTrigger objSelf)
      {
        Trigger = objSelf;
      }
    }

    [ScriptEvent(EventScriptType.TriggerOnClicked)]
    public sealed class OnClicked : Event<NwTrigger, OnClicked>
    {
      public NwTrigger Trigger { get; private set; }

      protected override void PrepareEvent(NwTrigger objSelf)
      {
        Trigger = objSelf;
      }
    }
  }
}