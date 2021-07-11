using System;
using Anvil.API;
using NWN.API.Constants;
using NWN.API.Events;

namespace NWN.API.Events
{
  public sealed class OnDMGiveAlignment : IEvent
  {
    public Alignment Alignment { get; internal init; }

    public int Amount { get; internal init; }

    public NwObject Target { get; internal init; }

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
    /// <inheritdoc cref="NWN.API.Events.OnDMGiveAlignment"/>
    public event Action<OnDMGiveAlignment> OnDMGiveAlignment
    {
      add => EventService.Subscribe<OnDMGiveAlignment, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMGiveAlignment, DMEventFactory>(LoginCreature, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="NWN.API.Events.OnDMGiveAlignment"/>
    public event Action<OnDMGiveAlignment> OnDMGiveAlignment
    {
      add => EventService.SubscribeAll<OnDMGiveAlignment, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMGiveAlignment, DMEventFactory>(value);
    }
  }
}
