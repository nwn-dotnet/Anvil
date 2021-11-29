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
    /// Triggered when a <see cref="NwPlayer"/> that has targeted something.
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnPlayerTarget)]
    public sealed class OnPlayerTarget : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwPlayer"/> that has targeted something.
      /// </summary>
      public NwPlayer Player { get; } = NWScript.GetLastPlayerToSelectTarget().ToNwPlayer();

      /// <summary>
      /// Gets the <see cref="NwObject"/> that has been targeted by <see cref="Player"/>, otherwise the area if a position was selected.
      /// </summary>
      public NwObject TargetObject { get; } = NWScript.GetTargetingModeSelectedObject().ToNwObject();

      /// <summary>
      /// Gets the position targeted by the <see cref="NwPlayer"/>.
      /// </summary>
      public Vector3 TargetPosition { get; } = NWScript.GetTargetingModeSelectedPosition();

      /// <summary>
      /// Gets if the player cancelled target selection.
      /// </summary>
      public bool IsCancelled
      {
        get => TargetObject == null;
      }

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
    /// <inheritdoc cref="ModuleEvents.OnPlayerTarget"/>
    public event Action<ModuleEvents.OnPlayerTarget> OnPlayerTarget
    {
      add => EventService.SubscribeAll<ModuleEvents.OnPlayerTarget, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnPlayerTarget, GameEventFactory>(value);
    }
  }

  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="ModuleEvents.OnPlayerTarget"/>
    public event Action<ModuleEvents.OnPlayerTarget> OnPlayerTarget
    {
      add => EventService.Subscribe<ModuleEvents.OnPlayerTarget, GameEventFactory, GameEventFactory.RegistrationData>(ControlledCreature, new GameEventFactory.RegistrationData(NwModule.Instance), value);
      remove => EventService.Unsubscribe<ModuleEvents.OnPlayerTarget, GameEventFactory>(ControlledCreature, value);
    }
  }
}
