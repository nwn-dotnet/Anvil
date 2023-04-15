using System;
using Anvil.Services;

// ReSharper disable InconsistentNaming
namespace Anvil.Native
{
  internal static unsafe partial class Functions
  {
    public static class CNWSMessage
    {
      [NativeFunction("_ZN11CNWSMessage32ComputeGameObjectUpdateForObjectEP10CNWSPlayerP10CNWSObjectP16CGameObjectArrayj", "?ComputeGameObjectUpdateForObject@CNWSMessage@@QEAAXPEAVCNWSPlayer@@PEAVCNWSObject@@PEAVCGameObjectArray@@I@Z")]
      public delegate void ComputeGameObjectUpdateForObject(void* pMessage, void* pPlayer, void* pPlayerGameObject, void* pGameObjectArray, uint oidObjectToUpdate);

      [NativeFunction("_ZN11CNWSMessage38HandlePlayerToServerBarter_StartBarterEP10CNWSPlayer", "?HandlePlayerToServerBarter_StartBarter@CNWSMessage@@QEAAHPEAVCNWSPlayer@@@Z")]
      public delegate int HandlePlayerToServerBarter_StartBarter(void* pMessage, void* pPlayer);

      [NativeFunction("_ZN11CNWSMessage32HandlePlayerToServerCheatMessageEP10CNWSPlayerh", "?HandlePlayerToServerCheatMessage@CNWSMessage@@QEAAHPEAVCNWSPlayer@@E@Z")]
      public delegate int HandlePlayerToServerCheatMessage(void* pMessage, void* pPlayer, byte nMinor);

      [NativeFunction("_ZN11CNWSMessage40HandlePlayerToServerDungeonMasterMessageEP10CNWSPlayerhi", "?HandlePlayerToServerDungeonMasterMessage@CNWSMessage@@QEAAHPEAVCNWSPlayer@@EH@Z")]
      public delegate int HandlePlayerToServerDungeonMasterMessage(void* pMessage, void* pPlayer, byte nMinor, int bGroup);

      [NativeFunction("_ZN11CNWSMessage34HandlePlayerToServerLevelUpMessageEP10CNWSPlayerh", "?HandlePlayerToServerLevelUpMessage@CNWSMessage@@QEAAHPEAVCNWSPlayer@@E@Z")]
      public delegate int HandlePlayerToServerLevelUpMessage(void* pMessage, void* pPlayer, byte nMinor);

      [NativeFunction("_ZN11CNWSMessage35HandlePlayerToServerMapPinChangePinEP10CNWSPlayer", "?HandlePlayerToServerMapPinChangePin@CNWSMessage@@QEAAHPEAVCNWSPlayer@@@Z")]
      public delegate int HandlePlayerToServerMapPinChangePin(void* pMessage, void* pPlayer);

      [NativeFunction("_ZN11CNWSMessage39HandlePlayerToServerMapPinDestroyMapPinEP10CNWSPlayer", "?HandlePlayerToServerMapPinDestroyMapPin@CNWSMessage@@QEAAHPEAVCNWSPlayer@@@Z")]
      public delegate int HandlePlayerToServerMapPinDestroyMapPin(void* pMessage, void* pPlayer);

      [NativeFunction("_ZN11CNWSMessage37HandlePlayerToServerMapPinSetMapPinAtEP10CNWSPlayer", "?HandlePlayerToServerMapPinSetMapPinAt@CNWSMessage@@QEAAHPEAVCNWSPlayer@@@Z")]
      public delegate int HandlePlayerToServerMapPinSetMapPinAt(void* pMessage, void* pPlayer);

      [NativeFunction("_ZN11CNWSMessage25HandlePlayerToServerPartyEP10CNWSPlayerh", "?HandlePlayerToServerParty@CNWSMessage@@QEAAHPEAVCNWSPlayer@@E@Z")]
      public delegate int HandlePlayerToServerParty(void* pMessage, void* pPlayer, byte nMinor);

      [NativeFunction("_ZN11CNWSMessage36HandlePlayerToServerQuickChatMessageEP10CNWSPlayerh", "?HandlePlayerToServerQuickChatMessage@CNWSMessage@@QEAAHPEAVCNWSPlayer@@E@Z")]
      public delegate int HandlePlayerToServerQuickChatMessage(void* pMessage, void* pPlayer, byte nMinor);

      [NativeFunction("_ZN11CNWSMessage40SendServerToPlayerAmbientBattleMusicPlayEji", "?SendServerToPlayerAmbientBattleMusicPlay@CNWSMessage@@QEAAHIH@Z")]
      public delegate int SendServerToPlayerAmbientBattleMusicPlay(void* pMessage, uint nPlayer, int bPlay);

