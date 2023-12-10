using System;
using System.Collections.Generic;
using System.Text;
using Anvil.API;
using Anvil.API.Events;
using Anvil.Native;
using NWN.Native.API;
using ClassType = NWN.Native.API.ClassType;
using CombatMode = Anvil.API.CombatMode;
using CreatureSize = Anvil.API.CreatureSize;
using Feat = NWN.Native.API.Feat;
using RacialType = NWN.Native.API.RacialType;

namespace Anvil.Services
{
  [ServiceBinding(typeof(WeaponService))]
  [ServiceBindingOptions(Lazy = true)]
  public sealed unsafe class WeaponService : IDisposable
  {
    private readonly Dictionary<uint, HashSet<ushort>> epicWeaponDevastatingCriticalMap = new Dictionary<uint, HashSet<ushort>>();
    private readonly Dictionary<uint, HashSet<ushort>> epicWeaponFocusMap = new Dictionary<uint, HashSet<ushort>>();
    private readonly Dictionary<uint, HashSet<ushort>> epicWeaponOverwhelmingCriticalMap = new Dictionary<uint, HashSet<ushort>>();
    private readonly Dictionary<uint, HashSet<ushort>> epicWeaponSpecializationMap = new Dictionary<uint, HashSet<ushort>>();
    private readonly EventService eventService;
    private readonly FunctionHook<Functions.CNWSCreatureStats.GetAttackModifierVersus> getAttackModifierVersusHook;
    private readonly FunctionHook<Functions.CNWSCreatureStats.GetDamageBonus> getDamageBonusHook;
    private readonly FunctionHook<Functions.CNWSCreatureStats.GetEpicWeaponDevastatingCritical> getEpicWeaponDevastatingCriticalHook;
    private readonly FunctionHook<Functions.CNWSCreatureStats.GetEpicWeaponFocus> getEpicWeaponFocusHook;
    private readonly FunctionHook<Functions.CNWSCreatureStats.GetEpicWeaponOverwhelmingCritical> getEpicWeaponOverwhelmingCriticalHook;
    private readonly FunctionHook<Functions.CNWSCreatureStats.GetEpicWeaponSpecialization> getEpicWeaponSpecializationHook;
    private readonly FunctionHook<Functions.CNWSCreatureStats.GetIsWeaponOfChoice> getIsWeaponOfChoiceHook;
    private readonly FunctionHook<Functions.CNWSCreatureStats.GetMeleeAttackBonus> getMeleeAttackBonusHook;
    private readonly FunctionHook<Functions.CNWSCreatureStats.GetMeleeDamageBonus> getMeleeDamageBonusHook;
    private readonly FunctionHook<Functions.CNWSCreatureStats.GetRangedAttackBonus> getRangedAttackBonusHook;
    private readonly FunctionHook<Functions.CNWSCreatureStats.GetRangedDamageBonus> getRangedDamageBonusHook;
    private readonly FunctionHook<Functions.CNWSCreatureStats.GetUseMonkAttackTables> getUseMonkAttackTablesHook;
    private readonly FunctionHook<Functions.CNWSCreatureStats.GetWeaponFinesse> getWeaponFinesseHook;

    private readonly FunctionHook<Functions.CNWSCreatureStats.GetWeaponFocus> getWeaponFocusHook;
    private readonly FunctionHook<Functions.CNWSCreatureStats.GetWeaponImprovedCritical> getWeaponImprovedCriticalHook;
    private readonly FunctionHook<Functions.CNWSCreatureStats.GetWeaponSpecialization> getWeaponSpecializationHook;
    private readonly Dictionary<uint, HashSet<ushort>> greaterWeaponFocusMap = new Dictionary<uint, HashSet<ushort>>();
    private readonly Dictionary<uint, HashSet<ushort>> greaterWeaponSpecializationMap = new Dictionary<uint, HashSet<ushort>>();
    private readonly HookService hookService;

    private readonly Dictionary<uint, MaxRangedAttackDistanceOverride> maxRangedAttackDistanceOverrideMap = new Dictionary<uint, MaxRangedAttackDistanceOverride>();
    private readonly HashSet<uint> monkWeaponSet = new HashSet<uint>();
    private readonly Dictionary<uint, byte> weaponFinesseSizeMap = new Dictionary<uint, byte>();

    private readonly Dictionary<uint, HashSet<ushort>> weaponFocusMap = new Dictionary<uint, HashSet<ushort>>();
    private readonly Dictionary<uint, HashSet<ushort>> weaponImprovedCriticalMap = new Dictionary<uint, HashSet<ushort>>();
    private readonly Dictionary<uint, HashSet<ushort>> weaponOfChoiceMap = new Dictionary<uint, HashSet<ushort>>();
    private readonly Dictionary<uint, HashSet<ushort>> weaponSpecializationMap = new Dictionary<uint, HashSet<ushort>>();

    private readonly HashSet<uint> weaponUnarmedSet = new HashSet<uint>();
    private bool combatModeEventSubscribed;

    private FunctionHook<Functions.CNWSCreature.MaxAttackRange>? maxAttackRangeHook;

