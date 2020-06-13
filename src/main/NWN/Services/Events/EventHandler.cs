using System;
using System.Collections.Generic;
using NWN.API;
using NWN.API.Events;

namespace NWN.Services
{
  public class EventHandler
  {
    private readonly Dictionary<NwObject, IEvent> objectEvents = new Dictionary<NwObject, IEvent>();

    public readonly string ScriptName = ScriptNameGenerator.Create();
    public readonly IEvent GlobalEvent;

    public static EventHandler Create<T>() where T : IEvent<T>, new()
    {
      return new EventHandler(new T());
    }

    private EventHandler(IEvent globalEvent)
    {
      this.GlobalEvent = globalEvent;
    }

    public void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : IEvent<TEvent>, new()
    {
      ((TEvent) GlobalEvent).Callbacks += handler;
    }

    public void Subscribe<TObject, TEvent>(TObject nwObject, Action<TEvent> handler) where TEvent : IEvent<TObject, TEvent>, new() where TObject : NwObject
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

    public void Unsubscribe<TEvent>(Action<TEvent> existingHandler) where TEvent : IEvent<TEvent>
    {
      ((TEvent) GlobalEvent).Callbacks -= existingHandler;

      foreach (IEvent subEvent in objectEvents.Values)
      {
        ((TEvent) subEvent).Callbacks -= existingHandler;
      }
    }

    public void Unsubscribe<TObject, TEvent>(TObject nwObject, Action<TEvent> existingHandler) where TEvent : IEvent<TObject, TEvent>, new() where TObject : NwObject
    {
      if (objectEvents.TryGetValue(nwObject, out IEvent objectEvent))
      {
        ((TEvent) objectEvent).Callbacks -= existingHandler;
      }
    }

    public void CallEvents(NwObject objSelf)
    {
      GlobalEvent.BroadcastEvent(objSelf);
      if (objectEvents.TryGetValue(objSelf, out IEvent objEvent))
      {
        objEvent.BroadcastEvent(objSelf);
      }
    }
  }
}