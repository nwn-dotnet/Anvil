using System;
using System.Collections.Generic;
using System.Text;
using Anvil.API;
using NWN.API;
using NWN.API.Events;
using NWN.Native.API;
using ClassType = NWN.Native.API.ClassType;
using CombatMode = Anvil.API.CombatMode;
using CreatureSize = NWN.Native.API.CreatureSize;
using Feat = NWN.Native.API.Feat;
using RacialType = NWN.Native.API.RacialType;

namespace Anvil.Services
{
  [ServiceBinding(typeof(WeaponService))]
  [ServiceBindingOptions(Lazy = true)]
  public sealed unsafe class WeaponService : IDisposable
  {
    private readonly HookService hookService;
    private readonly EventService eventService;

    private delegate int GetWeaponFocusHook(void* pStats, void* pWeapon);

    private delegate int GetEpicWeaponFocusHook(void* pStats, void* pWeapon);

    private delegate int GetWeaponFinesseHook(void* pStats, void* pWeapon);

    private delegate int GetWeaponImprovedCriticalHook(void* pStats, void* pWeapon);

    private delegate int GetEpicWeaponOverwhelmingCriticalHook(void* pStats, void* pWeapon);

    private delegate int GetEpicWeaponDevastatingCriticalHook(void* pStats, void* pWeapon);

    private delegate int GetWeaponSpecializationHook(void* pStats, void* pWeapon);

    private delegate int GetEpicWeaponSpecializationHook(void* pStats, void* pWeapon);

    private delegate int GetIsWeaponOfChoiceHook(void* pStats, uint nBaseItem);

    private delegate int GetDamageBonusHook(void* pStats, void* pCreature, int bOffHand);

    private delegate int GetMeleeDamageBonusHook(void* pStats, int bOffHand, byte nCreatureWeaponIndex);

    private delegate int GetRangedDamageBonusHook(void* pStats);

    private delegate int GetMeleeAttackBonusHook(void* pStats, int bOffHand, int bIncludeBase, int bTouchAttack);

    private delegate int GetRangedAttackBonusHook(void* pStats, int bIncludeBase, int bTouchAttack);

    private delegate int GetAttackModifierVersusHook(void* pStats, void* pCreature);

    private delegate int GetUseMonkAttackTablesHook(void* pStats, int bForceUnarmed);

    private delegate float MaxAttackRangeHook(void* pCreature, uint oidTarget, int bBaseValue, int bPassiveRange);

    private readonly FunctionHook<GetWeaponFocusHook> getWeaponFocusHook;
    private readonly FunctionHook<GetEpicWeaponFocusHook> getEpicWeaponFocusHook;
    private readonly FunctionHook<GetWeaponFinesseHook> getWeaponFinesseHook;
    private readonly FunctionHook<GetWeaponImprovedCriticalHook> getWeaponImprovedCriticalHook;
    private readonly FunctionHook<GetEpicWeaponOverwhelmingCriticalHook> getEpicWeaponOverwhelmingCriticalHook;
    private readonly FunctionHook<GetEpicWeaponDevastatingCriticalHook> getEpicWeaponDevastatingCriticalHook;
    private readonly FunctionHook<GetWeaponSpecializationHook> getWeaponSpecializationHook;
    private readonly FunctionHook<GetEpicWeaponSpecializationHook> getEpicWeaponSpecializationHook;
    private readonly FunctionHook<GetIsWeaponOfChoiceHook> getIsWeaponOfChoiceHook;
    private readonly FunctionHook<GetDamageBonusHook> getDamageBonusHook;
    private readonly FunctionHook<GetMeleeDamageBonusHook> getMeleeDamageBonusHook;
    private readonly FunctionHook<GetRangedDamageBonusHook> getRangedDamageBonusHook;
    private readonly FunctionHook<GetMeleeAttackBonusHook> getMeleeAttackBonusHook;
    private readonly FunctionHook<GetRangedAttackBonusHook> getRangedAttackBonusHook;
    private readonly FunctionHook<GetAttackModifierVersusHook> getAttackModifierVersusHook;
    private readonly FunctionHook<GetUseMonkAttackTablesHook> getUseMonkAttackTablesHook;