    public WeaponService(HookService hookService, EventService eventService)
    {
      this.hookService = hookService;
      this.eventService = eventService;

      getWeaponFocusHook = hookService.RequestHook<Functions.CNWSCreatureStats.GetWeaponFocus>(OnGetWeaponFocus, HookOrder.Late);
      getEpicWeaponFocusHook = hookService.RequestHook<Functions.CNWSCreatureStats.GetEpicWeaponFocus>(OnGetEpicWeaponFocus, HookOrder.Late);
      getWeaponFinesseHook = hookService.RequestHook<Functions.CNWSCreatureStats.GetWeaponFinesse>(OnGetWeaponFinesse, HookOrder.Final);
      getWeaponImprovedCriticalHook = hookService.RequestHook<Functions.CNWSCreatureStats.GetWeaponImprovedCritical>(OnGetWeaponImprovedCritical, HookOrder.Late);
      getEpicWeaponOverwhelmingCriticalHook = hookService.RequestHook<Functions.CNWSCreatureStats.GetEpicWeaponOverwhelmingCritical>(OnGetEpicWeaponOverwhelmingCritical, HookOrder.Late);
      getEpicWeaponDevastatingCriticalHook = hookService.RequestHook<Functions.CNWSCreatureStats.GetEpicWeaponDevastatingCritical>(OnGetEpicWeaponDevastatingCritical, HookOrder.Late);
      getWeaponSpecializationHook = hookService.RequestHook<Functions.CNWSCreatureStats.GetWeaponSpecialization>(OnGetWeaponSpecialization, HookOrder.Late);
      getEpicWeaponSpecializationHook = hookService.RequestHook<Functions.CNWSCreatureStats.GetEpicWeaponSpecialization>(OnGetEpicWeaponSpecialization, HookOrder.Late);
      getIsWeaponOfChoiceHook = hookService.RequestHook<Functions.CNWSCreatureStats.GetIsWeaponOfChoice>(OnGetIsWeaponOfChoice, HookOrder.Late);
      getDamageBonusHook = hookService.RequestHook<Functions.CNWSCreatureStats.GetDamageBonus>(OnGetDamageBonus, HookOrder.Late);
      getMeleeDamageBonusHook = hookService.RequestHook<Functions.CNWSCreatureStats.GetMeleeDamageBonus>(OnGetMeleeDamageBonus, HookOrder.Late);
      getRangedDamageBonusHook = hookService.RequestHook<Functions.CNWSCreatureStats.GetRangedDamageBonus>(OnGetRangedDamageBonus, HookOrder.Late);
      getMeleeAttackBonusHook = hookService.RequestHook<Functions.CNWSCreatureStats.GetMeleeAttackBonus>(OnGetMeleeAttackBonus, HookOrder.Late);
      getRangedAttackBonusHook = hookService.RequestHook<Functions.CNWSCreatureStats.GetRangedAttackBonus>(OnGetRangedAttackBonus, HookOrder.Late);
      getAttackModifierVersusHook = hookService.RequestHook<Functions.CNWSCreatureStats.GetAttackModifierVersus>(OnGetAttackModifierVersus, HookOrder.Late);
      getUseMonkAttackTablesHook = hookService.RequestHook<Functions.CNWSCreatureStats.GetUseMonkAttackTables>(OnGetUseMonkAttackTables, HookOrder.Final);

      weaponFinesseSizeMap[(uint)BaseItem.Rapier] = (byte)CreatureSize.Medium;
    }

    /// <summary>
    /// Called when an attack results in a devastating critical hit. Subscribe and modify the event data to implement custom behaviours.
    /// </summary>
    public event Action<DevastatingCriticalData>? OnDevastatingCriticalHit;

    /// <summary>
    /// Gets or sets whether the "Good Aim" feat should also apply to slings.
    /// </summary>
    public bool EnableSlingGoodAimFeat { get; set; } = false;

    /// <summary>
    /// Gets or sets the attack bonus granted to a creature with a "Greater Weapon Focus" feat.<br/>
    /// This does not exist in the base game, see <see cref="AddGreaterWeaponFocusFeat"/> to add feats that grant this bonus.
    /// </summary>
    public int GreaterWeaponFocusAttackBonus { get; set; } = 1;

    /// <summary>
    /// Gets or sets the damage bonus granted to a creature with a "Greater Weapon Specialization" feat.<br/>
    /// This does not exist in the base game, see <see cref="AddGreaterWeaponSpecializationFeat"/> to add feats that grant this bonus.
    /// </summary>
    public int GreaterWeaponSpecializationDamageBonus { get; set; } = 2;

    /// <summary>
    /// Adds the specified feat as a devastating critical feat for the specified base item type.
    /// </summary>
    /// <param name="baseItem">The base item type to be mapped.</param>
    /// <param name="feat">The feat to map to the base item.</param>
    public void AddEpicWeaponDevastatingCriticalFeat(NwBaseItem baseItem, NwFeat feat)
    {
      epicWeaponDevastatingCriticalMap.AddElement(baseItem.Id, feat.Id);
    }

    /// <summary>
    /// Adds the specified feat as an epic weapon focus feat for the specified base item type.
    /// </summary>
    /// <param name="baseItem">The base item type to be mapped.</param>
    /// <param name="feat">The feat to map to the base item.</param>
    public void AddEpicWeaponFocusFeat(NwBaseItem baseItem, NwFeat feat)
    {
      epicWeaponFocusMap.AddElement(baseItem.Id, feat.Id);
    }

    /// <summary>
    /// Adds the specified feat as an epic overwhelming critical feat for the specified base item type.
    /// </summary>
    /// <param name="baseItem">The base item type to be mapped.</param>
    /// <param name="feat">The feat to map to the base item.</param>
    public void AddEpicWeaponOverwhelmingCriticalFeat(NwBaseItem baseItem, NwFeat feat)
    {
      epicWeaponOverwhelmingCriticalMap.AddElement(baseItem.Id, feat.Id);
    }

