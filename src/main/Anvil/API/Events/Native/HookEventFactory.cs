using System;
using Anvil.Services;

namespace Anvil.API.Events
{
  public abstract class HookEventFactory
  {
    [Inject]
    protected static Lazy<EventService> EventService { get; private set; }

    [Inject]
    protected static HookService HookService { get; private set; }

    [Inject]
    protected static VirtualMachine VirtualMachine { get; private set; }

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
  }
}
