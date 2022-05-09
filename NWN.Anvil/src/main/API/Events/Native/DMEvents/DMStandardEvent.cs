using System;
using Anvil.API.Events;

namespace Anvil.API.Events
{
  public abstract class DMStandardEvent : IEvent
  {
    public NwPlayer DungeonMaster { get; internal init; }

    public bool Skip { get; set; }

    NwObject? IEvent.Context => DungeonMaster?.LoginCreature;
  }

  public sealed class OnDMAppear : DMStandardEvent {}

  public sealed class OnDMDisappear : DMStandardEvent {}

  public sealed class OnDMSetFaction : DMStandardEvent {}

  public sealed class OnDMTakeItem : DMStandardEvent {}

  public sealed class OnDMSetStat : DMStandardEvent {}

  public sealed class OnDMGetVariable : DMStandardEvent {}

  public sealed class OnDMSetVariable : DMStandardEvent {}

  public sealed class OnDMSetTime : DMStandardEvent {}

  public sealed class OnDMSetDate : DMStandardEvent {}

  public sealed class OnDMSetFactionReputation : DMStandardEvent {}

  public sealed class OnDMGetFactionReputation : DMStandardEvent {}

  public sealed class OnDMPlayerDMLogout : DMStandardEvent {}
}

namespace Anvil.API
{
  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="Events.OnDMAppear"/>
    public event Action<OnDMAppear> OnDMAppear
    {
      add => EventService.Subscribe<OnDMAppear, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMAppear, DMEventFactory>(LoginCreature, value);
    }

    /// <inheritdoc cref="Events.OnDMDisappear"/>
    public event Action<OnDMDisappear> OnDMDisappear
    {
      add => EventService.Subscribe<OnDMDisappear, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMDisappear, DMEventFactory>(LoginCreature, value);
    }

    /// <inheritdoc cref="Events.OnDMGetFactionReputation"/>
    public event Action<OnDMGetFactionReputation> OnDMGetFactionReputation
    {
      add => EventService.Subscribe<OnDMGetFactionReputation, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMGetFactionReputation, DMEventFactory>(LoginCreature, value);
    }

    /// <inheritdoc cref="Events.OnDMGetVariable"/>
    public event Action<OnDMGetVariable> OnDMGetVariable
    {
      add => EventService.Subscribe<OnDMGetVariable, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMGetVariable, DMEventFactory>(LoginCreature, value);
    }

    /// <inheritdoc cref="Events.OnDMPlayerDMLogout"/>
    public event Action<OnDMPlayerDMLogout> OnDMPlayerDMLogout
    {
      add => EventService.Subscribe<OnDMPlayerDMLogout, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMPlayerDMLogout, DMEventFactory>(LoginCreature, value);
    }

    /// <inheritdoc cref="Events.OnDMSetDate"/>
    public event Action<OnDMSetDate> OnDMSetDate
    {
      add => EventService.Subscribe<OnDMSetDate, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMSetDate, DMEventFactory>(LoginCreature, value);
    }

    /// <inheritdoc cref="Events.OnDMSetFaction"/>
    public event Action<OnDMSetFaction> OnDMSetFaction
    {
      add => EventService.Subscribe<OnDMSetFaction, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMSetFaction, DMEventFactory>(LoginCreature, value);
    }

    /// <inheritdoc cref="Events.OnDMSetFactionReputation"/>
    public event Action<OnDMSetFactionReputation> OnDMSetFactionReputation
    {
      add => EventService.Subscribe<OnDMSetFactionReputation, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMSetFactionReputation, DMEventFactory>(LoginCreature, value);
    }

    /// <inheritdoc cref="Events.OnDMSetStat"/>
    public event Action<OnDMSetStat> OnDMSetStat
    {
      add => EventService.Subscribe<OnDMSetStat, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMSetStat, DMEventFactory>(LoginCreature, value);
    }

