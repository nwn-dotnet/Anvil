using System;
using NWM.API.Constants;
using NWN;

namespace NWM.API.Events
{
  public static class AreaEvents
  {
    [ScriptEvent(EventScriptType.AreaOnEnter)]
    public class OnEnter : IEvent<NwArea, OnEnter>
    {
      public NwArea Area { get; private set; }
      public NwGameObject EnteringObject { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Area = (NwArea) objSelf;
        EnteringObject = NWScript.GetEnteringObject().ToNwObject<NwGameObject>();

        Callbacks?.Invoke(this);
      }

      public event Action<OnEnter> Callbacks;
    }

    [ScriptEvent(EventScriptType.AreaOnExit)]
    public class OnExit : IEvent<NwArea, OnExit>
    {
      public NwArea Area { get; private set; }
      public NwGameObject ExitingObject { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Area = (NwArea) objSelf;
        ExitingObject = NWScript.GetExitingObject().ToNwObject<NwGameObject>();

        Callbacks?.Invoke(this);
      }

      public event Action<OnExit> Callbacks;
    }

    [ScriptEvent(EventScriptType.AreaOnHeartbeat)]
    public class OnHeartbeat : IEvent<NwArea, OnHeartbeat>
    {
      public NwArea Area { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Area = (NwArea) objSelf;
        Callbacks?.Invoke(this);
      }

      public event Action<OnHeartbeat> Callbacks;
    }

    [ScriptEvent(EventScriptType.AreaOnUserDefinedEvent)]
    public class OnUserDefined : IEvent<NwArea, OnUserDefined>
    {
      public NwArea Area { get; private set; }
      public int EventNumber { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Area = (NwArea) objSelf;
        EventNumber = NWScript.GetUserDefinedEventNumber();
        Callbacks?.Invoke(this);
      }

      public event Action<OnUserDefined> Callbacks;
    }
  }
}