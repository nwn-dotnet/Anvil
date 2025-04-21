using System.Numerics;
using Anvil.Services;

// ReSharper disable InconsistentNaming
namespace Anvil.Native
{
  internal static unsafe partial class Functions
  {
    public static class CNWSCreature
    {
      [NativeFunction("_ZN12CNWSCreature12AddAssociateEjt", "?AddAssociate@CNWSCreature@@QEAAXIG@Z")]
      public delegate void AddAssociate(void* pCreature, uint oidAssociate, ushort associateType);

      [NativeFunction("_ZN12CNWSCreature19AddCastSpellActionsEjiiii6Vectorjiiihiiih", "?AddCastSpellActions@CNWSCreature@@QEAAHIHHHHVVector@@IHHHEHHHE@Z")]
      public delegate int AddCastSpellActions(void* pCreature, uint nSpellId, int nMultiClass, int nDomainLevel,
        int nMetaType, int bSpontaneousCast, Vector3 vTargetLocation, uint oidTarget, int bAreaTarget, int bAddToFront,
        int bFake, byte nProjectilePathType, int bInstant, int bAllowPolymorphedCast, int nFeat, byte nCasterLevel);

      [NativeFunction("_ZN12CNWSCreature7AddGoldEii", "?AddGold@CNWSCreature@@QEAAXHH@Z")]
      public delegate void AddGold(void* pCreature, int nGold, int bDisplayFeedback);

      [NativeFunction("_ZN12CNWSCreature12AIActionHealEP20CNWSObjectActionNode", "?AIActionHeal@CNWSCreature@@QEAAIPEAVCNWSObjectActionNode@@@Z")]
      public delegate uint AIActionHeal(void* pCreature, void* pNode);

      [NativeFunction("_ZN12CNWSCreature18AIActionDisarmTrapEP20CNWSObjectActionNode", "?AIActionDisarmTrap@CNWSCreature@@QEAAIPEAVCNWSObjectActionNode@@@Z")]
      public delegate uint AIActionDisarmTrap(void* pCreature, void* pNode);

      [NativeFunction("_ZN12CNWSCreature19AIActionExamineTrapEP20CNWSObjectActionNode", "?AIActionExamineTrap@CNWSCreature@@QEAAIPEAVCNWSObjectActionNode@@@Z")]
      public delegate uint AIActionExamineTrap(void* pCreature, void* pNode);

      [NativeFunction("_ZN12CNWSCreature16AIActionFlagTrapEP20CNWSObjectActionNode", "?AIActionFlagTrap@CNWSCreature@@QEAAIPEAVCNWSObjectActionNode@@@Z")]
      public delegate uint AIActionFlagTrap(void* pCreature, void* pNode);

      [NativeFunction("_ZN12CNWSCreature19AIActionRecoverTrapEP20CNWSObjectActionNode", "?AIActionRecoverTrap@CNWSCreature@@QEAAIPEAVCNWSObjectActionNode@@@Z")]
      public delegate uint AIActionRecoverTrap(void* pCreature, void* pNode);

      [NativeFunction("_ZN12CNWSCreature15AIActionSetTrapEP20CNWSObjectActionNode", "?AIActionSetTrap@CNWSCreature@@QEAAIPEAVCNWSObjectActionNode@@@Z")]
      public delegate uint AIActionSetTrap(void* pCreature, void* pNode);

      [NativeFunction("_ZN12CNWSCreature12AIActionRestEP20CNWSObjectActionNode", "?AIActionRest@CNWSCreature@@QEAAIPEAVCNWSObjectActionNode@@@Z")]
      public delegate uint AIActionRest(void* pCreature, void* pNode);

      [NativeFunction("_ZN12CNWSCreature18BroadcastSpellCastEjht", "?BroadcastSpellCast@CNWSCreature@@QEAAXIEG@Z")]
      public delegate void BroadcastSpellCast(void* pCreature, uint nSpellId, byte nMultiClass, ushort nFeat);

