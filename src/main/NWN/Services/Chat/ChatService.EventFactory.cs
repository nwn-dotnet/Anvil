using System;
using NWN.API;
using NWN.API.Events;

namespace NWN.Services
{
  public sealed partial class ChatService : IEventFactory<NullRegistrationData>
  {
    private readonly EventService eventService;

    private bool isEventHooked;

    private bool ProcessEvent(ChatChannel chatChannel, string message, NwGameObject sender, NwPlayer target)
    {
      OnChatMessageSend eventData = eventService.ProcessEvent(new OnChatMessageSend
      {
        ChatChannel = chatChannel,
        Message = message,
        Sender = sender,
        Target = target,
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