    private readonly Dictionary<uint, HashSet<ushort>> weaponFocusMap = new Dictionary<uint, HashSet<ushort>>();
    private readonly Dictionary<uint, HashSet<ushort>> epicWeaponFocusMap = new Dictionary<uint, HashSet<ushort>>();
    private readonly Dictionary<uint, byte> weaponFinesseSizeMap = new Dictionary<uint, byte>();
    private readonly Dictionary<uint, HashSet<ushort>> weaponImprovedCriticalMap = new Dictionary<uint, HashSet<ushort>>();
    private readonly Dictionary<uint, HashSet<ushort>> weaponSpecializationMap = new Dictionary<uint, HashSet<ushort>>();
    private readonly Dictionary<uint, HashSet<ushort>> epicWeaponSpecializationMap = new Dictionary<uint, HashSet<ushort>>();
    private readonly Dictionary<uint, HashSet<ushort>> epicWeaponOverwhelmingCriticalMap = new Dictionary<uint, HashSet<ushort>>();
    private readonly Dictionary<uint, HashSet<ushort>> epicWeaponDevastatingCriticalMap = new Dictionary<uint, HashSet<ushort>>();
    private readonly Dictionary<uint, HashSet<ushort>> weaponOfChoiceMap = new Dictionary<uint, HashSet<ushort>>();
    private readonly Dictionary<uint, HashSet<ushort>> greaterWeaponSpecializationMap = new Dictionary<uint, HashSet<ushort>>();
    private readonly Dictionary<uint, HashSet<ushort>> greaterWeaponFocusMap = new Dictionary<uint, HashSet<ushort>>();

    private readonly HashSet<uint> weaponUnarmedSet = new HashSet<uint>();
    private readonly HashSet<uint> monkWeaponSet = new HashSet<uint>();

    private readonly Dictionary<uint, MaxRangedAttackDistanceOverride> maxRangedAttackDistanceOverrideMap = new Dictionary<uint, MaxRangedAttackDistanceOverride>();

    private FunctionHook<MaxAttackRangeHook> maxAttackRangeHook;
    private bool combatModeEventSubscribed;

