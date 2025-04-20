using System;
using Anvil.API.Events;

namespace Anvil.API.Events
{
  public abstract class ItemHandlerEvent : IEvent
  {
    /// <summary>
    /// Gets or sets if this event should be skipped.
    /// </summary>
    public bool Skip { get; set; }

    /// <summary>
    /// Gets the item associated with this event.
    /// </summary>
    public NwItem Item { get; set; } = null!;

    NwObject IEvent.Context => Item;
  }

  /// <summary>
  /// Called when an item is destroyed, reducing the stack size of an item.
  /// </summary>
  public sealed class OnItemDecrementStackSize : ItemHandlerEvent;

  /// <summary>
  /// Called when an item is destroyed, deleting the item.
  /// </summary>
  public sealed class OnItemDestroy : ItemHandlerEvent;
}

namespace Anvil.API
{
  public sealed partial class NwItem
  {
    /// <inheritdoc cref="Events.OnItemDecrementStackSize"/>
    public event Action<OnItemDecrementStackSize> OnItemDecrementStackSize
    {
      add => EventService.Subscribe<OnItemDecrementStackSize, ItemEventHandlerEventFactory>(this, value);
      remove => EventService.Unsubscribe<OnItemDecrementStackSize, ItemEventHandlerEventFactory>(this, value);
    }

    /// <inheritdoc cref="Events.OnItemDestroy"/>
    public event Action<OnItemDestroy> OnItemDestroy
    {
      add => EventService.Subscribe<OnItemDestroy, ItemEventHandlerEventFactory>(this, value);
      remove => EventService.Unsubscribe<OnItemDestroy, ItemEventHandlerEventFactory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnItemDecrementStackSize"/>
    public event Action<OnItemDecrementStackSize> OnItemDecrementStackSize
    {
      add => EventService.SubscribeAll<OnItemDecrementStackSize, ItemEventHandlerEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnItemDecrementStackSize, ItemEventHandlerEventFactory>(value);
    }

    /// <inheritdoc cref="Events.OnItemDestroy"/>
    public event Action<OnItemDestroy> OnItemDestroy
    {
      add => EventService.SubscribeAll<OnItemDestroy, ItemEventHandlerEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnItemDestroy, ItemEventHandlerEventFactory>(value);
    }
  }
}
