using NWN.API;
using NWN.API.Events;
using NWN.Core;
using NWN.Core.NWNX;

namespace NWNX.API.Events
{
  public static class ServerSendAreaEvents
  {
    [NWNXEvent("NWNX_ON_SERVER_SEND_AREA_BEFORE")]
    public sealed class OnServerSendAreaBefore : IEvent
    {
      public NwPlayer Player { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlayer>();

      public NwArea Area { get; } = EventsPlugin.GetEventData("AREA").ParseObject<NwArea>();

      /// <summary>
      /// Gets a value indicating whether this is the player's first time logging in to the server since a restart.
      /// </summary>
      public bool NewToModule { get; } = EventsPlugin.GetEventData("PLAYER_NEW_TO_MODULE").ParseInt().ToBool();

      NwObject IEvent.Context => Player;
    }

    [NWNXEvent("NWNX_ON_SERVER_SEND_AREA_AFTER")]
    public sealed class OnServerSendAreaAfter : IEvent
    {
      public NwPlayer Player { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlayer>();

      public NwArea Area { get; } = EventsPlugin.GetEventData("AREA").ParseObject<NwArea>();

      /// <summary>
      /// Gets a value indicating whether this is the player's first time logging in to the server since a restart.
      /// </summary>
      public bool NewToModule { get; } = EventsPlugin.GetEventData("PLAYER_NEW_TO_MODULE").ParseInt().ToBool();

      NwObject IEvent.Context => Player;
    }
  }
}
