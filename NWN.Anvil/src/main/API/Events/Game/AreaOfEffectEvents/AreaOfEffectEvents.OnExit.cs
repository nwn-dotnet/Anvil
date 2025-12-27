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
      /// <summary>
      /// Gets the <see cref="NwAreaOfEffect"/> associated with this event.
      /// </summary>
      public NwAreaOfEffect Effect { get; } = NWScript.OBJECT_SELF.ToNwObject<NwAreaOfEffect>()!;

      /// <summary>
      /// Gets the <see cref="NwGameObject"/> that exited the area of effect.
      /// </summary>
      public NwGameObject Exiting { get; } = NWScript.GetExitingObject().ToNwObject<NwGameObject>()!;

      /// <summary>
      /// Gets the spell saving throw DC associated with this effect.
      /// </summary>
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
