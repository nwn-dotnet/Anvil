namespace NWM.API
{
  public enum EncounterEventType
  {
    Enter,
    Exhausted,
    Exit,
    Heartbeat,
    UserDefined
  }

  public sealed class EncounterEvents : NativeEventHandler<EncounterEventType>
  {
    protected override void HandleEvent(EncounterEventType eventType, NwObject objSelf)
    {
      throw new System.NotImplementedException();
    }
  }
}