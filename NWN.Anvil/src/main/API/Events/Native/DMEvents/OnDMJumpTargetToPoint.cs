using System;
using System.Numerics;
using Anvil.API.Events;

namespace Anvil.API.Events
{
  /// <summary>
  /// Triggered when a DM jumps one or more targets to a point.
  /// </summary>
  public sealed class OnDMJumpTargetToPoint : DMEvent
  {
    /// <summary>
    /// Gets the destination area.
    /// </summary>
    public NwArea NewArea { get; init; } = null!;

    /// <summary>
    /// Gets the destination position.
    /// </summary>
    public Vector3 NewPosition { get; init; }

    /// <summary>
    /// Gets the targets to be moved.
    /// </summary>
    public NwGameObject[] Targets { get; init; } = null!;
  }
}

namespace Anvil.API
{
  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="Events.OnDMJumpTargetToPoint"/>
    public event Action<OnDMJumpTargetToPoint> OnDMJumpTargetToPoint
    {
      add => EventService.Subscribe<OnDMJumpTargetToPoint, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMJumpTargetToPoint, DMEventFactory>(LoginCreature, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnDMJumpTargetToPoint"/>
    public event Action<OnDMJumpTargetToPoint> OnDMJumpTargetToPoint
    {
      add => EventService.SubscribeAll<OnDMJumpTargetToPoint, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMJumpTargetToPoint, DMEventFactory>(value);
    }
  }
}
