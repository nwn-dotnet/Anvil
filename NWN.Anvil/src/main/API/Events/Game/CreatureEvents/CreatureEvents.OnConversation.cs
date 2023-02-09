using System;
using Anvil.API.Events;
using NWN.Core;

namespace Anvil.API.Events
{
  /// <summary>
  /// Events for <see cref="NwCreature"/>.
  /// </summary>
  public static partial class CreatureEvents
  {
    /// <summary>
    /// Called when this creature starts a conversation, or hears a message they are listening for.
    /// </summary>
    [GameEvent(EventScriptType.CreatureOnDialogue)]
    public sealed class OnConversation : IEvent
    {
      /// <summary>
      /// Gets the creature attached to this conversation.
      /// </summary>
      public NwCreature Creature { get; } = NWScript.OBJECT_SELF.ToNwObject<NwCreature>()!;

      /// <summary>
      /// Gets the <see cref="NwCreature"/> currently speaking.
      /// </summary>
      [Obsolete("Use the Creature property instead.")]
      public NwCreature? CurrentSpeaker { get; } = NWScript.OBJECT_SELF.ToNwObject<NwCreature>();

      /// <summary>
      /// Gets the last <see cref="NwGameObject"/> that spoke in this conversation.
      /// </summary>
      public NwGameObject? LastSpeaker { get; } = NWScript.GetLastSpeaker().ToNwObject<NwGameObject>();

      /// <summary>
      /// Gets the <see cref="NwPlayer"/> speaker in this conversation.
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

      public static void Signal(NwCreature creature)
      {
        Event nwEvent = NWScript.EventConversation()!;
        NWScript.SignalEvent(creature, nwEvent);
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
