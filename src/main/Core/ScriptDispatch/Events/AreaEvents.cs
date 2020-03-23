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
    public delegate void HeartbeatEvent(NwArea area);
    public delegate void UserDefinedEvent(NwArea area, int eventNumber);

    internal override bool HandleScriptEvent(string scriptName, NwObject objSelf)
    {
      NwArea areaSelf = (NwArea) objSelf;

      switch (scriptName)
      {
        case "ent":
          OnEnter?.Invoke(areaSelf, EnteringObject);
          return true;
        case "exi":
          OnExit?.Invoke(areaSelf, ExitingObject);
          return true;
        case "hea":
          OnHeartbeat?.Invoke(areaSelf);
          return true;
        case "udef":
          OnUserDefined?.Invoke(areaSelf, NWScript.GetUserDefinedEventNumber());
          return true;
      }

      return false;
    }
  }
}