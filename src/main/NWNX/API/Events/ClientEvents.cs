using NWN.API;
using NWN.API.Events;
using NWN.Core;
using NWN.Core.NWNX;

namespace NWNX.API.Events
{
  public static class ClientEvents
  {
    [NWNXEvent("NWNX_ON_CLIENT_DISCONNECT_BEFORE")]
    public sealed class OnClientDisconnectBefore : IEventSkippable
    {
      public NwPlayer Player { get; } = (NwPlayer) NWScript.OBJECT_SELF.ToNwObject<NwPlayer>();

      public bool Skip { get; set; }

      NwObject IEvent.Context => Player;
    }

    [NWNXEvent("NWNX_ON_CLIENT_DISCONNECT_AFTER")]
    public sealed class OnClientDisconnectAfter : IEventSkippable
    {
      public NwPlayer Player { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlayer>();

      public bool Skip { get; set; }

      NwObject IEvent.Context => Player;
    }

    [NWNXEvent("NWNX_ON_CLIENT_CONNECT_BEFORE")]
    public sealed class OnClientConnectBefore : IEventSkippable
    {
      public string Username { get; } = EventsPlugin.GetEventData("PLAYER_NAME");

      public string CDKey { get; } = EventsPlugin.GetEventData("CDKEY");

      public bool DM { get; } = EventsPlugin.GetEventData("IS_DM").ParseInt().ToBool();

      public string IP { get; } = EventsPlugin.GetEventData("IP_ADDRESS");

      public bool Skip { get; set; }

      NwObject IEvent.Context => null;
    }

    [NWNXEvent("NWNX_ON_CLIENT_CONNECT_AFTER")]
    public sealed class OnClientConnectAfter : IEventSkippable
    {
      public string Username { get; } = EventsPlugin.GetEventData("PLAYER_NAME");

      public string CDKey { get; } = EventsPlugin.GetEventData("CDKEY");

      public bool DM { get; } = EventsPlugin.GetEventData("IS_DM").ParseInt().ToBool();

      public string IP { get; } = EventsPlugin.GetEventData("IP_ADDRESS");

      public bool Skip { get; set; }

      NwObject IEvent.Context => null;
    }
  }
}
