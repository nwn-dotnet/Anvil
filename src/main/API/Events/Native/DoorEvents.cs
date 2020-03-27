namespace NWM.API
{
  public enum DoorEventType
  {
    AreaTransitionClick,
    Close,
    Damaged,
    Death,
    FailToOpen,
    Heartbeat,
    Lock,
    Open,
    PhysicalAttacked,
    SpellCastAt,
    Unlock,
    UserDefined
  }

  public sealed class DoorEvents : NativeEventHandler<DoorEventType>
  {
    protected override void HandleEvent(DoorEventType eventType, NwObject objSelf)
    {
      throw new System.NotImplementedException();
    }
  }
}