    /// <summary>
    /// Adds the specified feat as an epic weapon specialization feat for the specified base item type.
    /// </summary>
    /// <param name="baseItem">The base item type to be mapped.</param>
    /// <param name="feat">The feat to map to the base item.</param>
    public void AddEpicWeaponSpecializationFeat(NwBaseItem baseItem, NwFeat feat)
    {
      epicWeaponSpecializationMap.AddElement(baseItem.Id, feat.Id);
    }

    /// <summary>
    /// Adds the specified feat as a greater weapon focus feat for the specified base item type.<br/>
    /// This adds the <see cref="GreaterWeaponFocusAttackBonus"/> to the weapon's attack roll for characters with the specified feat.
    /// </summary>
    /// <param name="baseItem">The base item type to be mapped.</param>
    /// <param name="feat">The feat to map to the base item.</param>
    public void AddGreaterWeaponFocusFeat(NwBaseItem baseItem, NwFeat feat)
    {
      greaterWeaponFocusMap.AddElement(baseItem.Id, feat.Id);
    }

    /// <summary>
    /// Adds the specified feat as a greater weapon specialization feat for the specified base item type.<br/>
    /// This adds the <see cref="GreaterWeaponSpecializationDamageBonus"/> to the weapon's damage roll for characters with the specified feat.
    /// </summary>
    /// <param name="baseItem">The base item type to be mapped.</param>
    /// <param name="feat">The feat to map to the base item.</param>
    public void AddGreaterWeaponSpecializationFeat(NwBaseItem baseItem, NwFeat feat)
    {
      greaterWeaponSpecializationMap.AddElement(baseItem.Id, feat.Id);
    }

    /// <summary>
    /// Adds the specified feat as a weapon focus feat for the specified base item type.
    /// </summary>
    /// <param name="baseItem">The base item type to be mapped.</param>
    /// <param name="feat">The feat to map to the base item.</param>
    public void AddWeaponFocusFeat(NwBaseItem baseItem, NwFeat feat)
    {
      weaponFocusMap.AddElement(baseItem.Id, feat.Id);
    }

    /// <summary>
    /// Adds the specified feat as an improved critical feat for the specified base item type.
    /// </summary>
    /// <param name="baseItem">The base item type to be mapped.</param>
    /// <param name="feat">The feat to map to the base item.</param>
    public void AddWeaponImprovedCriticalFeat(NwBaseItem baseItem, NwFeat feat)
    {
      weaponImprovedCriticalMap.AddElement(baseItem.Id, feat.Id);
    }

    /// <summary>
    /// Adds the specified feat as a weapon of choice feat for the specified base item type.
    /// </summary>
    /// <param name="baseItem">The base item type to be mapped.</param>
    /// <param name="feat">The feat to map to the base item.</param>
    public void AddWeaponOfChoiceFeat(NwBaseItem baseItem, NwFeat feat)
    {
      weaponOfChoiceMap.AddElement(baseItem.Id, feat.Id);
    }

    /// <summary>
    /// Adds the specified feat as a weapon specialization feat for the specified base item type.
    /// </summary>
    /// <param name="baseItem">The base item type to be mapped.</param>
    /// <param name="feat">The feat to map to the base item.</param>
    public void AddWeaponSpecializationFeat(NwBaseItem baseItem, NwFeat feat)
    {
      weaponSpecializationMap.AddElement(baseItem.Id, feat.Id);
    }

    /// <summary>
    /// Gets the required creature size needed for the specified base item type to be finessable. This function only returns values assigned in <see cref="SetWeaponFinesseSize"/>.
    /// </summary>
    /// <param name="baseItem">The base item type to query.</param>
    /// <returns>The size of the creature needed to consider this weapon finessable.</returns>
    public CreatureSize GetWeaponFinesseSize(NwBaseItem baseItem)
    {
      return weaponFinesseSizeMap.TryGetValue(baseItem.Id, out byte size) ? (CreatureSize)size : CreatureSize.Invalid;
    }

    /// <summary>
    /// Overrides the max attack distance of ranged weapons.
    /// </summary>
    /// <param name="baseItem">The base item type.</param>
    /// <param name="max">The maximum attack distance. Default is 40.0f.</param>
    /// <param name="maxPassive">The maximum passive attack distance. Default is 20.0f. Seems to be used by the engine to determine a new nearby target when needed.</param>
    /// <param name="preferred">The preferred attack distance. See the PrefAttackDist column in baseitems.2da, default seems to be 30.0f for ranged weapons.</param>
    /// <remarks>maxPassive should probably be lower than max, half of max seems to be a good start. preferred should be at least ~0.5f lower than max.</remarks>
    public void SetMaxRangedAttackDistanceOverride(NwBaseItem baseItem, float max, float maxPassive, float preferred)
    {
      if (!baseItem.IsRangedWeapon)
      {
        return;
      }

      baseItem.PreferredAttackDistance = preferred;

      MaxRangedAttackDistanceOverride overrideData;
      overrideData.MaxRangedAttackDistance = max;
      overrideData.MaxRangedPassiveAttackDistance = maxPassive;

      maxRangedAttackDistanceOverrideMap[baseItem.Id] = overrideData;
      maxAttackRangeHook ??= hookService.RequestHook<Functions.CNWSCreature.MaxAttackRange>(OnMaxAttackRange, HookOrder.Final);
    }

    /// <summary>
    /// Sets the required creature size needed for the specified base item type to be finessable.
    /// </summary>
    /// <param name="baseItem">The base item type to be mapped.</param>
    /// <param name="size">The size of the creature needed to consider this weapon finessable.</param>
    public void SetWeaponFinesseSize(NwBaseItem baseItem, CreatureSize size)
    {
      weaponFinesseSizeMap[baseItem.Id] = (byte)size;
    }

