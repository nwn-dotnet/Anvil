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
    [GameEvent(EventScriptType.DoorOnTrapTriggered)]
    public sealed class OnTrapTriggered : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwDoor"/> that has had a trap triggered.
      /// </summary>
      public NwDoor Door { get; } = NWScript.OBJECT_SELF.ToNwObject<NwDoor>()!;

      /// <summary>
      /// Gets the <see cref="NwGameObject"/> that triggered this trap.
      /// </summary>
      public NwGameObject TriggeredBy { get; } = NWScript.GetEnteringObject().ToNwObject<NwGameObject>()!;

      NwObject IEvent.Context => Door;
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwDoor
  {
    /// <inheritdoc cref="DoorEvents.OnTrapTriggered"/>
    public event Action<DoorEvents.OnTrapTriggered> OnTrapTriggered
    {
      add => EventService.Subscribe<DoorEvents.OnTrapTriggered, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<DoorEvents.OnTrapTriggered, GameEventFactory>(this, value);
    }
  }
}
