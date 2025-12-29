using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Native;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  /// <summary>
  /// Triggered for each individual melee or ranged attack in a flurry.
  /// </summary>
  public sealed class OnCreatureAttack : IEvent
  {
    /// <summary>
    /// Gets the attacking creature.
    /// </summary>
    public NwCreature Attacker { get; private init; } = null!;

    /// <summary>
    /// Gets the attack roll modifier applied to this attack.
    /// </summary>
    public int AttackModifier { get; private init; }

    /// <summary>
    /// Gets the 1-based index of this attack within the current flurry.
    /// </summary>
    public int AttackNumber { get; private init; }

    /// <summary>
    /// Gets or sets the resolved outcome for this attack.
    /// </summary>
    public AttackResult AttackResult
    {
      get => (AttackResult)CombatAttackData.m_nAttackResult;
      set => CombatAttackData.m_nAttackResult = (byte)value;
    }

    /// <summary>
    /// Gets the raw d20 attack roll value.
    /// </summary>
    public byte AttackRoll { get; private init; }

    /// <summary>
    /// Gets the internal attack type for this strike.
    /// </summary>
    public int AttackType { get; private init; }

    /// <summary>
    /// Gets the mutable damage data for this attack.
    /// </summary>
    public DamageData<short> DamageData { get; private init; } = null!;

    /// <summary>
    /// Gets a value indicating whether the attack was deflected.
    /// </summary>
    public bool IsAttackDeflected { get; private init; }

    /// <summary>
    /// Gets a value indicating whether the attack is a coup de grâce.
    /// </summary>
    public bool IsCoupDeGrace { get; private init; }

    /// <summary>
    /// Gets a value indicating whether the attack threatens a critical hit.
    /// </summary>
    public bool IsCriticalThreat { get; private init; }

    /// <summary>
    /// Gets a value indicating whether the attack is ranged.
    /// </summary>
    public bool IsRangedAttack { get; private init; }

    /// <summary>
    /// Gets a value indicating whether this attack is a killing blow.
    /// </summary>
    public bool KillingBlow { get; private init; }

    /// <summary>
    /// Gets the sneak/death attack state for this strike.
    /// </summary>
    public SneakAttack SneakAttack { get; private init; }

    /// <summary>
    /// Gets the target of the attack.
    /// </summary>
    public NwGameObject Target { get; private init; } = null!;

    /// <summary>
    /// Gets the total damage value for this attack.
    /// </summary>
    public int TotalDamage { get; private init; }

    /// <summary>
    /// Gets the weapon attack type for this strike.
    /// </summary>
    public WeaponAttackType WeaponAttackType { get; private init; }

    NwObject IEvent.Context => Attacker;

    private CNWSCombatAttackData CombatAttackData { get; init; } = null!;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<Functions.CNWSCreature.SignalMeleeDamage> signalMeleeDamageHook = null!;
      private static FunctionHook<Functions.CNWSCreature.SignalRangedDamage> signalRangedDamageHook = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void*, int, void> pSignalMeleeDamageHook = &OnSignalMeleeDamage;
        signalMeleeDamageHook = HookService.RequestHook<Functions.CNWSCreature.SignalMeleeDamage>(pSignalMeleeDamageHook, HookOrder.Late);

        delegate* unmanaged<void*, void*, int, void> pSignalRangedDamageHook = &OnSignalRangedDamage;
        signalRangedDamageHook = HookService.RequestHook<Functions.CNWSCreature.SignalRangedDamage>(pSignalRangedDamageHook, HookOrder.Late);

        return [signalMeleeDamageHook, signalRangedDamageHook];
      }

      private static OnCreatureAttack[] GetAttackEvents(void* pCreature, void* pTarget, int nAttacks)
      {
        CNWSCreature? creature = CNWSCreature.FromPointer(pCreature);
        NwGameObject? target = CNWSObject.FromPointer(pTarget).ToNwObject<NwGameObject>();

        if (creature == null || target == null)
        {
          return [];
        }

        NwCreature nwCreature = creature.ToNwObject<NwCreature>()!;

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
          ProcessEvent(EventCallbackType.Before, eventData);
        }

        signalMeleeDamageHook.CallOriginal(pCreature, pTarget, nAttacks);

        foreach (OnCreatureAttack eventData in attackEvents)
        {
          ProcessEvent(EventCallbackType.After, eventData);
        }
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
          ProcessEvent(EventCallbackType.Before, eventData);
        }

        signalRangedDamageHook.CallOriginal(pCreature, pTarget, nAttacks);

        foreach (OnCreatureAttack eventData in attackEvents)
        {
          ProcessEvent(EventCallbackType.After, eventData);
        }
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
