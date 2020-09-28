using System;
using System.Collections.Generic;
using NWN.API;
using NWN.API.Events;

namespace NWN.Services
{
  public class EventHandler
  {
    private readonly Dictionary<NwObject, Event> objectEvents = new Dictionary<NwObject, Event>();

    internal readonly string ScriptName = ScriptNameGenerator.Create();
    internal readonly Event GlobalEvent;

    internal static EventHandler Create<T>() where T : Event<T>, new()
    {
      return new EventHandler(new T());
    }

    private EventHandler(Event globalEvent)
    {
      this.GlobalEvent = globalEvent;
    }

    internal void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : Event<TEvent>, new()
    {
      ((TEvent) GlobalEvent).Callbacks += handler;
    }

    internal void Subscribe<TObject, TEvent>(TObject nwObject, Action<TEvent> handler) where TEvent : Event<TObject, TEvent>, new() where TObject : NwObject
    {
      if (objectEvents.TryGetValue(nwObject, out Event objectEvent))
      {
        ((TEvent) objectEvent).Callbacks += handler;
        return;
      }

      TEvent newHandler = new TEvent();
      newHandler.Callbacks += handler;
      objectEvents[nwObject] = newHandler;
    }

    internal void Unsubscribe<TEvent>(Action<TEvent> existingHandler) where TEvent : Event<TEvent>
    {
      ((TEvent) GlobalEvent).Callbacks -= existingHandler;

      foreach (Event subEvent in objectEvents.Values)
      {
        ((TEvent) subEvent).Callbacks -= existingHandler;
      }
    }

    internal void Unsubscribe<TObject, TEvent>(TObject nwObject, Action<TEvent> existingHandler) where TEvent : Event<TObject, TEvent>, new() where TObject : NwObject
    {
      if (objectEvents.TryGetValue(nwObject, out Event objectEvent))
      {
        ((TEvent) objectEvent).Callbacks -= existingHandler;
      }
    }

    internal void CallEvents(NwObject objSelf)
    {
      GlobalEvent.BroadcastEvent(objSelf);
      if (objectEvents.TryGetValue(objSelf, out Event objEvent))
      {
        objEvent.BroadcastEvent(objSelf);
      }
    }
  }
}
