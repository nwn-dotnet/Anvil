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
    public Ability AbilityType { get; private init; }
    public bool BaseScore { get; private init; }
    public int Value { get; private init; }

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
        delegate* unmanaged<void*, byte, void> pSTRBaseHook = &OnSetSTRBaseHook;
        setSTRBaseHook = HookService.RequestHook<SetSTRBaseHook>(pSTRBaseHook, FunctionsLinux._ZN17CNWSCreatureStats10SetSTRBaseEh, HookOrder.Early);

        delegate* unmanaged<void*, byte, void> pDEXBaseHook = &OnSetDEXBaseHook;
        setDEXBaseHook = HookService.RequestHook<SetDEXBaseHook>(pDEXBaseHook, FunctionsLinux._ZN17CNWSCreatureStats10SetDEXBaseEh, HookOrder.Early);

        delegate* unmanaged<void*, byte, void> pCONBaseHook = &OnSetCONBaseHook;
        setCONBaseHook = HookService.RequestHook<SetCONBaseHook>(pCONBaseHook, FunctionsLinux._ZN17CNWSCreatureStats10SetCONBaseEhi, HookOrder.Early);

        delegate* unmanaged<void*, byte, void> pINTBaseHook = &OnSetINTBaseHook;
        setINTBaseHook = HookService.RequestHook<SetINTBaseHook>(pINTBaseHook, FunctionsLinux._ZN17CNWSCreatureStats10SetINTBaseEh, HookOrder.Early);

        delegate* unmanaged<void*, byte, void> pWISBaseHook = &OnSetWISBaseHook;
        setWISBaseHook = HookService.RequestHook<SetWISBaseHook>(pWISBaseHook, FunctionsLinux._ZN17CNWSCreatureStats10SetWISBaseEh, HookOrder.Early);

        delegate* unmanaged<void*, byte, void> pCHABaseHook = &OnSetCHABaseHook;
        setCHABaseHook = HookService.RequestHook<SetCHABaseHook>(pCHABaseHook, FunctionsLinux._ZN17CNWSCreatureStats10SetCHABaseEh, HookOrder.Early);

        return new IDisposable[] { setSTRBaseHook, setDEXBaseHook, setCONBaseHook, setINTBaseHook, setWISBaseHook, setCHABaseHook };
      }

      [UnmanagedCallersOnly]
      private static void OnSetSTRBaseHook(void* pCreatureStats, byte value) {
        ProcessEvent(new OnAbilityScoreChange
        {
          Creature = CNWSCreatureStats.FromPointer(pCreatureStats).m_pBaseCreature.ToNwObject<NwCreature>(),
          AbilityType = Ability.Strength,
          Value = value,
        });

        setSTRBaseHook.CallOriginal(pCreatureStats, value);
       }

      [UnmanagedCallersOnly]
      private static void OnSetDEXBaseHook(void* pCreatureStats, byte value) {
        ProcessEvent(new OnAbilityScoreChange
        {
          Creature = CNWSCreatureStats.FromPointer(pCreatureStats).m_pBaseCreature.ToNwObject<NwCreature>(),
          AbilityType = Ability.Dexterity,
          Value = value,
        });
      }

      [UnmanagedCallersOnly]
      private static void OnSetCONBaseHook(void* pCreatureStats, byte value) {
        ProcessEvent(new OnAbilityScoreChange
        {
          Creature = CNWSCreatureStats.FromPointer(pCreatureStats).m_pBaseCreature.ToNwObject<NwCreature>(),
          AbilityType = Ability.Constitution,
          Value = value,
        });
      }

      [UnmanagedCallersOnly]
      private static void OnSetINTBaseHook(void* pCreatureStats, byte value) {
        ProcessEvent(new OnAbilityScoreChange
        {
          Creature = CNWSCreatureStats.FromPointer(pCreatureStats).m_pBaseCreature.ToNwObject<NwCreature>(),
          AbilityType = Ability.Intelligence,
          Value = value,
        }); }

      [UnmanagedCallersOnly]
      private static void OnSetWISBaseHook(void* pCreatureStats, byte value) {
        ProcessEvent(new OnAbilityScoreChange
        {
          Creature = CNWSCreatureStats.FromPointer(pCreatureStats).m_pBaseCreature.ToNwObject<NwCreature>(),
          AbilityType = Ability.Wisdom,
          Value = value,
        }); }

      [UnmanagedCallersOnly]
      private static void OnSetCHABaseHook(void* pCreatureStats, byte value) {
        ProcessEvent(new OnAbilityScoreChange
        {
          Creature = CNWSCreatureStats.FromPointer(pCreatureStats).m_pBaseCreature.ToNwObject<NwCreature>(),
          AbilityType = Ability.Charisma,
          Value = value,
        });
      }
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
