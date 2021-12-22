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
    /// Triggered when a <see cref="NwPlayer"/> selects a character and logged into the module.
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnClientEnter)]
    public sealed class OnClientEnter : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwPlayer"/> that triggered the event.
      /// </summary>
      public NwPlayer Player { get; } = NWScript.GetEnteringObject().ToNwPlayer();

      NwObject IEvent.Context => Player.ControlledCreature;
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwModule
  {
    /// <inheritdoc cref="ModuleEvents.OnClientEnter"/>
    public event Action<ModuleEvents.OnClientEnter> OnClientEnter
    {
      add => EventService.SubscribeAll<ModuleEvents.OnClientEnter, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnClientEnter, GameEventFactory>(value);
    }
  }

  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="ModuleEvents.OnClientEnter"/>
    public event Action<ModuleEvents.OnClientEnter> OnClientEnter
    {
      add => EventService.Subscribe<ModuleEvents.OnClientEnter, GameEventFactory, GameEventFactory.RegistrationData>(ControlledCreature, new GameEventFactory.RegistrationData(NwModule.Instance), value);
      remove => EventService.Unsubscribe<ModuleEvents.OnClientEnter, GameEventFactory>(ControlledCreature, value);
    }
  }
}