      [NativeFunction("_ZN11CNWSMessage33SendServerToPlayerArea_ClientAreaEP10CNWSPlayerP8CNWSAreafffRK6Vectori", "?SendServerToPlayerArea_ClientArea@CNWSMessage@@QEAAHPEAVCNWSPlayer@@PEAVCNWSArea@@MMMAEBVVector@@H@Z")]
      public delegate int SendServerToPlayerArea_ClientArea(void* pMessage, void* pPlayer, void* pArea, float fX, float fY, float fZ, void* vNewOrientation, int bPlayerIsNewToModule);

      [NativeFunction("_ZN11CNWSMessage35SendServerToPlayerBarterCloseBarterEjji", "?SendServerToPlayerBarterCloseBarter@CNWSMessage@@QEAAHIIH@Z")]
      public delegate int SendServerToPlayerBarterCloseBarter(void* pMessage, uint nInitiatorId, uint nRecipientId, int bAccepted);

      [NativeFunction("_ZN11CNWSMessage27SendServerToPlayerCCMessageEjhP16CNWCCMessageDataP20CNWSCombatAttackData", "?SendServerToPlayerCCMessage@CNWSMessage@@QEAAHIEPEAVCNWCCMessageData@@PEAVCNWSCombatAttackData@@@Z")]
      public delegate int SendServerToPlayerCCMessage(void* pMessage, uint nPlayerId, byte nMinor, void* pMessageData, void* pAttackData);

      [NativeFunction("_ZN11CNWSMessage26SendServerToPlayerCharListEP10CNWSPlayer", "?SendServerToPlayerCharList@CNWSMessage@@QEAAHPEAVCNWSPlayer@@@Z")]
      public delegate int SendServerToPlayerCharList(void* pMessage, void* pPlayer);

      [NativeFunction("_ZN11CNWSMessage28SendServerToPlayerChat_PartyEjj10CExoString", "?SendServerToPlayerChat_Party@CNWSMessage@@QEAAHIIVCExoString@@@Z")]
      public delegate int SendServerToPlayerChat_Party(void* pMessage, uint nPlayerId, uint oidSpeaker, void* sSpeakerMessage);

      [NativeFunction("_ZN11CNWSMessage28SendServerToPlayerChat_ShoutEjj10CExoString", "?SendServerToPlayerChat_Shout@CNWSMessage@@IEAAHIIVCExoString@@@Z")]
      public delegate int SendServerToPlayerChat_Shout(void* pMessage, uint nPlayerId, uint oidSpeaker, void* sSpeakerMessage);

      [NativeFunction("_ZN11CNWSMessage27SendServerToPlayerChat_TellEjj10CExoString", "?SendServerToPlayerChat_Tell@CNWSMessage@@IEAAHIIVCExoString@@@Z")]
      public delegate int SendServerToPlayerChat_Tell(void* pMessage, uint nPlayerId, uint oidSpeaker, void* sSpeakerMessage);

      [NativeFunction("_ZN11CNWSMessage29SendServerToPlayerChatMessageEhj10CExoStringjRKS0_", "?SendServerToPlayerChatMessage@CNWSMessage@@QEAAHEIVCExoString@@IAEBV2@@Z")]
      public delegate int SendServerToPlayerChatMessage(void* pMessage, ChatChannel nChatMessageType, uint oidSpeaker, void* sSpeakerMessage, uint nTellPlayerId, void* tellName);

      [NativeFunction("_ZN11CNWSMessage46SendServerToPlayerDungeonMasterUpdatePartyListEj", "?SendServerToPlayerDungeonMasterUpdatePartyList@CNWSMessage@@QEAAHI@Z")]
      public delegate int SendServerToPlayerDungeonMasterUpdatePartyList(void* pMessage, uint nPlayerID);

      [NativeFunction("_ZN11CNWSMessage41SendServerToPlayerExamineGui_CreatureDataEP10CNWSPlayerj", "?SendServerToPlayerExamineGui_CreatureData@CNWSMessage@@QEAAHPEAVCNWSPlayer@@I@Z")]
      public delegate int SendServerToPlayerExamineGui_CreatureData(void* pMessage, void* pPlayer, uint oidCreatureID);

      [NativeFunction("_ZN11CNWSMessage37SendServerToPlayerExamineGui_DoorDataEP10CNWSPlayerj", "?SendServerToPlayerExamineGui_DoorData@CNWSMessage@@QEAAHPEAVCNWSPlayer@@I@Z")]
      public delegate int SendServerToPlayerExamineGui_DoorData(void* pMessage, void* pPlayer, uint oidDoor);

