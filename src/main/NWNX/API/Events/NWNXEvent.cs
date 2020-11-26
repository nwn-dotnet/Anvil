using System;
using NWN.API;
using NWN.API.Events;
using NWN.Services;

namespace NWNX.API.Events
{
  public abstract class NWNXEvent<T> : IEvent<T> where T : NWNXEvent<T>
  {
    private event Action<T> Callbacks;

    internal string ScriptName;

    bool IEvent.HasSubscribers
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
      ProcessEvent();
      return ScriptHandleResult.Handled;
    }

    protected abstract void PrepareEvent(NwObject objSelf);

    protected virtual void ProcessEvent()
    {
      InvokeCallbacks();
    }

    protected void InvokeCallbacks()
    {
      Callbacks?.Invoke((T) this);
    }
  }
}
