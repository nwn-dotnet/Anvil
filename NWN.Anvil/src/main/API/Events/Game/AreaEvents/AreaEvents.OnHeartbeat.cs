using System;
using Anvil.API.Events;
using NWN.Core;

namespace Anvil.API.Events
{
  /// <summary>
  /// Built-in events associated with a specific area.
  /// </summary>
  public static partial class AreaEvents
  {
    /// <summary>
    /// Called at a regular interval (approx. 6 seconds).
    /// </summary>
    [GameEvent(EventScriptType.AreaOnHeartbeat)]
    public sealed class OnHeartbeat : IEvent
    {
      public NwArea Area { get; } = NWScript.OBJECT_SELF.ToNwObject<NwArea>()!;

      NwObject IEvent.Context => Area;
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwArea
  {
    /// <inheritdoc cref="AreaEvents.OnHeartbeat"/>
    public event Action<AreaEvents.OnHeartbeat> OnHeartbeat
    {
      add => EventService.Subscribe<AreaEvents.OnHeartbeat, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<AreaEvents.OnHeartbeat, GameEventFactory>(this, value);
    }
  }
}
