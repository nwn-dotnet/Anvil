using NWN.API;
using NWN.Core;
using NWN.Core.NWNX;

namespace NWNX.API.Events
{
  /// <summary>
  /// Item Container Events.
  /// </summary>
  public static class ItemContainerEvents
  {
    [NWNXEvent("NWNX_ON_ITEM_INVENTORY_OPEN_BEFORE")]
    public class OnInventoryOpenBefore : EventSkippable<OnInventoryOpenBefore>
    {
      /// <summary>
      /// Gets the Container.
      /// </summary>
      public NwObject Container { get; private set; }

      /// <summary>
      /// Gets the Container owner.
      /// </summary>
      public NwObject Owner { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Container = objSelf;
        Owner = NWScript.StringToObject(EventsPlugin.GetEventData("OWNER")).ToNwObject<NwGameObject>();
      }
    }

    [NWNXEvent("NWNX_ON_ITEM_INVENTORY_OPEN_AFTER")]
    public class OnInventoryOpenAfter : EventSkippable<OnInventoryOpenAfter>
    {
      /// <summary>
      /// Gets the Container.
      /// </summary>
      public NwObject Container { get; private set; }

      /// <summary>
      /// Gets the Container owner.
      /// </summary>
      public NwObject Owner { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Container = objSelf;
        Owner = NWScript.StringToObject(EventsPlugin.GetEventData("OWNER")).ToNwObject<NwGameObject>();
      }
    }

    [NWNXEvent("NWNX_ON_ITEM_INVENTORY_CLOSE_BEFORE")]
    public class OnInventoryCloseBefore : EventSkippable<OnInventoryCloseBefore>
    {
      /// <summary>
      /// Gets the Container.
      /// </summary>
      public NwObject Container { get; private set; }

      /// <summary>
      /// Gets the Container owner.
      /// </summary>
      public NwObject Owner { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Container = objSelf;
        Owner = NWScript.StringToObject(EventsPlugin.GetEventData("OWNER")).ToNwObject<NwGameObject>();
      }
    }

    [NWNXEvent("NWNX_ON_ITEM_INVENTORY_CLOSE_AFTER")]
    public class OnInventoryCloseAfter : EventSkippable<OnInventoryCloseAfter>
    {
      /// <summary>
      /// Gets the Container.
      /// </summary>
      public NwObject Container { get; private set; }

      /// <summary>
      /// Gets the Container owner.
      /// </summary>
      public NwObject Owner { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Container = objSelf;
        Owner = NWScript.StringToObject(EventsPlugin.GetEventData("OWNER")).ToNwObject<NwGameObject>();
      }
    }
  }
}
