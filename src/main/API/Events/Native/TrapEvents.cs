namespace NWM.API
{
  public enum TrapEventType
  {
    Disarm,
    TrapTriggered
  }

  public sealed class TrapEvents : NativeEventHandler<TrapEventType>
  {
    protected override void HandleEvent(TrapEventType eventType, NwObject objSelf)
    {
      throw new System.NotImplementedException();
    }
  }
}