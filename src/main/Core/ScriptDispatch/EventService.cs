using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NLog;
using NWM.API;
using NWNX;

namespace NWM.Core
{
  [Service(typeof(IScriptDispatcher), IsCollection = true)]
  public sealed class EventService : IScriptDispatcher
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private Dictionary<Type, EventInfoAttribute> cachedEventInfo = new Dictionary<Type, EventInfoAttribute>();
    private Dictionary<string, IEvent> scriptToEventMap = new Dictionary<string, IEvent>();

    public void SubscribeExplicit<TEvent>(string scriptName, Action<TEvent> handler) where TEvent : IEvent<TEvent>, new()
    {
      TEvent gameEvent = GetOrCreateEvent<TEvent>(GetEventInfo(typeof(TEvent)), scriptName);
      gameEvent.Callbacks += handler;
    }

    public void Subscribe<TEvent>(string scriptPrefix, Action<TEvent> handler) where TEvent : IEvent<TEvent>, new()
    {
      EventInfoAttribute eventInfo = GetEventInfo(typeof(TEvent));

      TEvent gameEvent = eventInfo.EventType == EventType.Native ? GetOrCreateEvent<TEvent>(eventInfo, scriptPrefix + eventInfo.DefaultScriptSuffix) : GetOrCreateEvent<TEvent>(eventInfo);
      gameEvent.Callbacks += handler;
    }

    public void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : IEvent<TEvent>, new()
    {
      Type tEventType = typeof(TEvent);
      EventInfoAttribute eventInfo = GetEventInfo(tEventType);
      if (eventInfo.EventType == EventType.Native)
      {
        Log.Error($"A subscribe attempt was made for native event {tEventType.FullName} without a prefix or name defined! This is only supported on NWNX events. Use the scriptPrefix overload method instead.");
        return;
      }

      TEvent gameEvent = GetOrCreateEvent<TEvent>(eventInfo);
      gameEvent.Callbacks += handler;
    }

    public void Unsubscribe<TEvent>(Action<TEvent> existingHandler) where TEvent : IEvent<TEvent>
    {
      TEvent gameEvent = scriptToEventMap.Values.OfType<TEvent>().FirstOrDefault();
      if (gameEvent != null)
      {
        gameEvent.Callbacks -= existingHandler;
      }
    }

    private TEvent GetOrCreateEvent<TEvent>(EventInfoAttribute eventInfo, string scriptName) where TEvent : IEvent<TEvent>, new()
    {
      if (!scriptToEventMap.TryGetValue(scriptName, out IEvent mappedEvent))
      {
        TEvent retVal = CreateEvent<TEvent>(eventInfo, scriptName);;
        scriptToEventMap[scriptName] = retVal;
        return retVal;
      }
      else if(mappedEvent is TEvent retVal)
      {
        return retVal;
      }

      throw new InvalidOperationException($"Script {scriptName} is already bound to {mappedEvent.GetType().FullName}! Single to Many event mappings are not supported.");
    }

    private TEvent GetOrCreateEvent<TEvent>(EventInfoAttribute eventInfo) where TEvent : IEvent<TEvent>, new()
    {
      TEvent retVal = scriptToEventMap.Values.OfType<TEvent>().FirstOrDefault();
      if (retVal == null)
      {
        string scriptName = Guid.NewGuid().ToString("N");
        retVal = CreateEvent<TEvent>(eventInfo, scriptName);
        scriptToEventMap[scriptName] = retVal;
      }

      return retVal;
    }

    private TEvent CreateEvent<TEvent>(EventInfoAttribute eventInfo, string scriptName) where  TEvent : IEvent<TEvent>, new()
    {
      TEvent retVal = new TEvent();

      switch (eventInfo.EventType)
      {
        case EventType.NWNX:
          EventsPlugin.SubscribeEvent(eventInfo.EventName, scriptName);
          break;
      }

      return retVal;
    }

    private EventInfoAttribute GetEventInfo(Type type)
    {
      if(cachedEventInfo.TryGetValue(type, out EventInfoAttribute eventInfo))
      {
        return eventInfo;
      }

      eventInfo = type.GetCustomAttribute<EventInfoAttribute>();
      if (eventInfo == null)
      {
        throw new InvalidOperationException($"Event Type {type.GetFullName()} does not define an event info attribute!");
      }

      cachedEventInfo[type] = eventInfo;
      return eventInfo;
    }

    public int ExecuteScript(string scriptName, uint oidSelf)
    {
      if (scriptToEventMap.TryGetValue(scriptName, out IEvent gameEvent))
      {
        gameEvent.BroadcastEvent(oidSelf.ToNwObject());
        return ScriptDispatchConstants.SCRIPT_HANDLED;
      }

      return ScriptDispatchConstants.SCRIPT_NOT_HANDLED;
    }
  }
}