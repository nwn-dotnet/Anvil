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

    protected FunctionHook<THook> Hook { get; private set; }

    protected abstract HookInfo HookInfo { get; }

    protected abstract THook Handler { get; }

    protected NativeEventFactory(HookService hookService)
    {
      this.hookService = hookService;
    }

    void IEventFactory.Init(EventService eventService)
    {
      this.eventService = eventService;

      if (Hook != null)
      {
        Hook = hookService.RequestHook(HookInfo.Address, Handler, HookInfo.Order);
      }
    }

    void IEventFactory.Unregister<T>()
    {
      Hook.Dispose();
      Hook = null;
    }

    protected TEvent ProcessEvent<TEvent>(TEvent evt) where TEvent : IEvent
      => eventService.ProcessEvent(evt);
  }
}
