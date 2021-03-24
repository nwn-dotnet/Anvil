using System;
using System.Collections.Generic;
using System.Reflection;
using NLog;
using NWN.API;
using NWN.API.Events;
using NWN.Core.NWNX;
using NWN.Services;
using NWNX.API.Events;

namespace NWNX.Services
{
  [ServiceBinding(typeof(IEventFactory))]
  [ServiceBinding(typeof(IScriptDispatcher))]
  public class NWNXEventFactory : IEventFactory, IScriptDispatcher
  {
    private const string InternalScriptName = "____nixie_event";

    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    // Caches
    private readonly Dictionary<Type, NWNXEventAttribute> eventInfoCache = new Dictionary<Type, NWNXEventAttribute>();
    private readonly Dictionary<string, Func<IEvent>> eventConstructorCache = new Dictionary<string, Func<IEvent>>();

    private EventService eventService;

    public void Register<TEvent>() where TEvent : IEvent, new()
    {
      string eventName = GetEventInfo(typeof(TEvent)).EventName;
      CheckConstructorRegistered<TEvent>(eventName);

      UpdateEventScript(eventName);
    }

    void IEventFactory.Init(EventService eventService)
    {
      this.eventService = eventService;
    }

    void IEventFactory.Unregister<TEvent>()
    {
      string eventName = GetEventInfo(typeof(TEvent)).EventName;
      EventsPlugin.UnsubscribeEvent(eventName, InternalScriptName);
    }

    ScriptHandleResult IScriptDispatcher.ExecuteScript(string scriptName, uint oidSelf)
    {
      if (eventService == null || scriptName != InternalScriptName)
      {
        return ScriptHandleResult.NotHandled;
      }

      string eventName = EventsPlugin.GetCurrentEvent();
      if (string.IsNullOrEmpty(eventName))
      {
        return ScriptHandleResult.NotHandled;
      }

      if (eventConstructorCache.TryGetValue(eventName, out Func<IEvent> value))
      {
        ProcessEvent(value.Invoke());
        return ScriptHandleResult.Handled;
      }

      return ScriptHandleResult.NotHandled;
    }

    private void ProcessEvent(IEvent eventInstance)
    {
      eventService.ProcessEvent(eventInstance);

      if (eventInstance is IEventNWNXResult nwnxEvent && nwnxEvent.EventResult != null)
      {
        EventsPlugin.SetEventResult(nwnxEvent.EventResult);
      }

      if (eventInstance is IEventSkippable {Skip: true})
      {
        EventsPlugin.SkipEvent();
      }
    }

    private void UpdateEventScript(string eventName)
    {
      Log.Debug($"Hooking NWNX script event \"{eventName}\".");
      EventsPlugin.SubscribeEvent(eventName, InternalScriptName);
    }

    private NWNXEventAttribute GetEventInfo(Type type)
    {
      if (eventInfoCache.TryGetValue(type, out NWNXEventAttribute eventAttribute))
      {
        return eventAttribute;
      }

      eventAttribute = LoadEventInfo(type);
      eventInfoCache[type] = eventAttribute;

      return eventAttribute;
    }

    private NWNXEventAttribute LoadEventInfo(Type type)
    {
      NWNXEventAttribute attribute = type.GetCustomAttribute<NWNXEventAttribute>();
      if (attribute != null)
      {
        return attribute;
      }

      throw new InvalidOperationException($"Event Type {type.GetFullName()} does not define an event info attribute!");
    }

    private void CheckConstructorRegistered<TEvent>(string eventName) where TEvent : IEvent, new()
    {
      if (eventConstructorCache.ContainsKey(eventName))
      {
        return;
      }

      static IEvent Constructor() => new TEvent();
      eventConstructorCache[eventName] = Constructor;
    }
  }
}
