using NWN.API;
using NWN.Core.NWNX;

namespace NWNX.API.Events
{
  public static class ClientEvents
  {
    [NWNXEvent("NWNX_ON_CLIENT_DISCONNECT_BEFORE")]
    public class OnClientDisconnectBefore : NWNXEventSkippable<OnClientDisconnectBefore>
    {
      public NwPlayer Player { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
        => Player = (NwPlayer) objSelf;
    }

    [NWNXEvent("NWNX_ON_CLIENT_DISCONNECT_AFTER")]
    public class OnClientDisconnectAfter : NWNXEventSkippable<OnClientDisconnectAfter>
    {
      public NwPlayer Player { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
        => Player = (NwPlayer) objSelf;
    }

    [NWNXEvent("NWNX_ON_CLIENT_CONNECT_BEFORE")]
    public class OnClientConnectBefore : NWNXEventSkippable<OnClientConnectBefore>
    {
      public NwModule Module { get; private set; }

      public string Username { get; private set; }

      public string CDKey { get; private set; }

      public bool DM { get; private set; }

      public string IP { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Module = (NwModule) objSelf;
        Username = EventsPlugin.GetEventData("PLAYER_NAME");
        CDKey = EventsPlugin.GetEventData("CDKEY");
        DM = EventsPlugin.GetEventData("IS_DM").ParseInt().ToBool();
        IP = EventsPlugin.GetEventData("IP_ADDRESS");
      }
    }

    [NWNXEvent("NWNX_ON_CLIENT_CONNECT_AFTER")]
    public class OnClientConnectAfter : NWNXEventSkippable<OnClientConnectAfter>
    {
      public NwModule Module { get; private set; }

      public string Username { get; private set; }

      public string CDKey { get; private set; }

      public bool DM { get; private set; }

      public string IP { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Module = (NwModule) objSelf;
        Username = EventsPlugin.GetEventData("PLAYER_NAME");
        CDKey = EventsPlugin.GetEventData("CDKEY");
        DM = EventsPlugin.GetEventData("IS_DM").ParseInt().ToBool();
        IP = EventsPlugin.GetEventData("IP_ADDRESS");
      }
    }
  }
}
