using System;
using Anvil.API.Events;

namespace Anvil.API.Events
{
  /// <summary>
  /// Global module events.
  /// </summary>
  public static partial class ModuleEvents
  {
    /// <summary>
    /// Triggered when the module is initially loaded. This event must be hooked in your service constructor, otherwise it will be missed.
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnModuleLoad)]
    public sealed class OnModuleLoad : IEvent
    {
      NwObject IEvent.Context
      {
        get => null;
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwModule
  {
    /// <inheritdoc cref="ModuleEvents.OnModuleLoad"/>
    public event Action<ModuleEvents.OnModuleLoad> OnModuleLoad
    {
      add => EventService.SubscribeAll<ModuleEvents.OnModuleLoad, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnModuleLoad, GameEventFactory>(value);
    }
  }
}
