using System;
using System.Runtime.InteropServices;
using NWN.API.Events;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public sealed class OnCreatureAttack : IEvent
  {
    public AttackResult AttackResult
    {
      get => (AttackResult)CombatAttackData.m_nAttackResult;
      set => CombatAttackData.m_nAttackResult = (byte)value;
    }

    public NwCreature Attacker { get; private init; }

    public NwGameObject Target { get; private init; }

    public int AttackNumber { get; private init; }

    public WeaponAttackType WeaponAttackType { get; private init; }

    public SneakAttack SneakAttack { get; private init; }

    public int AttackType { get; private init; }

    public bool KillingBlow { get; private init; }

    public DamageData<short> DamageData { get; private init; }

    public int TotalDamage { get; private init; }

    private CNWSCombatAttackData CombatAttackData { get; init; }

    NwObject IEvent.Context
    {
      get => Attacker;
    }

    internal sealed unsafe class Factory : MultiHookEventFactory
    {
      internal delegate void SignalMeleeDamageHook(void* pCreature, void* pTarget, int nAttacks);

      internal delegate void SignalRangedDamageHook(void* pCreature, void* pTarget, int nAttacks);

      private static FunctionHook<SignalMeleeDamageHook> signalMeleeDamageHook;
      private static FunctionHook<SignalRangedDamageHook> signalRangedDamageHook;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void*, int, void> pSignalMeleeDamageHook = &OnSignalMeleeDamage;
        signalMeleeDamageHook = HookService.RequestHook<SignalMeleeDamageHook>(pSignalMeleeDamageHook, FunctionsLinux._ZN12CNWSCreature17SignalMeleeDamageEP10CNWSObjecti, HookOrder.Late);

        delegate* unmanaged<void*, void*, int, void> pSignalRangedDamageHook = &OnSignalRangedDamage;
        signalRangedDamageHook = HookService.RequestHook<SignalRangedDamageHook>(pSignalRangedDamageHook, FunctionsLinux._ZN12CNWSCreature18SignalRangedDamageEP10CNWSObjecti, HookOrder.Late);

        return new IDisposable[] { signalMeleeDamageHook, signalRangedDamageHook };
      }

      [UnmanagedCallersOnly]
      private static void OnSignalMeleeDamage(void* pCreature, void* pTarget, int nAttacks)
      {
        OnCreatureAttack[] attackEvents = GetAttackEvents(pCreature, pTarget, nAttacks);
        foreach (OnCreatureAttack eventData in attackEvents)
        {
          ProcessEvent(eventData);
        }

        signalMeleeDamageHook.CallOriginal(pCreature, pTarget, nAttacks);
      }

      [UnmanagedCallersOnly]
      private static void OnSignalRangedDamage(void* pCreature, void* pTarget, int nAttacks)
      {
        OnCreatureAttack[] attackEvents = GetAttackEvents(pCreature, pTarget, nAttacks);
        foreach (OnCreatureAttack eventData in attackEvents)
        {
          ProcessEvent(eventData);
        }

        signalRangedDamageHook.CallOriginal(pCreature, pTarget, nAttacks);
      }

      private static OnCreatureAttack[] GetAttackEvents(void* pCreature, void* pTarget, int nAttacks)
      {
        CNWSCreature cnwsCreature = CNWSCreature.FromPointer(pCreature);
        NwCreature creature = cnwsCreature.ToNwObject<NwCreature>();
        NwGameObject target = CNWSObject.FromPointer(pTarget).ToNwObject<NwGameObject>();

        // m_nCurrentAttack points to the attack after this flurry.
        int attackNumberOffset = cnwsCreature.m_pcCombatRound.m_nCurrentAttack - nAttacks;
        CNWSCombatRound combatRound = cnwsCreature.m_pcCombatRound;

        // Create an event for each attack in the flurry
        OnCreatureAttack[] attackEvents = new OnCreatureAttack[nAttacks];
        for (int i = 0; i < nAttacks; i++)
        {
          attackEvents[i] = GetEventData(creature, target, combatRound, attackNumberOffset + i);
        }

        return attackEvents;
      }

      private static OnCreatureAttack GetEventData(NwCreature creature, NwGameObject target, CNWSCombatRound combatRound, int attackNumber)
      {
        CNWSCombatAttackData combatAttackData = combatRound.GetAttack(attackNumber);

        return new OnCreatureAttack
        {
          CombatAttackData = combatAttackData,
          Attacker = creature,
          Target = target,
          AttackNumber = attackNumber + 1, // 1-based for backwards compatibility
          WeaponAttackType = (WeaponAttackType)combatAttackData.m_nWeaponAttackType,
          SneakAttack = (SneakAttack)(combatAttackData.m_bSneakAttack + (combatAttackData.m_bDeathAttack << 1)),
          KillingBlow = combatAttackData.m_bKillingBlow.ToBool(),
          AttackType = combatAttackData.m_nAttackType,
          DamageData = new DamageData<short>(combatAttackData.m_nDamage),
          TotalDamage = combatAttackData.GetTotalDamage(),
        };
      }
    }
  }
}

namespace NWN.API
{
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="NWN.API.Events.OnCreatureAttack"/>
    public event Action<OnCreatureAttack> OnCreatureAttack
    {
      add => EventService.Subscribe<OnCreatureAttack, OnCreatureAttack.Factory>(this, value);
      remove => EventService.Unsubscribe<OnCreatureAttack, OnCreatureAttack.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="NWN.API.Events.OnCreatureAttack"/>
    public event Action<OnCreatureAttack> OnCreatureAttack
    {
      add => EventService.SubscribeAll<OnCreatureAttack, OnCreatureAttack.Factory>(value);
      remove => EventService.UnsubscribeAll<OnCreatureAttack, OnCreatureAttack.Factory>(value);
    }
  }
}
