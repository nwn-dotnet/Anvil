using NWN.API;
using NWN.Core.NWNX;

namespace NWNX.API.Events
{
  public static class ServerSendAreaEvents
  {
    [NWNXEvent("NWNX_ON_SERVER_SEND_AREA_BEFORE")]
    public class OnServerSendAreaBefore : NWNXEvent<OnServerSendAreaBefore>
    {
      public NwPlayer Player { get; private set; }

      public NwArea Area { get; private set; }

      /// <summary>
      /// Gets a value indicating whether this is the player's first time logging in to the server since a restart.
      /// </summary>
      public bool NewToModule { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Player = (NwPlayer) objSelf;
        Area = EventsPlugin.GetEventData("AREA").ParseObject<NwArea>();
        NewToModule = EventsPlugin.GetEventData("IS_DM").ParseInt().ToBool();
      }
    }

    [NWNXEvent("NWNX_ON_SERVER_SEND_AREA_AFTER")]
    public class OnServerSendAreaAfter : NWNXEvent<OnServerSendAreaAfter>
    {
      public NwPlayer Player { get; private set; }

      public NwArea Area { get; private set; }

      /// <summary>
      /// Gets a value indicating whether this is the player's first time logging in to the server since a restart.
      /// </summary>
      public bool NewToModule { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Player = (NwPlayer) objSelf;
        Area = EventsPlugin.GetEventData("AREA").ParseObject<NwArea>();
        NewToModule = EventsPlugin.GetEventData("IS_DM").ParseInt().ToBool();
      }
    }
  }
}
