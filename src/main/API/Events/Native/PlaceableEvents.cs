namespace NWM.API
{
  public enum PlaceableEventType
  {
    Close,
    Damaged,
    Death,
    Disturbed,
    Heartbeat,
    Lock,
    Open,
    PhysicalAttacked,
    SpellCastAt,
    Unlock,
    Used,
    UserDefined
  }

  public sealed class PlaceableEvents : NativeEventHandler<PlaceableEventType>
  {
    protected override void HandleEvent(PlaceableEventType eventType, NwObject objSelf)
    {
      throw new System.NotImplementedException();
    }
  }
}