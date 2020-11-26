using System;
using System.Collections.Generic;
using NWN.API;
using NWN.API.Events;

namespace NWN.Services
{
  internal sealed class EventHandler
  {
    private readonly Dictionary<NwObject, IEvent> objectEvents = new Dictionary<NwObject, IEvent>();

    public readonly string ScriptName = ScriptNameGenerator.Create();

    internal void Subscribe<TObject, TEvent>(TObject nwObject, Action<TEvent> handler) where TEvent : NativeEvent<TObject, TEvent>, new() where TObject : NwObject
    {
      if (objectEvents.TryGetValue(nwObject, out IEvent objEvent))
      {
        ((IEvent<TEvent>)objEvent).Subscribe(handler);
        return;
      }

      TEvent newHandler = new TEvent();
      ((IEvent<TEvent>)newHandler).Subscribe(handler);
      objectEvents[nwObject] = newHandler;
    }

    internal bool Unsubscribe<TObject, TEvent>(TObject nwObject, Action<TEvent> existingHandler) where TEvent : NativeEvent<TObject, TEvent>, new() where TObject : NwObject
    {
      if (objectEvents.TryGetValue(nwObject, out IEvent objEvent))
      {
        ((IEvent<TEvent>)objEvent).Unsubscribe(existingHandler);
        return !objEvent.HasSubscribers;
      }

      return false;
    }

    internal ScriptHandleResult CallEvents(NwObject objSelf)
    {
      if (objectEvents.TryGetValue(objSelf, out IEvent objEvent))
      {
        return objEvent.Broadcast(objSelf);
      }

      return ScriptHandleResult.NotHandled;
    }
  }
}
