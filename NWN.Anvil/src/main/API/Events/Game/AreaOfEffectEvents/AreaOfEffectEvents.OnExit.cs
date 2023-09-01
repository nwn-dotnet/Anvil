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
    /// Called when an object exits the area of effect.
    /// </summary>
    [GameEvent(EventScriptType.AreaOfEffectOnObjectExit)]
    public sealed class OnExit : IEvent
    {
      public NwAreaOfEffect Effect { get; } = NWScript.OBJECT_SELF.ToNwObject<NwAreaOfEffect>()!;

      public NwGameObject Exiting { get; } = NWScript.GetExitingObject().ToNwObject<NwGameObject>()!;

      public int SpellSaveDC { get; } = NWScript.GetSpellSaveDC();

      NwObject IEvent.Context => Effect;
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwAreaOfEffect
  {
    /// <inheritdoc cref="AreaOfEffectEvents.OnExit"/>
    public event Action<AreaOfEffectEvents.OnExit> OnExit
    {
      add => EventService.Subscribe<AreaOfEffectEvents.OnExit, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<AreaOfEffectEvents.OnExit, GameEventFactory>(this, value);
    }
  }
}
