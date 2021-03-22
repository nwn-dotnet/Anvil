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

    private class EventHandler<T> : EventHandler where T : IEvent
    {
      private readonly Action<T> Callback;

      private readonly Func<T> ScriptEvent;
      private readonly bool CallOriginal;

      public EventHandler(Func<T> scriptEvent, Action<T> callback, bool callOriginal)
      {
        ScriptEvent = scriptEvent;
        Callback = callback;
        CallOriginal = callOriginal;
      }

      public override ScriptHandleResult Broadcast()
      {
        T eventData = ScriptEvent.Invoke();
        Callback?.Invoke(eventData);

        if (CallOriginal)
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
