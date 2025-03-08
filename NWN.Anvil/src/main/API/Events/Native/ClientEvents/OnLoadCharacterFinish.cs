using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Native;
using NWN.Native.API;
using Anvil.Services;

namespace Anvil.API.Events
{
  /// <summary>
  /// Called immediately after a player character is loaded by the server.
  /// </summary>
  public sealed class OnLoadCharacterFinish : IEvent
  {
    public NwPlayer Player { get; private init; } = null!;

    NwObject? IEvent.Context => null;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<Functions.CServerExoAppInternal.LoadCharacterFinish> Hook { get; set; } = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void*, int, int, int> pHook = &OnLoadCharacterFinish;
        Hook = HookService.RequestHook<Functions.CServerExoAppInternal.LoadCharacterFinish>(pHook, HookOrder.Early);
        return [Hook];
      }

      [UnmanagedCallersOnly]
      private static int OnLoadCharacterFinish(void* pServerExoAppInternal, void* pPlayer, int bUseSaveGameCharacter, int bUseStateDataInSaveGame)
      {
        NwPlayer? player = CNWSPlayer.FromPointer(pPlayer).ToNwPlayer();
        if (player == null)
        {
          return Hook.CallOriginal(pServerExoAppInternal, pPlayer, bUseSaveGameCharacter, bUseStateDataInSaveGame);
        }

        OnLoadCharacterFinish eventData = ProcessEvent(EventCallbackType.Before, new OnLoadCharacterFinish
        {
          Player = player,
        });

        int retVal = Hook.CallOriginal(pServerExoAppInternal, pPlayer, bUseSaveGameCharacter, bUseStateDataInSaveGame);
        ProcessEvent(EventCallbackType.After, eventData);
        return retVal;
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnLoadCharacterFinish"/>
    public event Action<OnLoadCharacterFinish> OnLoadCharacterFinish
    {
      add => EventService.SubscribeAll<OnLoadCharacterFinish, OnLoadCharacterFinish.Factory>(value);
      remove => EventService.UnsubscribeAll<OnLoadCharacterFinish, OnLoadCharacterFinish.Factory>(value);
    }
  }
}
