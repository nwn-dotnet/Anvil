using System;

namespace NWN.API.Events
{
  public abstract class NativeEvent
  {
    public void ProcessEvent(NwObject objSelf)
    {
      PrepareEvent(objSelf);
      InvokeCallbacks();
    }

    protected abstract void PrepareEvent(NwObject objSelf);

    protected abstract void InvokeCallbacks();
  }

  public abstract class NativeEvent<TObject, TEvent> : NativeEvent where TEvent : NativeEvent<TObject, TEvent> where TObject : NwObject
  {
    internal event Action<TEvent> Callbacks;

    internal bool HasSubscribers
    {
      get => Callbacks != null;
    }

    protected override void InvokeCallbacks()
    {
      Callbacks?.Invoke((TEvent) this);
    }

    protected sealed override void PrepareEvent(NwObject objSelf)
    {
      PrepareEvent((TObject) objSelf);
    }

    protected abstract void PrepareEvent(TObject objSelf);
  }
}
