using System;
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
    /// Triggered when a <see cref="NwPlayer"/> levels up.
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnPlayerLevelUp)]
    public sealed class OnPlayerLevelUp : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwPlayer"/> that has triggered the event.
      /// </summary>
      public NwPlayer Player { get; } = NWScript.GetPCLevellingUp().ToNwPlayer()!;

      NwObject? IEvent.Context => Player.ControlledCreature;
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwModule
  {
    /// <inheritdoc cref="ModuleEvents.OnPlayerLevelUp"/>
    public event Action<ModuleEvents.OnPlayerLevelUp> OnPlayerLevelUp
    {
      add => EventService.SubscribeAll<ModuleEvents.OnPlayerLevelUp, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnPlayerLevelUp, GameEventFactory>(value);
    }
  }

  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="ModuleEvents.OnPlayerLevelUp"/>
    public event Action<ModuleEvents.OnPlayerLevelUp> OnPlayerLevelUp
    {
      add => EventService.Subscribe<ModuleEvents.OnPlayerLevelUp, GameEventFactory, GameEventFactory.RegistrationData>(ControlledCreature, new GameEventFactory.RegistrationData(NwModule.Instance), value);
      remove => EventService.Unsubscribe<ModuleEvents.OnPlayerLevelUp, GameEventFactory>(ControlledCreature, value);
    }
  }
}
