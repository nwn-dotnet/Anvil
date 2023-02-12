using System;
using Anvil.API.Events;
using NWN.Core;

namespace Anvil.API.Events
{
  /// <summary>
  /// Events for door objects.
  /// </summary>
  public static partial class DoorEvents
  {
    /// <summary>
    /// Called when this door starts a conversation, or hears a message they are listening for.
    /// </summary>
    [GameEvent(EventScriptType.DoorOnDialogue)]
    public sealed class OnConversation : IEvent
    {
      /// <summary>
      /// Gets the door attached to this conversation.
      /// </summary>
      public NwDoor Door { get; } = NWScript.OBJECT_SELF.ToNwObject<NwDoor>()!;

      /// <summary>
      /// Gets the last <see cref="NwGameObject"/> that spoke in this conversation.
      /// </summary>
      public NwGameObject? LastSpeaker { get; } = NWScript.GetLastSpeaker().ToNwObject<NwGameObject>();

      /// <summary>
      /// Gets the <see cref="NwPlayer"/> speaker in this conversation.
      /// </summary>
      public NwPlayer? PlayerSpeaker { get; } = NWScript.GetPCSpeaker().ToNwPlayer();

      /// <summary>
      /// Gets the listen pattern that matched the message sent to this door.
      /// </summary>
      public int ListenPattern { get; } = NWScript.GetListenPatternNumber();

      NwObject IEvent.Context => Door;

      public static void Signal(NwDoor door)
      {
        Event nwEvent = NWScript.EventConversation()!;
        NWScript.SignalEvent(door, nwEvent);
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
