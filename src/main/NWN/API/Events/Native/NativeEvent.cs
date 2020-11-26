using System;
using NWN.Services;

namespace NWN.API.Events
{
  public abstract class NativeEvent<TObject, TEvent> : IEvent<TEvent> where TEvent : NativeEvent<TObject, TEvent> where TObject : NwObject
  {
    private event Action<TEvent> Callbacks;

    bool IEvent.HasSubscribers
    {
      get => Callbacks != null;
    }

    void IEvent<TEvent>.Subscribe(Action<TEvent> callback)
      => Callbacks += callback;

    void IEvent<TEvent>.Unsubscribe(Action<TEvent> callback)
      => Callbacks -= callback;

    void IEvent.ClearSubscribers()
      => Callbacks = null;

    ScriptHandleResult IEvent.Broadcast(NwObject objSelf)
    {
      PrepareEvent((TObject) objSelf);
      ProcessEvent();
      return ScriptHandleResult.Handled;
    }

    protected abstract void PrepareEvent(TObject objSelf);

    protected virtual void ProcessEvent()
    {
      InvokeCallbacks();
    }

    protected void InvokeCallbacks()
    {
      Callbacks?.Invoke((TEvent) this);
    }
  }
}
