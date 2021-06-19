using System;
using System.Numerics;
using NWN.API.Events;

namespace NWN.API.Events
{
  public abstract class DMTeleportEvent : IEvent
  {
    public NwArea TargetArea { get; internal init; }

    public Vector3 TargetPosition { get; internal init; }

    public NwPlayer DungeonMaster { get; internal init; }

    public bool Skip { get; set; }

    NwObject IEvent.Context
    {
      get => DungeonMaster?.LoginCreature;
    }
  }

  public sealed class OnDMJumpToPoint : DMTeleportEvent {}

  public sealed class OnDMJumpAllPlayersToPoint : DMTeleportEvent {}
}

namespace NWN.API
{
  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="NWN.API.Events.OnDMJumpToPoint"/>
    public event Action<OnDMJumpToPoint> OnDMJumpToPoint
    {
      add => EventService.Subscribe<OnDMJumpToPoint, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMJumpToPoint, DMEventFactory>(LoginCreature, value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnDMJumpAllPlayersToPoint"/>
    public event Action<OnDMJumpAllPlayersToPoint> OnDMJumpAllPlayersToPoint
    {
      add => EventService.Subscribe<OnDMJumpAllPlayersToPoint, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMJumpAllPlayersToPoint, DMEventFactory>(LoginCreature, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="NWN.API.Events.OnDMJumpToPoint"/>
    public event Action<OnDMJumpToPoint> OnDMJumpToPoint
    {
      add => EventService.SubscribeAll<OnDMJumpToPoint, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMJumpToPoint, DMEventFactory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnDMJumpAllPlayersToPoint"/>
    public event Action<OnDMJumpAllPlayersToPoint> OnDMJumpAllPlayersToPoint
    {
      add => EventService.SubscribeAll<OnDMJumpAllPlayersToPoint, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMJumpAllPlayersToPoint, DMEventFactory>(value);
    }
  }
}
