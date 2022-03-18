using System;
using System.Collections.Generic;
using Anvil.Services;

namespace Anvil.API.Events
{
  [ServiceBinding(typeof(IEventFactory))]
  public abstract class HookEventFactory : IEventFactory<NullRegistrationData>, IDisposable
  {
    [Inject]
    protected static Lazy<EventService> EventService { get; private set; }

    [Inject]
    protected static HookService HookService { get; private set; }

    [Inject]
    protected static VirtualMachine VirtualMachine { get; private set; }

    private readonly HashSet<Type> activeEvents = new HashSet<Type>();
    private IDisposable[] hooks;

    public void Dispose()
    {
      hooks.DisposeAll();
      hooks = null;
    }

    void IEventFactory<NullRegistrationData>.Register<TEvent>(NullRegistrationData data)
    {
      hooks ??= RequestHooks();
      activeEvents.Add(typeof(TEvent));
    }

    void IEventFactory.Unregister<TEvent>()
    {
      activeEvents.Remove(typeof(TEvent));
      if (activeEvents.Count == 0)
      {
        Dispose();
      }
    }

    protected static TEvent ProcessEvent<TEvent>(TEvent eventData, bool executeInScriptContext = true) where TEvent : IEvent
    {
      if (executeInScriptContext)
      {
        VirtualMachine.ExecuteInScriptContext(() => { eventData = EventService.Value.ProcessEvent(eventData); }, eventData.Context);
      }
      else
      {
        EventService.Value.ProcessEvent(eventData);
      }

      return eventData;
    }

    protected abstract IDisposable[] RequestHooks();
  }
}