    /// <summary>
    /// Sets the specified weapon base item to be considered a monk weapon.
    /// </summary>
    /// <param name="baseItem">The base item type to be considered a monk weapon.</param>
    public void SetWeaponIsMonkWeapon(NwBaseItem baseItem)
    {
      monkWeaponSet.Add(baseItem.Id);

      if (!combatModeEventSubscribed)
      {
        eventService.SubscribeAll<OnCombatModeToggle, OnCombatModeToggle.Factory>(OnCombatModeToggle);
        combatModeEventSubscribed = true;
      }
    }

    /// <summary>
    /// Sets the specified weapon base item to be considered as unarmed for the weapon finesse feat.
    /// </summary>
    /// <param name="baseItem">The base item type to be considered unarmed.</param>
    public void SetWeaponUnarmed(NwBaseItem baseItem)
    {
      weaponUnarmedSet.Add(baseItem.Id);
    }

    void IDisposable.Dispose()
    {
      getWeaponFocusHook.Dispose();
      getEpicWeaponFocusHook.Dispose();
      getWeaponFinesseHook.Dispose();
      getWeaponImprovedCriticalHook.Dispose();
      getEpicWeaponOverwhelmingCriticalHook.Dispose();
      getEpicWeaponDevastatingCriticalHook.Dispose();
      getWeaponSpecializationHook.Dispose();
      getEpicWeaponSpecializationHook.Dispose();
      getIsWeaponOfChoiceHook.Dispose();
      getDamageBonusHook.Dispose();
      getMeleeDamageBonusHook.Dispose();
      getRangedDamageBonusHook.Dispose();
      getMeleeAttackBonusHook.Dispose();
      getRangedAttackBonusHook.Dispose();
      getAttackModifierVersusHook.Dispose();
      getUseMonkAttackTablesHook.Dispose();
      maxAttackRangeHook?.Dispose();
    }

    private CNWSItem? GetEquippedWeapon(CNWSCreature creature, bool offHand)
    {
      CNWSItem weapon;

      if (offHand)
      {
        weapon = creature.m_pInventory.GetItemInSlot((uint)EquipmentSlot.LeftHand);
        if (weapon == null)
        {
          CNWSItem main = creature.m_pInventory.GetItemInSlot((uint)EquipmentSlot.RightHand);
          if (main != null)
          {
            CNWBaseItem baseWeapon = NWNXLib.Rules().m_pBaseItemArray.GetBaseItem((int)main.m_nBaseItem);
            if (baseWeapon != null && baseWeapon.m_nWeaponWield == 8)
            {
              weapon = main;
            }
          }
        }
      }
      else
      {
        weapon = creature.m_pInventory.GetItemInSlot((uint)EquipmentSlot.RightHand);
      }

      return weapon;
    }

    private int GetLevelByClass(CNWSCreatureStats stats, uint classType)
    {
      for (byte i = 0; i < stats.m_nNumMultiClasses; i++)
      {
        CNWSCreatureStats_ClassInfo classInfo = stats.GetClassInfo(i);
        if (classInfo.m_nClass == classType)
        {
          return classInfo.m_nLevel;
        }
      }

      return 0;
    }

    private bool IsUnarmedWeapon(CNWSItem? weapon)
    {
      if (weapon == null || weapon.Pointer == IntPtr.Zero)
      {
        return true;
      }

      // In case of standard unarmed weapon return true
      switch ((BaseItem)weapon.m_nBaseItem)
      {
        case BaseItem.Gloves:
        case BaseItem.Bracer:
        case BaseItem.CreatureSlashWeapon:
        case BaseItem.CreaturePierceWeapon:
        case BaseItem.CreatureBludgeWeapon:
        case BaseItem.CreatureSlashPierceWeapon:
          return true;
        default:
          return weaponUnarmedSet.Contains(weapon.m_nBaseItem);
      }
    }

    private bool IsWeaponLight(CNWSCreatureStats stats, CNWSItem weapon, bool finesse)
    {
      if (IsUnarmedWeapon(weapon))
      {
        return true;
      }

      CNWSCreature creature = stats.m_pBaseCreature;

      if (creature == null)
      {
        return false;
      }

      int creatureSize = creature.m_nCreatureSize;
      if (creatureSize is < (int)CreatureSize.Tiny or > (int)CreatureSize.Huge)
      {
        return false;
      }

      if (finesse)
      {
        const byte defaultSize = (byte)CreatureSize.Huge + 1;
        byte size = weaponFinesseSizeMap.GetValueOrDefault(weapon.m_nBaseItem, defaultSize);

        if (creatureSize >= size)
        {
          return true;
        }
      }

      int rel = stats.m_pBaseCreature.GetRelativeWeaponSize(weapon);
      if (finesse && creatureSize < (int)CreatureSize.Small)
      {
        return rel <= 0;
      }

      return rel < 0;
    }

    private void OnCombatModeToggle(OnCombatModeToggle onToggle)
    {
      CNWSCreature creature = onToggle.Creature.Creature;

      // Flurry of blows automatic cancel
      if (onToggle.NewMode == CombatMode.None && onToggle.ForceNewMode && creature.m_nCombatMode == (byte)CombatMode.FlurryOfBlows)
      {
        if (creature.m_pStats.GetUseMonkAttackTables(0).ToBool())
        {
          onToggle.PreventToggle = true;
          return;
        }
      }

      // Flurry of blows manual cancel
      if (onToggle.NewMode == CombatMode.FlurryOfBlows && !onToggle.ForceNewMode)
      {
        onToggle.NewMode = CombatMode.None;
        onToggle.ForceNewModeOverride = ForceNewModeOverride.Force;
      }

      if (onToggle.PreventToggle)
      {
        return;
      }

      // Flurry of blows manual activation.
      if (onToggle.NewMode == CombatMode.FlurryOfBlows && onToggle.ForceNewMode)
      {
        if (creature.m_pStats.GetUseMonkAttackTables(0).ToBool())
        {
          creature.m_nCombatMode = (byte)CombatMode.FlurryOfBlows;
          creature.SetActivity(0x4000, 1);
          onToggle.PreventToggle = true;
        }
      }
    }

