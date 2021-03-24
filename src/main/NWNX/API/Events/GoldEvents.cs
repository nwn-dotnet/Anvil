using NWN.API;
using NWN.API.Events;
using NWN.Core;
using NWN.Core.NWNX;

namespace NWNX.API.Events
{
  public static class GoldEvents
  {
    [NWNXEvent("NWNX_ON_INVENTORY_ADD_GOLD_BEFORE")]
    public sealed class OnInventoryAddGoldBefore : IEventSkippable
    {
      public NwPlayer Player { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlayer>();

      public int Gold { get; } = EventsPlugin.GetEventData("GOLD").ParseInt();

      public bool Skip { get; set; }

      NwObject IEvent.Context => Player;
    }

    [NWNXEvent("NWNX_ON_INVENTORY_ADD_GOLD_AFTER")]
    public sealed class OnInventoryAddGoldAfter : IEventSkippable
    {
      public NwPlayer Player { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlayer>();

      public int Gold { get; } = EventsPlugin.GetEventData("GOLD").ParseInt();

      public bool Skip { get; set; }

      NwObject IEvent.Context => Player;
    }

    [NWNXEvent("NWNX_ON_INVENTORY_REMOVE_GOLD_BEFORE")]
    public sealed class OnInventoryRemoveGoldBefore : IEventSkippable
    {
      public NwPlayer Player { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlayer>();

      public int Gold { get; } = EventsPlugin.GetEventData("GOLD").ParseInt();

      public bool Skip { get; set; }

      NwObject IEvent.Context => Player;
    }

    [NWNXEvent("NWNX_ON_INVENTORY_REMOVE_GOLD_AFTER")]
    public sealed class OnInventoryRemoveGoldAfter : IEventSkippable
    {
      public NwPlayer Player { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlayer>();

      public int Gold { get; } = EventsPlugin.GetEventData("GOLD").ParseInt();

      public bool Skip { get; set; }

      NwObject IEvent.Context => Player;
    }
  }
}
