using NWN.Core;

namespace Anvil.API.Events
{
  public static class DialogEvents
  {
    public sealed class ActionTaken : IEvent
    {
      public ActionTaken()
      {
        CurrentSpeaker = NWScript.OBJECT_SELF.ToNwObject<NwGameObject>();
        PlayerSpeaker = NWScript.GetPCSpeaker().ToNwPlayer();
        LastSpeaker = NWScript.GetLastSpeaker().ToNwObject<NwGameObject>();
      }

      /// <summary>
      /// Gets the creature/object currently speaking.
      /// </summary>
      public NwGameObject CurrentSpeaker { get; }

      /// <summary>
      /// Gets the last creature/object that spoke in this conversation.
      /// </summary>
      public NwGameObject LastSpeaker { get; }

      /// <summary>
      /// Gets the player speaker in this conversation.
      /// </summary>
      public NwPlayer PlayerSpeaker { get; }

      NwObject IEvent.Context => CurrentSpeaker;

      public void PauseConversation()
      {
        NWScript.ActionPauseConversation();
      }

      public void ResumeConversation()
      {
        NWScript.ActionResumeConversation();
      }
    }

    public sealed class AppearsWhen : IEvent
    {
      public AppearsWhen()
      {
        CurrentSpeaker = NWScript.OBJECT_SELF.ToNwObject<NwGameObject>();
        PlayerSpeaker = NWScript.GetPCSpeaker().ToNwPlayer();
        LastSpeaker = NWScript.GetLastSpeaker().ToNwObject<NwGameObject>();
      }

      /// <summary>
      /// Gets the creature/object currently speaking.
      /// </summary>
      public NwGameObject CurrentSpeaker { get; }

      /// <summary>
      /// Gets the last creature/object that spoke in this conversation.
      /// </summary>
      public NwGameObject LastSpeaker { get; }

      /// <summary>
      /// Gets the player speaker in this conversation.
      /// </summary>
      public NwPlayer PlayerSpeaker { get; }

      NwObject IEvent.Context => CurrentSpeaker;
    }
  }
}
