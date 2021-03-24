using NWN.API;
using NWN.API.Events;
using NWN.Core;
using NWN.Core.NWNX;

namespace NWNX.API.Events
{
  public static class BarterEvents
  {
    [NWNXEvent("NWNX_ON_BARTER_START_BEFORE")]
    public sealed class OnBarterStartBefore : IEventSkippable
    {
      public NwPlayer Player { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlayer>();

      public NwPlayer Target { get; } = EventsPlugin.GetEventData("BARTER_TARGET").ParseObject<NwPlayer>();

      public bool Skip { get; set; }

      NwObject IEvent.Context => Player;
    }

    [NWNXEvent("NWNX_ON_BARTER_START_AFTER")]
    public sealed class OnBarterStartAfter : IEventSkippable
    {
      public NwPlayer Player { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlayer>();

      public NwPlayer Target { get; } = EventsPlugin.GetEventData("BARTER_TARGET").ParseObject<NwPlayer>();

      public bool Skip { get; set; }

      NwObject IEvent.Context => Player;
    }
  }
}
