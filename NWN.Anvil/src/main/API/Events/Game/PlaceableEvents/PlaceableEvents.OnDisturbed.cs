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
    /// Called when <see cref="NwPlaceable"/> inventory has been disturbed.
    /// </summary>
    [GameEvent(EventScriptType.PlaceableOnInventoryDisturbed)]
    public sealed class OnDisturbed : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwItem"/> that triggered the disturb event on <see cref="NwPlaceable"/>.
      /// </summary>
      public NwItem? DisturbedItem { get; } = NWScript.GetInventoryDisturbItem().ToNwObject<NwItem>();

      /// <summary>
      /// Gets the object that disturbed <see cref="NwPlaceable"/>.
      /// </summary>
      public NwGameObject? Disturber { get; } = NWScript.GetLastDisturbed().ToNwObject<NwGameObject>();

      /// <summary>
      /// Gets the <see cref="InventoryDisturbType"/>.
      /// </summary>
      public InventoryDisturbType DisturbType { get; } = (InventoryDisturbType)NWScript.GetInventoryDisturbType();

      /// <summary>
      /// Gets the <see cref="NwPlaceable"/> that was disturbed.
      /// </summary>
      public NwPlaceable Placeable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>()!;

      NwObject IEvent.Context => Placeable;
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwPlaceable
  {
    /// <inheritdoc cref="PlaceableEvents.OnDisturbed"/>
    public event Action<PlaceableEvents.OnDisturbed> OnDisturbed
    {
      add => EventService.Subscribe<PlaceableEvents.OnDisturbed, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<PlaceableEvents.OnDisturbed, GameEventFactory>(this, value);
    }
  }
}
