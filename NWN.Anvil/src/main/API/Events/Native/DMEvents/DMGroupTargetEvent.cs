using System;
using Anvil.API.Events;

namespace Anvil.API.Events
{
  public abstract class DMGroupTargetEvent : IEvent
  {
    public NwPlayer DungeonMaster { get; internal init; }

    public bool Skip { get; set; }
    public NwObject[] Targets { get; internal init; }

    NwObject? IEvent.Context => DungeonMaster?.LoginCreature;
  }

  public sealed class OnDMHeal : DMGroupTargetEvent {}

  public sealed class OnDMKill : DMGroupTargetEvent {}

  public sealed class OnDMForceRest : DMGroupTargetEvent {}

  public sealed class OnDMToggleInvulnerable : DMGroupTargetEvent {}

  public sealed class OnDMLimbo : DMGroupTargetEvent {}

  public sealed class OnDMToggleAI : DMGroupTargetEvent {}

  public sealed class OnDMToggleImmortal : DMGroupTargetEvent {}
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
