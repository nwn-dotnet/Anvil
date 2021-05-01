using System;
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

    NwObject IEvent.Context => Player;

    [NativeFunction(NWNXLib.Functions._ZN11CNWSMessage33SendServerToPlayerArea_ClientAreaEP10CNWSPlayerP8CNWSAreafffRK6Vectori)]
    internal delegate int SendServerToPlayerAreaClientAreaHook(IntPtr pMessage, IntPtr pPlayer, IntPtr pArea, float fX, float fY, float fZ, IntPtr vNewOrientation, int bPlayerIsNewToModule);

    internal class Factory : NativeEventFactory<SendServerToPlayerAreaClientAreaHook>
    {
      public Factory(Lazy<EventService> eventService, HookService hookService) : base(eventService, hookService) {}

      protected override FunctionHook<SendServerToPlayerAreaClientAreaHook> RequestHook(HookService hookService)
        => hookService.RequestHook<SendServerToPlayerAreaClientAreaHook>(OnSendServerToPlayerAreaClientArea, HookOrder.Earliest);

      private int OnSendServerToPlayerAreaClientArea(IntPtr pMessage, IntPtr pPlayer, IntPtr pArea, float fX, float fY, float fZ, IntPtr vNewOrientation, int bPlayerIsNewToModule)
      {
        ProcessEvent(new OnServerSendArea
        {
          Area = new NwArea(new CNWSArea(pArea, false)),
          Player = new NwPlayer(new CNWSPlayer(pPlayer, false)),
          IsPlayerNewToModule = bPlayerIsNewToModule.ToBool(),
        });

        return Hook.CallOriginal(pMessage, pPlayer, pArea, fX, fY, fZ, vNewOrientation, bPlayerIsNewToModule);
      }
    }
  }
}
