using System;
using Anvil.API.Events;

namespace Anvil.API.Events
{
  /// <summary>
  /// Base event for DM actions targeting a single object.
  /// </summary>
  public abstract class DMSingleTargetEvent : DMEvent
  {
    /// <summary>
    /// Gets the target affected by the action.
    /// </summary>
    public NwObject Target { get; internal init; } = null!;
  }

  /// <summary>
  /// Triggered when a DM teleports/jumps to a specific object.
  /// </summary>
  public sealed class OnDMGoTo : DMSingleTargetEvent;

  /// <summary>
  /// Triggered when a DM possesses a creature.
  /// </summary>
  public sealed class OnDMPossess : DMSingleTargetEvent;

  /// <summary>
  /// Triggered when a DM possesses a creature with full DM powers.
  /// </summary>
  public sealed class OnDMPossessFullPower : DMSingleTargetEvent;

  /// <summary>
  /// Triggered when a DM toggles the lock state of a door/placeable.
  /// </summary>
  public sealed class OnDMToggleLock : DMSingleTargetEvent;

  /// <summary>
  /// Triggered when a DM toggles the trap state of a door/placeable/trigger.
  /// </summary>
  public sealed class OnDMDisableTrap : DMSingleTargetEvent;
}

namespace Anvil.API
{
  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="Events.OnDMDisableTrap"/>
    public event Action<OnDMDisableTrap> OnDMDisableTrap
    {
      add => EventService.Subscribe<OnDMDisableTrap, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMDisableTrap, DMEventFactory>(LoginCreature, value);
    }

    /// <inheritdoc cref="Events.OnDMGoTo"/>
    public event Action<OnDMGoTo> OnDMGoTo
    {
      add => EventService.Subscribe<OnDMGoTo, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMGoTo, DMEventFactory>(LoginCreature, value);
    }

    /// <inheritdoc cref="Events.OnDMPossess"/>
    public event Action<OnDMPossess> OnDMPossess
    {
      add => EventService.Subscribe<OnDMPossess, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMPossess, DMEventFactory>(LoginCreature, value);
    }

    /// <inheritdoc cref="Events.OnDMPossessFullPower"/>
    public event Action<OnDMPossessFullPower> OnDMPossessFullPower
    {
      add => EventService.Subscribe<OnDMPossessFullPower, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMPossessFullPower, DMEventFactory>(LoginCreature, value);
    }

    /// <inheritdoc cref="Events.OnDMToggleLock"/>
    public event Action<OnDMToggleLock> OnDMToggleLock
    {
      add => EventService.Subscribe<OnDMToggleLock, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMToggleLock, DMEventFactory>(LoginCreature, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnDMDisableTrap"/>
    public event Action<OnDMDisableTrap> OnDMDisableTrap
    {
      add => EventService.SubscribeAll<OnDMDisableTrap, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMDisableTrap, DMEventFactory>(value);
    }

    /// <inheritdoc cref="Events.OnDMGoTo"/>
    public event Action<OnDMGoTo> OnDMGoTo
    {
      add => EventService.SubscribeAll<OnDMGoTo, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMGoTo, DMEventFactory>(value);
    }

    /// <inheritdoc cref="Events.OnDMPossess"/>
    public event Action<OnDMPossess> OnDMPossess
    {
      add => EventService.SubscribeAll<OnDMPossess, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMPossess, DMEventFactory>(value);
    }

    /// <inheritdoc cref="Events.OnDMPossessFullPower"/>
    public event Action<OnDMPossessFullPower> OnDMPossessFullPower
    {
      add => EventService.SubscribeAll<OnDMPossessFullPower, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMPossessFullPower, DMEventFactory>(value);
    }

    /// <inheritdoc cref="Events.OnDMToggleLock"/>
    public event Action<OnDMToggleLock> OnDMToggleLock
    {
      add => EventService.SubscribeAll<OnDMToggleLock, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMToggleLock, DMEventFactory>(value);
    }
  }
}
