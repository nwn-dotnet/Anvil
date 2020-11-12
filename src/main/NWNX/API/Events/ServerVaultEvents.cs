using NWN.API;

namespace NWNX.API.Events
{
  public static class ServerVaultEvents
  {
    [NWNXEvent("NWNX_ON_SERVER_CHARACTER_SAVE_BEFORE")]
    public class OnServerCharacterSaveBefore : NWNXEventSkippable<OnServerCharacterSaveBefore>
    {
      public NwPlayer Player { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Player = (NwPlayer) objSelf;
      }
    }

    [NWNXEvent("NWNX_ON_SERVER_CHARACTER_SAVE_AFTER")]
    public class OnServerCharacterSaveAfter : NWNXEvent<OnServerCharacterSaveAfter>
    {
      public NwPlayer Player { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Player = (NwPlayer) objSelf;
      }
    }
  }
}
