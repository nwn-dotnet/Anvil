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
    /// Triggered when a <see cref="NwPlayer"/> clicks the respawn button on the death screen.
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnRespawnButtonPressed)]
    public sealed class OnPlayerRespawn : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwPlayer"/> that clicked the respawn button on the death screen.
      /// </summary>
      public NwPlayer Player { get; } = NWScript.GetLastRespawnButtonPresser().ToNwPlayer()!;

      NwObject? IEvent.Context => Player.ControlledCreature;
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwModule
  {
    /// <inheritdoc cref="ModuleEvents.OnPlayerRespawn"/>
    public event Action<ModuleEvents.OnPlayerRespawn> OnPlayerRespawn
    {
      add => EventService.SubscribeAll<ModuleEvents.OnPlayerRespawn, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnPlayerRespawn, GameEventFactory>(value);
    }
  }

  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="ModuleEvents.OnPlayerRespawn"/>
    public event Action<ModuleEvents.OnPlayerRespawn> OnPlayerRespawn
    {
      add => EventService.Subscribe<ModuleEvents.OnPlayerRespawn, GameEventFactory, GameEventFactory.RegistrationData>(ControlledCreature, new GameEventFactory.RegistrationData(NwModule.Instance), value);
      remove => EventService.Unsubscribe<ModuleEvents.OnPlayerRespawn, GameEventFactory>(ControlledCreature, value);
    }
  }
}
