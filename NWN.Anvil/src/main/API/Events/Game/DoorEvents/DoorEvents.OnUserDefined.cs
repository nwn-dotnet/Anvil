using System;
using Anvil.API.Events;
using NWN.Core;

namespace Anvil.API.Events
{
  /// <summary>
  /// Built-in events associated with a specific door.
  /// </summary>
  public static partial class DoorEvents
  {
    /// <summary>
    /// Triggered for a door when the associated <see cref="Signal"/> method is called.
    /// </summary>
    /// <seealso cref="Signal"/>
    [GameEvent(EventScriptType.DoorOnUserDefined)]
    public sealed class OnUserDefined : IEvent
    {
      /// <summary>
      /// Gets the door receiving the user-defined event.
      /// </summary>
      public NwDoor Door { get; } = NWScript.OBJECT_SELF.ToNwObject<NwDoor>()!;

      /// <summary>
      /// Gets the arbitrary event number supplied by the caller.
      /// </summary>
      public int EventNumber { get; } = NWScript.GetUserDefinedEventNumber();

      NwObject IEvent.Context => Door;

      /// <summary>
      /// Signals a user-defined event with the given ID to a door.
      /// </summary>
      /// <param name="door">The door to receive the event.</param>
      /// <param name="eventId">The integer event identifier to deliver.</param>
      public static void Signal(NwDoor door, int eventId)
      {
        Event nwEvent = NWScript.EventUserDefined(eventId)!;
        NWScript.SignalEvent(door, nwEvent);
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwDoor
  {
    /// <inheritdoc cref="DoorEvents.OnUserDefined"/>
    public event Action<DoorEvents.OnUserDefined> OnUserDefined
    {
      add => EventService.Subscribe<DoorEvents.OnUserDefined, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<DoorEvents.OnUserDefined, GameEventFactory>(this, value);
    }
  }
}
