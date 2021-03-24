using System;
using NWN.Services;

namespace NWN.API.Events
{
  [ServiceBinding(typeof(IEventFactory))]
  public abstract class NativeEventFactory<THook> : IEventFactory, IDisposable
    where THook : Delegate
  {
    private readonly Lazy<EventService> eventService;
    private readonly HookService hookService;

    protected NativeEventFactory(Lazy<EventService> eventService, HookService hookService)
    {
      this.eventService = eventService;
      this.hookService = hookService;
    }

    protected FunctionHook<THook> Hook { get; private set; }

    protected abstract FunctionHook<THook> RequestHook(HookService hookService);

    protected TEvent ProcessEvent<TEvent>(TEvent evt) where TEvent : IEvent
    {
      VirtualMachine.ExecuteInScriptContext(() =>
      {
        evt = eventService.Value.ProcessEvent(evt);
      });

      return evt;
    }

    void IEventFactory.Init()
    {
      Hook ??= RequestHook(hookService);
    }

    void IEventFactory.Unregister<T>()
    {
      Hook.Dispose();
      Hook = null;
    }

    void IDisposable.Dispose()
    {
      Hook?.Dispose();
    }
  }
}
