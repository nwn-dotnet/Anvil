using System;
using System.Collections.Generic;
using NWN.API;
using NWN.API.Events;

namespace NWN.Services
{
  internal class EventHandler {}

  internal class EventHandler<T> : EventHandler where T : IEvent
  {
    private Action<T> globalCallback;
    private readonly Dictionary<NwObject, Action<T>> filteredCallbacks = new Dictionary<NwObject, Action<T>>();

    public bool HasSubscribers
    {
      get => globalCallback != null && filteredCallbacks.Count > 0;
    }

    public void ProcessEvent(T evt)
    {
      globalCallback?.Invoke(evt);

      if (evt.HasContext && filteredCallbacks.TryGetValue(evt.Context, out Action<T> callback))
      {
        callback?.Invoke(evt);
      }
    }

    public void Subscribe(NwObject obj, Action<T> newHandler)
    {
      filteredCallbacks.TryGetValue(obj, out Action<T> handler);
      handler += newHandler;
      filteredCallbacks[obj] = handler;
    }

    public void SubscribeAll(Action<T> newHandler)
    {
      globalCallback += newHandler;
    }

    public void Unsubscribe(NwObject obj, Action<T> handlerToRemove)
    {
      if (!filteredCallbacks.TryGetValue(obj, out Action<T> handler))
      {
        return;
      }

      handler -= handlerToRemove;
      if (handler == null)
      {
        filteredCallbacks.Remove(obj);
      }
    }

    public void UnsubscribeAll(Action<T> handlerToRemove)
    {
      globalCallback -= handlerToRemove;
    }
  }
}
