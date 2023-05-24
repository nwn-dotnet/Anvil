using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Native;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnCombatRoundStart : IEvent
  {
    public NwCreature Creature { get; private init; } = null!;

    public NwGameObject Target { get; private init; } = null!;

    NwObject IEvent.Context => Creature;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<Functions.CNWSCombatRound.StartCombatRound> Hook { get; set; } = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, uint, void> pHook = &OnStartCombatRound;
        Hook = HookService.RequestHook<Functions.CNWSCombatRound.StartCombatRound>(pHook, HookOrder.Earliest);
        return new IDisposable[] { Hook };
      }

      [UnmanagedCallersOnly]
      private static void OnStartCombatRound(void* pCombatRound, uint oidTarget)
      {
        CNWSCombatRound combatRound = CNWSCombatRound.FromPointer(pCombatRound);

        OnCombatRoundStart eventData = ProcessEvent(EventCallbackType.Before, new OnCombatRoundStart
        {
          Creature = combatRound.m_pBaseCreature.ToNwObject<NwCreature>()!,
          Target = oidTarget.ToNwObject<NwGameObject>()!,
        });

        Hook.CallOriginal(pCombatRound, oidTarget);
        ProcessEvent(EventCallbackType.After, eventData);
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="Events.OnCombatRoundStart"/>
    public event Action<OnCombatRoundStart> OnCombatRoundStart
    {
      add => EventService.Subscribe<OnCombatRoundStart, OnCombatRoundStart.Factory>(this, value);
      remove => EventService.Unsubscribe<OnCombatRoundStart, OnCombatRoundStart.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnCombatRoundStart"/>
    public event Action<OnCombatRoundStart> OnCombatRoundStart
    {
      add => EventService.SubscribeAll<OnCombatRoundStart, OnCombatRoundStart.Factory>(value);
      remove => EventService.UnsubscribeAll<OnCombatRoundStart, OnCombatRoundStart.Factory>(value);
    }
  }
}
