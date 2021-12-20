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
    /// Triggered when a <see cref="NwCreature"/> leaves the server.
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnClientExit)]
    public sealed class OnClientLeave : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwPlayer"/> that is leaving.
      /// </summary>
      public NwPlayer Player { get; } = NWScript.GetExitingObject().ToNwPlayer(PlayerSearch.Login);

      NwObject IEvent.Context => Player.LoginCreature;
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwModule
  {
    /// <inheritdoc cref="ModuleEvents.OnClientLeave"/>
    public event Action<ModuleEvents.OnClientLeave> OnClientLeave
    {
      add => EventService.SubscribeAll<ModuleEvents.OnClientLeave, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnClientLeave, GameEventFactory>(value);
    }
  }

  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="ModuleEvents.OnClientLeave"/>
    public event Action<ModuleEvents.OnClientLeave> OnClientLeave
    {
      add => EventService.Subscribe<ModuleEvents.OnClientLeave, GameEventFactory, GameEventFactory.RegistrationData>(ControlledCreature, new GameEventFactory.RegistrationData(NwModule.Instance), value);
      remove => EventService.Unsubscribe<ModuleEvents.OnClientLeave, GameEventFactory>(ControlledCreature, value);
    }
  }
}
