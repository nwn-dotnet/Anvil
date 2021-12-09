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
    /// Called when <see cref="NwPlaceable"/> is being used.
    /// </summary>
    [GameEvent(EventScriptType.PlaceableOnUsed)]
    public sealed class OnUsed : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwPlaceable"/> that was used.
      /// </summary>
      public NwPlaceable Placeable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      /// <summary>
      /// Gets the <see cref="NwCreature"/> that used <see cref="NwPlaceable"/>.
      /// </summary>
      public NwCreature UsedBy { get; } = NWScript.GetLastUsedBy().ToNwObject<NwCreature>();

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
    /// <inheritdoc cref="PlaceableEvents.OnUsed"/>
    public event Action<PlaceableEvents.OnUsed> OnUsed
    {
      add => EventService.Subscribe<PlaceableEvents.OnUsed, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<PlaceableEvents.OnUsed, GameEventFactory>(this, value);
    }
  }
}
