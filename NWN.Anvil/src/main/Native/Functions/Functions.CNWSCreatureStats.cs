using Anvil.Services;

// ReSharper disable InconsistentNaming
namespace Anvil.Native
{
  internal static unsafe partial class Functions
  {
    public static class CNWSCreatureStats
    {
      [NativeFunction("_ZN17CNWSCreatureStats10CanLevelUpEv", "?CanLevelUp@CNWSCreatureStats@@QEAAHXZ")]
      public delegate int CanLevelUp(void* pCreatureStats);

      [NativeFunction("_ZN17CNWSCreatureStats23ClearMemorizedSpellSlotEhhh", "?ClearMemorizedSpellSlot@CNWSCreatureStats@@QEAAXEEE@Z")]
      public delegate void ClearMemorizedSpellSlot(void* pCreatureStats, byte nMultiClass, byte nSpellLevel, byte nSpellSlot);

      [NativeFunction("_ZN17CNWSCreatureStats23GetAttackModifierVersusEP12CNWSCreature", "?GetAttackModifierVersus@CNWSCreatureStats@@QEAAHPEAVCNWSCreature@@@Z")]
      public delegate int GetAttackModifierVersus(void* pCreatureStats, void* pCreature);

      [NativeFunction("_ZN17CNWSCreatureStats14GetDamageBonusEP12CNWSCreaturei", "?GetDamageBonus@CNWSCreatureStats@@QEAAHPEAVCNWSCreature@@H@Z")]
      public delegate int GetDamageBonus(void* pCreatureStats, void* pCreature, int bOffHand);

      [NativeFunction("_ZN17CNWSCreatureStats17GetEffectImmunityEhP12CNWSCreaturei", "?GetEffectImmunity@CNWSCreatureStats@@QEAAHEPEAVCNWSCreature@@H@Z")]
      public delegate int GetEffectImmunity(void* pCreatureStats, byte nType, void* pVerses, int bConsiderFeats);

      [NativeFunction("_ZN17CNWSCreatureStats32GetEpicWeaponDevastatingCriticalEP8CNWSItem", "?GetEpicWeaponDevastatingCritical@CNWSCreatureStats@@QEAAHPEAVCNWSItem@@@Z")]
      public delegate int GetEpicWeaponDevastatingCritical(void* pCreatureStats, void* pWeapon);

      [NativeFunction("_ZN17CNWSCreatureStats18GetEpicWeaponFocusEP8CNWSItem", "?GetEpicWeaponFocus@CNWSCreatureStats@@QEAAHPEAVCNWSItem@@@Z")]
      public delegate int GetEpicWeaponFocus(void* pCreatureStats, void* pWeapon);

      [NativeFunction("_ZN17CNWSCreatureStats33GetEpicWeaponOverwhelmingCriticalEP8CNWSItem", "?GetEpicWeaponOverwhelmingCritical@CNWSCreatureStats@@QEAAHPEAVCNWSItem@@@Z")]
      public delegate int GetEpicWeaponOverwhelmingCritical(void* pCreatureStats, void* pWeapon);

      [NativeFunction("_ZN17CNWSCreatureStats27GetEpicWeaponSpecializationEP8CNWSItem", "?GetEpicWeaponSpecialization@CNWSCreatureStats@@QEAAHPEAVCNWSItem@@@Z")]
      public delegate int GetEpicWeaponSpecialization(void* pCreatureStats, void* pWeapon);

      [NativeFunction("_ZN17CNWSCreatureStats19GetIsWeaponOfChoiceEj", "?GetIsWeaponOfChoice@CNWSCreatureStats@@QEAAHI@Z")]
      public delegate int GetIsWeaponOfChoice(void* pCreatureStats, uint nBaseItem);

      [NativeFunction("_ZN17CNWSCreatureStats19GetMeleeAttackBonusEiii", "?GetMeleeAttackBonus@CNWSCreatureStats@@QEAAHHHH@Z")]
      public delegate int GetMeleeAttackBonus(void* pCreatureStats, int bOffHand, int bIncludeBase, int bTouchAttack);

      [NativeFunction("_ZN17CNWSCreatureStats19GetMeleeDamageBonusEih", "?GetMeleeDamageBonus@CNWSCreatureStats@@QEAAHHE@Z")]
      public delegate int GetMeleeDamageBonus(void* pCreatureStats, int bOffHand, byte nCreatureWeaponIndex);

