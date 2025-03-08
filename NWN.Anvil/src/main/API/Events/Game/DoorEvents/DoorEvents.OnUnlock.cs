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
    [GameEvent(EventScriptType.DoorOnUnlock)]
    public sealed class OnUnlock : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwDoor"/> that was unlocked.
      /// </summary>
      public NwDoor Door { get; }

      /// <summary>
      /// Gets the <see cref="NwCreature"/> that unlocked <see cref="NwDoor"/>.
      /// </summary>
      public NwCreature UnlockedBy { get; }

      NwObject IEvent.Context => Door;

      public OnUnlock()
      {
        Door = NWScript.OBJECT_SELF.ToNwObject<NwDoor>()!;
        UnlockedBy = NWScript.GetLastUnlocked(Door).ToNwObject<NwCreature>()!;
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwDoor
  {
    /// <inheritdoc cref="DoorEvents.OnUnlock"/>
    public event Action<DoorEvents.OnUnlock> OnUnlock
    {
      add => EventService.Subscribe<DoorEvents.OnUnlock, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<DoorEvents.OnUnlock, GameEventFactory>(this, value);
    }
  }
}
