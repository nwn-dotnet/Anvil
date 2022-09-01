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
    private Lazy<EventService> EventService { get; init; } = null!;

    [Inject]
    private VirtualMachine VirtualMachine { get; init; } = null!;

    private bool isEventHooked;

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

    private OnChatMessageSend ProcessEvent(ChatChannel chatChannel, string message, NwObject sender, NwPlayer? target)
    {
      OnChatMessageSend eventData = ProcessEvent(EventCallbackType.Before, new OnChatMessageSend
      {
        ChatChannel = chatChannel,
        Message = message,
        Sender = sender,
        Target = target,
      });

      return eventData;
    }

    private OnChatMessageSend ProcessEvent(EventCallbackType callbackType, OnChatMessageSend eventData)
    {
      return VirtualMachine.ExecuteInScriptContext(() => EventService.Value.ProcessEvent(callbackType, eventData));
    }
  }
}
