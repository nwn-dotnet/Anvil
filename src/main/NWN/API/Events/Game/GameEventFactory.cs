using System;
using System.Collections.Generic;
using System.Reflection;
using NWN.API.Constants;
using NWN.Core;
using NWN.Services;

namespace NWN.API.Events
{
  [ServiceBinding(typeof(GameEventFactory))]
  public sealed class GameEventFactory : IEventFactory, IScriptDispatcher
  {
    private const string InternalScriptName = "_____nman_event";

    // Type info cache
    private readonly Dictionary<Type, NativeEventAttribute> cachedEventInfo = new Dictionary<Type, NativeEventAttribute>();

    private EventService eventService;
    private Dictionary<EventScriptType, Func<IEvent>> eventConstructors = new Dictionary<EventScriptType, Func<IEvent>>();

    public void Init(EventService eventService)
    {
      this.eventService = eventService;
    }

    public void Register<TEvent>(NwObject obj) where TEvent : IEvent, new()
    {
      EventScriptType scriptType = GetEventInfo(typeof(TEvent)).EventScriptType;
      CheckConstructorRegistered<TEvent>(scriptType);
      NWScript.SetEventScript(obj, (int)scriptType, InternalScriptName);
    }

    public void Unregister<TEvent>() where TEvent : IEvent {}

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

      if (eventConstructors.TryGetValue(eventScriptType, out Func<IEvent> value))
      {
        eventService.ProcessEvent(value.Invoke());
        return ScriptHandleResult.Handled;
      }

      return ScriptHandleResult.NotHandled;
    }

    private NativeEventAttribute GetEventInfo(Type type)
    {
      if (cachedEventInfo.TryGetValue(type, out NativeEventAttribute eventAttribute))
      {
        return eventAttribute;
      }

      eventAttribute = LoadEventInfo(type);
      cachedEventInfo[type] = eventAttribute;

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
      if (eventConstructors.ContainsKey(eventScriptType))
      {
        return;
      }

      static IEvent Constructor() => new TEvent();
      eventConstructors[eventScriptType] = Constructor;
    }
  }
}
