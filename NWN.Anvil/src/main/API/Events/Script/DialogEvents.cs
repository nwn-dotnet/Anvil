using NWN.Core;

namespace Anvil.API.Events
{
  /// <summary>
  /// Provides dialog-related script events and helpers sourced from NWScript.
  /// </summary>
  public static class DialogEvents
  {
    /// <summary>
    /// Triggered when a dialog "Action Taken" node is executed.
    /// </summary>
    /// <remarks>
    /// This class should be constructed manually as a part of a script handler, matching the script name specified in the dialog node.
    /// </remarks>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// [ScriptHandler("my_action_taken")]
    /// public void DialogActionTaken(CallInfo callInfo)
    /// {
    ///   DialogEvents.ActionTaken eventData = new DialogEvents.ActionTaken();
    ///   Log.Info($"Action taken, speaker: {eventData.CurrentSpeaker?.Name}, player: {eventData.PlayerSpeaker?.ControlledCreature?.Name}");
    /// }
    /// ]]>
    /// </code>
    /// </example>
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

      /// <summary>
      /// Pauses the current conversation. Wraps <c>NWScript.ActionPauseConversation()</c>.
      /// </summary>
      public void PauseConversation()
      {
        NWScript.ActionPauseConversation();
      }

      /// <summary>
      /// Resumes a paused conversation. Wraps <c>NWScript.ActionResumeConversation()</c>.
      /// </summary>
      public void ResumeConversation()
      {
        NWScript.ActionResumeConversation();
      }
    }

    /// <summary>
    /// Triggered when a dialog "Appears When" node is executed.
    /// </summary>
    /// <remarks>
    /// This event class should be constructed manually as a part of a script handler, matching the script name specified in the dialog node.
    /// </remarks>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// [ScriptHandler("is_gnome")]
    /// public bool IsGnome(CallInfo callInfo)
    /// {
    ///   DialogEvents.AppearsWhen appearsWhen = new DialogEvents.AppearsWhen();
    ///   return appearsWhen.PlayerSpeaker?.ControlledCreature?.Race == NwRace.FromRacialType(RacialType.Gnome);
    /// }
    /// ]]>
    /// </code>
    /// </example>
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
