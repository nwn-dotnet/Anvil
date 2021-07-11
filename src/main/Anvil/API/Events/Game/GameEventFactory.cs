using System;
using System.Collections.Generic;
using System.Reflection;
using Anvil.API;
using Anvil.Services;
using NLog;
using NWN.API.Constants;
using NWN.Core;

namespace NWN.API.Events
{
  [ServiceBinding(typeof(IEventFactory))]
  [ServiceBinding(typeof(IScriptDispatcher))]
  public sealed partial class GameEventFactory : IEventFactory<GameEventFactory.RegistrationData>, IScriptDispatcher
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    // Dependencies
    [Inject]
    private Lazy<EventService> EventService { get; init; }

    // Caches
    private readonly Dictionary<Type, GameEventAttribute> eventInfoCache = new Dictionary<Type, GameEventAttribute>();
    private readonly Dictionary<EventScriptType, Func<IEvent>> eventConstructorCache = new Dictionary<EventScriptType, Func<IEvent>>();
    private readonly Dictionary<EventKey, string> originalCallLookup = new Dictionary<EventKey, string>();

    public void Register<TEvent>(RegistrationData data) where TEvent : IEvent, new()
    {
      EventScriptType eventScriptType = GetEventInfo(typeof(TEvent)).EventScriptType;

      CheckConstructorRegistered<TEvent>(eventScriptType);
      UpdateEventScript(data.NwObject, eventScriptType, data.CallOriginal);
    }

    public void Unregister<TEvent>() where TEvent : IEvent, new() {}

    ScriptHandleResult IScriptDispatcher.ExecuteScript(string scriptName, uint oidSelf)
    {
      if (EventService == null || scriptName != ScriptConstants.GameEventScriptName)
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
        if (originalCallLookup.TryGetValue(new EventKey(eventScriptType, oidSelf), out scriptName))
        {
          NWScript.ExecuteScript(scriptName, oidSelf);
        }

        EventService.Value.ProcessEvent(value.Invoke());
        return ScriptHandleResult.Handled;
      }

      return ScriptHandleResult.NotHandled;
    }

    private void UpdateEventScript(NwObject nwObject, EventScriptType eventType, bool callOriginal)
    {
      string existingScript = NWScript.GetEventScript(nwObject, (int)eventType);
      if (existingScript == ScriptConstants.GameEventScriptName)
      {
        return;
      }

      Log.Debug($"Hooking native script event \"{eventType}\" on object \"{nwObject.Name}\". Previous script: \"{existingScript}\"");
      NWScript.SetEventScript(nwObject, (int)eventType, ScriptConstants.GameEventScriptName);

      if (callOriginal && !string.IsNullOrWhiteSpace(existingScript))
      {
        originalCallLookup[new EventKey(eventType, nwObject)] = existingScript;
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

      static IEvent Constructor()
      {
        return new TEvent();
      }

      eventConstructorCache[eventScriptType] = Constructor;
    }
  }
}
