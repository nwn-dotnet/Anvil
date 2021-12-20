using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnCombatRoundStart : IEvent
  {
    public NwCreature Creature { get; private init; }

    public NwGameObject Target { get; private init; }

    NwObject IEvent.Context => Creature;

    internal sealed unsafe class Factory : SingleHookEventFactory<Factory.StartCombatRoundHook>
    {
      internal delegate void StartCombatRoundHook(void* pCombatRound, uint oidTarget);

      protected override FunctionHook<StartCombatRoundHook> RequestHook()
      {
        delegate* unmanaged<void*, uint, void> pHook = &OnStartCombatRound;
        return HookService.RequestHook<StartCombatRoundHook>(pHook, FunctionsLinux._ZN15CNWSCombatRound16StartCombatRoundEj, HookOrder.Earliest);
      }

      [UnmanagedCallersOnly]
      private static void OnStartCombatRound(void* pCombatRound, uint oidTarget)
      {
        CNWSCombatRound combatRound = CNWSCombatRound.FromPointer(pCombatRound);

        ProcessEvent(new OnCombatRoundStart
        {
          Creature = combatRound.m_pBaseCreature.ToNwObject<NwCreature>(),
          Target = oidTarget.ToNwObject<NwGameObject>(),
        });

        Hook.CallOriginal(pCombatRound, oidTarget);
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
