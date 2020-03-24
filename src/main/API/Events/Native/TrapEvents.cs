namespace NWM.API
{
  public sealed class TrapEvents : EventHandler<TrapEventType>
  {
    protected override void HandleEvent(TrapEventType eventType, NwObject objSelf)
    {
      switch (eventType)
      {
        case TrapEventType.Disarm:
          break;
        case TrapEventType.TrapTriggered:
          break;
      }
    }
  }
}