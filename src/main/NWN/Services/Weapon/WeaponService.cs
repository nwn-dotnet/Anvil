using System;
using System.Collections.Generic;
using NWN.API;
using NWN.Native.API;

namespace NWN.Services
{
  [ServiceBinding(typeof(WeaponService))]
  [ServiceBindingOptions(Lazy = true)]
  public sealed unsafe class WeaponService
  {
    private readonly HookService hookService;

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

    private delegate int ToggleModeHook(void* pCreature, byte nMode);

    private FunctionHook<GetWeaponFocusHook> getWeaponFocusHook;
    private FunctionHook<GetEpicWeaponFocusHook> getEpicWeaponFocusHook;
    private FunctionHook<GetWeaponFinesseHook> getWeaponFinesseHook;
    private FunctionHook<GetWeaponImprovedCriticalHook> getWeaponImprovedCriticalHook;
    private FunctionHook<GetEpicWeaponOverwhelmingCriticalHook> getEpicWeaponOverwhelmingCriticalHook;
    private FunctionHook<GetEpicWeaponDevastatingCriticalHook> getEpicWeaponDevastatingCriticalHook;
    private FunctionHook<GetWeaponSpecializationHook> getWeaponSpecializationHook;
    private FunctionHook<GetEpicWeaponSpecializationHook> getEpicWeaponSpecializationHook;
    private FunctionHook<GetIsWeaponOfChoiceHook> getIsWeaponOfChoiceHook;
    private FunctionHook<GetDamageBonusHook> getDamageBonusHook;
    private FunctionHook<GetMeleeDamageBonusHook> getMeleeDamageBonusHook;
    private FunctionHook<GetRangedDamageBonusHook> getRangedDamageBonusHook;
    private FunctionHook<GetMeleeAttackBonusHook> getMeleeAttackBonusHook;
    private FunctionHook<GetRangedAttackBonusHook> getRangedAttackBonusHook;
    private FunctionHook<GetAttackModifierVersusHook> getAttackModifierVersusHook;
    private FunctionHook<GetUseMonkAttackTablesHook> getUseMonkAttackTablesHook;
    private FunctionHook<ToggleModeHook> toggleModeHook;

    private Dictionary<uint, HashSet<ushort>> weaponFocusMap = new Dictionary<uint, HashSet<ushort>>();
    private Dictionary<uint, HashSet<ushort>> epicWeaponFocusMap = new Dictionary<uint, HashSet<ushort>>();
    private Dictionary<uint, byte> weaponFinesseSizeMap = new Dictionary<uint, byte>();
    private Dictionary<uint, HashSet<ushort>> weaponImprovedCriticalMap = new Dictionary<uint, HashSet<ushort>>();
    private Dictionary<uint, HashSet<ushort>> weaponSpecializationMap = new Dictionary<uint, HashSet<ushort>>();
    private Dictionary<uint, HashSet<ushort>> epicWeaponSpecializationMap = new Dictionary<uint, HashSet<ushort>>();
    private Dictionary<uint, HashSet<ushort>> epicWeaponOverwhelmingCriticalMap = new Dictionary<uint, HashSet<ushort>>();
    private Dictionary<uint, HashSet<ushort>> epicWeaponDevastatingCriticalMap = new Dictionary<uint, HashSet<ushort>>();
    private Dictionary<uint, HashSet<ushort>> weaponOfChoiceMap = new Dictionary<uint, HashSet<ushort>>();
    private Dictionary<uint, HashSet<ushort>> greaterWeaponSpecializationMap = new Dictionary<uint, HashSet<ushort>>();
    private Dictionary<uint, HashSet<ushort>> greaterWeaponFocusMap = new Dictionary<uint, HashSet<ushort>>();

    private HashSet<uint> weaponUnarmedSet = new HashSet<uint>();
    private HashSet<uint> monkWeaponSet = new HashSet<uint>();

    private DevastatingCriticalData dcData;
    private Action<DevastatingCriticalData> dcCallback;

    private int greaterFocusAttackBonus = 1;
    private int greaeterWeaponSpecializationDamageBonus = 2;
    private bool gaSling = false;

    private Dictionary<uint, MaxRangedAttackDistanceOverride> maxRangedAttackDistanceOverrideMap = new Dictionary<uint, MaxRangedAttackDistanceOverride>();

    public WeaponService(HookService hookService)
    {
      this.hookService = hookService;

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

    private int OnGetWeaponFocus(void* pStats, void* pWeapon)
    {
      CNWSCreatureStats stats = new CNWSCreatureStats(pStats, false);
      uint weaponType = pWeapon == null ? (uint)BaseItem.Gloves : new CNWSItem(pWeapon, false).m_nBaseItem;

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
      CNWSCreatureStats stats = new CNWSCreatureStats(pStats, false);
      uint weaponType = pWeapon == null ? (uint)BaseItem.Gloves : new CNWSItem(pWeapon, false).m_nBaseItem;

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

      CNWSCreatureStats creatureStats = new CNWSCreatureStats(pStats, false);
      if (!creatureStats.HasFeat((ushort)Feat.WeaponFinesse).ToBool())
      {
        return 0;
      }

      return IsWeaponLight(creatureStats, new CNWSItem(pWeapon, false), true).ToInt();
    }

    private int OnGetWeaponImprovedCritical(void* pStats, void* pWeapon)
    {
      CNWSCreatureStats stats = new CNWSCreatureStats(pStats, false);
      uint weaponType = pWeapon == null ? (uint)BaseItem.Gloves : new CNWSItem(pWeapon, false).m_nBaseItem;

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
      CNWSCreatureStats stats = new CNWSCreatureStats(pStats, false);
      uint weaponType = pWeapon == null ? (uint)BaseItem.Gloves : new CNWSItem(pWeapon, false).m_nBaseItem;

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
      throw new NotImplementedException();
    }

    private int OnGetWeaponSpecialization(void* pStats, void* pWeapon)
    {
      throw new NotImplementedException();
    }

    private int OnGetEpicWeaponSpecialization(void* pStats, void* pWeapon)
    {
      throw new NotImplementedException();
    }

    private int OnGetIsWeaponOfChoice(void* pStats, uint nBaseItem)
    {
      throw new NotImplementedException();
    }

    private int OnGetDamageBonus(void* pStats, void* pCreature, int bOffHand)
    {
      throw new NotImplementedException();
    }

    private int OnGetMeleeDamageBonus(void* pStats, int bOffHand, byte nCreatureWeaponIndex)
    {
      throw new NotImplementedException();
    }

    private int OnGetRangedDamageBonus(void* pStats)
    {
      throw new NotImplementedException();
    }

    private int OnGetMeleeAttackBonus(void* pStats, int bOffHand, int bIncludeBase, int bTouchAttack)
    {
      throw new NotImplementedException();
    }

    private int OnGetRangedAttackBonus(void* pStats, int bIncludeBase, int bTouchAttack)
    {
      throw new NotImplementedException();
    }

    private int OnGetAttackModifierVersus(void* pStats, void* pCreature)
    {
      throw new NotImplementedException();
    }

    private int OnGetUseMonkAttackTables(void* pStats, int bForceUnarmed)
    {
      throw new NotImplementedException();
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
  }
}
