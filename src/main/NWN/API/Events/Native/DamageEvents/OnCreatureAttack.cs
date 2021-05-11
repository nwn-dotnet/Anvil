using System;
using System.Runtime.InteropServices;
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

    private CNWSCombatAttackData CombatAttackData { get; init; }

    NwObject IEvent.Context => Attacker;

    public static Type[] FactoryTypes { get; } = {typeof(MeleeDamageFactory), typeof(RangedDamageFactory)};

    internal sealed unsafe class MeleeDamageFactory : NativeEventFactory<MeleeDamageFactory.SignalMeleeDamageHook>
    {
      internal delegate void SignalMeleeDamageHook(void* pCreature, void* pTarget, int nAttacks);

      protected override FunctionHook<SignalMeleeDamageHook> RequestHook()
      {
        delegate* unmanaged<void*, void*, int, void> pHook = &OnSignalMeleeDamage;
        return HookService.RequestHook<SignalMeleeDamageHook>(pHook, FunctionsLinux._ZN12CNWSCreature17SignalMeleeDamageEP10CNWSObjecti, HookOrder.Late);
      }

      [UnmanagedCallersOnly]
      private static void OnSignalMeleeDamage(void* pCreature, void* pTarget, int nAttacks)
      {
        OnCreatureAttack[] attackEvents = GetAttackEvents(pCreature, pTarget, nAttacks);
        foreach (OnCreatureAttack eventData in attackEvents)
        {
          ProcessEvent(eventData);
        }

        Hook.CallOriginal(pCreature, pTarget, nAttacks);
      }
    }

    internal sealed unsafe class RangedDamageFactory : NativeEventFactory<RangedDamageFactory.SignalRangedDamageHook>
    {
      internal delegate void SignalRangedDamageHook(void* pCreature, void* pTarget, int nAttacks);

      protected override FunctionHook<SignalRangedDamageHook> RequestHook()
      {
        delegate* unmanaged<void*, void*, int, void> pHook = &OnSignalRangedDamage;
        return HookService.RequestHook<SignalRangedDamageHook>(pHook, FunctionsLinux._ZN12CNWSCreature18SignalRangedDamageEP10CNWSObjecti, HookOrder.Late);
      }

      [UnmanagedCallersOnly]
      private static void OnSignalRangedDamage(void* pCreature, void* pTarget, int nAttacks)
      {
        OnCreatureAttack[] attackEvents = GetAttackEvents(pCreature, pTarget, nAttacks);
        foreach (OnCreatureAttack eventData in attackEvents)
        {
          ProcessEvent(eventData);
        }

        Hook.CallOriginal(pCreature, pTarget, nAttacks);
      }
    }

    private static unsafe OnCreatureAttack[] GetAttackEvents(void* pCreature, void* pTarget, int nAttacks)
    {
      CNWSCreature cnwsCreature = new CNWSCreature(pCreature, false);
      NwCreature creature = cnwsCreature.m_idSelf.ToNwObject<NwCreature>();
      NwGameObject target = new CNWSObject(pTarget, false).m_idSelf.ToNwObject<NwGameObject>();

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
        DamageData = new DamageData<short>(combatAttackData.m_nDamage)
      };
    }
  }
}
