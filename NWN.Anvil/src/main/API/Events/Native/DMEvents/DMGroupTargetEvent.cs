using System;
using Anvil.API.Events;

namespace Anvil.API.Events
{
  /// <summary>
  /// Base event for DM actions targeting one or more objects.
  /// </summary>
  public abstract class DMGroupTargetEvent : DMEvent
  {
    /// <summary>
    /// Gets the targets affected by the action.
    /// </summary>
    public NwObject[] Targets { get; internal init; } = null!;
  }

  /// <summary>
  /// Triggered when a DM heals their current selection of creatures.
  /// </summary>
  public sealed class OnDMHeal : DMGroupTargetEvent;

  /// <summary>
  /// Triggered when a DM kills their current selection of creatures.
  /// </summary>
  public sealed class OnDMKill : DMGroupTargetEvent;

  /// <summary>
  /// Triggered when a DM forces their current selection of creatures to rest.
  /// </summary>
  public sealed class OnDMForceRest : DMGroupTargetEvent;

  /// <summary>
  /// Triggered when a DM toggles the invulnerability state of their current selection of creatures.
  /// </summary>
  public sealed class OnDMToggleInvulnerable : DMGroupTargetEvent;

  /// <summary>
  /// Triggered when a DM moves their current selection of creatures to limbo.
  /// </summary>
  public sealed class OnDMLimbo : DMGroupTargetEvent;

  /// <summary>
  /// Triggered when a DM toggles the AI state of their current selection of creatures.
  /// </summary>
  public sealed class OnDMToggleAI : DMGroupTargetEvent;

  /// <summary>
  /// Triggered when a DM toggles the Immortal state of their current selection of creatures.
  /// </summary>
  public sealed class OnDMToggleImmortal : DMGroupTargetEvent;
}

namespace Anvil.API
{
  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="Events.OnDMForceRest"/>
    public event Action<OnDMForceRest> OnDMForceRest
    {
      add => EventService.Subscribe<OnDMForceRest, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMForceRest, DMEventFactory>(LoginCreature, value);
    }

    /// <inheritdoc cref="Events.OnDMHeal"/>
    public event Action<OnDMHeal> OnDMHeal
    {
      add => EventService.Subscribe<OnDMHeal, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMHeal, DMEventFactory>(LoginCreature, value);
    }

    /// <inheritdoc cref="Events.OnDMKill"/>
    public event Action<OnDMKill> OnDMKill
    {
      add => EventService.Subscribe<OnDMKill, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMKill, DMEventFactory>(LoginCreature, value);
    }

    /// <inheritdoc cref="Events.OnDMLimbo"/>
    public event Action<OnDMLimbo> OnDMLimbo
    {
      add => EventService.Subscribe<OnDMLimbo, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMLimbo, DMEventFactory>(LoginCreature, value);
    }

    /// <inheritdoc cref="Events.OnDMToggleAI"/>
    public event Action<OnDMToggleAI> OnDMToggleAI
    {
      add => EventService.Subscribe<OnDMToggleAI, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMToggleAI, DMEventFactory>(LoginCreature, value);
    }

    /// <inheritdoc cref="Events.OnDMToggleImmortal"/>
    public event Action<OnDMToggleImmortal> OnDMToggleImmortal
    {
      add => EventService.Subscribe<OnDMToggleImmortal, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMToggleImmortal, DMEventFactory>(LoginCreature, value);
    }

    /// <inheritdoc cref="Events.OnDMToggleInvulnerable"/>
    public event Action<OnDMToggleInvulnerable> OnDMToggleInvulnerable
    {
      add => EventService.Subscribe<OnDMToggleInvulnerable, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMToggleInvulnerable, DMEventFactory>(LoginCreature, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnDMForceRest"/>
    public event Action<OnDMForceRest> OnDMForceRest
    {
      add => EventService.SubscribeAll<OnDMForceRest, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMForceRest, DMEventFactory>(value);
    }

    /// <inheritdoc cref="Events.OnDMHeal"/>
    public event Action<OnDMHeal> OnDMHeal
    {
      add => EventService.SubscribeAll<OnDMHeal, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMHeal, DMEventFactory>(value);
    }

    /// <inheritdoc cref="Events.OnDMKill"/>
    public event Action<OnDMKill> OnDMKill
    {
      add => EventService.SubscribeAll<OnDMKill, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMKill, DMEventFactory>(value);
    }

    /// <inheritdoc cref="Events.OnDMLimbo"/>
    public event Action<OnDMLimbo> OnDMLimbo
    {
      add => EventService.SubscribeAll<OnDMLimbo, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMLimbo, DMEventFactory>(value);
    }

    /// <inheritdoc cref="Events.OnDMToggleAI"/>
    public event Action<OnDMToggleAI> OnDMToggleAI
    {
      add => EventService.SubscribeAll<OnDMToggleAI, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMToggleAI, DMEventFactory>(value);
    }

    /// <inheritdoc cref="Events.OnDMToggleImmortal"/>
    public event Action<OnDMToggleImmortal> OnDMToggleImmortal
    {
      add => EventService.SubscribeAll<OnDMToggleImmortal, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMToggleImmortal, DMEventFactory>(value);
    }

    /// <inheritdoc cref="Events.OnDMToggleInvulnerable"/>
    public event Action<OnDMToggleInvulnerable> OnDMToggleInvulnerable
    {
      add => EventService.SubscribeAll<OnDMToggleInvulnerable, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMToggleInvulnerable, DMEventFactory>(value);
    }
  }
}