    private int OnGetAttackModifierVersus(void* pStats, void* pCreature)
    {
      int attackMod = getAttackModifierVersusHook.CallOriginal(pStats, pCreature);

      CNWSCreatureStats stats = CNWSCreatureStats.FromPointer(pStats);
      CNWSCreature creature = stats.m_pBaseCreature;
      CNWSCombatRound combatRound = creature.m_pcCombatRound;

      if (combatRound == null)
      {
        return attackMod;
      }

      CNWSItem weapon = combatRound.GetCurrentAttackWeapon(combatRound.GetWeaponAttackType());
      if (weapon == null)
      {
        return attackMod;
      }

      uint baseItem = weapon.m_nBaseItem;
      bool hasApplicableFeat = false;
      bool applicableFeatExists = greaterWeaponFocusMap.TryGetValue(baseItem, out HashSet<ushort>? types);

      if (applicableFeatExists)
      {
        foreach (ushort feat in types!)
        {
          hasApplicableFeat = stats.HasFeat(feat).ToBool();
          if (hasApplicableFeat)
          {
            break;
          }
        }
      }

      if (applicableFeatExists && hasApplicableFeat)
      {
        attackMod += GreaterWeaponFocusAttackBonus;

        if ((*NWNXLib.EnableCombatDebugging()).ToBool() && stats.m_bIsPC.ToBool())
        {
          CNWSCombatAttackData currentAttack = combatRound.GetAttack(combatRound.m_nCurrentAttack);
          StringBuilder debugMessage = new StringBuilder(currentAttack.m_sDamageDebugText.ToString());
          debugMessage.Append(" + ");
          debugMessage.Append(GreaterWeaponFocusAttackBonus);
          debugMessage.Append(" (Greater Weapon Focus Feat)");

          currentAttack.m_sDamageDebugText = debugMessage.ToString().ToExoString();
        }
      }

      if (EnableSlingGoodAimFeat && baseItem == (uint)BaseItem.Sling && stats.m_nRace != (ushort)RacialType.Halfling && stats.HasFeat((ushort)Feat.GoodAim).ToBool())
      {
        int goodAimModifier = NWNXLib.Rules().GetRulesetIntEntry(RulesetKeys.GOOD_AIM_MODIFIER, 1);
        attackMod += goodAimModifier;

        if ((*NWNXLib.EnableCombatDebugging()).ToBool() && stats.m_bIsPC.ToBool())
        {
          CNWSCombatAttackData currentAttack = combatRound.GetAttack(combatRound.m_nCurrentAttack);
          StringBuilder debugMessage = new StringBuilder(currentAttack.m_sDamageDebugText.ToString());
          debugMessage.Append(" + ");
          debugMessage.Append(goodAimModifier);
          debugMessage.Append(" (Good Aim Feat)");

          currentAttack.m_sDamageDebugText = debugMessage.ToString().ToExoString();
        }
      }

      return attackMod;
    }

    private int OnGetDamageBonus(void* pStats, void* pCreature, int bOffHand)
    {
      int damageBonus = getDamageBonusHook.CallOriginal(pStats, pCreature, bOffHand);

      CNWSCreatureStats stats = CNWSCreatureStats.FromPointer(pStats);
      CNWSCreature creature = stats.m_pBaseCreature;
      bool offHand = bOffHand.ToBool();
      CNWSItem? weapon = GetEquippedWeapon(creature, offHand);

      uint baseItem = weapon != null ? weapon.m_nBaseItem : (uint)BaseItem.Gloves;
      bool hasApplicableFeat = false;
      bool applicableFeatExists = greaterWeaponSpecializationMap.TryGetValue(baseItem, out HashSet<ushort>? types);

      if (applicableFeatExists)
      {
        foreach (ushort feat in types!)
        {
          hasApplicableFeat = stats.HasFeat(feat).ToBool();
          if (hasApplicableFeat)
          {
            break;
          }
        }
      }

      if (applicableFeatExists && hasApplicableFeat)
      {
        damageBonus += GreaterWeaponSpecializationDamageBonus;
        if ((*NWNXLib.EnableCombatDebugging()).ToBool() && stats.m_bIsPC.ToBool())
        {
          CNWSCombatAttackData currentAttack = creature.m_pcCombatRound.GetAttack(creature.m_pcCombatRound.m_nCurrentAttack);
          StringBuilder debugMessage = new StringBuilder(currentAttack.m_sDamageDebugText.ToString());
          debugMessage.Append(" + ");

          if (currentAttack.m_nAttackResult == 3)
          {
            int criticalThreat = stats.GetCriticalHitMultiplier(bOffHand);
            debugMessage.Append(GreaterWeaponSpecializationDamageBonus * criticalThreat);
            debugMessage.Append(" (Greater Weapon Specialization Feat) (Critical x");
            debugMessage.Append(criticalThreat);
            debugMessage.Append(")");
          }
          else
          {
            debugMessage.Append(GreaterWeaponSpecializationDamageBonus);
            debugMessage.Append(" (Greater Weapon Specialization Feat) ");
          }

          currentAttack.m_sDamageDebugText = debugMessage.ToString().ToExoString();
        }
      }

      return damageBonus;
    }

