using System;
using NWN.API;

namespace NWNX.API.Events
{
  public abstract class NWNXEvent
  {
    public void ProcessEvent(NwObject objSelf)
    {
      PrepareEvent(objSelf);
      ProcessEvent();
    }

    protected abstract void PrepareEvent(NwObject objSelf);

    protected abstract void ProcessEvent();
  }

  public abstract class NWNXEvent<T> : NWNXEvent where T : NWNXEvent<T>
  {
    public event Action<T> Callbacks;

    internal string ScriptName;

    internal bool HasSubscribers
    {
      get => Callbacks != null;
    }

    protected override void ProcessEvent()
    {
      InvokeCallbacks();
    }

    protected void InvokeCallbacks()
    {
      Callbacks?.Invoke((T) this);
    }
  }
}
