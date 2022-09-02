using System;
using Anvil.API.Events;

namespace Anvil.API.Events
{
  public sealed class OnDMGiveItem : DMEvent
  {
    public NwGameObject Target { get; internal init; } = null!;
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
