namespace NWM.API
{
  public sealed class TriggerEvents : EventHandler<TriggerEventType>
  {
    protected override void HandleEvent(TriggerEventType eventType, NwObject objSelf)
    {
      switch (eventType)
      {
        case TriggerEventType.AreaTransitionClick:
          break;
        case TriggerEventType.Click:
          break;
        case TriggerEventType.Enter:
          break;
        case TriggerEventType.Exit:
          break;
        case TriggerEventType.Heartbeat:
          break;
        case TriggerEventType.UserDefined:
          break;
      }
    }
  }
}