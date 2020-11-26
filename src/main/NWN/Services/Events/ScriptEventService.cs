using System;
using System.Collections.Generic;
using NWN.API;
using NWN.API.Events;

namespace NWN.Services
{
  [ServiceBinding(typeof(ScriptEventService))]
  [ServiceBinding(typeof(IScriptDispatcher))]
  public sealed class ScriptEventService : IScriptDispatcher
  {
    private readonly Dictionary<string, EventHandler> eventHandlers = new Dictionary<string, EventHandler>();

    public void SetHandler<T>(string scriptName, Action<T> callback, bool callOriginal = false) where T : IEvent<T>, new()
    {
      if (eventHandlers.TryGetValue(scriptName, out EventHandler handler))
      {
        handler.ScriptEvent.ClearSubscribers();
      }

      T scriptEvent = new T();
      scriptEvent.Subscribe(callback);

      eventHandlers[scriptName] = new EventHandler(scriptEvent, callOriginal);
    }

    public void ClearHandler(string scriptName)
    {
      if (eventHandlers.TryGetValue(scriptName, out EventHandler handler))
      {
        handler.ScriptEvent.ClearSubscribers();
        eventHandlers.Remove(scriptName);
      }
    }

    ScriptHandleResult IScriptDispatcher.ExecuteScript(string scriptName, uint oidSelf)
    {
      ScriptHandleResult result = ScriptHandleResult.NotHandled;
      if (eventHandlers.TryGetValue(scriptName, out EventHandler handler))
      {
        result = handler.ScriptEvent.Broadcast(oidSelf.ToNwObject());
        if (handler.CallOriginal)
        {
          result = ScriptHandleResult.NotHandled;
        }
      }

      return result;
    }

    private class EventHandler
    {
      public readonly IEvent ScriptEvent;
      public readonly bool CallOriginal;

      public EventHandler(IEvent scriptEvent, bool callOriginal)
      {
        ScriptEvent = scriptEvent;
        CallOriginal = callOriginal;
      }
    }
  }
}
