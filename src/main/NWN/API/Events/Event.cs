using System;

namespace NWN.API.Events
{
  public abstract class Event
  {
    public void BroadcastEvent(NwObject objSelf)
    {
      PrepareEvent(objSelf);
      InvokeCallbacks();
    }

    protected abstract void PrepareEvent(NwObject objSelf);
    protected abstract void InvokeCallbacks();
  }

  public abstract class Event<T> : Event where T : Event<T>
  {
    public event Action<T> Callbacks;

    protected override void InvokeCallbacks()
    {
      Callbacks?.Invoke((T) this);
    }
  }

  public abstract class Event<TObject, TEvent> : Event<TEvent> where TEvent : Event<TEvent> where TObject : NwObject
  {
    protected sealed override void PrepareEvent(NwObject objSelf)
    {
      PrepareEvent((TObject) objSelf);
    }

    protected abstract void PrepareEvent(TObject objSelf);
  }
}