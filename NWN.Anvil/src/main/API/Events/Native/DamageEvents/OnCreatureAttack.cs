using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnCreatureAttack : IEvent
  {
    public NwCreature Attacker { get; private init; }

    public int AttackModifier { get; private init; }

    public int AttackNumber { get; private init; }

    public AttackResult AttackResult
    {
      get => (AttackResult)CombatAttackData.m_nAttackResult;
      set => CombatAttackData.m_nAttackResult = (byte)value;
    }

    public byte AttackRoll { get; private init; }

    public int AttackType { get; private init; }

    public DamageData<short> DamageData { get; private init; }

    public bool IsAttackDeflected { get; private init; }

    public bool IsCoupDeGrace { get; private init; }

    public bool IsCriticalThreat { get; private init; }

    public bool IsRangedAttack { get; private init; }

    public bool KillingBlow { get; private init; }

    public SneakAttack SneakAttack { get; private init; }

    public NwGameObject Target { get; private init; }

    public int TotalDamage { get; private init; }

    public WeaponAttackType WeaponAttackType { get; private init; }

    NwObject? IEvent.Context => Attacker;

    private CNWSCombatAttackData CombatAttackData { get; init; }

    internal sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<SignalMeleeDamageHook> signalMeleeDamageHook;
      private static FunctionHook<SignalRangedDamageHook> signalRangedDamageHook;

      private delegate void SignalMeleeDamageHook(void* pCreature, void* pTarget, int nAttacks);

      private delegate void SignalRangedDamageHook(void* pCreature, void* pTarget, int nAttacks);

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void*, int, void> pSignalMeleeDamageHook = &OnSignalMeleeDamage;
        signalMeleeDamageHook = HookService.RequestHook<SignalMeleeDamageHook>(pSignalMeleeDamageHook, FunctionsLinux._ZN12CNWSCreature17SignalMeleeDamageEP10CNWSObjecti, HookOrder.Late);

        delegate* unmanaged<void*, void*, int, void> pSignalRangedDamageHook = &OnSignalRangedDamage;
        signalRangedDamageHook = HookService.RequestHook<SignalRangedDamageHook>(pSignalRangedDamageHook, FunctionsLinux._ZN12CNWSCreature18SignalRangedDamageEP10CNWSObjecti, HookOrder.Late);

        return new IDisposable[] { signalMeleeDamageHook, signalRangedDamageHook };
      }

      private static OnCreatureAttack[] GetAttackEvents(void* pCreature, void* pTarget, int nAttacks)
      {
        CNWSCreature creature = CNWSCreature.FromPointer(pCreature);
        NwGameObject target = CNWSObject.FromPointer(pTarget).ToNwObject<NwGameObject>();
        NwCreature nwCreature = creature.ToNwObject<NwCreature>();

        // m_nCurrentAttack points to the attack after this flurry.
        int attackNumberOffset = creature.m_pcCombatRound.m_nCurrentAttack - nAttacks;
        CNWSCombatRound combatRound = creature.m_pcCombatRound;

        // Create an event for each attack in the flurry
        OnCreatureAttack[] attackEvents = new OnCreatureAttack[nAttacks];
        for (int i = 0; i < nAttacks; i++)
        {
          attackEvents[i] = GetEventData(nwCreature, target, combatRound, attackNumberOffset + i);
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
          AttackRoll = combatAttackData.m_nToHitRoll,
          AttackModifier = combatAttackData.m_nToHitMod,
          IsRangedAttack = combatAttackData.m_bRangedAttack.ToBool(),
          IsCoupDeGrace = combatAttackData.m_bCoupDeGrace.ToBool(),
          IsAttackDeflected = combatAttackData.m_bAttackDeflected.ToBool(),
          IsCriticalThreat = combatAttackData.m_bCriticalThreat.ToBool(),
          DamageData = new DamageData<short>(combatAttackData.m_nDamage),
          TotalDamage = combatAttackData.GetTotalDamage(),
        };
      }

      [UnmanagedCallersOnly]
      private static void OnSignalMeleeDamage(void* pCreature, void* pTarget, int nAttacks)
      {
        if (pCreature == null || pTarget == null)
        {
          signalMeleeDamageHook.CallOriginal(pCreature, pTarget, nAttacks);
          return;
        }

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
        if (pCreature == null || pTarget == null)
        {
          signalRangedDamageHook.CallOriginal(pCreature, pTarget, nAttacks);
          return;
        }

        OnCreatureAttack[] attackEvents = GetAttackEvents(pCreature, pTarget, nAttacks);
        foreach (OnCreatureAttack eventData in attackEvents)
        {
          ProcessEvent(eventData);
        }

        signalRangedDamageHook.CallOriginal(pCreature, pTarget, nAttacks);
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="Events.OnCreatureAttack"/>
    public event Action<OnCreatureAttack> OnCreatureAttack
    {
      add => EventService.Subscribe<OnCreatureAttack, OnCreatureAttack.Factory>(this, value);
      remove => EventService.Unsubscribe<OnCreatureAttack, OnCreatureAttack.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnCreatureAttack"/>
    public event Action<OnCreatureAttack> OnCreatureAttack
    {
      add => EventService.SubscribeAll<OnCreatureAttack, OnCreatureAttack.Factory>(value);
      remove => EventService.UnsubscribeAll<OnCreatureAttack, OnCreatureAttack.Factory>(value);
    }
  }
}
