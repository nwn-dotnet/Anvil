using System;
using System.Runtime.InteropServices;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public sealed class OnClientLevelUpBegin : IEvent
  {
    public NwPlayer Player { get; private init; }

    public bool PreventLevelUp { get; set; }

    public Lazy<bool> Result { get; private set; }

    NwObject IEvent.Context => Player.ControlledCreature;

    internal sealed unsafe class Factory : SingleHookEventFactory<Factory.HandlePlayerToServerLevelUpMessageHook>
    {
      internal delegate int HandlePlayerToServerLevelUpMessageHook(void* pMessage, void* pPlayer, byte nMinor);

      protected override FunctionHook<HandlePlayerToServerLevelUpMessageHook> RequestHook()
      {
        delegate* unmanaged<void*, void*, byte, int> pHook = &OnHandlePlayerToServerLevelUpMessage;
        return HookService.RequestHook<HandlePlayerToServerLevelUpMessageHook>(pHook, FunctionsLinux._ZN11CNWSMessage34HandlePlayerToServerLevelUpMessageEP10CNWSPlayerh, HookOrder.Early);
      }

      [UnmanagedCallersOnly]
      private static int OnHandlePlayerToServerLevelUpMessage(void* pMessage, void* pPlayer, byte nMinor)
      {
        if (nMinor != (byte)MessageLevelUpMinor.Begin)
        {
          return Hook.CallOriginal(pMessage, pPlayer, nMinor);
        }

        CNWSPlayer player = new CNWSPlayer(pPlayer, false);

        OnClientLevelUpBegin eventData = new OnClientLevelUpBegin
        {
          Player = new NwPlayer(player),
        };

        eventData.Result = new Lazy<bool>(() => !eventData.PreventLevelUp && Hook.CallOriginal(pMessage, pPlayer, nMinor).ToBool());
        ProcessEvent(eventData);

        return eventData.Result.Value.ToInt();
      }
    }
  }
}
