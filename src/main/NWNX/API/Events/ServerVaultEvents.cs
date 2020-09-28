using NWN.API;
using NWN.API.Events;

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

    [NWNXEvent("NWNX_ON_SERVER_CHARACTER_SAVE_AFTER")]
    public class OnServerCharacterSaveAfter : Event<OnServerCharacterSaveAfter>
    {
      public NwPlayer Player { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Player = (NwPlayer) objSelf;
      }
    }
  }
}
