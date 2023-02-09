using System;
using Anvil.API.Events;
using NWN.Core;

#pragma warning disable CS0618

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
    [Obsolete("Use OnConversation instead.")]
    [GameEvent(EventScriptType.PlaceableOnDialogue)]
    public sealed class OnDialogue : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwPlaceable"/> that is in dialog.
      /// </summary>
      public NwPlaceable Placeable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>()!;

      /// <summary>
      /// Gets the listen pattern that matched the message sent to this placeable.
      /// </summary>
      public int ListenPattern { get; } = NWScript.GetListenPatternNumber();

      NwObject IEvent.Context => Placeable;
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwPlaceable
  {
    /// <inheritdoc cref="PlaceableEvents.OnDialogue"/>
    [Obsolete("Use OnConversation instead.")]
    public event Action<PlaceableEvents.OnDialogue> OnDialogue
    {
      add => EventService.Subscribe<PlaceableEvents.OnDialogue, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<PlaceableEvents.OnDialogue, GameEventFactory>(this, value);
    }
  }
}