    public WeaponService(HookService hookService, EventService eventService)
    {
      this.hookService = hookService;
      this.eventService = eventService;

      getWeaponFocusHook = hookService.RequestHook<GetWeaponFocusHook>(OnGetWeaponFocus, FunctionsLinux._ZN17CNWSCreatureStats14GetWeaponFocusEP8CNWSItem, HookOrder.Late);
      getEpicWeaponFocusHook = hookService.RequestHook<GetEpicWeaponFocusHook>(OnGetEpicWeaponFocus, FunctionsLinux._ZN17CNWSCreatureStats18GetEpicWeaponFocusEP8CNWSItem, HookOrder.Late);
      getWeaponFinesseHook = hookService.RequestHook<GetWeaponFinesseHook>(OnGetWeaponFinesse, FunctionsLinux._ZN17CNWSCreatureStats16GetWeaponFinesseEP8CNWSItem, HookOrder.Final);
      getWeaponImprovedCriticalHook = hookService.RequestHook<GetWeaponImprovedCriticalHook>(OnGetWeaponImprovedCritical, FunctionsLinux._ZN17CNWSCreatureStats25GetWeaponImprovedCriticalEP8CNWSItem, HookOrder.Late);
      getEpicWeaponOverwhelmingCriticalHook = hookService.RequestHook<GetEpicWeaponOverwhelmingCriticalHook>(OnGetEpicWeaponOverwhelmingCritical, FunctionsLinux._ZN17CNWSCreatureStats33GetEpicWeaponOverwhelmingCriticalEP8CNWSItem, HookOrder.Late);
      getEpicWeaponDevastatingCriticalHook = hookService.RequestHook<GetEpicWeaponDevastatingCriticalHook>(OnGetEpicWeaponDevastatingCritical, FunctionsLinux._ZN17CNWSCreatureStats32GetEpicWeaponDevastatingCriticalEP8CNWSItem, HookOrder.Late);
      getWeaponSpecializationHook = hookService.RequestHook<GetWeaponSpecializationHook>(OnGetWeaponSpecialization, FunctionsLinux._ZN17CNWSCreatureStats23GetWeaponSpecializationEP8CNWSItem, HookOrder.Late);
      getEpicWeaponSpecializationHook = hookService.RequestHook<GetEpicWeaponSpecializationHook>(OnGetEpicWeaponSpecialization, FunctionsLinux._ZN17CNWSCreatureStats27GetEpicWeaponSpecializationEP8CNWSItem, HookOrder.Late);
      getIsWeaponOfChoiceHook = hookService.RequestHook<GetIsWeaponOfChoiceHook>(OnGetIsWeaponOfChoice, FunctionsLinux._ZN17CNWSCreatureStats19GetIsWeaponOfChoiceEj, HookOrder.Late);
      getDamageBonusHook = hookService.RequestHook<GetDamageBonusHook>(OnGetDamageBonus, FunctionsLinux._ZN17CNWSCreatureStats14GetDamageBonusEP12CNWSCreaturei, HookOrder.Late);
      getMeleeDamageBonusHook = hookService.RequestHook<GetMeleeDamageBonusHook>(OnGetMeleeDamageBonus, FunctionsLinux._ZN17CNWSCreatureStats19GetMeleeDamageBonusEih, HookOrder.Late);
      getRangedDamageBonusHook = hookService.RequestHook<GetRangedDamageBonusHook>(OnGetRangedDamageBonus, FunctionsLinux._ZN17CNWSCreatureStats20GetRangedDamageBonusEv, HookOrder.Late);
      getMeleeAttackBonusHook = hookService.RequestHook<GetMeleeAttackBonusHook>(OnGetMeleeAttackBonus, FunctionsLinux._ZN17CNWSCreatureStats19GetMeleeAttackBonusEiii, HookOrder.Late);
      getRangedAttackBonusHook = hookService.RequestHook<GetRangedAttackBonusHook>(OnGetRangedAttackBonus, FunctionsLinux._ZN17CNWSCreatureStats20GetRangedAttackBonusEii, HookOrder.Late);
      getAttackModifierVersusHook = hookService.RequestHook<GetAttackModifierVersusHook>(OnGetAttackModifierVersus, FunctionsLinux._ZN17CNWSCreatureStats23GetAttackModifierVersusEP12CNWSCreature, HookOrder.Late);
      getUseMonkAttackTablesHook = hookService.RequestHook<GetUseMonkAttackTablesHook>(OnGetUseMonkAttackTables, FunctionsLinux._ZN17CNWSCreatureStats22GetUseMonkAttackTablesEi, HookOrder.Final);

      weaponFinesseSizeMap[(uint)BaseItem.Rapier] = (byte)CreatureSize.Medium;
    }

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
    /// Gets or sets whether the "Good Aim" feat should also apply to slings.
    /// </summary>
    public bool EnableSlingGoodAimFeat { get; set; } = false;

    /// <summary>
    /// Called when an attack results in a devastating critical hit. Subscribe and modify the event data to implement custom behaviours.
    /// </summary>
    public event Action<DevastatingCriticalData> OnDevastatingCriticalHit;

    /// <summary>
    /// Adds the specified feat as a weapon focus feat for the specified base item type.
    /// </summary>
    /// <param name="baseItem">The base item type to be mapped.</param>
    /// <param name="feat">The feat to map to the base item.</param>
    public void AddWeaponFocusFeat(BaseItemType baseItem, API.Feat feat)
    {
      weaponFocusMap.AddElement((uint)baseItem, (ushort)feat);
    }

    /// <summary>
    /// Adds the specified feat as an epic weapon focus feat for the specified base item type.
    /// </summary>
    /// <param name="baseItem">The base item type to be mapped.</param>
    /// <param name="feat">The feat to map to the base item.</param>
    public void AddEpicWeaponFocusFeat(BaseItemType baseItem, API.Feat feat)
    {
      epicWeaponFocusMap.AddElement((uint)baseItem, (ushort)feat);
    }

    /// <summary>
    /// Adds the specified feat as an improved critical feat for the specified base item type.
    /// </summary>
    /// <param name="baseItem">The base item type to be mapped.</param>
    /// <param name="feat">The feat to map to the base item.</param>
    public void AddWeaponImprovedCriticalFeat(BaseItemType baseItem, API.Feat feat)
    {
      weaponImprovedCriticalMap.AddElement((uint)baseItem, (ushort)feat);
    }

