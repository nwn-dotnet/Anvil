using System;
using Anvil.API.Events;
using Anvil.Services;

namespace Anvil.API.Events
{
  /// <summary>
  /// Triggered when a chat message is about to be sent.
  /// </summary>
  public sealed class OnChatMessageSend : IEvent
  {
    /// <summary>
    /// Gets the chat channel the message is sent to.
    /// </summary>
    public ChatChannel ChatChannel { get; internal init; }

    /// <summary>
    /// Gets the message text to be sent.
    /// </summary>
    public string Message { get; internal init; } = null!;

    /// <summary>
    /// Gets the sender of this message.
    /// </summary>
    /// <remarks>
    /// May be null when the message is sent by the server, e.g. with the method <see cref="NwModule.SendMessageToAllDMs(string)"/>.
    /// </remarks>
    /// <seealso cref="NwModule.SendMessageToAllDMs(string)"/>
    public NwObject? Sender { get; internal init; }

    /// <summary>
    /// Gets or sets if the default behavior of sending this message should be skipped.
    /// </summary>
    public bool Skip { get; set; }

    /// <summary>
    /// Gets the target player for this message (e.g. Tells). Returns null if the message has no specific target.
    /// </summary>
    public NwPlayer? Target { get; internal init; }

    NwObject? IEvent.Context => Sender;
  }
}

namespace Anvil.API
{
  public abstract partial class NwGameObject
  {
    /// <inheritdoc cref="Events.OnChatMessageSend"/>
    public event Action<OnChatMessageSend> OnChatMessageSend
    {
      add => EventService.Subscribe<OnChatMessageSend, ChatService>(this, value);
      remove => EventService.Unsubscribe<OnChatMessageSend, ChatService>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnChatMessageSend"/>
    public event Action<OnChatMessageSend> OnChatMessageSend
    {
      add => EventService.SubscribeAll<OnChatMessageSend, ChatService>(value);
      remove => EventService.UnsubscribeAll<OnChatMessageSend, ChatService>(value);
    }
  }
}
