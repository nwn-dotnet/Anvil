using System;
using NWN;

namespace NWM.API.Events
{
  public static class AreaEvents
  {
    [EventInfo(EventType.Native, DefaultScriptSuffix = "ent")]
    public class OnEnter : IEvent<OnEnter>
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

    [EventInfo(EventType.Native, DefaultScriptSuffix = "exi")]
    public class OnExit : IEvent<OnExit>
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

    [EventInfo(EventType.Native, DefaultScriptSuffix = "hea")]
    public class OnHeartbeat : IEvent<OnHeartbeat>
    {
      public NwArea Area { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Area = (NwArea) objSelf;
        Callbacks?.Invoke(this);
      }

      public event Action<OnHeartbeat> Callbacks;
    }

    [EventInfo(EventType.Native, DefaultScriptSuffix = "use")]
    public class OnUserDefined : IEvent<OnUserDefined>
    {
      public NwArea Area;
      public int EventNumber { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        throw new NotImplementedException();
      }

      public event Action<OnUserDefined> Callbacks;
    }
  }
}