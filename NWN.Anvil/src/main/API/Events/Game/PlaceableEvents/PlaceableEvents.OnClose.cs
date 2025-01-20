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
    /// Called when <see cref="NwCreature"/> has closed a <see cref="NwPlaceable"/>.
    /// </summary>
    [GameEvent(EventScriptType.PlaceableOnClosed)]
    public sealed class OnClose : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwCreature"/> that closed the <see cref="NwPlaceable"/>.
      /// </summary>
      public NwCreature? ClosedBy { get; }

      /// <summary>
      /// Gets the <see cref="NwPlaceable"/> that was closed.
      /// </summary>
      public NwPlaceable Placeable { get; }

      NwObject IEvent.Context => Placeable;

      public OnClose()
      {
        Placeable = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>()!;
        ClosedBy = NWScript.GetLastClosedBy(Placeable).ToNwObject<NwCreature>();
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwPlaceable
  {
    /// <inheritdoc cref="PlaceableEvents.OnClose"/>
    public event Action<PlaceableEvents.OnClose> OnClose
    {
      add => EventService.Subscribe<PlaceableEvents.OnClose, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<PlaceableEvents.OnClose, GameEventFactory>(this, value);
    }
  }
}
