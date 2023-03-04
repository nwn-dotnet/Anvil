using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  /// <summary>
  /// Called when a player uses a quick chat command.
  /// </summary>
  public sealed class OnPlayerQuickChat : IEvent
  {
    /// <summary>
    /// Gets the player that is doing the quick chat.
    /// </summary>
    public NwPlayer Player { get; private init; } = null!;

    /// <summary>
    /// Gets the quick chat type that is being played.
    /// </summary>
    public VoiceChatType VoiceChat { get; private init; }

    /// <summary>
    /// Gets or sets a value indicating whether the character should be prevented from using the quick chat.
    /// </summary>
    public bool PreventQuickChat { get; set; }

    NwObject? IEvent.Context => Player.ControlledCreature;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<HandlePlayerToServerQuickChatMessageHook> Hook { get; set; } = null!;

      [NativeFunction("_ZN11CNWSMessage36HandlePlayerToServerQuickChatMessageEP10CNWSPlayerh", "")]
      private delegate int HandlePlayerToServerQuickChatMessageHook(void* pMessage, void* pPlayer, byte nMinor);

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void*, byte, int> pHook = &OnHandlePlayerToServerQuickChatMessage;
        Hook = HookService.RequestHook<HandlePlayerToServerQuickChatMessageHook>(pHook, HookOrder.Early);
        return new IDisposable[] { Hook };
      }

      [UnmanagedCallersOnly]
      private static int OnHandlePlayerToServerQuickChatMessage(void* pMessage, void* pPlayer, byte nMinor)
      {
        CNWSMessage message = CNWSMessage.FromPointer(pMessage);
        OnPlayerQuickChat eventData = ProcessEvent(EventCallbackType.Before, new OnPlayerQuickChat
        {
          Player = CNWSPlayer.FromPointer(pPlayer).ToNwPlayer()!,
          VoiceChat = (VoiceChatType)message.PeekMessage<short>(0),
        });

        int retVal = !eventData.PreventQuickChat ? Hook.CallOriginal(pMessage, pPlayer, nMinor) : 0;
        ProcessEvent(EventCallbackType.After, eventData);

        return retVal;
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="Events.OnPlayerQuickChat"/>
    public event Action<OnPlayerQuickChat> OnPlayerQuickChat
    {
      add => EventService.Subscribe<OnPlayerQuickChat, OnPlayerQuickChat.Factory>(ControlledCreature, value);
      remove => EventService.Unsubscribe<OnPlayerQuickChat, OnPlayerQuickChat.Factory>(ControlledCreature, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnPlayerQuickChat"/>
    public event Action<OnPlayerQuickChat> OnPlayerQuickChat
    {
      add => EventService.SubscribeAll<OnPlayerQuickChat, OnPlayerQuickChat.Factory>(value);
      remove => EventService.UnsubscribeAll<OnPlayerQuickChat, OnPlayerQuickChat.Factory>(value);
    }
  }
}
