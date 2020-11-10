using NWN.API;
using NWN.Core.NWNX;

namespace NWNX.API.Events
{
  /// <summary>
  /// Item Container Events.
  /// </summary>
  public static partial class ItemEvents
  {
    [NWNXEvent("NWNX_ON_ITEM_INVENTORY_OPEN_BEFORE")]
    public class OnInventoryOpenBefore : NWNXEventSkippable<OnInventoryOpenBefore>
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
        Owner = EventsPlugin.GetEventData("OWNER").ParseObject<NwCreature>();
      }
    }

    [NWNXEvent("NWNX_ON_ITEM_INVENTORY_OPEN_AFTER")]
    public class OnInventoryOpenAfter : NWNXEventSkippable<OnInventoryOpenAfter>
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
        Owner = EventsPlugin.GetEventData("OWNER").ParseObject<NwCreature>();
      }
    }

    [NWNXEvent("NWNX_ON_ITEM_INVENTORY_CLOSE_BEFORE")]
    public class OnInventoryCloseBefore : NWNXEventSkippable<OnInventoryCloseBefore>
    {
      /// <summary>
      /// Gets the container being closed.
      /// </summary>
      public NwItem Container { get; private set; }

      /// <summary>
      /// Gets the container's owner.
      /// </summary>
      public NwCreature Owner { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Container = (NwItem)objSelf;
        Owner = EventsPlugin.GetEventData("OWNER").ParseObject<NwCreature>();
      }
    }

    [NWNXEvent("NWNX_ON_ITEM_INVENTORY_CLOSE_AFTER")]
    public class OnInventoryCloseAfter : NWNXEventSkippable<OnInventoryCloseAfter>
    {
      /// <summary>
      /// Gets the container being closed.
      /// </summary>
      public NwItem Container { get; private set; }

      /// <summary>
      /// Gets the container's owner.
      /// </summary>
      public NwCreature Owner { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Container = (NwItem)objSelf;
        Owner = EventsPlugin.GetEventData("OWNER").ParseObject<NwCreature>();
      }
    }
  }
}
