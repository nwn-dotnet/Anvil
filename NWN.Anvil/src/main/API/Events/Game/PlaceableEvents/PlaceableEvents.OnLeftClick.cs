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
    /// Called when <see cref="NwPlaceable"/> has been mousepad (left) clicked.
    /// </summary>
    [GameEvent(EventScriptType.PlaceableOnLeftClick)]
    public sealed class OnLeftClick : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwPlayer"/> that clicked on the <see cref="NwPlaceable"/>.
      /// </summary>
      public NwPlayer ClickedBy { get; } = NWScript.GetPlaceableLastClickedBy().ToNwPlayer();

      /// <summary>
      /// Gets the <see cref="NwPlaceable"/> that was left clicked.
      /// </summary>
      public NwPlaceable Placeable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      NwObject IEvent.Context
      {
        get => Placeable;
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwPlaceable
  {
    /// <inheritdoc cref="PlaceableEvents.OnLeftClick"/>
    public event Action<PlaceableEvents.OnLeftClick> OnLeftClick
    {
      add => EventService.Subscribe<PlaceableEvents.OnLeftClick, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<PlaceableEvents.OnLeftClick, GameEventFactory>(this, value);
    }
  }
}
