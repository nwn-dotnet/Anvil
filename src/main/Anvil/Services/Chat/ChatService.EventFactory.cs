using System;
using Anvil.API;
using Anvil.API.Events;

namespace Anvil.Services
{
  [ServiceBinding(typeof(IEventFactory))]
  public sealed partial class ChatService : IEventFactory<NullRegistrationData>
  {
    // Dependencies
    [Inject]
    private Lazy<EventService> EventService { get; init; }

    [Inject]
    private VirtualMachine VirtualMachine { get; init; }

    private bool isEventHooked;

    private bool ProcessEvent(ChatChannel chatChannel, string message, NwObject sender, NwPlayer target)
    {
      OnChatMessageSend eventData = null;

      VirtualMachine.ExecuteInScriptContext(() =>
      {
        eventData = EventService.Value.ProcessEvent(new OnChatMessageSend
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
