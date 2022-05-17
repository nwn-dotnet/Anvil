using System;
using Anvil.API.Events;

namespace Anvil.API.Events
{
  /// <summary>
  /// Global module events.
  /// </summary>
  public static partial class ModuleEvents
  {
    [GameEvent(EventScriptType.ModuleOnModuleStart)]
    public sealed class OnModuleStart : IEvent
    {
      NwObject? IEvent.Context => null;
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwModule
  {
    /// <inheritdoc cref="ModuleEvents.OnModuleStart"/>
    public event Action<ModuleEvents.OnModuleStart> OnModuleStart
    {
      add => EventService.SubscribeAll<ModuleEvents.OnModuleStart, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnModuleStart, GameEventFactory>(value);
    }
  }
}
