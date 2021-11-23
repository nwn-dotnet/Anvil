using System;
using System.Collections.Generic;
using Anvil.API;
using Anvil.API.Events;

namespace Anvil.Services
{
  public sealed partial class EventService
  {
    private abstract class EventHandler
    {
      public abstract void ProcessEvent(IEvent eventData);

      public abstract void ClearObjectSubscriptions(NwObject gameObject);
    }

    private sealed class EventHandler<T> : EventHandler where T : IEvent
    {
      private readonly Dictionary<NwObject, Action<T>> filteredCallbacks = new Dictionary<NwObject, Action<T>>();

      private Action<T> globalCallback;

      public bool HasSubscribers
      {
        get => globalCallback != null || filteredCallbacks.Count > 0;
      }

      public override void ProcessEvent(IEvent eventData)
      {
        ProcessEvent((T)eventData);
      }

      public override void ClearObjectSubscriptions(NwObject gameObject)
      {
        filteredCallbacks.Remove(gameObject);
      }

      private void ProcessEvent(T eventData)
      {
        TryInvoke(eventData, globalCallback);
        if (eventData.Context != null && filteredCallbacks.TryGetValue(eventData.Context, out Action<T> callback))
        {
          TryInvoke(eventData, callback);
        }
      }

      private static void TryInvoke(T eventData, Action<T> callback)
      {
        try
        {
          callback?.Invoke(eventData);
        }
        catch (Exception e)
        {
          if (eventData?.Context != null && eventData.Context.IsValid)
          {
            Log.Error(e, "An exception was thrown while trying to invoke event {Event}. Context Object {ObjectId} - {ObjectName}",
              typeof(T).Name,
              eventData.Context.ToString(),
              eventData.Context.Name);
          }
          else
          {
            Log.Error(e, "An exception was thrown while trying to invoke event {Event}", typeof(T).Name);
          }

          Log.Error(e);
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
        else
        {
          filteredCallbacks[obj] = handler;
        }
      }

      public void UnsubscribeAll(Action<T> handlerToRemove)
      {
        globalCallback -= handlerToRemove;
      }
    }
  }
}
