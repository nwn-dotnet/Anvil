namespace NWN.API.Events
{
  public sealed partial class GameEventFactory
  {
    public sealed class RegistrationData
    {
      public NwObject NwObject { get; }

      public bool CallOriginal { get; }

      public RegistrationData(NwObject nwObject, bool callOriginal = true)
      {
        NwObject = nwObject;
        CallOriginal = callOriginal;
      }
    }
  }
}
