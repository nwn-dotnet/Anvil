using System;
using Anvil.API.Events;
using NWN.Core;

namespace Anvil.API.Events
{
  /// <summary>
  /// Built-in events associated with a specific door.
  /// </summary>
  public static partial class DoorEvents
  {
    /// <summary>
    /// Triggered when the door starts dialogue, or hears a listen pattern.
    /// </summary>
    [GameEvent(EventScriptType.DoorOnDialogue)]
    public sealed class OnConversation : IEvent
    {
      /// <summary>
      /// Gets the door attached to this conversation.
      /// </summary>
      public NwDoor Door { get; } = NWScript.OBJECT_SELF.ToNwObject<NwDoor>()!;

      /// <summary>
      /// Gets the last <see cref="NwGameObject"/> that spoke to this door.
      /// Returns null if there is no last speaker.
      /// </summary>
      public NwGameObject? LastSpeaker { get; } = NWScript.GetLastSpeaker().ToNwObject<NwGameObject>();

      /// <summary>
      /// Gets the <see cref="NwPlayer"/> currently speaking, if any.
      /// Returns null if the speaker is not a player.
      /// </summary>
      public NwPlayer? PlayerSpeaker { get; } = NWScript.GetPCSpeaker().ToNwPlayer();

      /// <summary>
      /// Gets the listen pattern that matched the message sent to this door.
      /// </summary>
      public int ListenPattern { get; } = NWScript.GetListenPatternNumber();

      NwObject IEvent.Context => Door;

      /// <summary>
      /// Signals a conversation event on the specified door.
      /// </summary>
      /// <param name="door">The door to receive the conversation event.</param>
      public static void Signal(NwDoor door)
      {
        Event nwEvent = NWScript.EventConversation()!;
        NWScript.SignalEvent(door, nwEvent);
      }

      /// <summary>
      /// Pauses the current conversation for this door.
      /// </summary>
      public void PauseConversation()
      {
        NWScript.ActionPauseConversation();
      }

      /// <summary>
      /// Resumes a paused conversation for this door.
      /// </summary>
      public void ResumeConversation()
      {
        NWScript.ActionResumeConversation();
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwDoor
  {
    /// <inheritdoc cref="DoorEvents.OnConversation"/>
    public event Action<DoorEvents.OnConversation> OnConversation
    {
      add => EventService.Subscribe<DoorEvents.OnConversation, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<DoorEvents.OnConversation, GameEventFactory>(this, value);
    }
  }
}
