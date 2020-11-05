using System;
using NWN.Core;
using NWN.Services;

namespace NWN.API
{
  public abstract class DialogEvent
  {
    public ScriptHandleResult ProcessEvent(NwObject objSelf)
    {
      PrepareEvent(objSelf);
      return InvokeCallbacks();
    }

    protected abstract void PrepareEvent(NwObject objSelf);

    protected abstract ScriptHandleResult InvokeCallbacks();
  }

  public abstract class DialogEvent<T> : DialogEvent where T : DialogEvent<T>
  {
    public Action<T> Callback;

    protected override ScriptHandleResult InvokeCallbacks()
    {
      Callback?.Invoke((T) this);
      return ScriptHandleResult.Handled;
    }
  }

  public static class DialogEvents
  {
    public class ActionTaken : DialogEvent<ActionTaken>
    {
      public NwGameObject CurrentSpeaker { get; private set; }
      public NwPlayer PlayerSpeaker { get; private set; }
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

    public class AppearsWhen : DialogEvent<AppearsWhen>
    {
      public bool ShouldShow { get; set; }
      public NwGameObject CurrentSpeaker { get; private set; }
      public NwPlayer PlayerSpeaker { get; private set; }
      public NwGameObject LastSpeaker { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        CurrentSpeaker = (NwGameObject)objSelf;
        PlayerSpeaker = NWScript.GetPCSpeaker().ToNwObject<NwPlayer>();
        LastSpeaker = NWScript.GetLastSpeaker().ToNwObject<NwGameObject>();
      }

      protected override ScriptHandleResult InvokeCallbacks()
      {
        base.InvokeCallbacks();
        return ShouldShow ? ScriptHandleResult.True : ScriptHandleResult.False;
      }
    }
  }
}
