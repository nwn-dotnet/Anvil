using System;
using System.Collections.Generic;
using System.Reflection;
using NLog;
using NWN.API.Constants;
using NWN.Core;
using NWN.Services;

namespace NWN.API.Events
{
  [ServiceBinding(typeof(IEventFactory))]
  [ServiceBinding(typeof(IScriptDispatcher))]
  public sealed class GameEventFactory : IEventFactory, IScriptDispatcher
  {
    private const string InternalScriptName = "_____nman_event";

    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    // Caches
    private readonly Dictionary<Type, NativeEventAttribute> eventInfoCache = new Dictionary<Type, NativeEventAttribute>();
    private readonly Dictionary<EventScriptType, Func<IEvent>> eventConstructorCache = new Dictionary<EventScriptType, Func<IEvent>>();

    private EventService eventService;

    public void Register<TEvent>(NwObject nwObject) where TEvent : IEvent, new()
    {
      EventScriptType eventScriptType = GetEventInfo(typeof(TEvent)).EventScriptType;
      CheckConstructorRegistered<TEvent>(eventScriptType);

      UpdateEventScript(nwObject, eventScriptType);
    }

    void IEventFactory.Init(EventService eventService)
    {
      this.eventService = eventService;
    }

    void IEventFactory.Unregister<TEvent>() {}

    ScriptHandleResult IScriptDispatcher.ExecuteScript(string scriptName, uint oidSelf)
    {
      if (eventService == null || scriptName != InternalScriptName)
      {
        return ScriptHandleResult.NotHandled;
      }

      EventScriptType eventScriptType = (EventScriptType)NWScript.GetCurrentlyRunningEvent();
      if (eventScriptType == EventScriptType.None)
      {
        return ScriptHandleResult.NotHandled;
      }

      if (eventConstructorCache.TryGetValue(eventScriptType, out Func<IEvent> value))
      {
        eventService.ProcessEvent(value.Invoke());
        return ScriptHandleResult.Handled;
      }

      return ScriptHandleResult.NotHandled;
    }

    private void UpdateEventScript(NwObject nwObject, EventScriptType eventType)
    {
      string existingScript = NWScript.GetEventScript(nwObject, (int) eventType);
      if (existingScript == InternalScriptName)
      {
        return;
      }

      Log.Debug($"Hooking native script event \"{eventType}\" on object \"{nwObject.Name}\". Previous script: \"{existingScript}\"");
      NWScript.SetEventScript(nwObject, (int) eventType, InternalScriptName);
    }

    private NativeEventAttribute GetEventInfo(Type type)
    {
      if (eventInfoCache.TryGetValue(type, out NativeEventAttribute eventAttribute))
      {
        return eventAttribute;
      }

      eventAttribute = LoadEventInfo(type);
      eventInfoCache[type] = eventAttribute;

      return eventAttribute;
    }

    private NativeEventAttribute LoadEventInfo(Type type)
    {
      NativeEventAttribute attribute = type.GetCustomAttribute<NativeEventAttribute>();
      if (attribute != null)
      {
        return attribute;
      }

      throw new InvalidOperationException($"Event Type {type.GetFullName()} does not define an event info attribute!");
    }

    private void CheckConstructorRegistered<TEvent>(EventScriptType eventScriptType) where TEvent : IEvent, new()
    {
      if (eventConstructorCache.ContainsKey(eventScriptType))
      {
        return;
      }

      static IEvent Constructor() => new TEvent();
      eventConstructorCache[eventScriptType] = Constructor;
    }
  }
}