      [NativeFunction("_ZN12CNWSCreature12CanEquipItemEP8CNWSItemPjiiiP10CNWSPlayer", "?CanEquipItem@CNWSCreature@@QEAAEPEAVCNWSItem@@PEAIHHHPEAVCNWSPlayer@@@Z")]
      public delegate int CanEquipItem(void* pCreature, void* pItem, uint* pEquipToSLot, int bEquipping, int bLoading, int bDisplayFeedback, void* pFeedbackPlayer);

      [NativeFunction("_ZN12CNWSCreature10CanUseItemEP8CNWSItemi", "?CanUseItem@CNWSCreature@@QEAAHPEAVCNWSItem@@H@Z")]
      public delegate int CanUseItem(void* pCreature, void* pItem, int bIgnoreIdentifiedFlag);

      [NativeFunction("_ZN12CNWSCreature18CheckProficienciesEP8CNWSItemj", "?CheckProficiencies@CNWSCreature@@IEAAHPEAVCNWSItem@@I@Z")]
      public delegate int CheckProficiencies(void* pCreature, void* pItem, uint nEquipToSlot);

      [NativeFunction("_ZN12CNWSCreature17DoListenDetectionEPS_i", "?DoListenDetection@CNWSCreature@@IEAAHPEAV1@H@Z")]
      public delegate int DoListenDetection(void* pCreature, void* pTarget, int bTargetInvisible);

      [NativeFunction("_ZN12CNWSCreature15DoSpotDetectionEPS_i", "?DoSpotDetection@CNWSCreature@@IEAAHPEAV1@H@Z")]
      public delegate int DoSpotDetection(void* pCreature, void* pTarget, int bTargetInvisible);

      [NativeFunction("_ZN12CNWSCreature11GetWalkRateEv", "?GetWalkRate@CNWSCreature@@QEAAMXZ")]
      public delegate float GetWalkRate(void* pCreature);

      [NativeFunction("_ZN12CNWSCreature11LearnScrollEj", "?LearnScroll@CNWSCreature@@QEAAHI@Z")]
      public delegate int LearnScroll(void* pCreature, uint oidScrollToLearn);

      [NativeFunction("_ZN12CNWSCreature14MaxAttackRangeEjii", "?MaxAttackRange@CNWSCreature@@QEAAMIHH@Z")]
      public delegate float MaxAttackRange(void* pCreature, uint oidTarget, int bBaseValue, int bPassiveRange);

      [NativeFunction("_ZN12CNWSCreature17PayToIdentifyItemEjj", "?PayToIdentifyItem@CNWSCreature@@QEAAXII@Z")]
      public delegate void PayToIdentifyItem(void* pCreature, uint oidItem, uint oidStore);

      [NativeFunction("_ZN12CNWSCreature15PossessFamiliarEv", "?PossessFamiliar@CNWSCreature@@QEAAXXZ")]
      public delegate void PossessFamiliar(void* pCreature);

      [NativeFunction("_ZN12CNWSCreature15RemoveAssociateEj", "?RemoveAssociate@CNWSCreature@@QEAAXI@Z")]
      public delegate void RemoveAssociate(void* pCreature, uint oidAssociate);

      [NativeFunction("_ZN12CNWSCreature10RemoveGoldEii", "?RemoveGold@CNWSCreature@@QEAAXHH@Z")]
      public delegate void RemoveGold(void* pCreature, int nGold, int bDisplayFeedback);

      [NativeFunction("_ZN12CNWSCreature10RequestBuyEjjj", "?RequestBuy@CNWSCreature@@QEAAHIII@Z")]
      public delegate int RequestBuy(void* pCreature, uint oidItemToBuy, uint oidStore, uint oidDesiredRepository);

      [NativeFunction("_ZN12CNWSCreature11RequestSellEjj", "?RequestSell@CNWSCreature@@QEAAHII@Z")]
      public delegate int RequestSell(void* pCreature, uint oidItemToBuy, uint oidStore);

      [NativeFunction("_ZN12CNWSCreature17ResolveInitiativeEv", "?ResolveInitiative@CNWSCreature@@QEAAXXZ")]
      public delegate void ResolveInitiative(void* pCreature);

