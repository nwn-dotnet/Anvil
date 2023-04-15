using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Native;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnServerSendArea : IEvent
  {
    public NwArea Area { get; private init; } = null!;

    /// <summary>
    /// Gets a value indicating whether this is the player's first time logging in to the server since a restart.
    /// </summary>
    public bool IsPlayerNewToModule { get; private init; }

    public NwPlayer Player { get; private init; } = null!;

    NwObject? IEvent.Context => Player.ControlledCreature;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<Functions.CNWSMessage.SendServerToPlayerArea_ClientArea> Hook { get; set; } = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void*, void*, float, float, float, void*, int, int> pHook = &OnSendServerToPlayerAreaClientArea;
        Hook = HookService.RequestHook<Functions.CNWSMessage.SendServerToPlayerArea_ClientArea>(pHook, HookOrder.Earliest);
        return new IDisposable[] { Hook };
      }

      [UnmanagedCallersOnly]
      private static int OnSendServerToPlayerAreaClientArea(void* pMessage, void* pPlayer, void* pArea, float fX, float fY, float fZ, void* vNewOrientation, int bPlayerIsNewToModule)
      {
        OnServerSendArea eventData = ProcessEvent(EventCallbackType.Before, new OnServerSendArea
        {
          Area = CNWSArea.FromPointer(pArea).ToNwObject<NwArea>()!,
          Player = CNWSPlayer.FromPointer(pPlayer).ToNwPlayer()!,
          IsPlayerNewToModule = bPlayerIsNewToModule.ToBool(),
        });

        int retVal = Hook.CallOriginal(pMessage, pPlayer, pArea, fX, fY, fZ, vNewOrientation, bPlayerIsNewToModule);
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
    /// <inheritdoc cref="Events.OnServerSendArea"/>
    public event Action<OnServerSendArea> OnServerSendArea
    {
      add => EventService.Subscribe<OnServerSendArea, OnServerSendArea.Factory>(ControlledCreature, value);
      remove => EventService.Unsubscribe<OnServerSendArea, OnServerSendArea.Factory>(ControlledCreature, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnServerSendArea"/>
    public event Action<OnServerSendArea> OnServerSendArea
    {
      add => EventService.SubscribeAll<OnServerSendArea, OnServerSendArea.Factory>(value);
      remove => EventService.UnsubscribeAll<OnServerSendArea, OnServerSendArea.Factory>(value);
    }
  }
}
