using System;
using System.Collections.Generic;
using NWN.API;
using NWN.API.Events;

namespace NWN.Services
{
  internal sealed class EventHandler
  {
    private readonly Dictionary<NwObject, NativeEvent> objectEvents = new Dictionary<NwObject, NativeEvent>();

    public readonly string ScriptName = ScriptNameGenerator.Create();

    internal void Subscribe<TObject, TEvent>(TObject nwObject, Action<TEvent> handler) where TEvent : NativeEvent<TObject, TEvent>, new() where TObject : NwObject
    {
      if (objectEvents.TryGetValue(nwObject, out NativeEvent objectEvent))
      {
        ((TEvent) objectEvent).Callbacks += handler;
        return;
      }

      TEvent newHandler = new TEvent();
      newHandler.Callbacks += handler;
      objectEvents[nwObject] = newHandler;
    }

    internal bool Unsubscribe<TObject, TEvent>(TObject nwObject, Action<TEvent> existingHandler) where TEvent : NativeEvent<TObject, TEvent>, new() where TObject : NwObject
    {
      if (objectEvents.TryGetValue(nwObject, out NativeEvent objectEvent))
      {
        TEvent tEvent = (TEvent)objectEvent;
        tEvent.Callbacks -= existingHandler;

        return !tEvent.HasSubscribers;
      }

      return false;
    }

    internal void CallEvents(NwObject objSelf)
    {
      if (objectEvents.TryGetValue(objSelf, out NativeEvent objEvent))
      {
        objEvent.ProcessEvent(objSelf);
      }
    }
  }
}
