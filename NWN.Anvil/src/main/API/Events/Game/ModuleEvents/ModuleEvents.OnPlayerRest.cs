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
    /// Triggered when <see cref="NwPlayer"/> presses the rest button and begins to rest, cancelled rest, or finished rest.
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnPlayerRest)]
    public sealed class OnPlayerRest : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwPlayer"/> that triggered the event.
      /// </summary>
      public NwPlayer Player { get; } = NWScript.GetLastPCRested().ToNwPlayer();

      /// <summary>
      /// Gets the <see cref="RestEventType"/> that was triggered.
      /// </summary>
      public RestEventType RestEventType { get; } = (RestEventType)NWScript.GetLastRestEventType();

      NwObject IEvent.Context => Player.ControlledCreature;
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwModule
  {
    /// <inheritdoc cref="ModuleEvents.OnPlayerRest"/>
    public event Action<ModuleEvents.OnPlayerRest> OnPlayerRest
    {
      add => EventService.SubscribeAll<ModuleEvents.OnPlayerRest, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnPlayerRest, GameEventFactory>(value);
    }
  }

  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="ModuleEvents.OnPlayerRest"/>
    public event Action<ModuleEvents.OnPlayerRest> OnPlayerRest
    {
      add => EventService.Subscribe<ModuleEvents.OnPlayerRest, GameEventFactory, GameEventFactory.RegistrationData>(ControlledCreature, new GameEventFactory.RegistrationData(NwModule.Instance), value);
      remove => EventService.Unsubscribe<ModuleEvents.OnPlayerRest, GameEventFactory>(ControlledCreature, value);
    }
  }
}