    private int OnGetEpicWeaponDevastatingCritical(void* pStats, void* pWeapon)
    {
      CNWSCreatureStats stats = CNWSCreatureStats.FromPointer(pStats);
      CNWSItem weapon = CNWSItem.FromPointer(pWeapon);
      uint weaponType = weapon == null ? (uint)BaseItem.Gloves : CNWSItem.FromPointer(pWeapon).m_nBaseItem;

      bool hasApplicableFeat = false;
      bool applicableFeatExists = epicWeaponDevastatingCriticalMap.TryGetValue(weaponType, out HashSet<ushort>? types);

      if (applicableFeatExists)
      {
        foreach (ushort feat in types!)
        {
          hasApplicableFeat = stats.HasFeat(feat).ToBool();
          if (hasApplicableFeat)
          {
            break;
          }
        }
      }

      bool canUseFeat = applicableFeatExists && hasApplicableFeat || getEpicWeaponDevastatingCriticalHook.CallOriginal(pStats, pWeapon).ToBool();

      if (weapon != null && canUseFeat && OnDevastatingCriticalHit != null)
      {
        CNWSCreature creature = stats.m_pBaseCreature;
        CNWSCombatRound combatRound = creature.m_pcCombatRound;
        CNWSCombatAttackData attackData = combatRound.GetAttack(combatRound.m_nCurrentAttack);

        DevastatingCriticalData devastatingCriticalData = new DevastatingCriticalData
        {
          Attacker = creature.ToNwObject<NwCreature>()!,
          Weapon = weapon.ToNwObject<NwItem>()!,
          Target = creature.m_oidAttackTarget.ToNwObject<NwGameObject>()!,
          Damage = attackData.GetTotalDamage(1),
        };

        OnDevastatingCriticalHit(devastatingCriticalData);
        if (devastatingCriticalData.Bypass)
        {
          attackData.m_bKillingBlow = 0;
          return 0;
        }
      }

      return canUseFeat.ToInt();
    }

    private int OnGetEpicWeaponFocus(void* pStats, void* pWeapon)
    {
      CNWSCreatureStats stats = CNWSCreatureStats.FromPointer(pStats);
      uint weaponType = pWeapon == null ? (uint)BaseItem.Gloves : CNWSItem.FromPointer(pWeapon).m_nBaseItem;

      bool hasApplicableFeat = false;
      bool applicableFeatExists = epicWeaponFocusMap.TryGetValue(weaponType, out HashSet<ushort>? types);

      if (applicableFeatExists)
      {
        foreach (ushort feat in types!)
        {
          hasApplicableFeat = stats.HasFeat(feat).ToBool() || feat == (ushort)Feat.EpicWeaponFocus_Creature && stats.HasFeat((ushort)Feat.EpicWeaponFocus_Unarmed).ToBool();
          if (hasApplicableFeat)
          {
            break;
          }
        }
      }

      return applicableFeatExists && hasApplicableFeat ? 1 : getEpicWeaponFocusHook.CallOriginal(pStats, pWeapon);
    }

    private int OnGetEpicWeaponOverwhelmingCritical(void* pStats, void* pWeapon)
    {
      CNWSCreatureStats stats = CNWSCreatureStats.FromPointer(pStats);
      uint weaponType = pWeapon == null ? (uint)BaseItem.Gloves : CNWSItem.FromPointer(pWeapon).m_nBaseItem;

      bool hasApplicableFeat = false;
      bool applicableFeatExists = epicWeaponOverwhelmingCriticalMap.TryGetValue(weaponType, out HashSet<ushort>? types);

      if (applicableFeatExists)
      {
        foreach (ushort feat in types!)
        {
          hasApplicableFeat = stats.HasFeat(feat).ToBool();
          if (hasApplicableFeat)
          {
            break;
          }
        }
      }

      return applicableFeatExists && hasApplicableFeat ? 1 : getEpicWeaponOverwhelmingCriticalHook.CallOriginal(pStats, pWeapon);
    }

    private int OnGetEpicWeaponSpecialization(void* pStats, void* pWeapon)
    {
      CNWSCreatureStats stats = CNWSCreatureStats.FromPointer(pStats);
      uint weaponType = pWeapon == null ? (uint)BaseItem.Gloves : CNWSItem.FromPointer(pWeapon).m_nBaseItem;

      bool hasApplicableFeat = false;
      bool applicableFeatExists = epicWeaponSpecializationMap.TryGetValue(weaponType, out HashSet<ushort>? types);

      if (applicableFeatExists)
      {
        foreach (ushort feat in types!)
        {
          hasApplicableFeat = stats.HasFeat(feat).ToBool();
          if (hasApplicableFeat)
          {
            break;
          }
        }
      }

      return applicableFeatExists && hasApplicableFeat ? 1 : getEpicWeaponSpecializationHook.CallOriginal(pStats, pWeapon);
    }

    private int OnGetIsWeaponOfChoice(void* pStats, uint nBaseItem)
    {
      CNWSCreatureStats stats = CNWSCreatureStats.FromPointer(pStats);

      bool hasApplicableFeat = false;
      bool applicableFeatExists = weaponOfChoiceMap.TryGetValue(nBaseItem, out HashSet<ushort>? types);

      if (applicableFeatExists)
      {
        foreach (ushort feat in types!)
        {
          hasApplicableFeat = stats.HasFeat(feat).ToBool();
          if (hasApplicableFeat)
          {
            break;
          }
        }
      }

      return applicableFeatExists && hasApplicableFeat ? 1 : getIsWeaponOfChoiceHook.CallOriginal(pStats, nBaseItem);
    }

