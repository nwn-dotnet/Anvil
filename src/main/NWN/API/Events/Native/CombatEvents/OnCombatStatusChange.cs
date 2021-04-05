using System;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public class OnCombatStatusChange : IEvent
  {
    private static readonly CServerExoApp ServerExoApp = NWNXLib.AppManager().m_pServerExoApp;

    public NwPlayer Player { get; private init; }

    public CombatStatus CombatStatus { get; private init; }

    NwObject IEvent.Context => Player;

    [NativeFunction(NWNXLib.Functions._ZN11CNWSMessage40SendServerToPlayerAmbientBattleMusicPlayEji)]
    internal delegate int SendServerToPlayerAmbientBattleMusicPlayHook(IntPtr pMessage, uint nPlayer, int bPlay);

    internal class Factory : NativeEventFactory<SendServerToPlayerAmbientBattleMusicPlayHook>
    {
      public Factory(Lazy<EventService> eventService, HookService hookService) : base(eventService, hookService) {}

      protected override FunctionHook<SendServerToPlayerAmbientBattleMusicPlayHook> RequestHook(HookService hookService)
        => hookService.RequestHook<SendServerToPlayerAmbientBattleMusicPlayHook>(OnSendServerToPlayerAmbientBattleMusicPlay, HookOrder.Earliest);

      private int OnSendServerToPlayerAmbientBattleMusicPlay(IntPtr pMessage, uint nPlayer, int bPlay)
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
