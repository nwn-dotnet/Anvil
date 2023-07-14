using System;
using Anvil.API.Events;
using NWN.Core;

namespace Anvil.API.Events
{
  /// <summary>
  /// Built-in events for effects created with <see cref="Effect.AreaOfEffect(Anvil.API.PersistentVfxType,Anvil.Services.ScriptCallbackHandle,Anvil.Services.ScriptCallbackHandle,Anvil.Services.ScriptCallbackHandle)"/>.
  /// </summary>
  public static partial class AreaOfEffectEvents
  {
    /// <summary>
    /// Called when an object enters the area of effect.
    /// </summary>
    [GameEvent(EventScriptType.AreaOfEffectOnObjectEnter)]
    public sealed class OnEnter : IEvent
    {
      public NwAreaOfEffect Effect { get; } = NWScript.OBJECT_SELF.ToNwObject<NwAreaOfEffect>()!;

      public NwGameObject Entering { get; } = NWScript.GetEnteringObject().ToNwObject<NwGameObject>()!;

      public int SpellSaveDC { get; } = NWScript.GetSpellSaveDC();

      NwObject IEvent.Context => Effect;
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwAreaOfEffect
  {
    /// <inheritdoc cref="AreaOfEffectEvents.OnEnter"/>
    public event Action<AreaOfEffectEvents.OnEnter> OnEnter
    {
      add => EventService.Subscribe<AreaOfEffectEvents.OnEnter, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<AreaOfEffectEvents.OnEnter, GameEventFactory>(this, value);
    }
  }
}