    /// <summary>
    /// Adds the specified feat as a weapon specialization feat for the specified base item type.
    /// </summary>
    /// <param name="baseItem">The base item type to be mapped.</param>
    /// <param name="feat">The feat to map to the base item.</param>
    public void AddWeaponSpecializationFeat(BaseItemType baseItem, API.Feat feat)
    {
      weaponSpecializationMap.AddElement((uint)baseItem, (ushort)feat);
    }

    /// <summary>
    /// Adds the specified feat as an epic weapon specialization feat for the specified base item type.
    /// </summary>
    /// <param name="baseItem">The base item type to be mapped.</param>
    /// <param name="feat">The feat to map to the base item.</param>
    public void AddEpicWeaponSpecializationFeat(BaseItemType baseItem, API.Feat feat)
    {
      epicWeaponSpecializationMap.AddElement((uint)baseItem, (ushort)feat);
    }

    /// <summary>
    /// Adds the specified feat as an epic overwhelming critical feat for the specified base item type.
    /// </summary>
    /// <param name="baseItem">The base item type to be mapped.</param>
    /// <param name="feat">The feat to map to the base item.</param>
    public void AddEpicWeaponOverwhelmingCriticalFeat(BaseItemType baseItem, API.Feat feat)
    {
      epicWeaponOverwhelmingCriticalMap.AddElement((uint)baseItem, (ushort)feat);
    }

    /// <summary>
    /// Adds the specified feat as a devastating critical feat for the specified base item type.
    /// </summary>
    /// <param name="baseItem">The base item type to be mapped.</param>
    /// <param name="feat">The feat to map to the base item.</param>
    public void AddEpicWeaponDevastatingCriticalFeat(BaseItemType baseItem, API.Feat feat)
    {
      epicWeaponDevastatingCriticalMap.AddElement((uint)baseItem, (ushort)feat);
    }

    /// <summary>
    /// Adds the specified feat as a weapon of choice feat for the specified base item type.
    /// </summary>
    /// <param name="baseItem">The base item type to be mapped.</param>
    /// <param name="feat">The feat to map to the base item.</param>
    public void AddWeaponOfChoiceFeat(BaseItemType baseItem, API.Feat feat)
    {
      weaponOfChoiceMap.AddElement((uint)baseItem, (ushort)feat);
    }

    /// <summary>
    /// Adds the specified feat as a greater weapon focus feat for the specified base item type.<br/>
    /// This adds the <see cref="GreaterWeaponFocusAttackBonus"/> to the weapon's attack roll for characters with the specified feat.
    /// </summary>
    /// <param name="baseItem">The base item type to be mapped.</param>
    /// <param name="feat">The feat to map to the base item.</param>
    public void AddGreaterWeaponFocusFeat(BaseItemType baseItem, API.Feat feat)
    {
      greaterWeaponFocusMap.AddElement((uint)baseItem, (ushort)feat);
    }

    /// <summary>
    /// Adds the specified feat as a greater weapon specialization feat for the specified base item type.<br/>
    /// This adds the <see cref="GreaterWeaponSpecializationDamageBonus"/> to the weapon's damage roll for characters with the specified feat.
    /// </summary>
    /// <param name="baseItem">The base item type to be mapped.</param>
    /// <param name="feat">The feat to map to the base item.</param>
    public void AddGreaterWeaponSpecializationFeat(BaseItemType baseItem, API.Feat feat)
    {
      greaterWeaponSpecializationMap.AddElement((uint)baseItem, (ushort)feat);
    }

    /// <summary>
    /// Gets the required creature size needed for the specified base item type to be finessable. This function only returns values assigned in <see cref="SetWeaponFinesseSize"/>.
    /// </summary>
    /// <param name="baseItem">The base item type to query.</param>
    /// <returns>The size of the creature needed to consider this weapon finessable.</returns>
    public API.CreatureSize GetWeaponFinesseSize(BaseItemType baseItem)
    {
      return weaponFinesseSizeMap.TryGetValue((uint)baseItem, out byte size) ? (API.CreatureSize)size : API.CreatureSize.Invalid;
    }

    /// <summary>
    /// Sets the required creature size needed for the specified base item type to be finessable.
    /// </summary>
    /// <param name="baseItem">The base item type to be mapped.</param>
    /// <param name="size">The size of the creature needed to consider this weapon finessable.</param>
    public void SetWeaponFinesseSize(BaseItemType baseItem, API.CreatureSize size)
    {
      weaponFinesseSizeMap[(uint)baseItem] = (byte)size;
    }

