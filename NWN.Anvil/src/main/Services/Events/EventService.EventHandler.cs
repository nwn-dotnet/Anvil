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
      public abstract void ClearObjectSubscriptions(NwObject gameObject);
      public abstract void ProcessEvent(IEvent eventData, EventCallbackType eventCallbackType);
    }

    private sealed class EventHandler<T> : EventHandler where T : IEvent
    {
      private readonly CallbackList beforeCallbackList = new CallbackList();
      private readonly CallbackList afterCallbackList = new CallbackList();

      private sealed class CallbackList
      {
        public readonly Dictionary<NwObject, Action<T>> FilteredCallbacks = new Dictionary<NwObject, Action<T>>();
        public Action<T>? GlobalCallback;
      }

      public bool HasSubscribers => beforeCallbackList.GlobalCallback != null ||
        beforeCallbackList.FilteredCallbacks.Count > 0 ||
        afterCallbackList.GlobalCallback != null ||
        afterCallbackList.FilteredCallbacks.Count > 0;

      public override void ClearObjectSubscriptions(NwObject gameObject)
      {
        beforeCallbackList.FilteredCallbacks.Remove(gameObject);
        afterCallbackList.FilteredCallbacks.Remove(gameObject);
      }

      public override void ProcessEvent(IEvent eventData, EventCallbackType eventCallbackType)
      {
        ProcessEvent((T)eventData, eventCallbackType);
      }

      public void Subscribe(NwObject obj, Action<T> newHandler, EventCallbackType eventCallbackType)
      {
        CallbackList callbackList = GetCallbackList(eventCallbackType);

        callbackList.FilteredCallbacks.TryGetValue(obj, out Action<T>? handler);
        handler += newHandler;
        callbackList.FilteredCallbacks[obj] = handler;
      }

      public void SubscribeAll(Action<T> newHandler, EventCallbackType eventCallbackType)
      {
        GetCallbackList(eventCallbackType).GlobalCallback += newHandler;
      }

      public void Unsubscribe(NwObject obj, Action<T> handlerToRemove, EventCallbackType eventCallbackType)
      {
        CallbackList callbackList = GetCallbackList(eventCallbackType);
        if (!callbackList.FilteredCallbacks.TryGetValue(obj, out Action<T>? handler))
        {
          return;
        }

        handler -= handlerToRemove;
        if (handler == null)
        {
          callbackList.FilteredCallbacks.Remove(obj);
        }
        else
        {
          callbackList.FilteredCallbacks[obj] = handler;
        }
      }

      public void UnsubscribeAll(Action<T> handlerToRemove, EventCallbackType eventCallbackType)
      {
        GetCallbackList(eventCallbackType).GlobalCallback -= handlerToRemove;
      }

      private static void TryInvoke(T eventData, Action<T>? callback)
      {
        try
        {
          callback?.Invoke(eventData);
        }
        catch (Exception e)
        {
          if (eventData.Context != null && eventData.Context.IsValid)
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

      private void ProcessEvent(T eventData, EventCallbackType eventCallbackType)
      {
        CallbackList callbackList = GetCallbackList(eventCallbackType);

        TryInvoke(eventData, callbackList.GlobalCallback);
        if (eventData.Context != null && callbackList.FilteredCallbacks.TryGetValue(eventData.Context, out Action<T>? callback))
        {
          TryInvoke(eventData, callback);
        }
      }

      private CallbackList GetCallbackList(EventCallbackType eventCallbackType)
      {
        return eventCallbackType switch
        {
          EventCallbackType.Before => beforeCallbackList,
          EventCallbackType.After => afterCallbackList,
          _ => throw new ArgumentOutOfRangeException(nameof(eventCallbackType), eventCallbackType, null),
        };
      }
    }
  }
}
