using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Anvil.Services;

namespace Anvil.Native
{
  [SuppressMessage("ReSharper", "InconsistentNaming")]
  internal static unsafe partial class Functions
  {
    public static class CNWSCreature
    {
      [NativeFunction("_ZN12CNWSCreature12AddAssociateEjt", "")]
      public delegate void AddAssociate(void* pCreature, uint oidAssociate, ushort associateType);

      [NativeFunction("_ZN12CNWSCreature19AddCastSpellActionsEjiiii6Vectorjiiihiiih", "")]
      public delegate int AddCastSpellActions(void* pCreature, uint nSpellId, int nMultiClass, int nDomainLevel,
        int nMetaType, int bSpontaneousCast, Vector3 vTargetLocation, uint oidTarget, int bAreaTarget, int bAddToFront,
        int bFake, byte nProjectilePathType, int bInstant, int bAllowPolymorphedCast, int nFeat, byte nCasterLevel);

      [NativeFunction("_ZN12CNWSCreature7AddGoldEii", "")]
      public delegate void AddGold(void* pCreature, int nGold, int bDisplayFeedback);

      [NativeFunction("_ZN12CNWSCreature12AIActionHealEP20CNWSObjectActionNode", "")]
      public delegate uint AIActionHeal(void* pCreature, void* pNode);

      [NativeFunction("_ZN12CNWSCreature12AIActionRestEP20CNWSObjectActionNode", "")]
      public delegate uint AIActionRest(void* pCreature, void* pNode);

      [NativeFunction("_ZN12CNWSCreature18BroadcastSpellCastEjht", "")]
      public delegate void BroadcastSpellCast(void* pCreature, uint nSpellId, byte nMultiClass, ushort nFeat);

      [NativeFunction("_ZN12CNWSCreature12CanEquipItemEP8CNWSItemPjiiiP10CNWSPlayer", "")]
      public delegate int CanEquipItem(void* pCreature, void* pItem, uint* pEquipToSLot, int bEquipping, int bLoading, int bDisplayFeedback, void* pFeedbackPlayer);

      [NativeFunction("_ZN12CNWSCreature10CanUseItemEP8CNWSItemi", "")]
      public delegate int CanUseItem(void* pCreature, void* pItem, int bIgnoreIdentifiedFlag);

      [NativeFunction("_ZN12CNWSCreature18CheckProficienciesEP8CNWSItemj", "")]
      public delegate int CheckProficiencies(void* pCreature, void* pItem, uint nEquipToSlot);

      [NativeFunction("_ZN12CNWSCreature17DoListenDetectionEPS_i", "")]
      public delegate int DoListenDetection(void* pCreature, void* pTarget, int bTargetInvisible);

      [NativeFunction("_ZN12CNWSCreature15DoSpotDetectionEPS_i", "")]
      public delegate int DoSpotDetection(void* pCreature, void* pTarget, int bTargetInvisible);

      [NativeFunction("_ZN12CNWSCreature11GetWalkRateEv", "")]
      public delegate float GetWalkRate(void* pCreature);

      [NativeFunction("_ZN12CNWSCreature11LearnScrollEj", "")]
      public delegate int LearnScroll(void* pCreature, uint oidScrollToLearn);

      [NativeFunction("_ZN12CNWSCreature14MaxAttackRangeEjii", "")]
      public delegate float MaxAttackRange(void* pCreature, uint oidTarget, int bBaseValue, int bPassiveRange);

      [NativeFunction("_ZN12CNWSCreature17PayToIdentifyItemEjj", "")]
      public delegate void PayToIdentifyItem(void* pCreature, uint oidItem, uint oidStore);

      [NativeFunction("_ZN12CNWSCreature15PossessFamiliarEv", "")]
      public delegate void PossessFamiliar(void* pCreature);

      [NativeFunction("_ZN12CNWSCreature15RemoveAssociateEj", "")]
      public delegate void RemoveAssociate(void* pCreature, uint oidAssociate);

      [NativeFunction("_ZN12CNWSCreature10RemoveGoldEii", "")]
      public delegate void RemoveGold(void* pCreature, int nGold, int bDisplayFeedback);

      [NativeFunction("_ZN12CNWSCreature10RequestBuyEjjj", "")]
      public delegate int RequestBuy(void* pCreature, uint oidItemToBuy, uint oidStore, uint oidDesiredRepository);

      [NativeFunction("_ZN12CNWSCreature11RequestSellEjj", "")]
      public delegate int RequestSell(void* pCreature, uint oidItemToBuy, uint oidStore);

      [NativeFunction("_ZN12CNWSCreature17ResolveInitiativeEv", "")]
      public delegate void ResolveInitiative(void* pObject);

      [NativeFunction("_ZN12CNWSCreature8RunEquipEjjj", "")]
      public delegate int RunEquip(void* pCreature, uint oidItemToEquip, uint nInventorySlot, uint oidFeedbackPlayer);

      [NativeFunction("_ZN12CNWSCreature19SendFeedbackMessageEtP16CNWCCMessageDataP10CNWSPlayer", "")]
      public delegate void SendFeedbackMessage(void* pCreature, ushort nFeedbackId, void* pMessageData, void* pFeedbackPlayer);

      [NativeFunction("_ZN12CNWSCreature13SetCombatModeEhi", "")]
      public delegate void SetCombatMode(void* pCreature, byte nNewMode, int bForceNewMode);

      [NativeFunction("_ZN12CNWSCreature13SetDetectModeEh", "")]
      public delegate void SetDetectMode(void* pCreature, byte nDetectMode);

      [NativeFunction("_ZN12CNWSCreature14SetStealthModeEh", "")]
      public delegate void SetStealthMode(void* pCreature, byte nStealthMode);

      [NativeFunction("_ZN12CNWSCreature17SignalMeleeDamageEP10CNWSObjecti", "")]
      public delegate void SignalMeleeDamage(void* pCreature, void* pTarget, int nAttacks);

      [NativeFunction("_ZN12CNWSCreature18SignalRangedDamageEP10CNWSObjecti", "")]
      public delegate void SignalRangedDamage(void* pCreature, void* pTarget, int nAttacks);

      [NativeFunction("_ZN12CNWSCreature10RunUnequipEjjhhij", "")]
      public delegate int UnequipItem(void* pCreature, uint oidItemToUnequip, uint oidTargetRepository, byte x, byte y, int bMergeIntoRepository, uint oidFeedbackPlayer);

      [NativeFunction("_ZN12CNWSCreature17UnpossessFamiliarEv", "")]
      public delegate void UnpossessFamiliar(void* pCreature);

      [NativeFunction("_ZN12CNWSCreature7UseFeatEttjjP6Vector", "")]
      public delegate int UseFeat(void* pCreature, ushort nFeat, ushort nSubFeat, uint oidTarget, uint oidArea, void* pTargetPos);

      [NativeFunction("_ZN12CNWSCreature7UseItemEjhhj6Vectorji", "")]
      public delegate int UseItem(void* pCreature, uint oidItem, byte nActivePropertyIndex, byte nSubPropertyIndex, uint oidTarget, Vector3 vTargetPosition, uint oidArea, int bUseCharges);

      [NativeFunction("_ZN12CNWSCreature8UseSkillEhhj6Vectorjji", "")]
      public delegate int UseSkill(void* pCreature, byte nSkill, byte nSubSkill, uint oidTarget, Vector3 vTargetPosition, uint oidArea, uint oidUsedItem, int nActivePropertyIndex);
    }
  }
}
