using System;
using NWN.API.Events;

namespace NWN.API.Events
{
  public abstract class DMGroupTargetEvent : IEvent
  {
    public NwObject[] Targets { get; internal init; }

    public NwPlayer DungeonMaster { get; internal init; }

    public bool Skip { get; set; }

    NwObject IEvent.Context
    {
      get => DungeonMaster?.LoginCreature;
    }
  }

  public sealed class OnDMHeal : DMGroupTargetEvent {}

  public sealed class OnDMKill : DMGroupTargetEvent {}

  public sealed class OnDMForceRest : DMGroupTargetEvent {}

  public sealed class OnDMToggleInvulnerable : DMGroupTargetEvent {}

  public sealed class OnDMLimbo : DMGroupTargetEvent {}

  public sealed class OnDMToggleAI : DMGroupTargetEvent {}

  public sealed class OnDMToggleImmortal : DMGroupTargetEvent {}
}

namespace NWN.API
{
  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="NWN.API.Events.OnDMHeal"/>
    public event Action<OnDMHeal> OnDMHeal
    {
      add => EventService.Subscribe<OnDMHeal, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMHeal, DMEventFactory>(LoginCreature, value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnDMKill"/>
    public event Action<OnDMKill> OnDMKill
    {
      add => EventService.Subscribe<OnDMKill, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMKill, DMEventFactory>(LoginCreature, value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnDMForceRest"/>
    public event Action<OnDMForceRest> OnDMForceRest
    {
      add => EventService.Subscribe<OnDMForceRest, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMForceRest, DMEventFactory>(LoginCreature, value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnDMToggleInvulnerable"/>
    public event Action<OnDMToggleInvulnerable> OnDMToggleInvulnerable
    {
      add => EventService.Subscribe<OnDMToggleInvulnerable, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMToggleInvulnerable, DMEventFactory>(LoginCreature, value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnDMLimbo"/>
    public event Action<OnDMLimbo> OnDMLimbo
    {
      add => EventService.Subscribe<OnDMLimbo, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMLimbo, DMEventFactory>(LoginCreature, value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnDMToggleAI"/>
    public event Action<OnDMToggleAI> OnDMToggleAI
    {
      add => EventService.Subscribe<OnDMToggleAI, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMToggleAI, DMEventFactory>(LoginCreature, value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnDMToggleImmortal"/>
    public event Action<OnDMToggleImmortal> OnDMToggleImmortal
    {
      add => EventService.Subscribe<OnDMToggleImmortal, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMToggleImmortal, DMEventFactory>(LoginCreature, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="NWN.API.Events.OnDMHeal"/>
    public event Action<OnDMHeal> OnDMHeal
    {
      add => EventService.SubscribeAll<OnDMHeal, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMHeal, DMEventFactory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnDMKill"/>
    public event Action<OnDMKill> OnDMKill
    {
      add => EventService.SubscribeAll<OnDMKill, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMKill, DMEventFactory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnDMForceRest"/>
    public event Action<OnDMForceRest> OnDMForceRest
    {
      add => EventService.SubscribeAll<OnDMForceRest, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMForceRest, DMEventFactory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnDMToggleInvulnerable"/>
    public event Action<OnDMToggleInvulnerable> OnDMToggleInvulnerable
    {
      add => EventService.SubscribeAll<OnDMToggleInvulnerable, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMToggleInvulnerable, DMEventFactory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnDMLimbo"/>
    public event Action<OnDMLimbo> OnDMLimbo
    {
      add => EventService.SubscribeAll<OnDMLimbo, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMLimbo, DMEventFactory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnDMToggleAI"/>
    public event Action<OnDMToggleAI> OnDMToggleAI
    {
      add => EventService.SubscribeAll<OnDMToggleAI, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMToggleAI, DMEventFactory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnDMToggleImmortal"/>
    public event Action<OnDMToggleImmortal> OnDMToggleImmortal
    {
      add => EventService.SubscribeAll<OnDMToggleImmortal, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMToggleImmortal, DMEventFactory>(value);
    }
  }
}
