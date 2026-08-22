using System;
using Anvil.API.Events;

namespace Anvil.API.Events
{
  /// <summary>
  /// Triggered when a DM appears in the game world, becoming visible to all other players.
  /// </summary>
  public sealed class OnDMAppear : DMEvent;

  /// <summary>
  /// Triggered when a DM disappears from the game world, becoming invisible to all other players.
  /// </summary>
  public sealed class OnDMDisappear : DMEvent;

  /// <summary>
  /// Triggered when a DM sets an object's faction.
  /// </summary>
  public sealed class OnDMSetFaction : DMEvent;

  /// <summary>
  /// Triggered when a DM takes an item from an object.
  /// </summary>
  public sealed class OnDMTakeItem : DMEvent;

  /// <summary>
  /// Triggered when a DM sets an ability score on a creature.
  /// </summary>
  public sealed class OnDMSetStat : DMEvent;

  /// <summary>
  /// Triggered when a DM reads a variable from an object.
  /// </summary>
  public sealed class OnDMGetVariable : DMEvent;

  /// <summary>
  /// Triggered when a DM sets a variable on an object.
  /// </summary>
  public sealed class OnDMSetVariable : DMEvent;

  /// <summary>
  /// Triggered when a DM modifies the module time.
  /// </summary>
  public sealed class OnDMSetTime : DMEvent;

  /// <summary>
  /// Triggered when a DM modifies the module date.
  /// </summary>
  public sealed class OnDMSetDate : DMEvent;

  /// <summary>
  /// Triggered when a DM sets module-wide faction reputation values.
  /// </summary>
  public sealed class OnDMSetFactionReputation : DMEvent;

  /// <summary>
  /// Triggered when a DM queries module-wide faction reputation values.
  /// </summary>
  public sealed class OnDMGetFactionReputation : DMEvent;

  /// <summary>
  /// Triggered when a temporary player DM logs out, and becomes a regular player.
  /// </summary>
  /// <seealso cref="NwPlayer.IsPlayerDM"/>
  public sealed class OnDMPlayerDMLogout : DMEvent;
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
