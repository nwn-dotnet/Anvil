using NWN.API;
using NWN.Core.NWNX;

namespace NWNX.API.Events
{
  public static class GoldEvents
  {
    [NWNXEvent("NWNX_ON_INVENTORY_ADD_GOLD_BEFORE")]
    public class OnInventoryAddGoldBefore : NWNXEventSkippable<OnInventoryAddGoldBefore>
    {
      public NwPlayer Player { get; private set; }

      public int Gold { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Player = (NwPlayer) objSelf;
        Gold = EventsPlugin.GetEventData("GOLD").ParseInt();
      }
    }

    [NWNXEvent("NWNX_ON_INVENTORY_ADD_GOLD_AFTER")]
    public class OnInventoryAddGoldAfter : NWNXEventSkippable<OnInventoryAddGoldAfter>
    {
      public NwPlayer Player { get; private set; }

      public int Gold { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Player = (NwPlayer) objSelf;
        Gold = EventsPlugin.GetEventData("GOLD").ParseInt();
      }
    }

    [NWNXEvent("NWNX_ON_INVENTORY_REMOVE_GOLD_BEFORE")]
    public class OnInventoryRemoveGoldBefore : NWNXEventSkippable<OnInventoryRemoveGoldBefore>
    {
      public NwPlayer Player { get; private set; }

      public int Gold { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Player = (NwPlayer) objSelf;
        Gold = EventsPlugin.GetEventData("GOLD").ParseInt();
      }
    }

    [NWNXEvent("NWNX_ON_INVENTORY_REMOVE_GOLD_AFTER")]
    public class OnInventoryRemoveGoldAfter : NWNXEventSkippable<OnInventoryRemoveGoldAfter>
    {
      public NwPlayer Player { get; private set; }

      public int Gold { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Player = (NwPlayer) objSelf;
        Gold = EventsPlugin.GetEventData("GOLD").ParseInt();
      }
    }
  }
}
