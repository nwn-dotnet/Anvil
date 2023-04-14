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
    /// Triggered when a <see cref="NwPlayer"/> dies.
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnPlayerDeath)]
    public sealed class OnPlayerDeath : IEvent
    {
      public OnPlayerDeath()
      {
        Killer = NWScript.GetLastHostileActor(DeadPlayer.ControlledCreature).ToNwObject()
          ?? NWScript.GetLastDamager(DeadPlayer.ControlledCreature).ToNwObject();
      }

      /// <summary>
      /// Gets the <see cref="NwPlayer"/> that has triggered the event.
      /// </summary>
      public NwPlayer DeadPlayer { get; } = NWScript.GetLastPlayerDied().ToNwPlayer()!;

      /// <summary>
      /// Gets the <see cref="NwGameObject"/> that caused <see cref="NwPlayer"/> to trigger the event.
      /// </summary>
      public NwObject? Killer { get; }

      NwObject? IEvent.Context => DeadPlayer.ControlledCreature;
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwModule
  {
    /// <inheritdoc cref="ModuleEvents.OnPlayerDeath"/>
    public event Action<ModuleEvents.OnPlayerDeath> OnPlayerDeath
    {
      add => EventService.SubscribeAll<ModuleEvents.OnPlayerDeath, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnPlayerDeath, GameEventFactory>(value);
    }
  }

  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="ModuleEvents.OnPlayerDeath"/>
    public event Action<ModuleEvents.OnPlayerDeath> OnPlayerDeath
    {
      add => EventService.Subscribe<ModuleEvents.OnPlayerDeath, GameEventFactory, GameEventFactory.RegistrationData>(ControlledCreature, new GameEventFactory.RegistrationData(NwModule.Instance), value);
      remove => EventService.Unsubscribe<ModuleEvents.OnPlayerDeath, GameEventFactory>(ControlledCreature, value);
    }
  }
}
