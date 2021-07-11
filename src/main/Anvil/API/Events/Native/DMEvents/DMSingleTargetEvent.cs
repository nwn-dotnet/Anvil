using System;
using NWN.API.Events;

namespace NWN.API.Events
{
  public abstract class DMSingleTargetEvent : IEvent
  {
    public NwObject Target { get; internal init; }

    public NwPlayer DungeonMaster { get; internal init; }

    public bool Skip { get; set; }

    NwObject IEvent.Context
    {
      get => DungeonMaster?.LoginCreature;
    }
  }

  public sealed class OnDMGoTo : DMSingleTargetEvent {}

  public sealed class OnDMPossess : DMSingleTargetEvent {}

  public sealed class OnDMPossessFullPower : DMSingleTargetEvent {}

  public sealed class OnDMToggleLock : DMSingleTargetEvent {}

  public sealed class OnDMDisableTrap : DMSingleTargetEvent {}
}

namespace NWN.API
{
  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="NWN.API.Events.OnDMGoTo"/>
    public event Action<OnDMGoTo> OnDMGoTo
    {
      add => EventService.Subscribe<OnDMGoTo, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMGoTo, DMEventFactory>(LoginCreature, value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnDMPossess"/>
    public event Action<OnDMPossess> OnDMPossess
    {
      add => EventService.Subscribe<OnDMPossess, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMPossess, DMEventFactory>(LoginCreature, value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnDMPossessFullPower"/>
    public event Action<OnDMPossessFullPower> OnDMPossessFullPower
    {
      add => EventService.Subscribe<OnDMPossessFullPower, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMPossessFullPower, DMEventFactory>(LoginCreature, value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnDMToggleLock"/>
    public event Action<OnDMToggleLock> OnDMToggleLock
    {
      add => EventService.Subscribe<OnDMToggleLock, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMToggleLock, DMEventFactory>(LoginCreature, value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnDMDisableTrap"/>
    public event Action<OnDMDisableTrap> OnDMDisableTrap
    {
      add => EventService.Subscribe<OnDMDisableTrap, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMDisableTrap, DMEventFactory>(LoginCreature, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="NWN.API.Events.OnDMGoTo"/>
    public event Action<OnDMGoTo> OnDMGoTo
    {
      add => EventService.SubscribeAll<OnDMGoTo, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMGoTo, DMEventFactory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnDMPossess"/>
    public event Action<OnDMPossess> OnDMPossess
    {
      add => EventService.SubscribeAll<OnDMPossess, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMPossess, DMEventFactory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnDMPossessFullPower"/>
    public event Action<OnDMPossessFullPower> OnDMPossessFullPower
    {
      add => EventService.SubscribeAll<OnDMPossessFullPower, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMPossessFullPower, DMEventFactory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnDMToggleLock"/>
    public event Action<OnDMToggleLock> OnDMToggleLock
    {
      add => EventService.SubscribeAll<OnDMToggleLock, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMToggleLock, DMEventFactory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnDMDisableTrap"/>
    public event Action<OnDMDisableTrap> OnDMDisableTrap
    {
      add => EventService.SubscribeAll<OnDMDisableTrap, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMDisableTrap, DMEventFactory>(value);
    }
  }
}
