using System;
using System.Collections.Generic;
using System.Linq;
using NWN.Services;

namespace NWN.API.Events
{
  [ServiceBinding(typeof(IEventFactory))]
  [ServiceBinding(typeof(IScriptDispatcher))]
  public class ScriptEventFactory : IEventFactory, IScriptDispatcher
  {
    // Caches
    private readonly Dictionary<Type, Func<IEvent>> eventConstructorCache = new Dictionary<Type, Func<IEvent>>();

    private readonly Dictionary<string, Type> eventMappings = new Dictionary<string, Type>();

    private EventService eventService;

    public void Register<TEvent>(string scriptName) where TEvent : IEvent, new()
    {
      CheckConstructorRegistered<TEvent>();
      eventMappings[scriptName] = typeof(TEvent);
    }

    public void Unregister(string scriptName)
    {
      eventMappings.Remove(scriptName);
    }

    void IEventFactory.Init(EventService eventService)
    {
      this.eventService = eventService;
    }

    void IEventFactory.Unregister<TEvent>()
    {
      foreach (KeyValuePair<string, Type> match in eventMappings.Where(eventMapping => eventMapping.Value == typeof(TEvent)).ToList())
      {
        eventMappings.Remove(match.Key);
      }
    }

    ScriptHandleResult IScriptDispatcher.ExecuteScript(string scriptName, uint oidSelf)
    {
      if (eventMappings.TryGetValue(scriptName, out Type eventType))
      {
        eventService.ProcessEvent(eventConstructorCache[eventType].Invoke());
        return ScriptHandleResult.Handled;
      }

      return ScriptHandleResult.NotHandled;
    }

    private void CheckConstructorRegistered<TEvent>() where TEvent : IEvent, new()
    {
      if (eventConstructorCache.ContainsKey(typeof(TEvent)))
      {
        return;
      }

      static IEvent Constructor() => new TEvent();
      eventConstructorCache[typeof(TEvent)] = Constructor;
    }
  }
}
