using System;
using System.Collections.Generic;
using NWN.API.Events;

namespace NWN.Services
{
  [ServiceBinding(typeof(ScriptEventService))]
  [ServiceBinding(typeof(IScriptDispatcher))]
  public sealed class ScriptEventService : IScriptDispatcher
  {
    private readonly Dictionary<string, EventHandler> eventHandlers = new Dictionary<string, EventHandler>();

    [Obsolete("The ScriptEventService will be removed in a future release. Events can be made using ScriptHandler attributes, and calling the event constructor manually.")]
    public void SetHandler<T>(string scriptName, Action<T> callback, bool callOriginal = false) where T : IEvent, new()
    {
      EventHandler<T> handler = new EventHandler<T>(() => new T(), callback, callOriginal);
      eventHandlers[scriptName] = handler;
    }

    public void ClearHandler(string scriptName)
    {
      eventHandlers.Remove(scriptName);
    }

    ScriptHandleResult IScriptDispatcher.ExecuteScript(string scriptName, uint oidSelf)
    {
      if (eventHandlers.TryGetValue(scriptName, out EventHandler handler))
      {
        return handler.Broadcast();
      }

      return ScriptHandleResult.NotHandled;
    }

    private abstract class EventHandler
    {
      public abstract ScriptHandleResult Broadcast();
    }

    private class EventHandler<T> : EventHandler where T : IEvent
    {
      public readonly Action<T> Callback;

      public readonly Func<T> ScriptEvent;
      public readonly bool CallOriginal;

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
