using NWN.Core;

namespace Anvil.API.Events
{
  public static class DialogEvents
  {
    public sealed class ActionTaken : IEvent
    {
      /// <summary>
      /// Gets the creature/object currently speaking.
      /// </summary>
      public NwGameObject? CurrentSpeaker { get; } = NWScript.OBJECT_SELF.ToNwObject<NwGameObject>();

      /// <summary>
      /// Gets the last creature/object that spoke in this conversation.
      /// </summary>
      public NwGameObject? LastSpeaker { get; } = NWScript.GetLastSpeaker().ToNwObject<NwGameObject>();

      /// <summary>
      /// Gets the player speaker in this conversation.
      /// </summary>
      public NwPlayer? PlayerSpeaker { get; } = NWScript.GetPCSpeaker().ToNwPlayer();

      NwObject? IEvent.Context => CurrentSpeaker;

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
      /// <summary>
      /// Gets the creature/object currently speaking.
      /// </summary>
      public NwGameObject? CurrentSpeaker { get; } = NWScript.OBJECT_SELF.ToNwObject<NwGameObject>();

      /// <summary>
      /// Gets the last creature/object that spoke in this conversation.
      /// </summary>
      public NwGameObject? LastSpeaker { get; } = NWScript.GetLastSpeaker().ToNwObject<NwGameObject>();

      /// <summary>
      /// Gets the player speaker in this conversation.
      /// </summary>
      public NwPlayer? PlayerSpeaker { get; } = NWScript.GetPCSpeaker().ToNwPlayer();

      NwObject? IEvent.Context => CurrentSpeaker;
    }
  }
}
