using System;
using NWM.API;
using NWM.API.Events;
using NWNX;

namespace NWMX.API.Events
{
  public static class ServerVaultEvents
  {
    [NWNXEvent("NWNX_ON_SERVER_CHARACTER_SAVE_BEFORE")]
    public class OnServerCharacterSaveBefore : IEvent<OnServerCharacterSaveBefore>
    {
      public NwPlayer Player { get; private set; }
      public bool Skip { get; set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Player = (NwPlayer) objSelf;
        Skip = false;

        Callbacks?.Invoke(this);
        if (Skip)
        {
          EventsPlugin.SkipEvent();
        }
      }

      public event Action<OnServerCharacterSaveBefore> Callbacks;
    }
  }
}