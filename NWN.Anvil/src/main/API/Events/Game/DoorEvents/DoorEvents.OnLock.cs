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
    [GameEvent(EventScriptType.DoorOnLock)]
    public sealed class OnLock : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwDoor"/> that was locked.
      /// </summary>
      public NwDoor Door { get; } = NWScript.OBJECT_SELF.ToNwObject<NwDoor>()!;

      /// <summary>
      /// Gets the <see cref="NwCreature"/> that locked this <see cref="NwDoor"/>.
      /// </summary>
      public NwCreature LockedBy { get; } = NWScript.GetLastLocked().ToNwObject<NwCreature>()!;

      NwObject IEvent.Context => Door;
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwDoor
  {
    /// <inheritdoc cref="DoorEvents.OnLock"/>
    public event Action<DoorEvents.OnLock> OnLock
    {
      add => EventService.Subscribe<DoorEvents.OnLock, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<DoorEvents.OnLock, GameEventFactory>(this, value);
    }
  }
}
