using NWN.API;
using NWN.Core;
using NWN.Core.NWNX;

namespace NWNX.API.Events
{
  public static class InventoryEvents
  {
    [NWNXEvent("NWNX_ON_ITEM_INVENTORY_OPEN_BEFORE")]
    public class OnItemInventoryOpenBefore : EventSkippable<OnItemInventoryOpenBefore>
    {
      public NwObject Container { get; private set; }

      public NwObject Owner { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Container = objSelf;
        Owner = NWScript.StringToObject(EventsPlugin.GetEventData("OWNER")).ToNwObject<NwGameObject>();
      }
    }

    [NWNXEvent("NWNX_ON_ITEM_INVENTORY_OPEN_AFTER")]
    public class OnItemInventoryOpenAfter : EventSkippable<OnItemInventoryOpenAfter>
    {
      public NwObject Container { get; private set; }

      public NwObject Owner { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Container = objSelf;
        Owner = NWScript.StringToObject(EventsPlugin.GetEventData("OWNER")).ToNwObject<NwGameObject>();
      }
    }

    [NWNXEvent("NWNX_ON_ITEM_INVENTORY_CLOSE_BEFORE")]
    public class OnItemInventoryCloseBefore : EventSkippable<OnItemInventoryCloseBefore>
    {
      public NwObject Container { get; private set; }

      public NwObject Owner { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Container = objSelf;
        Owner = NWScript.StringToObject(EventsPlugin.GetEventData("OWNER")).ToNwObject<NwGameObject>();
      }
    }

    [NWNXEvent("NWNX_ON_ITEM_INVENTORY_CLOSE_AFTER")]
    public class OnItemInventoryCloseAfter : EventSkippable<OnItemInventoryCloseAfter>
    {
      public NwObject Container { get; private set; }

      public NwObject Owner { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Container = objSelf;
        Owner = NWScript.StringToObject(EventsPlugin.GetEventData("OWNER")).ToNwObject<NwGameObject>();
      }
    }
  }
}
