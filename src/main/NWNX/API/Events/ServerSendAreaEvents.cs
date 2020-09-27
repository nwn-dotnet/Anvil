using NWN.API;
using NWN.API.Events;
using NWN.Core;
using NWN.Core.NWNX;

namespace NWNX.API.Events
{
  public class ServerSendAreaEvents
  {
    [NWNXEvent("NWNX_ON_SERVER_SEND_AREA_BEFORE")]
    public class OnServerSendAreaBefore : Event<OnServerSendAreaBefore>
    {
      public NwPlayer Player { get; private set; }

      public NwArea Area { get; private set; }

      /// <summary>
      /// Gets whether this is the player's first time logging in to the server since a restart.
      /// </summary>
      public bool NewToModule { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Player = (NwPlayer) objSelf;
        Area = NWScript.StringToObject(EventsPlugin.GetEventData("AREA")).ToNwObject<NwArea>();
        NewToModule = EventsPlugin.GetEventData("IS_DM").ParseInt().ToBool();
      }
    }

    [NWNXEvent("NWNX_ON_SERVER_SEND_AREA_AFTER")]
    public class OnServerSendAreaAfter : Event<OnServerSendAreaAfter>
    {
      public NwPlayer Player { get; private set; }

      public NwArea Area { get; private set; }

      /// <summary>
      /// Gets whether this is the player's first time logging in to the server since a restart.
      /// </summary>
      public bool NewToModule { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Player = (NwPlayer) objSelf;
        Area = NWScript.StringToObject(EventsPlugin.GetEventData("AREA")).ToNwObject<NwArea>();
        NewToModule = EventsPlugin.GetEventData("IS_DM").ParseInt().ToBool();
      }
    }
  }
}
