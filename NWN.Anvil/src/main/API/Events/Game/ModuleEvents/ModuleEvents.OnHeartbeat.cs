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
    /// Triggered every server heartbeat (~6 seconds).
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnHeartbeat)]
    public sealed class OnHeartbeat : IEvent
    {
      NwObject IEvent.Context => null;
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwModule
  {
    /// <inheritdoc cref="ModuleEvents.OnHeartbeat"/>
    public event Action<ModuleEvents.OnHeartbeat> OnHeartbeat
    {
      add => EventService.SubscribeAll<ModuleEvents.OnHeartbeat, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnHeartbeat, GameEventFactory>(value);
    }
  }
}