      [NativeFunction("_ZN17CNWSCreatureStats20GetRangedAttackBonusEii", "?GetRangedAttackBonus@CNWSCreatureStats@@QEAAHHH@Z")]
      public delegate int GetRangedAttackBonus(void* pCreatureStats, int bIncludeBase, int bTouchAttack);

      [NativeFunction("_ZN17CNWSCreatureStats20GetRangedDamageBonusEv", "?GetRangedDamageBonus@CNWSCreatureStats@@QEAAHXZ")]
      public delegate int GetRangedDamageBonus(void* pCreatureStats);

      [NativeFunction("_ZN17CNWSCreatureStats22GetUseMonkAttackTablesEi", "?GetUseMonkAttackTables@CNWSCreatureStats@@QEAAHH@Z")]
      public delegate int GetUseMonkAttackTables(void* pCreatureStats, int bForceUnarmed);

      [NativeFunction("_ZN17CNWSCreatureStats16GetWeaponFinesseEP8CNWSItem", "?GetWeaponFinesse@CNWSCreatureStats@@QEAAHPEAVCNWSItem@@@Z")]
      public delegate int GetWeaponFinesse(void* pCreatureStats, void* pWeapon);

      [NativeFunction("_ZN17CNWSCreatureStats14GetWeaponFocusEP8CNWSItem", "?GetWeaponFocus@CNWSCreatureStats@@QEAAHPEAVCNWSItem@@@Z")]
      public delegate int GetWeaponFocus(void* pCreatureStats, void* pWeapon);

      [NativeFunction("_ZN17CNWSCreatureStats25GetWeaponImprovedCriticalEP8CNWSItem", "?GetWeaponImprovedCritical@CNWSCreatureStats@@QEAAHPEAVCNWSItem@@@Z")]
      public delegate int GetWeaponImprovedCritical(void* pCreatureStats, void* pWeapon);

      [NativeFunction("_ZN17CNWSCreatureStats23GetWeaponSpecializationEP8CNWSItem", "?GetWeaponSpecialization@CNWSCreatureStats@@QEAAHPEAVCNWSItem@@@Z")]
      public delegate int GetWeaponSpecialization(void* pCreatureStats, void* pWeapon);

      [NativeFunction("_ZN17CNWSCreatureStats9LevelDownEP13CNWLevelStats", "?LevelDown@CNWSCreatureStats@@QEAAXPEAVCNWLevelStats@@@Z")]
      public delegate void LevelDown(void* pCreatureStats, void* pLevelUpStats);

      [NativeFunction("_ZN17CNWSCreatureStats7LevelUpEP13CNWLevelStatshhhi", "?LevelUp@CNWSCreatureStats@@QEAAXPEAVCNWLevelStats@@EEEH@Z")]
      public delegate void LevelUp(void* pCreatureStats, void* pLevelUpStats, byte nDomain1, byte nDomain2, byte nSchool, int bAddStatsToList);

      [NativeFunction("_ZN17CNWSCreatureStats16LevelUpAutomaticEhih", "?LevelUpAutomatic@CNWSCreatureStats@@QEAAHEHE@Z")]
      public delegate void LevelUpAutomatic(void* pCreatureStats, byte nClass, int bReadyAllSpells, byte nPackage);

      [NativeFunction("_ZN17CNWSCreatureStats21SetMemorizedSpellSlotEhhjhhi", "?SetMemorizedSpellSlot@CNWSCreatureStats@@QEAAHEEIEEH@Z")]
      public delegate int SetMemorizedSpellSlot(void* pCreatureStats, byte nMultiClass, byte nSpellSlot,
        uint nSpellId, byte nDomainLevel, byte nMetaType, int bFromClient);

      [NativeFunction("_ZN17CNWSCreatureStats15ValidateLevelUpEP13CNWLevelStatshhh", "?ValidateLevelUp@CNWSCreatureStats@@QEAAIPEAVCNWLevelStats@@EEE@Z")]
      public delegate uint ValidateLevelUp(void* pCreatureStats, void* pLevelUpStats, byte nDomain1, byte nDomain2, byte nSchool);
    }
  }
}
