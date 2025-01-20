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
    [GameEvent(EventScriptType.DoorOnOpen)]
    public sealed class OnOpen : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwDoor"/> that was opened.
      /// </summary>
      public NwDoor Door { get; }

      /// <summary>
      /// Gets the <see cref="NwCreature"/> that opened the <see cref="NwDoor"/>.
      /// </summary>
      public NwGameObject OpenedBy { get; }

      NwObject IEvent.Context => Door;

      public OnOpen()
      {
        Door = NWScript.OBJECT_SELF.ToNwObject<NwDoor>()!;
        OpenedBy = NWScript.GetLastOpenedBy(Door).ToNwObject<NwGameObject>()!;
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwDoor
  {
    /// <inheritdoc cref="DoorEvents.OnOpen"/>
    public event Action<DoorEvents.OnOpen> OnOpen
    {
      add => EventService.Subscribe<DoorEvents.OnOpen, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<DoorEvents.OnOpen, GameEventFactory>(this, value);
    }
  }
}
