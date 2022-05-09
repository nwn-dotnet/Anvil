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
    /// Called when <see cref="NwPlaceable"/> has a trap triggered.
    /// </summary>
    [GameEvent(EventScriptType.PlaceableOnTrapTriggered)]
    public sealed class OnTrapTriggered : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwPlaceable"/> that had a trap triggered.
      /// </summary>
      public NwPlaceable Placeable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>()!;

      NwObject? IEvent.Context => Placeable;
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwPlaceable
  {
    /// <inheritdoc cref="PlaceableEvents.OnTrapTriggered"/>
    public event Action<PlaceableEvents.OnTrapTriggered> OnTrapTriggered
    {
      add => EventService.Subscribe<PlaceableEvents.OnTrapTriggered, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<PlaceableEvents.OnTrapTriggered, GameEventFactory>(this, value);
    }
  }
}
