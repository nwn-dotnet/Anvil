using System;
using System.Collections.Generic;
using System.Reflection;
using NLog;
using NWM.API;
using NWM.API.Events;
using NWMX.API.Events;
using NWN;
using NWNX;

namespace NWM.Core
{
  [Service(typeof(IScriptDispatcher), IsCollection = true)]
  public sealed class EventService : IScriptDispatcher
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    // Dependencies
    private InteropService interopService;

    // Type info cache
    private Dictionary<Type, ScriptEventAttribute> cachedScriptEventInfo = new Dictionary<Type, ScriptEventAttribute>();
    private Dictionary<Type, NWNXEventAttribute> cachedNwnxEventInfo = new Dictionary<Type, NWNXEventAttribute>();

    // Lookup Data
    private Dictionary<string, EventHandler> scriptToEventMap = new Dictionary<string, EventHandler>();
    private Dictionary<Type, EventHandler> typeToHandlerMap = new Dictionary<Type, EventHandler>();

    public EventService(InteropService interopService)
    {
      this.interopService = interopService;
    }

    public void Unsubscribe<TEvent>(Action<TEvent> existingHandler) where TEvent : IEvent<TEvent>
    {
      if (typeToHandlerMap.TryGetValue(typeof(TEvent), out EventHandler eventHandler))
      {
        eventHandler.Unsubscribe(existingHandler);
      }
    }

    public void Unsubscribe<TObject, TEvent>(TObject nwObject, Action<TEvent> existingHandler) where TEvent : IEvent<TObject, TEvent>, new() where TObject : NwObject
    {
      if (typeToHandlerMap.TryGetValue(typeof(TEvent), out EventHandler eventHandler))
      {
        eventHandler.Unsubscribe(nwObject, existingHandler);
      }
    }

    public void Register<TObject, TEvent>(TObject nwObject) where TEvent : IEvent<TObject, TEvent>, new() where TObject : NwObject
    {
      EventHandler eventHandler = GetOrCreateHandler<TEvent>();
      CheckEventHooked<TObject, TEvent>(nwObject, eventHandler);
    }

    public void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : IEvent<TEvent>, new()
    {
      EventHandler eventHandler = GetOrCreateHandler<TEvent>();
      eventHandler.Subscribe(handler);
    }

    public void Subscribe<TObject, TEvent>(TObject nwObject, Action<TEvent> handler) where TEvent : IEvent<TObject, TEvent>, new() where TObject : NwObject
    {
      EventHandler eventHandler = GetOrCreateHandler<TEvent>();
      eventHandler.Subscribe(nwObject, handler);
      CheckEventHooked<TObject, TEvent>(nwObject, eventHandler);
    }

    private EventHandler GetOrCreateHandler<TEvent>() where TEvent : IEvent<TEvent>, new()
    {
      Type type = typeof(TEvent);
      if (typeToHandlerMap.TryGetValue(type, out EventHandler eventHandler))
      {
        return eventHandler;
      }

      if (GetEventInfo(type, cachedScriptEventInfo) != null)
      {
        return CreateEventHandler<TEvent>();
      }

      NWNXEventAttribute nwnxInfo = GetEventInfo(type, cachedNwnxEventInfo);
      if (nwnxInfo != null)
      {
        eventHandler = CreateEventHandler<TEvent>();
        EventsPlugin.SubscribeEvent(nwnxInfo.EventName, eventHandler.ScriptName);
        return eventHandler;
      }

      throw new InvalidOperationException($"Event Type {type.GetFullName()} does not define an event info attribute!");
    }

    private EventHandler CreateEventHandler<TEvent>() where TEvent : IEvent<TEvent>, new()
    {
      EventHandler eventHandler = EventHandler.Create<TEvent>();

      scriptToEventMap[eventHandler.ScriptName] = eventHandler;
      typeToHandlerMap[typeof(TEvent)] = eventHandler;
      return eventHandler;
    }

    private void CheckEventHooked<TObject, TEvent>(TObject nwObject, EventHandler eventHandler) where TEvent : IEvent<TObject, TEvent>, new() where TObject : NwObject
    {
      // Nothing to hook.
      if (nwObject == null)
      {
        return;
      }

      // We only need to hook native events, as nwnx events are set up during handler creation.
      ScriptEventAttribute scriptEventInfo = GetEventInfo(typeof(TEvent), cachedScriptEventInfo);
      if (scriptEventInfo == null)
      {
        return;
      }

      string existingScript = NWScript.GetEventScript(nwObject, (int) scriptEventInfo.EventScriptType);
      if (existingScript == eventHandler.ScriptName)
      {
        return;
      }

      Log.Debug($"Hooking native script event \"{scriptEventInfo.EventScriptType}\" on object \"{nwObject.Name}\". Previous script: \"{existingScript}\"");

      NWScript.SetEventScript(nwObject, (int) scriptEventInfo.EventScriptType, eventHandler.ScriptName);
      if (!string.IsNullOrEmpty(existingScript))
      {
        eventHandler.Subscribe<TObject, TEvent>(nwObject, gameEvent => ContinueWithNative(existingScript, nwObject));
      }
    }

    private void ContinueWithNative(string scriptName, NwObject objSelf)
    {
      interopService.ExecuteNss(scriptName, objSelf);
    }

    private T GetEventInfo<T>(Type type, Dictionary<Type, T> cache) where T : Attribute
    {
      if (cache.TryGetValue(type, out T eventInfo))
      {
        return eventInfo;
      }

      eventInfo = type.GetCustomAttribute<T>();
      cache[type] = eventInfo;

      return eventInfo;
    }

    public int ExecuteScript(string scriptName, uint oidSelf)
    {
      if (scriptToEventMap.TryGetValue(scriptName, out EventHandler eventHandler))
      {
        eventHandler.CallEvents(oidSelf.ToNwObject());
        return ScriptDispatchConstants.SCRIPT_HANDLED;
      }

      return ScriptDispatchConstants.SCRIPT_NOT_HANDLED;
    }
  }
}