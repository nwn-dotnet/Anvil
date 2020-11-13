using System;
using NWN.Services;

namespace NWN.API.Events
{
  public abstract class ScriptEvent
  {
    public ScriptHandleResult ProcessEvent(NwObject objSelf)
    {
      PrepareEvent(objSelf);
      return InvokeCallbacks();
    }

    protected abstract void PrepareEvent(NwObject objSelf);

    protected abstract ScriptHandleResult InvokeCallbacks();
  }

  public abstract class ScriptEvent<T> : ScriptEvent where T : ScriptEvent<T>
  {
    public Action<T> Callback;

    protected override ScriptHandleResult InvokeCallbacks()
    {
      Callback?.Invoke((T) this);
      return ScriptHandleResult.Handled;
    }
  }
}
