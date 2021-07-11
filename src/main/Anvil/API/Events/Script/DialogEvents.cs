using Anvil.API;
using NWN.Core;

namespace NWN.API.Events
{
  public static class DialogEvents
  {
    public class ActionTaken : IEvent
    {
      /// <summary>
      /// Gets the creature/object currently speaking.
      /// </summary>
      public NwGameObject CurrentSpeaker { get; }

      /// <summary>
      /// Gets the player speaker in this conversation.
      /// </summary>
      public NwPlayer PlayerSpeaker { get; }

      /// <summary>
      /// Gets the last creature/object that spoke in this conversation.
      /// </summary>
      public NwGameObject LastSpeaker { get; }

      NwObject IEvent.Context
      {
        get => CurrentSpeaker;
      }

      public ActionTaken()
      {
        CurrentSpeaker = NWScript.OBJECT_SELF.ToNwObject<NwGameObject>();
        PlayerSpeaker = NWScript.GetPCSpeaker().ToNwPlayer();
        LastSpeaker = NWScript.GetLastSpeaker().ToNwObject<NwGameObject>();
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

    public class AppearsWhen : IEvent
    {
      /// <summary>
      /// Gets the creature/object currently speaking.
      /// </summary>
      public NwGameObject CurrentSpeaker { get; }

      /// <summary>
      /// Gets the player speaker in this conversation.
      /// </summary>
      public NwPlayer PlayerSpeaker { get; }

      /// <summary>
      /// Gets the last creature/object that spoke in this conversation.
      /// </summary>
      public NwGameObject LastSpeaker { get; }

      NwObject IEvent.Context
      {
        get => CurrentSpeaker;
      }

      public AppearsWhen()
      {
        CurrentSpeaker = NWScript.OBJECT_SELF.ToNwObject<NwGameObject>();
        PlayerSpeaker = NWScript.GetPCSpeaker().ToNwPlayer();
        LastSpeaker = NWScript.GetLastSpeaker().ToNwObject<NwGameObject>();
      }
    }
  }
}
