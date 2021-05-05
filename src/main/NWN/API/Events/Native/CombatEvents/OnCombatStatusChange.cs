using System;
using System.Runtime.InteropServices;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public sealed class OnCombatStatusChange : IEvent
  {
    private static readonly CServerExoApp ServerExoApp = NWNXLib.AppManager().m_pServerExoApp;

    public NwPlayer Player { get; private init; }

    public CombatStatus CombatStatus { get; private init; }

    NwObject IEvent.Context => Player;

    internal sealed unsafe class Factory : NativeEventFactory<Factory.SendServerToPlayerAmbientBattleMusicPlayHook>
    {
      internal delegate int SendServerToPlayerAmbientBattleMusicPlayHook(void* pMessage, uint nPlayer, int bPlay);

      protected override FunctionHook<SendServerToPlayerAmbientBattleMusicPlayHook> RequestHook()
      {
        delegate* unmanaged<void*, uint, int, int> pHook = &OnSendServerToPlayerAmbientBattleMusicPlay;
        return HookService.RequestHook<SendServerToPlayerAmbientBattleMusicPlayHook>(NWNXLib.Functions._ZN11CNWSMessage40SendServerToPlayerAmbientBattleMusicPlayEji, pHook, HookOrder.Earliest);
      }

      [UnmanagedCallersOnly]
      private static int OnSendServerToPlayerAmbientBattleMusicPlay(void* pMessage, uint nPlayer, int bPlay)
      {
        CNWSPlayer player = ServerExoApp.GetClientObjectByPlayerId(nPlayer).AsNWSPlayer();
        if (player != null)
        {
          ProcessEvent(new OnCombatStatusChange
          {
            Player = new NwPlayer(player),
            CombatStatus = bPlay.ToBool() ? CombatStatus.EnterCombat : CombatStatus.ExitCombat
          });
        }

        return Hook.CallOriginal(pMessage, nPlayer, bPlay);
      }
    }
  }
}
