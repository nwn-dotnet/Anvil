using System;
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

    [NativeFunction(NWNXLib.Functions._ZN12CNWSCreature17SignalMeleeDamageEP10CNWSObjecti)]
    internal delegate void SignalMeleeDamageHook(IntPtr pCreature, IntPtr pTarget, int nAttacks);

    [NativeFunction(NWNXLib.Functions._ZN12CNWSCreature18SignalRangedDamageEP10CNWSObjecti)]
    internal delegate void SignalRangedDamageHook(IntPtr pCreature, IntPtr pTarget, int nAttacks);

    public static Type[] FactoryTypes { get; } = {typeof(MeleeDamageFactory), typeof(RangedDamageFactory)};

    internal class MeleeDamageFactory : NativeEventFactory<SignalMeleeDamageHook>
    {
      public MeleeDamageFactory(Lazy<EventService> eventService, HookService hookService) : base(eventService, hookService) {}

      protected override FunctionHook<SignalMeleeDamageHook> RequestHook(HookService hookService)
        => hookService.RequestHook<SignalMeleeDamageHook>(OnSignalMeleeDamage, HookOrder.Late);

      private void OnSignalMeleeDamage(IntPtr pCreature, IntPtr pTarget, int nAttacks)
      {
        OnCreatureAttack[] attackEvents = GetAttackEvents(pCreature, pTarget, nAttacks);
        foreach (OnCreatureAttack eventData in attackEvents)
        {
          ProcessEvent(eventData);
        }

        Hook.CallOriginal(pCreature, pTarget, nAttacks);
      }
    }

    internal class RangedDamageFactory : NativeEventFactory<SignalRangedDamageHook>
    {
      public RangedDamageFactory(Lazy<EventService> eventService, HookService hookService) : base(eventService, hookService) {}

      protected override FunctionHook<SignalRangedDamageHook> RequestHook(HookService hookService)
        => hookService.RequestHook<SignalRangedDamageHook>(OnSignalRangedDamage, HookOrder.Late);

      private void OnSignalRangedDamage(IntPtr pCreature, IntPtr pTarget, int nAttacks)
      {
        OnCreatureAttack[] attackEvents = GetAttackEvents(pCreature, pTarget, nAttacks);
        foreach (OnCreatureAttack eventData in attackEvents)
        {
          ProcessEvent(eventData);
        }

        Hook.CallOriginal(pCreature, pTarget, nAttacks);
      }
    }

    private static OnCreatureAttack[] GetAttackEvents(IntPtr pCreature, IntPtr pTarget, int nAttacks)
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
