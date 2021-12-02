using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnAbilityScoreChange : IEvent
  {
    public NwCreature Creature { get; private init; }

    NwObject IEvent.Context
    {
      get => Creature;
    }

    internal sealed unsafe class Factory : MultiHookEventFactory
    {
      private static FunctionHook<SetSTRBaseHook> setSTRBaseHook;
      private static FunctionHook<SetDEXBaseHook> setDEXBaseHook;
      private static FunctionHook<SetCONBaseHook> setCONBaseHook;
      private static FunctionHook<SetINTBaseHook> setINTBaseHook;
      private static FunctionHook<SetWISBaseHook> setWISBaseHook;
      private static FunctionHook<SetCHABaseHook> setCHABaseHook;

      internal delegate void SetSTRBaseHook(void* pCreatureStats, byte value);

      internal delegate void SetDEXBaseHook(void* pCreatureStats, byte value);

      internal delegate void SetCONBaseHook(void* pCreatureStats, byte value);

      internal delegate void SetINTBaseHook(void* pCreatureStats, byte value);

      internal delegate void SetWISBaseHook(void* pCreatureStats, byte value);

      internal delegate void SetCHABaseHook(void* pCreatureStats, byte value);

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, byte> pHook = &OnSetSTRBaseHook;
        setSTRBaseHook = HookService.RequestHook<SetSTRBaseHook>(pHook, FunctionsLinux._ZN17CNWSCreatureStats10SetSTRBaseEh, HookOrder.Early);

        delegate* unmanaged<void*, byte> pDEXBaseHook = &OnSetDEXBaseHook;
        setDEXBaseHook = HookService.RequestHook<SetDEXBaseHook>(pHook, FunctionsLinux._ZN17CNWSCreatureStats10SetDEXBaseEh, HookOrder.Early);

        delegate* unmanaged<void*, byte> pCONBaseHook = &OnSetCONBaseHook;
        setCONBaseHook = HookService.RequestHook<SetCONBaseHook>(pHook, FunctionsLinux._ZN17CNWSCreatureStats10SetCONBaseEhi, HookOrder.Early);

        delegate* unmanaged<void*, byte> pSTRBaseHook = &OnSetINTBaseHook;
        setINTBaseHook = HookService.RequestHook<SetINTBaseHook>(pHook, FunctionsLinux._ZN17CNWSCreatureStats10SetINTBaseEh, HookOrder.Early);

        delegate* unmanaged<void*, byte> pWISBaseHook = &OnSetWISBaseHook;
        setWISBaseHook = HookService.RequestHook<SetWISBaseHook>(pHook, FunctionsLinux._ZN17CNWSCreatureStats10SetWISBaseEh, HookOrder.Early);

        delegate* unmanaged<void*, byte> pCHABaseHook = &OnSetCHABaseHook;
        setCHABaseHook = HookService.RequestHook<SetCHABaseHook>(pHook, FunctionsLinux._ZN17CNWSCreatureStats10SetCHABaseEh, HookOrder.Early);

        return new IDisposable[] { setSTRBaseHook, setDEXBaseHook, setCONBaseHook, setINTBaseHook, setWISBaseHook, setCHABaseHook };
      }

      [UnmanagedCallersOnly]
      private static byte OnSetSTRBaseHook(void* pCreatureStats) { if (pCreatureStats != null) { } }

      [UnmanagedCallersOnly]
      private static byte OnSetDEXBaseHook(void* pCreatureStats) { if (pCreatureStats != null) { } }

      [UnmanagedCallersOnly]
      private static byte OnSetCONBaseHook(void* pCreatureStats) { if (pCreatureStats != null) { } }

      [UnmanagedCallersOnly]
      private static byte OnSetINTBaseHook(void* pCreatureStats) { if (pCreatureStats != null) { } }

      [UnmanagedCallersOnly]
      private static byte OnSetWISBaseHook(void* pCreatureStats) { if (pCreatureStats != null) { } }

      [UnmanagedCallersOnly]
      private static byte OnSetCHABaseHook(void* pCreatureStats) { if (pCreatureStats != null) { } }
    }
  }
}


namespace Anvil.API
{
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="Events.OnAbilityScoreChange"/>
    public event Action<OnAbilityScoreChange> OnAbilityScoreChange
    {
      add => EventService.Subscribe<OnAbilityScoreChange, OnAbilityScoreChange.Factory>(this, value);
      remove => EventService.Unsubscribe<OnAbilityScoreChange, OnAbilityScoreChange.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnAbilityScoreChange"/>
    public event Action<OnAbilityScoreChange> OnAbilityScoreChange
    {
      add => EventService.SubscribeAll<OnAbilityScoreChange, OnAbilityScoreChange.Factory>(value);
      remove => EventService.UnsubscribeAll<OnAbilityScoreChange, OnAbilityScoreChange.Factory>(value);
    }
  }
}
