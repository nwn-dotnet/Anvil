using System;
using Anvil.API.Events;

namespace Anvil.API.Events
{
  public abstract class DMGiveEvent : DMEvent
  {
    public int Amount { get; internal init; }

    public NwGameObject Target { get; internal init; } = null!;
  }

  public sealed class OnDMGiveXP : DMGiveEvent;

  public sealed class OnDMGiveLevel : DMGiveEvent;

  public sealed class OnDMGiveGold : DMGiveEvent;
}

namespace Anvil.API
{
  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="Events.OnDMGiveGold"/>
    public event Action<OnDMGiveGold> OnDMGiveGold
    {
      add => EventService.Subscribe<OnDMGiveGold, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMGiveGold, DMEventFactory>(LoginCreature, value);
    }

    /// <inheritdoc cref="Events.OnDMGiveLevel"/>
    public event Action<OnDMGiveLevel> OnDMGiveLevel
    {
      add => EventService.Subscribe<OnDMGiveLevel, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMGiveLevel, DMEventFactory>(LoginCreature, value);
    }

    /// <inheritdoc cref="Events.OnDMGiveXP"/>
    public event Action<OnDMGiveXP> OnDMGiveXP
    {
      add => EventService.Subscribe<OnDMGiveXP, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMGiveXP, DMEventFactory>(LoginCreature, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnDMGiveGold"/>
    public event Action<OnDMGiveGold> OnDMGiveGold
    {
      add => EventService.SubscribeAll<OnDMGiveGold, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMGiveGold, DMEventFactory>(value);
    }

    /// <inheritdoc cref="Events.OnDMGiveLevel"/>
    public event Action<OnDMGiveLevel> OnDMGiveLevel
    {
      add => EventService.SubscribeAll<OnDMGiveLevel, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMGiveLevel, DMEventFactory>(value);
    }

    /// <inheritdoc cref="Events.OnDMGiveXP"/>
    public event Action<OnDMGiveXP> OnDMGiveXP
    {
      add => EventService.SubscribeAll<OnDMGiveXP, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMGiveXP, DMEventFactory>(value);
    }
  }
}
