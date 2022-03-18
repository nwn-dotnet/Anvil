using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnCombatStatusChange : IEvent
  {
    private static readonly CServerExoApp ServerExoApp = NWNXLib.AppManager().m_pServerExoApp;

    public CombatStatus CombatStatus { get; private init; }

    public NwPlayer Player { get; private init; }

    NwObject IEvent.Context => Player.ControlledCreature;

    internal sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<SendServerToPlayerAmbientBattleMusicPlayHook> Hook { get; set; }

      private delegate int SendServerToPlayerAmbientBattleMusicPlayHook(void* pMessage, uint nPlayer, int bPlay);

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, uint, int, int> pHook = &OnSendServerToPlayerAmbientBattleMusicPlay;
        Hook = HookService.RequestHook<SendServerToPlayerAmbientBattleMusicPlayHook>(pHook, FunctionsLinux._ZN11CNWSMessage40SendServerToPlayerAmbientBattleMusicPlayEji, HookOrder.Earliest);
        return new IDisposable[] { Hook };
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
            CombatStatus = bPlay.ToBool() ? CombatStatus.EnterCombat : CombatStatus.ExitCombat,
          });
        }

        return Hook.CallOriginal(pMessage, nPlayer, bPlay);
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="Events.OnCombatStatusChange"/>
    public event Action<OnCombatStatusChange> OnCombatStatusChange
    {
      add => EventService.Subscribe<OnCombatStatusChange, OnCombatStatusChange.Factory>(ControlledCreature, value);
      remove => EventService.Unsubscribe<OnCombatStatusChange, OnCombatStatusChange.Factory>(ControlledCreature, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnCombatStatusChange"/>
    public event Action<OnCombatStatusChange> OnCombatStatusChange
    {
      add => EventService.SubscribeAll<OnCombatStatusChange, OnCombatStatusChange.Factory>(value);
      remove => EventService.UnsubscribeAll<OnCombatStatusChange, OnCombatStatusChange.Factory>(value);
    }
  }
}
