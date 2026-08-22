using System;
using System.Numerics;
using Anvil.API.Events;

namespace Anvil.API.Events
{
  /// <summary>
  /// Base event for DM teleport actions.
  /// </summary>
  public abstract class DMTeleportEvent : DMEvent
  {
    /// <summary>
    /// Gets the destination area.
    /// </summary>
    public NwArea TargetArea { get; internal init; } = null!;

    /// <summary>
    /// Gets the destination position.
    /// </summary>
    public Vector3 TargetPosition { get; internal init; }
  }

  /// <summary>
  /// Triggered when a DM teleports to a specific location.
  /// </summary>
  public sealed class OnDMJumpToPoint : DMTeleportEvent;

  /// <summary>
  /// Triggered when a DM teleports all online players in the module to a specific location.
  /// </summary>
  public sealed class OnDMJumpAllPlayersToPoint : DMTeleportEvent;
}

namespace Anvil.API
{
  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="Events.OnDMJumpAllPlayersToPoint"/>
    public event Action<OnDMJumpAllPlayersToPoint> OnDMJumpAllPlayersToPoint
    {
      add => EventService.Subscribe<OnDMJumpAllPlayersToPoint, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMJumpAllPlayersToPoint, DMEventFactory>(LoginCreature, value);
    }

    /// <inheritdoc cref="Events.OnDMJumpToPoint"/>
    public event Action<OnDMJumpToPoint> OnDMJumpToPoint
    {
      add => EventService.Subscribe<OnDMJumpToPoint, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMJumpToPoint, DMEventFactory>(LoginCreature, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnDMJumpAllPlayersToPoint"/>
    public event Action<OnDMJumpAllPlayersToPoint> OnDMJumpAllPlayersToPoint
    {
      add => EventService.SubscribeAll<OnDMJumpAllPlayersToPoint, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMJumpAllPlayersToPoint, DMEventFactory>(value);
    }

    /// <inheritdoc cref="Events.OnDMJumpToPoint"/>
    public event Action<OnDMJumpToPoint> OnDMJumpToPoint
    {
      add => EventService.SubscribeAll<OnDMJumpToPoint, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMJumpToPoint, DMEventFactory>(value);
    }
  }
}
