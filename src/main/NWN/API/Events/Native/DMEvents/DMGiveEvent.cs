using System;
using NWN.API.Events;

namespace NWN.API.Events
{
  public abstract class DMGiveEvent : IEvent
  {
    public NwGameObject Target { get; internal init; }

    public int Amount { get; internal init; }

    public NwPlayer DungeonMaster { get; internal init; }

    public bool Skip { get; set; }

    NwObject IEvent.Context
    {
      get => DungeonMaster?.LoginCreature;
    }
  }

  public sealed class OnDMGiveXP : DMGiveEvent {}

  public sealed class OnDMGiveLevel : DMGiveEvent {}

  public sealed class OnDMGiveGold : DMGiveEvent {}
}

namespace NWN.API
{
  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="NWN.API.Events.OnDMGiveXP"/>
    public event Action<OnDMGiveXP> OnDMGiveXP
    {
      add => EventService.Subscribe<OnDMGiveXP, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMGiveXP, DMEventFactory>(LoginCreature, value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnDMGiveLevel"/>
    public event Action<OnDMGiveLevel> OnDMGiveLevel
    {
      add => EventService.Subscribe<OnDMGiveLevel, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMGiveLevel, DMEventFactory>(LoginCreature, value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnDMGiveGold"/>
    public event Action<OnDMGiveGold> OnDMGiveGold
    {
      add => EventService.Subscribe<OnDMGiveGold, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMGiveGold, DMEventFactory>(LoginCreature, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="NWN.API.Events.OnDMGiveXP"/>
    public event Action<OnDMGiveXP> OnDMGiveXP
    {
      add => EventService.SubscribeAll<OnDMGiveXP, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMGiveXP, DMEventFactory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnDMGiveLevel"/>
    public event Action<OnDMGiveLevel> OnDMGiveLevel
    {
      add => EventService.SubscribeAll<OnDMGiveLevel, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMGiveLevel, DMEventFactory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnDMGiveGold"/>
    public event Action<OnDMGiveGold> OnDMGiveGold
    {
      add => EventService.SubscribeAll<OnDMGiveGold, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMGiveGold, DMEventFactory>(value);
    }
  }
}
