using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnCombatRoundStart : IEvent
  {
    public NwCreature Creature { get; private init; } = null!;

    public NwGameObject Target { get; private init; } = null!;

    NwObject IEvent.Context => Creature;

    internal sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<StartCombatRoundHook> Hook { get; set; } = null!;

      private delegate void StartCombatRoundHook(void* pCombatRound, uint oidTarget);

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, uint, void> pHook = &OnStartCombatRound;
        Hook = HookService.RequestHook<StartCombatRoundHook>(pHook, FunctionsLinux._ZN15CNWSCombatRound16StartCombatRoundEj, HookOrder.Earliest);
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
