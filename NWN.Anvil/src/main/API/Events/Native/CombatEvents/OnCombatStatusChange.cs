using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Native;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnCombatStatusChange : IEvent
  {
    private static readonly CServerExoApp ServerExoApp = NWNXLib.AppManager().m_pServerExoApp;

    public CombatStatus CombatStatus { get; private init; }

    public NwPlayer Player { get; private init; } = null!;

    NwObject? IEvent.Context => Player.ControlledCreature;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<Functions.CNWSMessage.SendServerToPlayerAmbientBattleMusicPlay> Hook { get; set; } = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, uint, int, int> pHook = &OnSendServerToPlayerAmbientBattleMusicPlay;
        Hook = HookService.RequestHook<Functions.CNWSMessage.SendServerToPlayerAmbientBattleMusicPlay>(pHook, HookOrder.Earliest);
        return new IDisposable[] { Hook };
      }

      [UnmanagedCallersOnly]
      private static int OnSendServerToPlayerAmbientBattleMusicPlay(void* pMessage, uint nPlayer, int bPlay)
      {
        NwPlayer? player = ServerExoApp.GetClientObjectByPlayerId(nPlayer).AsNWSPlayer().ToNwPlayer();
        OnCombatStatusChange? eventData = null;

        if (player != null)
        {
          eventData = ProcessEvent(EventCallbackType.Before, new OnCombatStatusChange
          {
            Player = player,
            CombatStatus = bPlay.ToBool() ? CombatStatus.EnterCombat : CombatStatus.ExitCombat,
          });
        }

        int retVal = Hook.CallOriginal(pMessage, nPlayer, bPlay);
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
