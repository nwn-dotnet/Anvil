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
    /// Called when this placeable starts a conversation, or hears a message they are listening for.
    /// </summary>
    [GameEvent(EventScriptType.PlaceableOnDialogue)]
    public sealed class OnConversation : IEvent
    {
      /// <summary>
      /// Gets the placeable attached to this conversation.
      /// </summary>
      public NwPlaceable Placeable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>()!;

      /// <summary>
      /// Gets the last <see cref="NwGameObject"/> that spoke in this conversation.
      /// </summary>
      public NwGameObject? LastSpeaker { get; } = NWScript.GetLastSpeaker().ToNwObject<NwGameObject>();

      /// <summary>
      /// Gets the <see cref="NwPlayer"/> speaker in this conversation.
      /// </summary>
      public NwPlayer? PlayerSpeaker { get; } = NWScript.GetPCSpeaker().ToNwPlayer();

      /// <summary>
      /// Gets the listen pattern that matched the message sent to this placeable.
      /// </summary>
      public int ListenPattern { get; } = NWScript.GetListenPatternNumber();

      NwObject IEvent.Context => Placeable;

      public static void Signal(NwPlaceable placeable)
      {
        Event nwEvent = NWScript.EventConversation()!;
        NWScript.SignalEvent(placeable, nwEvent);
      }

      public void PauseConversation()
      {
        NWScript.ActionPauseConversation();
      }

      public void ResumeConversation()
      {
        NWScript.ActionResumeConversation();
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwPlaceable
  {
    /// <inheritdoc cref="PlaceableEvents.OnConversation"/>
    public event Action<PlaceableEvents.OnConversation> OnConversation
    {
      add => EventService.Subscribe<PlaceableEvents.OnConversation, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<PlaceableEvents.OnConversation, GameEventFactory>(this, value);
    }
  }
}
