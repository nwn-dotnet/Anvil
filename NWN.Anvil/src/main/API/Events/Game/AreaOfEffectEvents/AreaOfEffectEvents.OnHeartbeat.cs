using System;
using Anvil.API.Events;
using Anvil.Services;
using NWN.Core;

namespace Anvil.API.Events
{
  /// <summary>
  /// Events for effects created with <see cref="Effect.AreaOfEffect(PersistentVfxType,ScriptCallbackHandle,ScriptCallbackHandle,ScriptCallbackHandle)"/>.
  /// </summary>
  public static partial class AreaOfEffectEvents
  {
    /// <summary>
    /// Called at a regular interval (approx. 6 seconds).
    /// </summary>
    [GameEvent(EventScriptType.AreaOfEffectOnHeartbeat)]
    public sealed class OnHeartbeat : IEvent
    {
      public NwAreaOfEffect Effect { get; } = NWScript.OBJECT_SELF.ToNwObject<NwAreaOfEffect>()!;

      NwObject IEvent.Context => Effect;
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwAreaOfEffect
  {
    /// <inheritdoc cref="AreaEvents.OnHeartbeat"/>
    public event Action<AreaOfEffectEvents.OnHeartbeat> OnHeartbeat
    {
      add => EventService.Subscribe<AreaOfEffectEvents.OnHeartbeat, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<AreaOfEffectEvents.OnHeartbeat, GameEventFactory>(this, value);
    }
  }
}
