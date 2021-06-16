using System;
using System.Numerics;
using NWN.API.Events;

namespace NWN.API.Events
{
  public class OnDMJumpTargetToPoint : IEvent
  {
    public NwArea NewArea { get; init; }

    public Vector3 NewPosition { get; init; }

    public NwGameObject[] Targets { get; init; }

    public NwPlayer DungeonMaster { get; init; }

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
    /// <inheritdoc cref="NWN.API.Events.OnDMJumpTargetToPoint"/>
    public event Action<OnDMJumpTargetToPoint> OnDMJumpTargetToPoint
    {
      add => EventService.Subscribe<OnDMJumpTargetToPoint, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMJumpTargetToPoint, DMEventFactory>(LoginCreature, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="NWN.API.Events.OnDMJumpTargetToPoint"/>
    public event Action<OnDMJumpTargetToPoint> OnDMJumpTargetToPoint
    {
      add => EventService.SubscribeAll<OnDMJumpTargetToPoint, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMJumpTargetToPoint, DMEventFactory>(value);
    }
  }
}
