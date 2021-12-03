using System;
using Anvil.API.Events;

namespace Anvil.API.Events
{
  public sealed class OnDMViewInventory : IEvent
  {
    public NwPlayer DungeonMaster { get; internal init; }
    public bool IsOpening { get; internal init; }

    public bool Skip { get; set; }

    public NwGameObject Target { get; internal init; }

    NwObject IEvent.Context
    {
      get => DungeonMaster?.LoginCreature;
    }
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
