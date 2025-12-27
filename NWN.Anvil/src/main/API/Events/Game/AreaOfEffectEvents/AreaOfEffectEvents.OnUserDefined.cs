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
    /// Triggered for an area of effect when the associated <see cref="Signal"/> method is called.
    /// </summary>
    /// <seealso cref="Signal"/>
    [GameEvent(EventScriptType.AreaOfEffectOnUserDefinedEvent)]
    public sealed class OnUserDefined : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwAreaOfEffect"/> associated with this event.
      /// </summary>
      public NwAreaOfEffect Effect { get; } = NWScript.OBJECT_SELF.ToNwObject<NwAreaOfEffect>()!;

      NwObject IEvent.Context => Effect;

      /// <summary>
      /// Sends a user-defined event to the specified area of effect.
      /// </summary>
      /// <param name="areaOfEffect">The area of effect to signal.</param>
      /// <param name="eventId">The user-defined event number to signal.</param>
      public static void Signal(NwAreaOfEffect areaOfEffect, int eventId)
      {
        Event nwEvent = NWScript.EventUserDefined(eventId)!;
        NWScript.SignalEvent(areaOfEffect, nwEvent);
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwAreaOfEffect
  {
    /// <inheritdoc cref="AreaOfEffectEvents.OnUserDefined"/>
    public event Action<AreaOfEffectEvents.OnUserDefined> OnUserDefined
    {
      add => EventService.Subscribe<AreaOfEffectEvents.OnUserDefined, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<AreaOfEffectEvents.OnUserDefined, GameEventFactory>(this, value);
    }
  }
}
