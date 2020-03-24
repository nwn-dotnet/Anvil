namespace NWM.API
{
  public sealed class PlaceableEvents : EventHandler<PlaceableEventType>
  {
    protected override void HandleEvent(PlaceableEventType eventType, NwObject objSelf)
    {
      switch (eventType)
      {
        case PlaceableEventType.Close:
          break;
        case PlaceableEventType.Damaged:
          break;
        case PlaceableEventType.Death:
          break;
        case PlaceableEventType.Disturbed:
          break;
        case PlaceableEventType.Heartbeat:
          break;
        case PlaceableEventType.Lock:
          break;
        case PlaceableEventType.Open:
          break;
        case PlaceableEventType.PhysicalAttacked:
          break;
        case PlaceableEventType.SpellCastAt:
          break;
        case PlaceableEventType.Unlock:
          break;
        case PlaceableEventType.Used:
          break;
        case PlaceableEventType.UserDefined:
          break;
      }
    }
  }
}