namespace NWM.API
{
  public enum TriggerEventType
  {
    AreaTransitionClick,
    Click,
    Enter,
    Exit,
    Heartbeat,
    UserDefined
  }

  public sealed class TriggerEvents : NativeEventHandler<TriggerEventType>
  {
    protected override void HandleEvent(TriggerEventType eventType, NwObject objSelf)
    {
      throw new System.NotImplementedException();
    }
  }
}