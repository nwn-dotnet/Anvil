using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Native;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnClientLevelUpBegin : IEvent
  {
    public NwPlayer Player { get; private init; } = null!;

    public bool PreventLevelUp { get; set; }

    public Lazy<bool> Result { get; private set; } = null!;

    NwObject? IEvent.Context => Player.ControlledCreature;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<Functions.CNWSMessage.HandlePlayerToServerLevelUpMessage> Hook { get; set; } = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void*, byte, int> pHook = &OnHandlePlayerToServerLevelUpMessage;
        Hook = HookService.RequestHook<Functions.CNWSMessage.HandlePlayerToServerLevelUpMessage>(pHook, HookOrder.Early);
        return [Hook];
      }

      [UnmanagedCallersOnly]
      private static int OnHandlePlayerToServerLevelUpMessage(void* pMessage, void* pPlayer, byte nMinor)
      {
        if (nMinor != (byte)MessageLevelUpMinor.Begin)
        {
          return Hook.CallOriginal(pMessage, pPlayer, nMinor);
        }

        OnClientLevelUpBegin eventData = new OnClientLevelUpBegin
        {
          Player = CNWSPlayer.FromPointer(pPlayer).ToNwPlayer()!,
        };

        eventData.Result = new Lazy<bool>(() => !eventData.PreventLevelUp && Hook.CallOriginal(pMessage, pPlayer, nMinor).ToBool());
        ProcessEvent(EventCallbackType.Before, eventData);

        if (eventData.PreventLevelUp)
        {
          CNWSMessage message = CNWSMessage.FromPointer(pMessage);
          message.ClearReadMessage();
        }

        int retVal = eventData.Result.Value.ToInt();
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
    /// <inheritdoc cref="Events.OnClientLevelUpBegin"/>
    public event Action<OnClientLevelUpBegin> OnClientLevelUpBegin
    {
      add => EventService.Subscribe<OnClientLevelUpBegin, OnClientLevelUpBegin.Factory>(ControlledCreature, value);
      remove => EventService.Unsubscribe<OnClientLevelUpBegin, OnClientLevelUpBegin.Factory>(ControlledCreature, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnClientLevelUpBegin"/>
    public event Action<OnClientLevelUpBegin> OnClientLevelUpBegin
    {
      add => EventService.SubscribeAll<OnClientLevelUpBegin, OnClientLevelUpBegin.Factory>(value);
      remove => EventService.UnsubscribeAll<OnClientLevelUpBegin, OnClientLevelUpBegin.Factory>(value);
    }
  }
}
