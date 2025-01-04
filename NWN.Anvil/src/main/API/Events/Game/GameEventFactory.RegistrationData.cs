namespace Anvil.API.Events
{
  public sealed partial class GameEventFactory
  {
    public sealed class RegistrationData(NwObject nwObject, bool callOriginal = true)
    {
      public bool CallOriginal { get; } = callOriginal;
      public NwObject NwObject { get; } = nwObject;
    }
  }
}
