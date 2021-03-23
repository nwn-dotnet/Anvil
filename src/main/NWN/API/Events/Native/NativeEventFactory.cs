using System;
using NWN.Services;

namespace NWN.API.Events
{
  [ServiceBinding(typeof(IEventFactory))]
  public abstract class NativeEventFactory<THook> : IEventFactory
    where THook : Delegate
  {
    private readonly HookService hookService;
    private EventService eventService;

    protected FunctionHook<THook> FunctionHook { get; private set; }

    protected virtual int FunctionHookOrder { get; } = HookOrder.Default;

    protected abstract THook Handler { get; }

    protected NativeEventFactory(HookService hookService)
    {
      this.hookService = hookService;
    }

    void IEventFactory.Init(EventService eventService)
    {
      this.eventService = eventService;
      FunctionHook ??= hookService.RequestHook(Handler, FunctionHookOrder);
    }

    void IEventFactory.Unregister<T>()
    {
      FunctionHook.Dispose();
      FunctionHook = null;
    }

    protected TEvent ProcessEvent<TEvent>(TEvent evt) where TEvent : IEvent
      => eventService.ProcessEvent(evt);
  }
}
