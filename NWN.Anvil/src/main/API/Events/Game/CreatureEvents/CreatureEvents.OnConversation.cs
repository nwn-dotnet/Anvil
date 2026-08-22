using System;
using Anvil.API.Events;
using NWN.Core;

namespace Anvil.API.Events
{
  /// <summary>
  /// Built-in events associated with a specific creature.
  /// </summary>
  public static partial class CreatureEvents
  {
    /// <summary>
    /// Triggered when the creature starts dialogue, or hears a listen pattern.
    /// </summary>
    [GameEvent(EventScriptType.CreatureOnDialogue)]
    public sealed class OnConversation : IEvent
    {
      /// <summary>
      /// Gets the creature attached to this conversation.
      /// </summary>
      public NwCreature Creature { get; } = NWScript.OBJECT_SELF.ToNwObject<NwCreature>()!;

      /// <summary>
      /// Gets the last <see cref="NwGameObject"/> that spoke to this creature.
      /// Returns null if there is no last speaker.
      /// </summary>
      public NwGameObject? LastSpeaker { get; } = NWScript.GetLastSpeaker().ToNwObject<NwGameObject>();

      /// <summary>
      /// Gets the <see cref="NwPlayer"/> currently speaking, if any.
      /// Returns null if the speaker is not a player.
      /// </summary>
      public NwPlayer? PlayerSpeaker { get; } = NWScript.GetPCSpeaker().ToNwPlayer();

      /// <summary>
      /// Gets the listen pattern that matched the message sent to this creature.
      /// </summary>
      public int ListenPattern { get; } = NWScript.GetListenPatternNumber();

      /// <summary>
      /// Gets the associate command that matched the message sent to this creature.
      /// </summary>
      public AssociateCommand AssociateCommand => (AssociateCommand)ListenPattern;

      NwObject IEvent.Context => Creature;

      /// <summary>
      /// Signals a conversation event on the specified creature.
      /// </summary>
      /// <param name="creature">The creature to receive the conversation event.</param>
      public static void Signal(NwCreature creature)
      {
        Event nwEvent = NWScript.EventConversation()!;
        NWScript.SignalEvent(creature, nwEvent);
      }

      /// <summary>
      /// Pauses the current conversation for this creature.
      /// </summary>
      public void PauseConversation()
      {
        NWScript.ActionPauseConversation();
      }

      /// <summary>
      /// Resumes a paused conversation for this creature.
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
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="CreatureEvents.OnConversation"/>
    public event Action<CreatureEvents.OnConversation> OnConversation
    {
      add => EventService.Subscribe<CreatureEvents.OnConversation, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<CreatureEvents.OnConversation, GameEventFactory>(this, value);
    }
  }
}
