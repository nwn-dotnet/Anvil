using System;
using System.Collections.Generic;
using NWN.API;
using NWN.API.Events;

namespace NWN.Services
{
  public class EventHandler
  {
    private readonly Dictionary<NwObject, IEvent> objectEvents = new Dictionary<NwObject, IEvent>();

    internal readonly string ScriptName = ScriptNameGenerator.Create();
    internal readonly IEvent GlobalEvent;

    internal static EventHandler Create<T>() where T : IEvent<T>, new()
    {
      return new EventHandler(new T());
    }

    private EventHandler(IEvent globalEvent)
    {
      this.GlobalEvent = globalEvent;
    }

    internal void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : IEvent<TEvent>, new()
    {
      ((TEvent) GlobalEvent).Callbacks += handler;
    }

    internal void Subscribe<TObject, TEvent>(TObject nwObject, Action<TEvent> handler) where TEvent : IEvent<TObject, TEvent>, new() where TObject : NwObject
    {
      if (objectEvents.TryGetValue(nwObject, out IEvent objectEvent))
      {
        ((TEvent) objectEvent).Callbacks += handler;
        return;
      }

      TEvent newHandler = new TEvent();
      newHandler.Callbacks += handler;
      objectEvents[nwObject] = newHandler;
    }

    internal void Unsubscribe<TEvent>(Action<TEvent> existingHandler) where TEvent : IEvent<TEvent>
    {
      ((TEvent) GlobalEvent).Callbacks -= existingHandler;

      foreach (IEvent subEvent in objectEvents.Values)
      {
        ((TEvent) subEvent).Callbacks -= existingHandler;
      }
    }

    internal void Unsubscribe<TObject, TEvent>(TObject nwObject, Action<TEvent> existingHandler) where TEvent : IEvent<TObject, TEvent>, new() where TObject : NwObject
    {
      if (objectEvents.TryGetValue(nwObject, out IEvent objectEvent))
      {
        ((TEvent) objectEvent).Callbacks -= existingHandler;
      }
    }

    internal void CallEvents(NwObject objSelf)
    {
      GlobalEvent.BroadcastEvent(objSelf);
      if (objectEvents.TryGetValue(objSelf, out IEvent objEvent))
      {
        objEvent.BroadcastEvent(objSelf);
      }
    }
  }
}