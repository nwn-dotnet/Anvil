using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NLog;
using NWM.API;
using NWM.API.Events;
using NWMX.API.Events;
using NWNX;

namespace NWM.Core
{
  [Service(typeof(IScriptDispatcher), IsCollection = true)]
  public sealed class EventService : IScriptDispatcher
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    // Type info cache
    private Dictionary<Type, IEventAttribute> cachedEventInfo = new Dictionary<Type, IEventAttribute>();

    // Lookup Data
    private Dictionary<string, EventHandler> scriptToEventMap = new Dictionary<string, EventHandler>();
    private Dictionary<Type, EventHandler> typeToHandlerMap = new Dictionary<Type, EventHandler>();

    public void SignalNWNXEvent(string eventName, NwObject target)
    {
      EventsPlugin.SignalEvent(eventName, target);
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

    private void CheckEventHooked<TObject, TEvent>(TObject nwObject, EventHandler eventHandler) where TEvent : IEvent<TObject, TEvent>, new() where TObject : NwObject
    {
      // Nothing to hook.
      if (nwObject == null)
      {
        return;
      }

      // We only need to hook native events, as nwnx events are set up during handler creation.
      IEventAttribute eventAttribute = GetEventInfo(typeof(TEvent));
      eventAttribute.InitObjectHook<TObject, TEvent>(eventHandler, nwObject, eventHandler.ScriptName);
    }

    private EventHandler GetOrCreateHandler<TEvent>() where TEvent : IEvent<TEvent>, new()
    {
      Type type = typeof(TEvent);
      if (typeToHandlerMap.TryGetValue(type, out EventHandler eventHandler))
      {
        return eventHandler;
      }

      IEventAttribute eventInfo = GetEventInfo(type);
      eventHandler = CreateEventHandler<TEvent>();
      eventInfo.InitHook(eventHandler.ScriptName);

      return eventHandler;
    }

    private IEventAttribute GetEventInfo(Type type)
    {
      if (cachedEventInfo.TryGetValue(type, out IEventAttribute eventAttribute))
      {
        return eventAttribute;
      }

      eventAttribute = LoadEventInfo(type);
      cachedEventInfo[type] = eventAttribute;

      return eventAttribute;
    }

    private IEventAttribute LoadEventInfo(Type type)
    {
      object eventAttribute = type.GetCustomAttributes(typeof(IEventAttribute), true).SingleOrDefault();
      if (eventAttribute != null)
      {
        return (IEventAttribute) eventAttribute;
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