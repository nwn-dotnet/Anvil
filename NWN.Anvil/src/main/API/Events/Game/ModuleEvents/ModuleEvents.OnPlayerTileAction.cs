using System;
using System.Numerics;
using Anvil.API.Events;
using NWN.Core;

namespace Anvil.API.Events
{
  /// <summary>
  /// Global module events.
  /// </summary>
  public static partial class ModuleEvents
  {
    /// <summary>
    /// Triggered when a player performs an action on an area tile.
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnPlayerTileAction)]
    public sealed class OnPlayerTileAction : IEvent
    {
      /// <summary>
      /// Gets the action ID (surfacemat.2da) that was selected by the player.
      /// </summary>
      public int ActionId { get; } = NWScript.GetLastTileActionId();

      /// <summary>
      /// Gets the <see cref="NwPlayer"/> that performed a tile action.
      /// </summary>
      public NwPlayer Player { get; } = NWScript.GetLastPlayerToDoTileAction().ToNwPlayer();

      /// <summary>
      /// Gets the position that was clicked.
      /// </summary>
      public Vector3 TargetPosition { get; } = NWScript.GetLastTileActionPosition();

      NwObject IEvent.Context
      {
        get => Player.ControlledCreature;
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwModule
  {
    /// <inheritdoc cref="ModuleEvents.OnPlayerTileAction"/>
    public event Action<ModuleEvents.OnPlayerTileAction> OnPlayerTileAction
    {
      add => EventService.SubscribeAll<ModuleEvents.OnPlayerTileAction, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnPlayerTileAction, GameEventFactory>(value);
    }
  }

  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="ModuleEvents.OnPlayerTileAction"/>
    public event Action<ModuleEvents.OnPlayerTileAction> OnPlayerTileAction
    {
      add => EventService.Subscribe<ModuleEvents.OnPlayerTileAction, GameEventFactory, GameEventFactory.RegistrationData>(ControlledCreature, new GameEventFactory.RegistrationData(NwModule.Instance), value);
      remove => EventService.Unsubscribe<ModuleEvents.OnPlayerTileAction, GameEventFactory>(ControlledCreature, value);
    }
  }
}
