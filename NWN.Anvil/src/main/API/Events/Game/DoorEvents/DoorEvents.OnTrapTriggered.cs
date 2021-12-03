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
    [GameEvent(EventScriptType.DoorOnTrapTriggered)]
    public sealed class OnTrapTriggered : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwDoor"/> that has had a trap triggered.
      /// </summary>
      public NwDoor Door { get; } = NWScript.OBJECT_SELF.ToNwObject<NwDoor>();

      NwObject IEvent.Context
      {
        get => Door;
      }
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
