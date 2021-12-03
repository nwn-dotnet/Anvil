namespace Anvil.API.Events
{
  public sealed partial class GameEventFactory
  {
    public sealed class RegistrationData
    {
      public RegistrationData(NwObject nwObject, bool callOriginal = true)
      {
        NwObject = nwObject;
        CallOriginal = callOriginal;
      }

      public bool CallOriginal { get; }
      public NwObject NwObject { get; }
    }
  }
}
