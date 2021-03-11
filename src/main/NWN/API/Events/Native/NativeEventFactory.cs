using System;
using NWN.Services;

namespace NWN.API.Events
{
  public abstract class NativeEventFactory<TEvent, THook> : IEventFactory
    where TEvent : IEvent<TEvent>
    where THook : Delegate
  {
    private readonly HookService hookService;
    private EventService eventService;

    protected FunctionHook<THook> Hook { get; private set; }

    protected abstract uint Address { get; }

    protected abstract THook Handler { get; }

    protected virtual int HookOrder { get; } = Services.HookOrder.Default;

    public NativeEventFactory(HookService hookService)
    {
      this.hookService = hookService;
    }

    public void Init(EventService eventService)
    {
      this.eventService = eventService;
    }

    public void Register<T>(NwObject obj) where T : IEvent<T>, new()
    {
      Hook = hookService.RequestHook(Address, Handler, HookOrder);
    }

    public void Unregister<T>() where T : IEvent<T>
    {
      Hook.Dispose();
    }

    protected TEvent ProcessEvent(TEvent evt)
      => eventService.ProcessEvent(evt);
  }
}
