using NWM.API;
using NWN;

namespace NWM.Core
{
  public class AreaEvents : EventHandler
  {
    public event AreaEnterEvent OnEnter;
    public event AreaExitEvent OnExit;
    public event HeartbeatEvent OnHeartbeat;
    public event UserDefinedEvent OnUserDefined;

    public delegate void AreaEnterEvent(NwArea area, NwGameObject enteringObj);
    public delegate void AreaExitEvent(NwArea area, NwGameObject exitingObj);
    public delegate void HeartbeatEvent();
    public delegate void UserDefinedEvent(int eventNumber);

    internal override bool HandleScriptEvent(string scriptName, NwObject objSelf)
    {
      switch (scriptName)
      {
        case "ent":
          OnEnter?.Invoke((NwArea) objSelf, (NwGameObject) EnteringObject);
          return true;
        case "exi":
          OnExit?.Invoke((NwArea) objSelf, (NwGameObject) ExitingObject);
          return true;
        case "hea":
          OnHeartbeat?.Invoke();
          return true;
        case "udef":
          OnUserDefined?.Invoke(NWScript.GetUserDefinedEventNumber());
          return true;
      }

      return false;
    }
  }
}