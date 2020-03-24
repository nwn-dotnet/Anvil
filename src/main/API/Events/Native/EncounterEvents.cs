namespace NWM.API
{
  public sealed class EncounterEvents : EventHandler<EncounterEventType>
  {
    protected override void HandleEvent(EncounterEventType eventType, NwObject objSelf)
    {
      switch (eventType)
      {
        case EncounterEventType.Enter:
          break;
        case EncounterEventType.Exhausted:
          break;
        case EncounterEventType.Exit:
          break;
        case EncounterEventType.Heartbeat:
          break;
        case EncounterEventType.UserDefined:
          break;
      }
    }
  }
}