      [NativeFunction("_ZN12CNWSCreature8RunEquipEjjj", "?RunEquip@CNWSCreature@@QEAAHIII@Z")]
      public delegate int RunEquip(void* pCreature, uint oidItemToEquip, uint nInventorySlot, uint oidFeedbackPlayer);

      [NativeFunction("_ZN12CNWSCreature10RunUnequipEjjhhij", "?RunUnequip@CNWSCreature@@QEAAHIIEEHI@Z")]
      public delegate int RunUnequip(void* pCreature, uint oidItemToUnequip, uint oidTargetRepository, byte x, byte y, int bMergeIntoRepository, uint oidFeedbackPlayer);

      [NativeFunction("_ZN12CNWSCreature19SendFeedbackMessageEtP16CNWCCMessageDataP10CNWSPlayer", "?SendFeedbackMessage@CNWSCreature@@QEAAXGPEAVCNWCCMessageData@@PEAVCNWSPlayer@@@Z")]
      public delegate void SendFeedbackMessage(void* pCreature, ushort nFeedbackId, void* pMessageData, void* pFeedbackPlayer);

      [NativeFunction("_ZN12CNWSCreature13SetCombatModeEhi", "?SetCombatMode@CNWSCreature@@QEAAXEH@Z")]
      public delegate void SetCombatMode(void* pCreature, byte nNewMode, int bForceNewMode);

      [NativeFunction("_ZN12CNWSCreature13SetDetectModeEh", "?SetDetectMode@CNWSCreature@@QEAAXE@Z")]
      public delegate void SetDetectMode(void* pCreature, byte nDetectMode);

      [NativeFunction("_ZN12CNWSCreature14SetStealthModeEh", "?SetStealthMode@CNWSCreature@@QEAAXE@Z")]
      public delegate void SetStealthMode(void* pCreature, byte nStealthMode);

      [NativeFunction("_ZN12CNWSCreature17SignalMeleeDamageEP10CNWSObjecti", "?SignalMeleeDamage@CNWSCreature@@IEAAXPEAVCNWSObject@@H@Z")]
      public delegate void SignalMeleeDamage(void* pCreature, void* pTarget, int nAttacks);

      [NativeFunction("_ZN12CNWSCreature18SignalRangedDamageEP10CNWSObjecti", "?SignalRangedDamage@CNWSCreature@@IEAAXPEAVCNWSObject@@H@Z")]
      public delegate void SignalRangedDamage(void* pCreature, void* pTarget, int nAttacks);

      [NativeFunction("_ZN12CNWSCreature17UnpossessFamiliarEv", "?UnpossessFamiliar@CNWSCreature@@QEAAXXZ")]
      public delegate void UnpossessFamiliar(void* pCreature);

      [NativeFunction("_ZN12CNWSCreature14UnsummonMyselfEv", "?UnsummonMyself@CNWSCreature@@QEAAXXZ")]
      public delegate void UnsummonMyself(void* pCreature);

      [NativeFunction("_ZN12CNWSCreature7UseFeatEttjjP6Vector", "?UseFeat@CNWSCreature@@QEAAHGGIIPEAVVector@@@Z")]
      public delegate int UseFeat(void* pCreature, ushort nFeat, ushort nSubFeat, uint oidTarget, uint oidArea, void* pTargetPos);

      [NativeFunction("_ZN12CNWSCreature7UseItemEjhhj6Vectorji", "?UseItem@CNWSCreature@@QEAAHIEEIVVector@@IH@Z")]
      public delegate int UseItem(void* pCreature, uint oidItem, byte nActivePropertyIndex, byte nSubPropertyIndex, uint oidTarget, Vector3 vTargetPosition, uint oidArea, int bUseCharges);

      [NativeFunction("_ZN12CNWSCreature8UseSkillEhhj6Vectorjji", "?UseSkill@CNWSCreature@@QEAAHEEIVVector@@IIH@Z")]
      public delegate int UseSkill(void* pCreature, byte nSkill, byte nSubSkill, uint oidTarget, Vector3 vTargetPosition, uint oidArea, uint oidUsedItem, int nActivePropertyIndex);
    }
  }
}
