using NWN.API;

namespace NWNX.API.Events
{
  public static class ServerVaultEvents
  {
    [NWNXEvent("NWNX_ON_SERVER_CHARACTER_SAVE_BEFORE")]
    public class OnServerCharacterSaveBefore : EventSkippable<OnServerCharacterSaveBefore>
    {
      public NwPlayer Player { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Player = (NwPlayer) objSelf;
      }
    }
  }
}