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
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();
    private const string InternalScriptName = "____nwnm_event";

    private readonly Lazy<EventService> eventService;

    // Caches
    private readonly Dictionary<Type, GameEventAttribute> eventInfoCache = new Dictionary<Type, GameEventAttribute>();
    private readonly Dictionary<EventScriptType, Func<IEvent>> eventConstructorCache = new Dictionary<EventScriptType, Func<IEvent>>();

    public GameEventFactory(Lazy<EventService> eventService)
    {
      this.eventService = eventService;
    }

    public void Register<TEvent>(NwObject nwObject, bool callOriginal = true) where TEvent : IEvent, new()
    {
      EventScriptType eventScriptType = GetEventInfo(typeof(TEvent)).EventScriptType;

      CheckConstructorRegistered<TEvent>(eventScriptType);
      UpdateEventScript<TEvent>(nwObject, eventScriptType, callOriginal);
    }

    void IEventFactory.Init() {}

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
        eventService.Value.ProcessEvent(value.Invoke());
        return ScriptHandleResult.Handled;
      }

      return ScriptHandleResult.NotHandled;
    }

    private void UpdateEventScript<TEvent>(NwObject nwObject, EventScriptType eventType, bool callOriginal) where TEvent : IEvent, new()
    {
      string existingScript = NWScript.GetEventScript(nwObject, (int) eventType);
      if (existingScript == InternalScriptName)
      {
        return;
      }

      Log.Debug($"Hooking native script event \"{eventType}\" on object \"{nwObject.Name}\". Previous script: \"{existingScript}\"");
      NWScript.SetEventScript(nwObject, (int) eventType, InternalScriptName);

      if (callOriginal)
      {
        eventService.Value.Subscribe<TEvent, GameEventFactory>(nwObject, (_) => NWScript.ExecuteScript(existingScript));
      }
    }

    private GameEventAttribute GetEventInfo(Type type)
    {
      if (eventInfoCache.TryGetValue(type, out GameEventAttribute eventAttribute))
      {
        return eventAttribute;
      }

      eventAttribute = LoadEventInfo(type);
      eventInfoCache[type] = eventAttribute;

      return eventAttribute;
    }

    private GameEventAttribute LoadEventInfo(Type type)
    {
      GameEventAttribute attribute = type.GetCustomAttribute<GameEventAttribute>();
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