    private int OnGetMeleeAttackBonus(void* pStats, int bOffHand, int bIncludeBase, int bTouchAttack)
    {
      int attackBonus = getMeleeAttackBonusHook.CallOriginal(pStats, bOffHand, bIncludeBase, bTouchAttack);

      if (bTouchAttack.ToBool())
      {
        return attackBonus;
      }

      CNWSCreatureStats stats = CNWSCreatureStats.FromPointer(pStats);
      CNWSCreature creature = stats.m_pBaseCreature;
      bool offHand = bOffHand.ToBool();

      CNWSItem? weapon = GetEquippedWeapon(creature, offHand);
      uint baseItem = weapon != null ? weapon.m_nBaseItem : (uint)BaseItem.Gloves;

      bool hasApplicableFeat = false;
      bool applicableFeatExists = greaterWeaponFocusMap.TryGetValue(baseItem, out HashSet<ushort>? types);

      if (applicableFeatExists)
      {
        foreach (ushort feat in types!)
        {
          hasApplicableFeat = stats.HasFeat(feat).ToBool();
          if (hasApplicableFeat)
          {
            break;
          }
        }
      }

      if (applicableFeatExists && hasApplicableFeat)
      {
        return attackBonus + GreaterWeaponFocusAttackBonus;
      }

      return attackBonus;
    }

    private int OnGetMeleeDamageBonus(void* pStats, int bOffHand, byte nCreatureWeaponIndex)
    {
      int damageBonus = getMeleeDamageBonusHook.CallOriginal(pStats, bOffHand, nCreatureWeaponIndex);

      CNWSCreatureStats stats = CNWSCreatureStats.FromPointer(pStats);
      CNWSCreature creature = stats.m_pBaseCreature;
      bool offHand = bOffHand.ToBool();

      CNWSItem? weapon = null;
      if (nCreatureWeaponIndex == 255)
      {
        weapon = GetEquippedWeapon(creature, offHand);
      }

      uint baseItem = weapon != null ? weapon.m_nBaseItem : (uint)BaseItem.Gloves;
      bool hasApplicableFeat = false;
      bool applicableFeatExists = greaterWeaponSpecializationMap.TryGetValue(baseItem, out HashSet<ushort>? types);

      if (applicableFeatExists)
      {
        foreach (ushort feat in types!)
        {
          hasApplicableFeat = stats.HasFeat(feat).ToBool();
          if (hasApplicableFeat)
          {
            break;
          }
        }
      }

      if (applicableFeatExists && hasApplicableFeat)
      {
        return damageBonus + GreaterWeaponSpecializationDamageBonus;
      }

      return damageBonus;
    }

    private int OnGetRangedAttackBonus(void* pStats, int bIncludeBase, int bTouchAttack)
    {
      int attackBonus = getRangedAttackBonusHook.CallOriginal(pStats, bIncludeBase, bTouchAttack);

      if (bTouchAttack.ToBool())
      {
        return attackBonus;
      }

      CNWSCreatureStats stats = CNWSCreatureStats.FromPointer(pStats);
      CNWSItem weapon = stats.m_pBaseCreature.m_pInventory.GetItemInSlot((uint)EquipmentSlot.RightHand);

      if (weapon == null)
      {
        return attackBonus;
      }

      uint baseItem = weapon.m_nBaseItem;
      bool hasApplicableFeat = false;
      bool applicableFeatExists = greaterWeaponFocusMap.TryGetValue(baseItem, out HashSet<ushort>? types);

      if (applicableFeatExists)
      {
        foreach (ushort feat in types!)
        {
          hasApplicableFeat = stats.HasFeat(feat).ToBool();
          if (hasApplicableFeat)
          {
            break;
          }
        }
      }

      if (applicableFeatExists && hasApplicableFeat)
      {
        attackBonus += GreaterWeaponFocusAttackBonus;
      }

      if (EnableSlingGoodAimFeat && baseItem == (uint)BaseItem.Sling && stats.m_nRace != (ushort)RacialType.Halfling && stats.HasFeat((ushort)Feat.GoodAim).ToBool())
      {
        attackBonus += 1;
      }

      return attackBonus;
    }

    private int OnGetRangedDamageBonus(void* pStats)
    {
      int damageBonus = getRangedDamageBonusHook.CallOriginal(pStats);

      CNWSCreatureStats stats = CNWSCreatureStats.FromPointer(pStats);
      CNWSItem weapon = stats.m_pBaseCreature.m_pInventory.GetItemInSlot((uint)EquipmentSlot.RightHand);

      if (weapon == null)
      {
        return damageBonus;
      }

      uint baseItem = weapon.m_nBaseItem;
      bool hasApplicableFeat = false;
      bool applicableFeatExists = greaterWeaponSpecializationMap.TryGetValue(baseItem, out HashSet<ushort>? types);

      if (applicableFeatExists)
      {
        foreach (ushort feat in types!)
        {
          hasApplicableFeat = stats.HasFeat(feat).ToBool();
          if (hasApplicableFeat)
          {
            break;
          }
        }
      }

      if (applicableFeatExists && hasApplicableFeat)
      {
        return damageBonus + GreaterWeaponSpecializationDamageBonus;
      }

      return damageBonus;
    }

