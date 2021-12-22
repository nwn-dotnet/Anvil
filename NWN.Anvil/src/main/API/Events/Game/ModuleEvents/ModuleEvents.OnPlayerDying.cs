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
    /// Triggered when a <see cref="NwPlayer"/> enters a dying state (&lt; 0 HP).
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnPlayerDying)]
    public sealed class OnPlayerDying : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwPlayer"/> that has triggered the event.
      /// </summary>
      public NwPlayer Player { get; } = NWScript.GetLastPlayerDying().ToNwPlayer();

      NwObject IEvent.Context => Player.ControlledCreature;
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwModule
  {
    /// <inheritdoc cref="ModuleEvents.OnPlayerDying"/>
    public event Action<ModuleEvents.OnPlayerDying> OnPlayerDying
    {
      add => EventService.SubscribeAll<ModuleEvents.OnPlayerDying, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnPlayerDying, GameEventFactory>(value);
    }
  }

  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="ModuleEvents.OnPlayerDying"/>
    public event Action<ModuleEvents.OnPlayerDying> OnPlayerDying
    {
      add => EventService.Subscribe<ModuleEvents.OnPlayerDying, GameEventFactory, GameEventFactory.RegistrationData>(ControlledCreature, new GameEventFactory.RegistrationData(NwModule.Instance), value);
      remove => EventService.Unsubscribe<ModuleEvents.OnPlayerDying, GameEventFactory>(ControlledCreature, value);
    }
  }
}
