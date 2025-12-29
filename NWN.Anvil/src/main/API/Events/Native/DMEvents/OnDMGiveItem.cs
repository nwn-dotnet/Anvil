using System;
using Anvil.API.Events;

namespace Anvil.API.Events
{
  /// <summary>
  /// Triggered when a DM gives an item to a target.
  /// </summary>
  public sealed class OnDMGiveItem : DMEvent
  {
    /// <summary>
    /// Gets the target that will receive the item.
    /// </summary>
    public NwGameObject Target { get; internal init; } = null!;

    /// <summary>
    /// Gets or sets the item to give. Set to null to cancel.
    /// </summary>
    public NwItem? Item { get; internal set; }
  }
}

namespace Anvil.API
{
  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="Events.OnDMGiveItem"/>
    public event Action<OnDMGiveItem> OnDMGiveItem
    {
      add => EventService.Subscribe<OnDMGiveItem, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMGiveItem, DMEventFactory>(LoginCreature, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnDMGiveItem"/>
    public event Action<OnDMGiveItem> OnDMGiveItem
    {
      add => EventService.SubscribeAll<OnDMGiveItem, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMGiveItem, DMEventFactory>(value);
    }
  }
}
