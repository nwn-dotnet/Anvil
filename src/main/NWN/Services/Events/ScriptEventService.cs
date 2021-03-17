using System;
using System.Collections.Generic;
using NWN.API.Events;

namespace NWN.Services
{
  [ServiceBinding(typeof(ScriptEventService))]
  public sealed class ScriptEventService
  {
    private readonly Dictionary<string, EventHandler> eventHandlers = new Dictionary<string, EventHandler>();

    private readonly EventService eventService;

    public ScriptEventService(EventService eventService)
    {
      this.eventService = eventService;
    }

    public void SetHandler<T>(string scriptName, Action<T> callback, bool callOriginal = false) where T : IEvent, new()
    {
      eventService.SubscribeAll<T, ScriptEventFactory>(callback).Register<T>(scriptName);

      // if (eventHandlers.TryGetValue(scriptName, out EventHandler handler))
      // {
      //   handler.ScriptEvent.ClearSubscribers();
      // }
      //
      // T scriptEvent = new T();
      // scriptEvent.Subscribe(callback);
      //
      // eventHandlers[scriptName] = new EventHandler(scriptEvent, callOriginal);
    }

    public void ClearHandler(string scriptName)
    {
      eventService.GetEventFactory<ScriptEventFactory>().Unregister(scriptName);

      // if (eventHandlers.TryGetValue(scriptName, out EventHandler handler))
      // {
      //   handler.ScriptEvent.ClearSubscribers();
      //   eventHandlers.Remove(scriptName);
      // }
    }

    private ScriptHandleResult ExecuteScript(string scriptName, uint oidSelf)
    {
      ScriptHandleResult result = ScriptHandleResult.NotHandled;
      if (eventHandlers.TryGetValue(scriptName, out EventHandler handler))
      {
        //result = handler.ScriptEvent.Broadcast(oidSelf.ToNwObject());
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
