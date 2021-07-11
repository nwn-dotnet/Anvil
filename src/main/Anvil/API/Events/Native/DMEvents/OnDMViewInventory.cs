using System;
using NWN.API.Events;

namespace NWN.API.Events
{
  public sealed class OnDMViewInventory : IEvent
  {
    public bool IsOpening { get; internal init; }

    public NwGameObject Target { get; internal init; }

    public NwPlayer DungeonMaster { get; internal init; }

    public bool Skip { get; set; }

    NwObject IEvent.Context
    {
      get => DungeonMaster?.LoginCreature;
    }
  }
}

namespace NWN.API
{
  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="NWN.API.Events.OnDMViewInventory"/>
    public event Action<OnDMViewInventory> OnDMViewInventory
    {
      add => EventService.Subscribe<OnDMViewInventory, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMViewInventory, DMEventFactory>(LoginCreature, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="NWN.API.Events.OnDMViewInventory"/>
    public event Action<OnDMViewInventory> OnDMViewInventory
    {
      add => EventService.SubscribeAll<OnDMViewInventory, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMViewInventory, DMEventFactory>(value);
    }
  }
}
