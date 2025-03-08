using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Anvil.Services;

namespace Anvil.API.Events
{
  [ServiceBinding(typeof(IEventFactory))]
  public abstract class HookEventFactory : IEventFactory<NullRegistrationData>, IDisposable
  {
    [Inject]
    protected static Lazy<EventService> EventService { get; private set; } = null!;

    [Inject]
    protected static HookService HookService { get; private set; } = null!;

    [Inject]
    protected static VirtualMachine VirtualMachine { get; private set; } = null!;

    private readonly HashSet<Type> activeEvents = [];
    private IDisposable[]? hooks;

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

    [return: NotNullIfNotNull("eventData")]
    protected static TEvent? ProcessEvent<TEvent>(EventCallbackType eventType, TEvent? eventData, bool executeInScriptContext = true) where TEvent : class, IEvent
    {
      if (eventData == null)
      {
        return null;
      }

      if (executeInScriptContext)
      {
        VirtualMachine.ExecuteInScriptContext(() =>
        {
          eventData = EventService.Value.ProcessEvent(eventType, eventData);
        }, eventData.Context);
      }
      else
      {
        EventService.Value.ProcessEvent(eventType, eventData);
      }

      return eventData;
    }

    protected abstract IDisposable[] RequestHooks();
  }
}
