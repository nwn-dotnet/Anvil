using NWN.API;
using NWN.Core.NWNX;

namespace NWNX.API.Events
{
    public static class GoldEvents
    {
        [NWNXEvent("NWNX_ON_INVENTORY_ADD_GOLD_BEFORE")]
        public class OnInventoryAddGoldBefore : EventSkippable<OnInventoryAddGoldBefore>
        {
            public NwPlayer Player { get; private set; }
            public int Gold { get; private set; }

            protected override void PrepareEvent(NwObject objSelf)
            {
                Player = (NwPlayer)objSelf;
                Gold = EventsPlugin.GetEventData("GOLD").ParseInt();
            }
        }

        [NWNXEvent("NWNX_ON_INVENTORY_ADD_GOLD_AFTER")]
        public class OnInventoryAddGoldAfter : EventSkippable<OnInventoryAddGoldAfter>
        {
            public NwPlayer Player { get; private set; }
            public int Gold { get; private set; }

            protected override void PrepareEvent(NwObject objSelf)
            {
                Player = (NwPlayer)objSelf;
                Gold = EventsPlugin.GetEventData("GOLD").ParseInt();
            }
        }

        [NWNXEvent("NWNX_ON_INVENTORY_REMOVE_GOLD_BEFORE")]
        public class OnInventoryRemoveGoldBefore : EventSkippable<OnInventoryRemoveGoldBefore>
        {
            public NwPlayer Player { get; private set; }
            public int Gold { get; private set; }

            protected override void PrepareEvent(NwObject objSelf)
            {
                Player = (NwPlayer)objSelf;
                Gold = EventsPlugin.GetEventData("GOLD").ParseInt();
            }
        }

        [NWNXEvent("NWNX_ON_INVENTORY_REMOVE_GOLD_AFTER")]
        public class OnInventoryRemoveGoldAfter : EventSkippable<OnInventoryRemoveGoldAfter>
        {
            public NwPlayer Player { get; private set; }
            public int Gold { get; private set; }

            protected override void PrepareEvent(NwObject objSelf)
            {
                Player = (NwPlayer)objSelf;
                Gold = EventsPlugin.GetEventData("GOLD").ParseInt();
            }
        }
    }
}
