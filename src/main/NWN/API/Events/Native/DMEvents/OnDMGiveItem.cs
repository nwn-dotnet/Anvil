using System;
using NWN.API.Events;

namespace NWN.API.Events
{
  public sealed class OnDMGiveItem : IEvent
  {
    public NwGameObject Target { get; internal init; }

    public NwItem Item { get; internal init; }

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
    /// <inheritdoc cref="NWN.API.Events.OnDMGiveItem"/>
    public event Action<OnDMGiveItem> OnDMGiveItem
    {
      add => EventService.Subscribe<OnDMGiveItem, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMGiveItem, DMEventFactory>(LoginCreature, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="NWN.API.Events.OnDMGiveItem"/>
    public event Action<OnDMGiveItem> OnDMGiveItem
    {
      add => EventService.SubscribeAll<OnDMGiveItem, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMGiveItem, DMEventFactory>(value);
    }
  }
}
