using System;
using NWN.Services;

namespace NWN.API.Events
{
  public abstract class HookEventFactory
  {
    protected static Lazy<EventService> EventService { get; private set; }

    protected static HookService HookService { get; private set; }

    protected static VirtualMachine VirtualMachine { get; private set; }

    [ServiceBinding(typeof(APIBindings))]
    [ServiceBindingOptions(BindingOrder.API)]
    internal sealed class APIBindings
    {
      public APIBindings(Lazy<EventService> eventService, HookService hookService, VirtualMachine virtualMachine)
      {
        EventService = eventService;
        HookService = hookService;
        VirtualMachine = virtualMachine;
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
  }
}
