using System;
using Anvil.API.Events;

namespace Anvil.API.Events
{
  /// <summary>
  /// Base event for DM actions that grant XP, levels, or gold to a target.
  /// </summary>
  public abstract class DMGiveEvent : DMEvent
  {
    /// <summary>
    /// Gets the amount to grant (XP, levels, or gold depending on context).
    /// </summary>
    public int Amount { get; internal init; }

    /// <summary>
    /// Gets the target that will receive the grant.
    /// </summary>
    public NwGameObject Target { get; internal init; } = null!;
  }

  /// <summary>
  /// Triggered when a DM gives XP to a player.
  /// </summary>
  public sealed class OnDMGiveXP : DMGiveEvent;

  /// <summary>
  /// Triggered when a DM gives a level to a player.
  /// </summary>
  public sealed class OnDMGiveLevel : DMGiveEvent;

  /// <summary>
  /// Triggered when a DM gives gold to a player.
  /// </summary>
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