      [NativeFunction("_ZN11CNWSMessage37SendServerToPlayerExamineGui_ItemDataEP10CNWSPlayerj", "?SendServerToPlayerExamineGui_ItemData@CNWSMessage@@QEAAHPEAVCNWSPlayer@@I@Z")]
      public delegate int SendServerToPlayerExamineGui_ItemData(void* pMessage, void* pPlayer, uint oidItem);

      [NativeFunction("_ZN11CNWSMessage42SendServerToPlayerExamineGui_PlaceableDataEP10CNWSPlayerj", "?SendServerToPlayerExamineGui_PlaceableData@CNWSMessage@@QEAAHPEAVCNWSPlayer@@I@Z")]
      public delegate int SendServerToPlayerExamineGui_PlaceableData(void* pMessage, void* pPlayer, uint oidPlaceable);

      [NativeFunction("_ZN11CNWSMessage37SendServerToPlayerExamineGui_TrapDataEP10CNWSPlayerjP12CNWSCreaturei", "?SendServerToPlayerExamineGui_TrapData@CNWSMessage@@QEAAHPEAVCNWSPlayer@@IPEAVCNWSCreature@@H@Z")]
      public delegate int SendServerToPlayerExamineGui_TrapData(void* pMessage, void* pPlayer, uint oidTrap, void* pCreature, int bSuccess);

      [NativeFunction("_ZN11CNWSMessage32SendServerToPlayerJournalUpdatedEP10CNWSPlayerii13CExoLocString", "?SendServerToPlayerJournalUpdated@CNWSMessage@@QEAAHPEAVCNWSPlayer@@HHVCExoLocString@@@Z")]
      public delegate int SendServerToPlayerJournalUpdated(void* pMessage, void* pPlayer, int bQuest, int bCompleted, CExoLocStringData cExoLocString);

      [NativeFunction("_ZN11CNWSMessage32SendServerToPlayerPlayerList_AddEjP10CNWSPlayer", "?SendServerToPlayerPlayerList_Add@CNWSMessage@@QEAAHIPEAVCNWSPlayer@@@Z")]
      public delegate int SendServerToPlayerPlayerList_Add(void* pMessage, uint nPlayerId, void* pNewPlayer);

      [NativeFunction("_ZN11CNWSMessage32SendServerToPlayerPlayerList_AllEP10CNWSPlayer", "?SendServerToPlayerPlayerList_All@CNWSMessage@@QEAAHPEAVCNWSPlayer@@@Z")]
      public delegate int SendServerToPlayerPlayerList_All(void* pMessage, void* pPlayer);

      [NativeFunction("_ZN11CNWSMessage35SendServerToPlayerPlayerList_DeleteEjP10CNWSPlayer", "?SendServerToPlayerPlayerList_Delete@CNWSMessage@@QEAAHIPEAVCNWSPlayer@@@Z")]
      public delegate int SendServerToPlayerPlayerList_Delete(void* pMessage, uint nPlayerId, void* pNewPlayer);

      [NativeFunction("_ZN11CNWSMessage31SendServerToPlayerPopUpGUIPanelEjiiii10CExoString", "?SendServerToPlayerPopUpGUIPanel@CNWSMessage@@QEAAHIHHHHVCExoString@@@Z")]
      public delegate int SendServerToPlayerPopUpGUIPanel(void* pMessage, uint observerOid, int nGuiPanel, int bGUIOption1, int bGUIOption2, int nStringReference, void** psStringReference);

      [NativeFunction("_ZN11CNWSMessage17TestObjectVisibleEP10CNWSObjectS1_", "?TestObjectVisible@CNWSMessage@@QEAAHPEAVCNWSObject@@0@Z")]
      public delegate int TestObjectVisible(void* pMessage, void* pAreaObject, void* pPlayerGameObject);

      [NativeFunction("_ZN11CNWSMessage31WriteGameObjUpdate_UpdateObjectEP10CNWSPlayerP10CNWSObjectP17CLastUpdateObjectjj", "?WriteGameObjUpdate_UpdateObject@CNWSMessage@@QEAAXPEAVCNWSPlayer@@PEAVCNWSObject@@PEAVCLastUpdateObject@@II@Z")]
      public delegate void WriteGameObjUpdate_UpdateObject(void* pMessage, void* pPlayer, void* pAreaObject, void* pLastUpdateObject, uint nObjectUpdatesRequired, uint nObjectAppearanceUpdatesRequired);
    }
  }
}
