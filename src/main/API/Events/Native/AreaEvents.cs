using NWN;

namespace NWM.API
{
  public enum AreaEventType
  {
    [DefaultScriptSuffix("ent")] Enter,
    [DefaultScriptSuffix("exi")] Exit,
    [DefaultScriptSuffix("hea")] Heartbeat,
    [DefaultScriptSuffix("use")] UserDefined
  }

  public class AreaEvents : NativeEventHandler<AreaEventType>
  {
    public event AreaEnterEvent OnEnter;
    public event AreaExitEvent OnExit;
    public event HeartbeatEvent OnHeartbeat;
    public event UserDefinedEvent OnUserDefined;

    public delegate void AreaEnterEvent(NwArea area, NwGameObject enteringObj);

    public delegate void AreaExitEvent(NwArea area, NwGameObject exitingObj);

    public delegate void HeartbeatEvent(NwArea area);

    public delegate void UserDefinedEvent(NwArea area, int eventNumber);

    protected override void HandleEvent(AreaEventType eventType, NwObject objSelf)
    {
      NwArea areaSelf = (NwArea) objSelf;

      switch (eventType)
      {
        case AreaEventType.Enter:
        {
          OnEnter?.Invoke(areaSelf, EnteringObject);
          break;
        }
        case AreaEventType.Exit:
        {
          OnExit?.Invoke(areaSelf, ExitingObject);
          break;
        }
        case AreaEventType.Heartbeat:
        {
          OnHeartbeat?.Invoke(areaSelf);
          break;
        }
        case AreaEventType.UserDefined:
        {
          OnUserDefined?.Invoke(areaSelf, NWScript.GetUserDefinedEventNumber());
          break;
        }
      }
    }
  }
}