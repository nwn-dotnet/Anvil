namespace NWM.API
{
  public sealed class DoorEvents : EventHandler<DoorEventType>
  {
    protected override void HandleEvent(DoorEventType eventType, NwObject objSelf)
    {
      switch (eventType)
      {
        case DoorEventType.AreaTransitionClick:
          break;
        case DoorEventType.Close:
          break;
        case DoorEventType.Damaged:
          break;
        case DoorEventType.Death:
          break;
        case DoorEventType.FailToOpen:
          break;
        case DoorEventType.Heartbeat:
          break;
        case DoorEventType.Lock:
          break;
        case DoorEventType.Open:
          break;
        case DoorEventType.PhysicalAttacked:
          break;
        case DoorEventType.SpellCastAt:
          break;
        case DoorEventType.Unlock:
          break;
        case DoorEventType.UserDefined:
          break;
      }
    }
  }
}