    /// <summary>
    /// Sets the specified weapon base item to be considered as unarmed for the weapon finesse feat.
    /// </summary>
    /// <param name="baseItem">The base item type to be considered unarmed.</param>
    public void SetWeaponUnarmed(BaseItemType baseItem)
    {
      weaponUnarmedSet.Add((uint)baseItem);
    }

    /// <summary>
    /// Sets the specified weapon base item to be considered a monk weapon.
    /// </summary>
    /// <param name="baseItem">The base item type to be considered a monk weapon.</param>
    public void SetWeaponIsMonkWeapon(BaseItemType baseItem)
    {
      monkWeaponSet.Add((uint)baseItem);

      if (!combatModeEventSubscribed)
      {
        eventService.SubscribeAll<OnCombatModeToggle, OnCombatModeToggle.Factory>(OnCombatModeToggle);
        combatModeEventSubscribed = true;
      }
    }

    /// <summary>
    /// Overrides the max attack distance of ranged weapons.
    /// </summary>
    /// <param name="baseItem">The base item type.</param>
    /// <param name="max">The maximum attack distance. Default is 40.0f.</param>
    /// <param name="maxPassive">The maximum passive attack distance. Default is 20.0f. Seems to be used by the engine to determine a new nearby target when needed.</param>
    /// <param name="preferred">The preferred attack distance. See the PrefAttackDist column in baseitems.2da, default seems to be 30.0f for ranged weapons.</param>
    /// <remarks>maxPassive should probably be lower than max, half of max seems to be a good start. preferred should be at least ~0.5f lower than max.</remarks>
    public void SetMaxRangedAttackDistanceOverride(BaseItemType baseItem, float max, float maxPassive, float preferred)
    {
      CNWBaseItem baseItemData = NWNXLib.Rules().m_pBaseItemArray.GetBaseItem((int)baseItem);
      if (baseItemData == null || baseItemData.m_nWeaponRanged <= 0)
      {
        return;
      }

      baseItemData.m_fPreferredAttackDist = preferred;

      MaxRangedAttackDistanceOverride overrideData;
      overrideData.MaxRangedAttackDistance = max;
      overrideData.MaxRangedPassiveAttackDistance = maxPassive;

      maxRangedAttackDistanceOverrideMap[(uint)baseItem] = overrideData;
      maxAttackRangeHook ??= hookService.RequestHook<MaxAttackRangeHook>(OnMaxAttackRange, FunctionsLinux._ZN12CNWSCreature14MaxAttackRangeEjii, HookOrder.Final);
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

    void IDisposable.Dispose()
    {
      getWeaponFocusHook?.Dispose();
      getEpicWeaponFocusHook?.Dispose();
      getWeaponFinesseHook?.Dispose();
      getWeaponImprovedCriticalHook?.Dispose();
      getEpicWeaponOverwhelmingCriticalHook?.Dispose();
      getEpicWeaponDevastatingCriticalHook?.Dispose();
      getWeaponSpecializationHook?.Dispose();
      getEpicWeaponSpecializationHook?.Dispose();
      getIsWeaponOfChoiceHook?.Dispose();
      getDamageBonusHook?.Dispose();
      getMeleeDamageBonusHook?.Dispose();
      getRangedDamageBonusHook?.Dispose();
      getMeleeAttackBonusHook?.Dispose();
      getRangedAttackBonusHook?.Dispose();
      getAttackModifierVersusHook?.Dispose();
      getUseMonkAttackTablesHook?.Dispose();
      maxAttackRangeHook?.Dispose();
    }

    private int OnGetWeaponFocus(void* pStats, void* pWeapon)
    {
      CNWSCreatureStats stats = CNWSCreatureStats.FromPointer(pStats);
      uint weaponType = pWeapon == null ? (uint)BaseItem.Gloves : CNWSItem.FromPointer(pWeapon).m_nBaseItem;

      bool hasApplicableFeat = false;
      bool applicableFeatExists = weaponFocusMap.TryGetValue(weaponType, out HashSet<ushort> types);

      if (applicableFeatExists)
      {
        foreach (ushort feat in types)
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

    private int OnGetEpicWeaponFocus(void* pStats, void* pWeapon)
    {
      CNWSCreatureStats stats = CNWSCreatureStats.FromPointer(pStats);
      uint weaponType = pWeapon == null ? (uint)BaseItem.Gloves : CNWSItem.FromPointer(pWeapon).m_nBaseItem;

      bool hasApplicableFeat = false;
      bool applicableFeatExists = epicWeaponFocusMap.TryGetValue(weaponType, out HashSet<ushort> types);

      if (applicableFeatExists)
      {
        foreach (ushort feat in types)
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

    private int OnGetWeaponImprovedCritical(void* pStats, void* pWeapon)
    {
      CNWSCreatureStats stats = CNWSCreatureStats.FromPointer(pStats);
      uint weaponType = pWeapon == null ? (uint)BaseItem.Gloves : CNWSItem.FromPointer(pWeapon).m_nBaseItem;

      bool hasApplicableFeat = false;
      bool applicableFeatExists = weaponImprovedCriticalMap.TryGetValue(weaponType, out HashSet<ushort> types);

      if (applicableFeatExists)
      {
        foreach (ushort feat in types)
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

    private int OnGetEpicWeaponOverwhelmingCritical(void* pStats, void* pWeapon)
    {
      CNWSCreatureStats stats = CNWSCreatureStats.FromPointer(pStats);
      uint weaponType = pWeapon == null ? (uint)BaseItem.Gloves : CNWSItem.FromPointer(pWeapon).m_nBaseItem;

      bool hasApplicableFeat = false;
      bool applicableFeatExists = epicWeaponOverwhelmingCriticalMap.TryGetValue(weaponType, out HashSet<ushort> types);

      if (applicableFeatExists)
      {
        foreach (ushort feat in types)
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

    private int OnGetEpicWeaponDevastatingCritical(void* pStats, void* pWeapon)
    {
      CNWSCreatureStats stats = CNWSCreatureStats.FromPointer(pStats);
      CNWSItem weapon = CNWSItem.FromPointer(pWeapon);
      uint weaponType = weapon == null ? (uint)BaseItem.Gloves : CNWSItem.FromPointer(pWeapon).m_nBaseItem;

      bool hasApplicableFeat = false;
      bool applicableFeatExists = epicWeaponDevastatingCriticalMap.TryGetValue(weaponType, out HashSet<ushort> types);

      if (applicableFeatExists)
      {
        foreach (ushort feat in types)
        {
          hasApplicableFeat = stats.HasFeat(feat).ToBool();
          if (hasApplicableFeat)
          {
            break;
          }
        }
      }

      bool canUseFeat = applicableFeatExists && hasApplicableFeat || getEpicWeaponDevastatingCriticalHook.CallOriginal(pStats, pWeapon).ToBool();

      if (canUseFeat && OnDevastatingCriticalHit != null)
      {
        CNWSCreature creature = stats.m_pBaseCreature;
        CNWSCombatRound combatRound = creature.m_pcCombatRound;
        CNWSCombatAttackData attackData = combatRound.GetAttack(combatRound.m_nCurrentAttack);

        DevastatingCriticalData devastatingCriticalData = new DevastatingCriticalData
        {
          Weapon = weapon.ToNwObject<NwItem>(),
          Target = creature.m_oidAttackTarget.ToNwObject<NwGameObject>(),
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

    private int OnGetWeaponSpecialization(void* pStats, void* pWeapon)
    {
      CNWSCreatureStats stats = CNWSCreatureStats.FromPointer(pStats);
      uint weaponType = pWeapon == null ? (uint)BaseItem.Gloves : CNWSItem.FromPointer(pWeapon).m_nBaseItem;

      bool hasApplicableFeat = false;
      bool applicableFeatExists = weaponSpecializationMap.TryGetValue(weaponType, out HashSet<ushort> types);

      if (applicableFeatExists)
      {
        foreach (ushort feat in types)
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

    private int OnGetEpicWeaponSpecialization(void* pStats, void* pWeapon)
    {
      CNWSCreatureStats stats = CNWSCreatureStats.FromPointer(pStats);
      uint weaponType = pWeapon == null ? (uint)BaseItem.Gloves : CNWSItem.FromPointer(pWeapon).m_nBaseItem;

      bool hasApplicableFeat = false;
      bool applicableFeatExists = epicWeaponSpecializationMap.TryGetValue(weaponType, out HashSet<ushort> types);

      if (applicableFeatExists)
      {
        foreach (ushort feat in types)
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
      bool applicableFeatExists = weaponOfChoiceMap.TryGetValue(nBaseItem, out HashSet<ushort> types);

      if (applicableFeatExists)
      {
        foreach (ushort feat in types)
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

    private int OnGetDamageBonus(void* pStats, void* pCreature, int bOffHand)
    {
      int damageBonus = getDamageBonusHook.CallOriginal(pStats, pCreature, bOffHand);

      CNWSCreatureStats stats = CNWSCreatureStats.FromPointer(pStats);
      CNWSCreature creature = stats.m_pBaseCreature;
      bool offHand = bOffHand.ToBool();
      CNWSItem weapon = GetEquippedWeapon(creature, offHand);

      uint baseItem = weapon != null ? weapon.m_nBaseItem : (uint)BaseItem.Gloves;
      bool hasApplicableFeat = false;
      bool applicableFeatExists = greaterWeaponSpecializationMap.TryGetValue(baseItem, out HashSet<ushort> types);

      if (applicableFeatExists)
      {
        foreach (ushort feat in types)
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

    private int OnGetMeleeDamageBonus(void* pStats, int bOffHand, byte nCreatureWeaponIndex)
    {
      int damageBonus = getMeleeDamageBonusHook.CallOriginal(pStats, bOffHand, nCreatureWeaponIndex);

      CNWSCreatureStats stats = CNWSCreatureStats.FromPointer(pStats);
      CNWSCreature creature = stats.m_pBaseCreature;
      bool offHand = bOffHand.ToBool();

      CNWSItem weapon = null;

      if (nCreatureWeaponIndex == 255)
      {
        weapon = GetEquippedWeapon(creature, offHand);
      }

      uint baseItem = weapon != null ? weapon.m_nBaseItem : (uint)BaseItem.Gloves;
      bool hasApplicableFeat = false;
      bool applicableFeatExists = greaterWeaponSpecializationMap.TryGetValue(baseItem, out HashSet<ushort> types);

      if (applicableFeatExists)
      {
        foreach (ushort feat in types)
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
      bool applicableFeatExists = greaterWeaponSpecializationMap.TryGetValue(baseItem, out HashSet<ushort> types);

      if (applicableFeatExists)
      {
        foreach (ushort feat in types)
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

      CNWSItem weapon = GetEquippedWeapon(creature, offHand);
      uint baseItem = weapon != null ? weapon.m_nBaseItem : (uint)BaseItem.Gloves;

      bool hasApplicableFeat = false;
      bool applicableFeatExists = greaterWeaponFocusMap.TryGetValue(baseItem, out HashSet<ushort> types);

      if (applicableFeatExists)
      {
        foreach (ushort feat in types)
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
      bool applicableFeatExists = greaterWeaponFocusMap.TryGetValue(baseItem, out HashSet<ushort> types);

      if (applicableFeatExists)
      {
        foreach (ushort feat in types)
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
      bool applicableFeatExists = greaterWeaponFocusMap.TryGetValue(baseItem, out HashSet<ushort> types);

      if (applicableFeatExists)
      {
        foreach (ushort feat in types)
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
        int goodAimModifier = NWNXLib.Rules().GetRulesetIntEntry("GOOD_AIM_MODIFIER".ToExoString(), 1);
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
      return (secondWeaponType == (uint)BaseItem.Kama || secondWeaponType == (uint)BaseItem.Torch || monkWeaponSet.Contains(secondWeaponType)).ToInt();
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
      if (creatureSize < (int)CreatureSize.Tiny || creatureSize > (int)CreatureSize.Huge)
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

    private bool IsUnarmedWeapon(CNWSItem weapon)
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

    private CNWSItem GetEquippedWeapon(CNWSCreature creature, bool offHand)
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
      for (int i = 0; i < stats.m_nNumMultiClasses; i++)
      {
        CNWSCreatureStats_ClassInfo classInfo = stats.m_ClassInfo[i];
        if (classInfo.m_nClass == classType)
        {
          return classInfo.m_nLevel;
        }
      }

      return 0;
    }
  }
}
