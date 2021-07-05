using System;
using NWN.API;
using NWN.API.Events;

namespace NWN.Services
{
  [ServiceBinding(typeof(IEventFactory))]
  public sealed partial class ChatService : IEventFactory<NullRegistrationData>
  {
    private readonly VirtualMachine virtualMachine;
    private readonly Lazy<EventService> eventService;

    private bool isEventHooked;

    private bool ProcessEvent(ChatChannel chatChannel, string message, NwObject sender, NwPlayer target)
    {
      OnChatMessageSend eventData = null;

      virtualMachine.ExecuteInScriptContext(() =>
      {
        eventData = eventService.Value.ProcessEvent(new OnChatMessageSend
        {
          ChatChannel = chatChannel,
          Message = message,
          Sender = sender,
          Target = target,
        });
      });

      return eventData.Skip;
    }

    void IEventFactory<NullRegistrationData>.Register<TEvent>(NullRegistrationData data)
    {
      if (typeof(TEvent) != typeof(OnChatMessageSend))
      {
        throw new NotSupportedException("TEvent must be OnChatMessageSend.");
      }

      isEventHooked = true;
    }

    void IEventFactory.Unregister<TEvent>()
    {
      if (typeof(TEvent) != typeof(OnChatMessageSend))
      {
        throw new NotSupportedException("TEvent must be OnChatMessageSend.");
      }

      isEventHooked = false;
    }
  }
}
