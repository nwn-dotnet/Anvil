using System;
using System.Runtime.InteropServices;
using Anvil.Services;
using NWN.API.Events;
using NWN.Native.API;

namespace NWN.API.Events
{
  public sealed class OnCombatStatusChange : IEvent
  {
    private static readonly CServerExoApp ServerExoApp = NWNXLib.AppManager().m_pServerExoApp;

    public NwPlayer Player { get; private init; }

    public CombatStatus CombatStatus { get; private init; }

    NwObject IEvent.Context
    {
      get => Player.ControlledCreature;
    }

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
            CombatStatus = bPlay.ToBool() ? CombatStatus.EnterCombat : CombatStatus.ExitCombat,
          });
        }

        return Hook.CallOriginal(pMessage, nPlayer, bPlay);
      }
    }
  }
}

namespace NWN.API
{
  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="NWN.API.Events.OnCombatStatusChange"/>
    public event Action<OnCombatStatusChange> OnCombatStatusChange
    {
      add => EventService.Subscribe<OnCombatStatusChange, OnCombatStatusChange.Factory>(ControlledCreature, value);
      remove => EventService.Unsubscribe<OnCombatStatusChange, OnCombatStatusChange.Factory>(ControlledCreature, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="NWN.API.Events.OnCombatStatusChange"/>
    public event Action<OnCombatStatusChange> OnCombatStatusChange
    {
      add => EventService.SubscribeAll<OnCombatStatusChange, OnCombatStatusChange.Factory>(value);
      remove => EventService.UnsubscribeAll<OnCombatStatusChange, OnCombatStatusChange.Factory>(value);
    }
  }
}
