using System;
using Anvil.API.Events;
using NWN.Core;

namespace Anvil.API.Events
{
  /// <summary>
  /// Events for door objects.
  /// </summary>
  public static partial class DoorEvents
  {
    [GameEvent(EventScriptType.DoorOnUserDefined)]
    public sealed class OnUserDefined : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwDoor"/> that is running a user defined event.
      /// </summary>
      public NwDoor Door { get; } = NWScript.OBJECT_SELF.ToNwObject<NwDoor>()!;

      /// <summary>
      /// Gets the specific event number used to trigger this user-defined event.
      /// </summary>
      public int EventNumber { get; } = NWScript.GetUserDefinedEventNumber();

      NwObject IEvent.Context => Door;

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
