using System;
using System.Numerics;
using Anvil.API.Events;

namespace Anvil.API.Events
{
  public sealed class OnDMJumpTargetToPoint : DMEvent
  {
    public NwArea NewArea { get; init; } = null!;

    public Vector3 NewPosition { get; init; }

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
