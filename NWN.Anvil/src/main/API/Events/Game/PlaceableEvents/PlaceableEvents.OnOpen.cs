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
    /// Called when <see cref="NwPlaceable"/> has been opened.
    /// </summary>
    [GameEvent(EventScriptType.PlaceableOnOpen)]
    public sealed class OnOpen : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwCreature"/> that opened the <see cref="NwPlaceable"/>.
      /// </summary>
      public NwCreature? OpenedBy { get; } = NWScript.GetLastOpenedBy().ToNwObject<NwCreature>();

      /// <summary>
      /// Gets the <see cref="NwPlaceable"/> that was opened.
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
    /// <inheritdoc cref="PlaceableEvents.OnOpen"/>
    public event Action<PlaceableEvents.OnOpen> OnOpen
    {
      add => EventService.Subscribe<PlaceableEvents.OnOpen, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<PlaceableEvents.OnOpen, GameEventFactory>(this, value);
    }
  }
}
