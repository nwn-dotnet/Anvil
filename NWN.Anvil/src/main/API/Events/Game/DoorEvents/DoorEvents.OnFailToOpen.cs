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
    [GameEvent(EventScriptType.DoorOnFailToOpen)]
    public sealed class OnFailToOpen : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwDoor"/> that failed to open.
      /// </summary>
      public NwDoor Door { get; } = NWScript.OBJECT_SELF.ToNwObject<NwDoor>()!;

      /// <summary>
      /// Gets the <see cref="NwCreature"/> that failed to unlock this <see cref="NwDoor"/>.
      /// </summary>
      public NwCreature WhoFailed { get; } = NWScript.GetClickingObject().ToNwObject<NwCreature>()!;

      NwObject? IEvent.Context => Door;
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwDoor
  {
    /// <inheritdoc cref="DoorEvents.OnFailToOpen"/>
    public event Action<DoorEvents.OnFailToOpen> OnFailToOpen
    {
      add => EventService.Subscribe<DoorEvents.OnFailToOpen, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<DoorEvents.OnFailToOpen, GameEventFactory>(this, value);
    }
  }
}
