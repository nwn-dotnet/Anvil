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
    /// Called when <see cref="NwPlaceable"/> starts a conversation.
    /// </summary>
    [GameEvent(EventScriptType.PlaceableOnDialogue)]
    public sealed class OnDialogue : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwPlaceable"/> that is in dialog.
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
    /// <inheritdoc cref="PlaceableEvents.OnDialogue"/>
    public event Action<PlaceableEvents.OnDialogue> OnDialogue
    {
      add => EventService.Subscribe<PlaceableEvents.OnDialogue, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<PlaceableEvents.OnDialogue, GameEventFactory>(this, value);
    }
  }
}
