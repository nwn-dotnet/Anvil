using NWN.API;
using NWN.Core;
using NWN.Core.NWNX;

namespace NWNX.API.Events
{
  /// <summary>
  /// Item Container Events.
  /// </summary>
  public static partial class ItemEvents
  {
    [NWNXEvent("NWNX_ON_ITEM_INVENTORY_OPEN_BEFORE")]
    public class OnInventoryOpenBefore : EventSkippable<OnInventoryOpenBefore>
    {
      /// <summary>
      /// Gets the container being opened.
      /// </summary>
      public NwItem Container { get; private set; }

      /// <summary>
      /// Gets the container's owner.
      /// </summary>
      public NwCreature Owner { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Container = (NwItem)objSelf;
        Owner = NWScript.StringToObject(EventsPlugin.GetEventData("OWNER")).ToNwObject<NwCreature>();
      }
    }

    [NWNXEvent("NWNX_ON_ITEM_INVENTORY_OPEN_AFTER")]
    public class OnInventoryOpenAfter : EventSkippable<OnInventoryOpenAfter>
    {
      /// <summary>
      /// Gets the container being opened.
      /// </summary>
      public NwItem Container { get; private set; }

      /// <summary>
      /// Gets the container's owner.
      /// </summary>
      public NwCreature Owner { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Container = (NwItem)objSelf;
        Owner = NWScript.StringToObject(EventsPlugin.GetEventData("OWNER")).ToNwObject<NwCreature>();
      }
    }

    [NWNXEvent("NWNX_ON_ITEM_INVENTORY_CLOSE_BEFORE")]
    public class OnInventoryCloseBefore : EventSkippable<OnInventoryCloseBefore>
    {
      /// <summary>
      /// Gets the container being opened.
      /// </summary>
      public NwItem Container { get; private set; }

      /// <summary>
      /// Gets the container's owner.
      /// </summary>
      public NwCreature Owner { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Container = (NwItem)objSelf;
        Owner = NWScript.StringToObject(EventsPlugin.GetEventData("OWNER")).ToNwObject<NwCreature>();
      }
    }

    [NWNXEvent("NWNX_ON_ITEM_INVENTORY_CLOSE_AFTER")]
    public class OnInventoryCloseAfter : EventSkippable<OnInventoryCloseAfter>
    {
      /// <summary>
      /// Gets the container being opened.
      /// </summary>
      public NwItem Container { get; private set; }

      /// <summary>
      /// Gets the container's owner.
      /// </summary>
      public NwCreature Owner { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Container = (NwItem)objSelf;
        Owner = NWScript.StringToObject(EventsPlugin.GetEventData("OWNER")).ToNwObject<NwCreature>();
      }
    }
  }
}
