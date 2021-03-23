using NWN.API;
using NWN.API.Events;
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
    public sealed class OnInventoryOpenBefore : IEventSkippable
    {
      /// <summary>
      /// Gets the container being opened.
      /// </summary>
      public NwItem Container { get; } = (NwItem)NWScript.OBJECT_SELF.ToNwObject<NwItem>();

      /// <summary>
      /// Gets the container's owner.
      /// </summary>
      public NwCreature Owner { get; } = EventsPlugin.GetEventData("OWNER").ParseObject<NwCreature>();

      public bool Skip { get; set; }

      NwObject IEvent.Context => Container;
    }

    [NWNXEvent("NWNX_ON_ITEM_INVENTORY_OPEN_AFTER")]
    public sealed class OnInventoryOpenAfter : IEventSkippable
    {
      /// <summary>
      /// Gets the container being opened.
      /// </summary>
      public NwItem Container { get; }= (NwItem)NWScript.OBJECT_SELF.ToNwObject<NwItem>();

      /// <summary>
      /// Gets the container's owner.
      /// </summary>
      public NwCreature Owner { get; }= EventsPlugin.GetEventData("OWNER").ParseObject<NwCreature>();

      public bool Skip { get; set; }

      NwObject IEvent.Context => Container;
    }

    [NWNXEvent("NWNX_ON_ITEM_INVENTORY_CLOSE_BEFORE")]
    public sealed class OnInventoryCloseBefore : IEventSkippable
    {
      /// <summary>
      /// Gets the container being closed.
      /// </summary>
      public NwItem Container { get; } = (NwItem)NWScript.OBJECT_SELF.ToNwObject<NwItem>();

      /// <summary>
      /// Gets the container's owner.
      /// </summary>
      public NwCreature Owner { get; } = EventsPlugin.GetEventData("OWNER").ParseObject<NwCreature>();

      public bool Skip { get; set; }

      NwObject IEvent.Context => Container;
    }

    [NWNXEvent("NWNX_ON_ITEM_INVENTORY_CLOSE_AFTER")]
    public sealed class OnInventoryCloseAfter : IEventSkippable
    {
      /// <summary>
      /// Gets the container being closed.
      /// </summary>
      public NwItem Container { get; } = (NwItem)NWScript.OBJECT_SELF.ToNwObject<NwItem>();

      /// <summary>
      /// Gets the container's owner.
      /// </summary>
      public NwCreature Owner { get; } = EventsPlugin.GetEventData("OWNER").ParseObject<NwCreature>();

      public bool Skip { get; set; }

      NwObject IEvent.Context => Container;
    }
  }
}
