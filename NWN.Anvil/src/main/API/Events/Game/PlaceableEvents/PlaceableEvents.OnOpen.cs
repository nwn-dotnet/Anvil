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
    /// Called when <see cref="NwPlaceable"/> has been opened.
    /// </summary>
    [GameEvent(EventScriptType.PlaceableOnOpen)]
    public sealed class OnOpen : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwCreature"/> that opened the <see cref="NwPlaceable"/>.
      /// </summary>
      public NwCreature? OpenedBy { get; }

      /// <summary>
      /// Gets the <see cref="NwPlaceable"/> that was opened.
      /// </summary>
      public NwPlaceable Placeable { get; }

      NwObject IEvent.Context => Placeable;

      public OnOpen()
      {
        Placeable = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>()!;
        OpenedBy = NWScript.GetLastOpenedBy(Placeable).ToNwObject<NwCreature>();
      }
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