    /// <inheritdoc cref="Events.OnDMSetTime"/>
    public event Action<OnDMSetTime> OnDMSetTime
    {
      add => EventService.Subscribe<OnDMSetTime, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMSetTime, DMEventFactory>(LoginCreature, value);
    }

    /// <inheritdoc cref="Events.OnDMSetVariable"/>
    public event Action<OnDMSetVariable> OnDMSetVariable
    {
      add => EventService.Subscribe<OnDMSetVariable, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMSetVariable, DMEventFactory>(LoginCreature, value);
    }

    /// <inheritdoc cref="Events.OnDMTakeItem"/>
    public event Action<OnDMTakeItem> OnDMTakeItem
    {
      add => EventService.Subscribe<OnDMTakeItem, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMTakeItem, DMEventFactory>(LoginCreature, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnDMAppear"/>
    public event Action<OnDMAppear> OnDMAppear
    {
      add => EventService.SubscribeAll<OnDMAppear, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMAppear, DMEventFactory>(value);
    }

    /// <inheritdoc cref="Events.OnDMDisappear"/>
    public event Action<OnDMDisappear> OnDMDisappear
    {
      add => EventService.SubscribeAll<OnDMDisappear, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMDisappear, DMEventFactory>(value);
    }

    /// <inheritdoc cref="Events.OnDMGetFactionReputation"/>
    public event Action<OnDMGetFactionReputation> OnDMGetFactionReputation
    {
      add => EventService.SubscribeAll<OnDMGetFactionReputation, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMGetFactionReputation, DMEventFactory>(value);
    }

    /// <inheritdoc cref="Events.OnDMGetVariable"/>
    public event Action<OnDMGetVariable> OnDMGetVariable
    {
      add => EventService.SubscribeAll<OnDMGetVariable, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMGetVariable, DMEventFactory>(value);
    }

    /// <inheritdoc cref="Events.OnDMPlayerDMLogout"/>
    public event Action<OnDMPlayerDMLogout> OnDMPlayerDMLogout
    {
      add => EventService.SubscribeAll<OnDMPlayerDMLogout, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMPlayerDMLogout, DMEventFactory>(value);
    }

    /// <inheritdoc cref="Events.OnDMSetDate"/>
    public event Action<OnDMSetDate> OnDMSetDate
    {
      add => EventService.SubscribeAll<OnDMSetDate, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMSetDate, DMEventFactory>(value);
    }

    /// <inheritdoc cref="Events.OnDMSetFaction"/>
    public event Action<OnDMSetFaction> OnDMSetFaction
    {
      add => EventService.SubscribeAll<OnDMSetFaction, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMSetFaction, DMEventFactory>(value);
    }

    /// <inheritdoc cref="Events.OnDMSetFactionReputation"/>
    public event Action<OnDMSetFactionReputation> OnDMSetFactionReputation
    {
      add => EventService.SubscribeAll<OnDMSetFactionReputation, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMSetFactionReputation, DMEventFactory>(value);
    }

    /// <inheritdoc cref="Events.OnDMSetStat"/>
    public event Action<OnDMSetStat> OnDMSetStat
    {
      add => EventService.SubscribeAll<OnDMSetStat, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMSetStat, DMEventFactory>(value);
    }

    /// <inheritdoc cref="Events.OnDMSetTime"/>
    public event Action<OnDMSetTime> OnDMSetTime
    {
      add => EventService.SubscribeAll<OnDMSetTime, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMSetTime, DMEventFactory>(value);
    }

    /// <inheritdoc cref="Events.OnDMSetVariable"/>
    public event Action<OnDMSetVariable> OnDMSetVariable
    {
      add => EventService.SubscribeAll<OnDMSetVariable, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMSetVariable, DMEventFactory>(value);
    }

    /// <inheritdoc cref="Events.OnDMTakeItem"/>
    public event Action<OnDMTakeItem> OnDMTakeItem
    {
      add => EventService.SubscribeAll<OnDMTakeItem, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMTakeItem, DMEventFactory>(value);
    }
  }
}
