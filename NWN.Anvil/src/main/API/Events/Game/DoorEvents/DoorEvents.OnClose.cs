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
    [GameEvent(EventScriptType.DoorOnClose)]
    public sealed class OnClose : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwCreature"/> that closed the <see cref="NwDoor"/>.
      /// </summary>
      public NwGameObject ClosedBy { get; }

      /// <summary>
      /// Gets the <see cref="NwDoor"/> that was closed.
      /// </summary>
      public NwDoor Door { get; }

      NwObject IEvent.Context => Door;

      public OnClose()
      {
        Door = NWScript.OBJECT_SELF.ToNwObject<NwDoor>()!;
        ClosedBy = NWScript.GetLastClosedBy(Door).ToNwObject<NwGameObject>()!;
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwDoor
  {
    /// <inheritdoc cref="DoorEvents.OnClose"/>
    public event Action<DoorEvents.OnClose> OnClose
    {
      add => EventService.Subscribe<DoorEvents.OnClose, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<DoorEvents.OnClose, GameEventFactory>(this, value);
    }
  }
}
