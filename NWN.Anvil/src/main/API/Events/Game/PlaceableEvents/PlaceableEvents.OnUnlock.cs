using System;
using Anvil.API.Events;
using NWN.Core;

namespace Anvil.API.Events
{
  /// <summary>
  /// Built-in events associated with a specific placeable.
  /// </summary>
  public static partial class PlaceableEvents
  {
    /// <summary>
    /// Called when <see cref="NwPlaceable"/> has been unlocked.
    /// </summary>
    [GameEvent(EventScriptType.PlaceableOnUnlock)]
    public sealed class OnUnlock : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwPlaceable"/> that was unlocked.
      /// </summary>
      public NwPlaceable Placeable { get; }

      /// <summary>
      /// Gets the <see cref="NwCreature"/> that unlocked <see cref="NwPlaceable"/>.
      /// </summary>
      public NwCreature? UnlockedBy { get; }

      NwObject IEvent.Context => Placeable;

      public OnUnlock()
      {
        Placeable = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>()!;
        UnlockedBy = NWScript.GetLastUnlocked(Placeable).ToNwObject<NwCreature>();
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwPlaceable
  {
    /// <inheritdoc cref="PlaceableEvents.OnUnlock"/>
    public event Action<PlaceableEvents.OnUnlock> OnUnlock
    {
      add => EventService.Subscribe<PlaceableEvents.OnUnlock, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<PlaceableEvents.OnUnlock, GameEventFactory>(this, value);
    }
  }
}
