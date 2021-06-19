using System;
using NWN.API.Events;
using NWN.Services;

namespace NWN.API.Events
{
  /// <summary>
  /// Called when a chat message is about to be sent by an object.
  /// </summary>
  public sealed class OnChatMessageSend : IEvent
  {
    /// <summary>
    /// Gets the chat channel that the message was sent to.
    /// </summary>
    public ChatChannel ChatChannel { get; internal init; }

    /// <summary>
    /// Gets the message to be sent.
    /// </summary>
    public string Message { get; internal init; }

    /// <summary>
    /// Gets the sender of this message.
    /// </summary>
    public NwGameObject Sender { get; internal init; }

    /// <summary>
    /// Gets the target of this message.<br/>
    /// Returns null if there is no target.
    /// </summary>
    public NwPlayer Target { get; internal init; }

    /// <summary>
    /// Skips this chat message.
    /// </summary>
    public bool Skip { get; set; }

    NwObject IEvent.Context
    {
      get => Sender;
    }
  }
}

namespace NWN.API
{
  public abstract partial class NwGameObject
  {
    /// <inheritdoc cref="NWN.API.Events.OnChatMessageSend"/>
    public event Action<OnChatMessageSend> OnChatMessageSend
    {
      add => EventService.Subscribe<OnChatMessageSend, ChatService>(this, value);
      remove => EventService.Unsubscribe<OnChatMessageSend, ChatService>(this, value);
    }
  }
}
