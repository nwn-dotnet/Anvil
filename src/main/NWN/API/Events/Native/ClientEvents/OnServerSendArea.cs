using System.Runtime.InteropServices;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public sealed class OnServerSendArea : IEvent
  {
    public NwPlayer Player { get; private init; }

    public NwArea Area { get; private init; }

    /// <summary>
    /// Gets a value indicating whether this is the player's first time logging in to the server since a restart.
    /// </summary>
    public bool IsPlayerNewToModule { get; private init; }

    NwObject IEvent.Context
    {
      get => Player.ControlledCreature;
    }

    internal sealed unsafe class Factory : SingleHookEventFactory<Factory.SendServerToPlayerAreaClientAreaHook>
    {
      internal delegate int SendServerToPlayerAreaClientAreaHook(void* pMessage, void* pPlayer, void* pArea, float fX, float fY, float fZ, void* vNewOrientation, int bPlayerIsNewToModule);

      protected override FunctionHook<SendServerToPlayerAreaClientAreaHook> RequestHook()
      {
        delegate* unmanaged<void*, void*, void*, float, float, float, void*, int, int> pHook = &OnSendServerToPlayerAreaClientArea;
        return HookService.RequestHook<SendServerToPlayerAreaClientAreaHook>(pHook, FunctionsLinux._ZN11CNWSMessage33SendServerToPlayerArea_ClientAreaEP10CNWSPlayerP8CNWSAreafffRK6Vectori, HookOrder.Earliest);
      }

      [UnmanagedCallersOnly]
      private static int OnSendServerToPlayerAreaClientArea(void* pMessage, void* pPlayer, void* pArea, float fX, float fY, float fZ, void* vNewOrientation, int bPlayerIsNewToModule)
      {
        ProcessEvent(new OnServerSendArea
        {
          Area = CNWSArea.FromPointer(pArea).ToNwObject<NwArea>(),
          Player = CNWSPlayer.FromPointer(pPlayer).ToNwPlayer(),
          IsPlayerNewToModule = bPlayerIsNewToModule.ToBool(),
        });

        return Hook.CallOriginal(pMessage, pPlayer, pArea, fX, fY, fZ, vNewOrientation, bPlayerIsNewToModule);
      }
    }
  }
}
