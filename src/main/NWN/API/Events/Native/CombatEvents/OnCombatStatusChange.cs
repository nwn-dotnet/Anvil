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

    NwObject IEvent.Context => Player.ControlledCreature;

    internal sealed unsafe class Factory : SingleHookEventFactory<Factory.SendServerToPlayerAmbientBattleMusicPlayHook>
    {
      internal delegate int SendServerToPlayerAmbientBattleMusicPlayHook(void* pMessage, uint nPlayer, int bPlay);

      protected override FunctionHook<SendServerToPlayerAmbientBattleMusicPlayHook> RequestHook()
      {
        delegate* unmanaged<void*, uint, int, int> pHook = &OnSendServerToPlayerAmbientBattleMusicPlay;
        return HookService.RequestHook<SendServerToPlayerAmbientBattleMusicPlayHook>(pHook, FunctionsLinux._ZN11CNWSMessage40SendServerToPlayerAmbientBattleMusicPlayEji, HookOrder.Earliest);
      }

      [UnmanagedCallersOnly]
      private static int OnSendServerToPlayerAmbientBattleMusicPlay(void* pMessage, uint nPlayer, int bPlay)
      {
        NwPlayer player = ServerExoApp.GetClientObjectByPlayerId(nPlayer).AsNWSPlayer().ToNwPlayer();
        if (player != null)
        {
          ProcessEvent(new OnCombatStatusChange
          {
            Player = player,
            CombatStatus = bPlay.ToBool() ? CombatStatus.EnterCombat : CombatStatus.ExitCombat
          });
        }

        return Hook.CallOriginal(pMessage, nPlayer, bPlay);
      }
    }
  }
}
