using System;
using Anvil.API.Events;
using NWN.Core;

namespace Anvil.API.Events
{
  /// <summary>
  /// Events for <see cref="NwPlaceable"/>.
  /// </summary>
  public static partial class PlaceableEvents
  {
    /// <summary>
    /// Called when <see cref="NwPlaceable"/> has been locked.
    /// </summary>
    [GameEvent(EventScriptType.PlaceableOnLock)]
    public sealed class OnLock : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwCreature"/> that locked this <see cref="NwPlaceable"/>.
      /// </summary>
      public NwCreature? LockedBy { get; } = NWScript.GetLastLocked().ToNwObject<NwCreature>();

      /// <summary>
      /// Gets the <see cref="NwPlaceable"/> that was locked.
      /// </summary>
      public NwPlaceable LockedPlaceable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>()!;

      NwObject IEvent.Context => LockedPlaceable;
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwPlaceable
  {
    /// <inheritdoc cref="PlaceableEvents.OnLock"/>
    public event Action<PlaceableEvents.OnLock> OnLock
    {
      add => EventService.Subscribe<PlaceableEvents.OnLock, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<PlaceableEvents.OnLock, GameEventFactory>(this, value);
    }
  }
}
