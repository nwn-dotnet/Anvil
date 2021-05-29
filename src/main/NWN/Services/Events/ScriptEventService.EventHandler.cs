using System;
using NWN.API.Events;

namespace NWN.Services
{
  public sealed partial class ScriptEventService
  {
    private abstract class EventHandler
    {
      public abstract ScriptHandleResult Broadcast();
    }

    private sealed class EventHandler<T> : EventHandler where T : IEvent
    {
      private readonly Action<T> callback;

      private readonly Func<T> scriptEvent;
      private readonly bool callOriginal;

      public EventHandler(Func<T> scriptEvent, Action<T> callback, bool callOriginal)
      {
        this.scriptEvent = scriptEvent;
        this.callback = callback;
        this.callOriginal = callOriginal;
      }

      public override ScriptHandleResult Broadcast()
      {
        T eventData = scriptEvent.Invoke();
        callback?.Invoke(eventData);

        if (callOriginal)
        {
          return ScriptHandleResult.NotHandled;
        }

        if (eventData is IEventScriptResult scriptResult)
        {
          return scriptResult.Result;
        }

        return ScriptHandleResult.Handled;
      }
    }
  }
}
