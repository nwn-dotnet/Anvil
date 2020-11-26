using NWN.Core;
using NWN.Services;

namespace NWN.API.Events
{
  public static class DialogEvents
  {
    public class ActionTaken : ScriptEvent<ActionTaken>
    {
      /// <summary>
      /// Gets the creature/object currently speaking.
      /// </summary>
      public NwGameObject CurrentSpeaker { get; private set; }

      /// <summary>
      /// Gets the player speaker in this conversation.
      /// </summary>
      public NwPlayer PlayerSpeaker { get; private set; }

      /// <summary>
      /// Gets the last creature/object that spoke in this conversation.
      /// </summary>
      public NwGameObject LastSpeaker { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        CurrentSpeaker = (NwGameObject)objSelf;
        PlayerSpeaker = NWScript.GetPCSpeaker().ToNwObject<NwPlayer>();
        LastSpeaker = NWScript.GetLastSpeaker().ToNwObject<NwGameObject>();
      }

      public void PauseConversation()
        => NWScript.ActionPauseConversation();

      public void ResumeConversation()
        => NWScript.ActionResumeConversation();
    }

    public class AppearsWhen : ScriptEvent<AppearsWhen>
    {
      /// <summary>
      /// Gets or sets a value indicating whether this dialog option should be shown.
      /// </summary>
      public bool ShouldShow { get; set; }

      /// <summary>
      /// Gets the creature/object currently speaking.
      /// </summary>
      public NwGameObject CurrentSpeaker { get; private set; }

      /// <summary>
      /// Gets the player speaker in this conversation.
      /// </summary>
      public NwPlayer PlayerSpeaker { get; private set; }

      /// <summary>
      /// Gets the last creature/object that spoke in this conversation.
      /// </summary>
      public NwGameObject LastSpeaker { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        CurrentSpeaker = (NwGameObject)objSelf;
        PlayerSpeaker = NWScript.GetPCSpeaker().ToNwObject<NwPlayer>();
        LastSpeaker = NWScript.GetLastSpeaker().ToNwObject<NwGameObject>();
      }

      protected override ScriptHandleResult ProcessEvent()
      {
        InvokeCallbacks();
        return ShouldShow ? ScriptHandleResult.True : ScriptHandleResult.False;
      }
    }
  }
}
