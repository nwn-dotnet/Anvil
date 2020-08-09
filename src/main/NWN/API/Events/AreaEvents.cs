using NWN.API.Constants;
using NWN.Core;

namespace NWN.API.Events
{
  public static class AreaEvents
  {
    [ScriptEvent(EventScriptType.AreaOnEnter)]
    public class OnEnter : Event<NwArea, OnEnter>
    {
      public NwArea Area { get; private set; }
      public NwGameObject EnteringObject { get; private set; }

      protected override void PrepareEvent(NwArea objSelf)
      {
        Area = objSelf;
        EnteringObject = NWScript.GetEnteringObject().ToNwObject<NwGameObject>();
      }
    }

    [ScriptEvent(EventScriptType.AreaOnExit)]
    public class OnExit : Event<NwArea, OnExit>
    {
      public NwArea Area { get; private set; }
      public NwGameObject ExitingObject { get; private set; }

      protected override void PrepareEvent(NwArea objSelf)
      {
        Area = objSelf;
        ExitingObject = NWScript.GetExitingObject().ToNwObject<NwGameObject>();
      }
    }

    [ScriptEvent(EventScriptType.AreaOnHeartbeat)]
    public class OnHeartbeat : Event<NwArea, OnHeartbeat>
    {
      public NwArea Area { get; private set; }

      protected override void PrepareEvent(NwArea objSelf)
      {
        Area = objSelf;
      }
    }

    [ScriptEvent(EventScriptType.AreaOnUserDefinedEvent)]
    public class OnUserDefined : Event<NwArea, OnUserDefined>
    {
      public NwArea Area { get; private set; }
      public int EventNumber { get; private set; }

      protected override void PrepareEvent(NwArea objSelf)
      {
        Area = objSelf;
        EventNumber = NWScript.GetUserDefinedEventNumber();
      }
    }
  }
}