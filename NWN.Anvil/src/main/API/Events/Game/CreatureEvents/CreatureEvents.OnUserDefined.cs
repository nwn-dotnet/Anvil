using System;
using Anvil.API.Events;
using NWN.Core;

namespace Anvil.API.Events
{
  /// <summary>
  /// Built-in events associated with a specific creature.
  /// </summary>
  public static partial class CreatureEvents
  {
    /// <summary>
    /// Triggered for a creature when the associated <see cref="Signal"/> method is called.
    /// </summary>
    /// <seealso cref="Signal"/>
    [GameEvent(EventScriptType.CreatureOnUserDefinedEvent)]
    public sealed class OnUserDefined : IEvent
    {
      /// <summary>
      /// Gets the creature receiving the user-defined event.
      /// </summary>
      public NwCreature Creature { get; } = NWScript.OBJECT_SELF.ToNwObject<NwCreature>()!;

      /// <summary>
      /// Gets the arbitrary event number supplied by the caller.
      /// </summary>
      public int EventNumber { get; } = NWScript.GetUserDefinedEventNumber();

      NwObject IEvent.Context => Creature;

      /// <summary>
      /// Signals a user-defined event with the given ID to a creature.
      /// </summary>
      /// <param name="creature">The creature to receive the event.</param>
      /// <param name="eventId">The integer event identifier to deliver.</param>
      public static void Signal(NwCreature creature, int eventId)
      {
        Event nwEvent = NWScript.EventUserDefined(eventId)!;
        NWScript.SignalEvent(creature, nwEvent);
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="CreatureEvents.OnUserDefined"/>
    public event Action<CreatureEvents.OnUserDefined> OnUserDefined
    {
      add => EventService.Subscribe<CreatureEvents.OnUserDefined, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<CreatureEvents.OnUserDefined, GameEventFactory>(this, value);
    }
  }
}
