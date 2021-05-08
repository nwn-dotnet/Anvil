using System;
using NWN.Services;

namespace NWN.API.Events
{
  public abstract class NativeEventFactory
  {
    protected static Lazy<EventService> EventService { get; private set; }

    protected static HookService HookService { get; private set; }

    [ServiceBinding(typeof(APIBindings))]
    [ServiceBindingOptions(BindingOrder.API)]
    internal sealed class APIBindings
    {
      public APIBindings(Lazy<EventService> eventService, HookService hookService)
      {
        EventService = eventService;
        HookService = hookService;
      }
    }
  }

  [ServiceBinding(typeof(IEventFactory))]
  public abstract class NativeEventFactory<THook> : NativeEventFactory, IEventFactory, IDisposable
    where THook : Delegate
  {
    protected static FunctionHook<THook> Hook { get; set; }

    protected abstract FunctionHook<THook> RequestHook();

    protected static TEvent ProcessEvent<TEvent>(TEvent eventData) where TEvent : IEvent
    {
      VirtualMachine.Instance.ExecuteInScriptContext(() =>
      {
        eventData = EventService.Value.ProcessEvent(eventData);
      }, eventData.Context);

      return eventData;
    }

    void IEventFactory.Init()
    {
      Hook ??= RequestHook();
    }

    void IEventFactory.Unregister<T>()
    {
      Hook.Dispose();
      Hook = null;
    }

    void IDisposable.Dispose()
    {
      Hook?.Dispose();
      Hook = null;
    }
  }
}
