using System;
using Anvil.API.Events;

namespace Anvil.API.Events
{
  /// <summary>
  /// Triggered when a DM opens or closes a target's inventory.
  /// </summary>
  public sealed class OnDMViewInventory : DMEvent
  {
    /// <summary>
    /// Gets a value indicating whether the inventory is opening (true) or closing (false).
    /// </summary>
    public bool IsOpening { get; internal init; }

    /// <summary>
    /// Gets the target whose inventory is viewed.
    /// </summary>
    public NwGameObject Target { get; internal init; } = null!;
  }
}

namespace Anvil.API
{
  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="Events.OnDMViewInventory"/>
    public event Action<OnDMViewInventory> OnDMViewInventory
    {
      add => EventService.Subscribe<OnDMViewInventory, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMViewInventory, DMEventFactory>(LoginCreature, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnDMViewInventory"/>
    public event Action<OnDMViewInventory> OnDMViewInventory
    {
      add => EventService.SubscribeAll<OnDMViewInventory, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMViewInventory, DMEventFactory>(value);
    }
  }
}
