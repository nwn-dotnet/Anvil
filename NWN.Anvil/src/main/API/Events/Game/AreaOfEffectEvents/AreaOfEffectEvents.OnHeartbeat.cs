using System;
using Anvil.API.Events;
using NWN.Core;

namespace Anvil.API.Events
{
  /// <summary>
  /// Built-in events for effects created with <see cref="Effect.AreaOfEffect"/>.
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

      public int SpellSaveDC { get; } = NWScript.GetSpellSaveDC();

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
