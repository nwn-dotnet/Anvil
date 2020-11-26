using System;
using NWN.Services;

namespace NWN.API.Events
{
  public abstract class ScriptEvent<T> : IEvent<T> where T : ScriptEvent<T>
  {
    private event Action<T> Callbacks;

    public bool HasSubscribers
    {
      get => Callbacks != null;
    }

    void IEvent<T>.Subscribe(Action<T> callback)
      => Callbacks += callback;

    void IEvent<T>.Unsubscribe(Action<T> callback)
      => Callbacks -= callback;

    void IEvent.ClearSubscribers()
      => Callbacks = null;

    ScriptHandleResult IEvent.Broadcast(NwObject objSelf)
    {
      PrepareEvent(objSelf);
      return ProcessEvent();
    }

    protected abstract void PrepareEvent(NwObject objSelf);

    protected virtual ScriptHandleResult ProcessEvent()
    {
      InvokeCallbacks();
      return ScriptHandleResult.Handled;
    }

    protected void InvokeCallbacks()
    {
      Callbacks?.Invoke((T) this);
    }
  }
}