    private int OnGetUseMonkAttackTables(void* pStats, int bForceUnarmed)
    {
      CNWSCreatureStats stats = CNWSCreatureStats.FromPointer(pStats);
      CNWSCreature creature = stats.m_pBaseCreature;

      int monkLevels = GetLevelByClass(stats, (uint)ClassType.Monk);

      if (monkLevels < 1 || !creature.GetUseMonkAbilities().ToBool())
      {
        return false.ToInt();
      }

      CNWSItem mainWeapon = creature.m_pInventory.GetItemInSlot((uint)EquipmentSlot.RightHand);
      if (mainWeapon == null)
      {
        return true.ToInt();
      }

      if (bForceUnarmed.ToBool())
      {
        return false.ToInt();
      }

      uint mainWeaponType = mainWeapon.m_nBaseItem;
      if (mainWeaponType != (uint)BaseItem.Kama && !monkWeaponSet.Contains(mainWeapon.m_nBaseItem))
      {
        return false.ToInt();
      }

      CNWSItem secondWeapon = creature.m_pInventory.GetItemInSlot((uint)EquipmentSlot.LeftHand);
      if (secondWeapon == null)
      {
        return true.ToInt();
      }

      uint secondWeaponType = secondWeapon.m_nBaseItem;
      return (secondWeaponType is (uint)BaseItem.Kama or (uint)BaseItem.Torch || monkWeaponSet.Contains(secondWeaponType)).ToInt();
    }

    private int OnGetWeaponFinesse(void* pStats, void* pWeapon)
    {
      if (pStats == null)
      {
        return false.ToInt();
      }

      CNWSCreatureStats creatureStats = CNWSCreatureStats.FromPointer(pStats);
      if (!creatureStats.HasFeat((ushort)Feat.WeaponFinesse).ToBool())
      {
        return 0;
      }

      return IsWeaponLight(creatureStats, CNWSItem.FromPointer(pWeapon), true).ToInt();
    }

    private int OnGetWeaponFocus(void* pStats, void* pWeapon)
    {
      CNWSCreatureStats stats = CNWSCreatureStats.FromPointer(pStats);
      uint weaponType = pWeapon == null ? (uint)BaseItem.Gloves : CNWSItem.FromPointer(pWeapon).m_nBaseItem;

      bool hasApplicableFeat = false;
      bool applicableFeatExists = weaponFocusMap.TryGetValue(weaponType, out HashSet<ushort>? types);

      if (applicableFeatExists)
      {
        foreach (ushort feat in types!)
        {
          hasApplicableFeat = stats.HasFeat(feat).ToBool() || feat == (ushort)Feat.WeaponFocus_Creature && stats.HasFeat((ushort)Feat.WeaponFocus_UnarmedStrike).ToBool();
          if (hasApplicableFeat)
          {
            break;
          }
        }
      }

      return applicableFeatExists && hasApplicableFeat ? 1 : getWeaponFocusHook.CallOriginal(pStats, pWeapon);
    }

    private int OnGetWeaponImprovedCritical(void* pStats, void* pWeapon)
    {
      CNWSCreatureStats stats = CNWSCreatureStats.FromPointer(pStats);
      uint weaponType = pWeapon == null ? (uint)BaseItem.Gloves : CNWSItem.FromPointer(pWeapon).m_nBaseItem;

      bool hasApplicableFeat = false;
      bool applicableFeatExists = weaponImprovedCriticalMap.TryGetValue(weaponType, out HashSet<ushort>? types);

      if (applicableFeatExists)
      {
        foreach (ushort feat in types!)
        {
          hasApplicableFeat = stats.HasFeat(feat).ToBool();
          if (hasApplicableFeat)
          {
            break;
          }
        }
      }

      return applicableFeatExists && hasApplicableFeat ? 1 : getWeaponImprovedCriticalHook.CallOriginal(pStats, pWeapon);
    }

    private int OnGetWeaponSpecialization(void* pStats, void* pWeapon)
    {
      CNWSCreatureStats stats = CNWSCreatureStats.FromPointer(pStats);
      uint weaponType = pWeapon == null ? (uint)BaseItem.Gloves : CNWSItem.FromPointer(pWeapon).m_nBaseItem;

      bool hasApplicableFeat = false;
      bool applicableFeatExists = weaponSpecializationMap.TryGetValue(weaponType, out HashSet<ushort>? types);

      if (applicableFeatExists)
      {
        foreach (ushort feat in types!)
        {
          hasApplicableFeat = stats.HasFeat(feat).ToBool();
          if (hasApplicableFeat)
          {
            break;
          }
        }
      }

      return applicableFeatExists && hasApplicableFeat ? 1 : getWeaponSpecializationHook.CallOriginal(pStats, pWeapon);
    }

    private float OnMaxAttackRange(void* pCreature, uint oidTarget, int bBaseValue, int bPassiveRange)
    {
      CNWSCreature creature = CNWSCreature.FromPointer(pCreature);
      CNWSItem equippedItem = creature.m_pInventory.GetItemInSlot((uint)EquipmentSlot.RightHand);
      if (equippedItem != null)
      {
        uint baseItemType = equippedItem.m_nBaseItem;
        CNWBaseItem baseItem = NWNXLib.Rules().m_pBaseItemArray.GetBaseItem((int)baseItemType);
        if (baseItem != null && baseItem.m_nWeaponRanged > 0 && maxRangedAttackDistanceOverrideMap.TryGetValue(baseItemType, out MaxRangedAttackDistanceOverride distanceOverride))
        {
          return bPassiveRange.ToBool() ? distanceOverride.MaxRangedPassiveAttackDistance : distanceOverride.MaxRangedAttackDistance;
        }
      }

      return creature.DesiredAttackRange(oidTarget, bBaseValue) + 1.5f;
    }
  }
}
