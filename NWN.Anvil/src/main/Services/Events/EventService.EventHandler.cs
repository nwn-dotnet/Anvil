using System;
using System.Collections.Generic;
using System.Linq;
using Anvil.API;
using Anvil.API.Events;
using Anvil.Plugins;

namespace Anvil.Services
{
  public sealed partial class EventService
  {
    private abstract class EventHandler
    {
      public abstract bool HasSubscribers { get; }
      public abstract void ClearObjectSubscriptions(NwObject gameObject);
      public abstract int ClearPluginSubscriptions(Plugin plugin);
      public abstract void ProcessEvent(IEvent eventData, EventCallbackType eventCallbackType);
      public abstract void UnregisterFactories();
    }

    private sealed class EventHandler<T> : EventHandler where T : IEvent, new()
    {
      private readonly CallbackList beforeCallbackList = new CallbackList();
      private readonly CallbackList afterCallbackList = new CallbackList();
      private readonly HashSet<IEventFactory> factories = new HashSet<IEventFactory>();

      private sealed class CallbackList
      {
        public readonly Dictionary<NwObject, Action<T>> FilteredCallbacks = new Dictionary<NwObject, Action<T>>();
        public Action<T>? GlobalCallback;

        public int ClearPluginSubscriptions(Plugin plugin)
        {
          int removed = RemovePluginCallbacks(ref GlobalCallback, plugin);
          foreach (NwObject gameObject in FilteredCallbacks.Keys.ToList())
          {
            Action<T>? callback = FilteredCallbacks[gameObject];
            removed += RemovePluginCallbacks(ref callback, plugin);

            if (callback == null)
            {
              FilteredCallbacks.Remove(gameObject);
            }
            else
            {
              FilteredCallbacks[gameObject] = callback;
            }
          }

          return removed;
        }

        private static int RemovePluginCallbacks(ref Action<T>? callback, Plugin plugin)
        {
          Delegate[] callbacks = callback?.GetInvocationList() ?? [];
          Delegate[] remaining = callbacks.Where(entry => !plugin.IsOwnerOf(entry)).ToArray();
          callback = (Action<T>?)Delegate.Combine(remaining);

          return callbacks.Length - remaining.Length;
        }
      }

      public override bool HasSubscribers => beforeCallbackList.GlobalCallback != null ||
        beforeCallbackList.FilteredCallbacks.Count > 0 ||
        afterCallbackList.GlobalCallback != null ||
        afterCallbackList.FilteredCallbacks.Count > 0;

      public override void ClearObjectSubscriptions(NwObject gameObject)
      {
        beforeCallbackList.FilteredCallbacks.Remove(gameObject);
        afterCallbackList.FilteredCallbacks.Remove(gameObject);
      }

      public override int ClearPluginSubscriptions(Plugin plugin)
      {
        return beforeCallbackList.ClearPluginSubscriptions(plugin) + afterCallbackList.ClearPluginSubscriptions(plugin);
      }

      public override void ProcessEvent(IEvent eventData, EventCallbackType eventCallbackType)
      {
        ProcessEvent((T)eventData, eventCallbackType);
      }

      public override void UnregisterFactories()
      {
        foreach (IEventFactory factory in factories)
        {
          factory.Unregister<T>();
        }

        factories.Clear();
      }

      public void Subscribe(NwObject obj, Action<T> newHandler, EventCallbackType eventCallbackType, IEventFactory? factory)
      {
        CallbackList callbackList = GetCallbackList(eventCallbackType);

        callbackList.FilteredCallbacks.TryGetValue(obj, out Action<T>? handler);
        handler += newHandler;
        callbackList.FilteredCallbacks[obj] = handler;
        TrackFactory(factory);
      }

      public void SubscribeAll(Action<T> newHandler, EventCallbackType eventCallbackType, IEventFactory? factory)
      {
        GetCallbackList(eventCallbackType).GlobalCallback += newHandler;
        TrackFactory(factory);
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

      private void TrackFactory(IEventFactory? factory)
      {
        if (factory != null)
        {
          factories.Add(factory);
        }
      }
    }
  }
}
