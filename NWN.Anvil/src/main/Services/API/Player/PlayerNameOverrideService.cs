using System.Collections.Generic;
using Anvil.API;
using Anvil.Internal;
using NWN.Native.API;

namespace Anvil.Services
{
  [ServiceBinding(typeof(PlayerNameOverrideService))]
  [ServiceBindingOptions(Lazy = true)]
  public sealed unsafe class PlayerNameOverrideService
  {
    private delegate void WriteGameObjUpdateUpdateObjectHook(void* pMessage, void* pPlayer, void* pAreaObject, void* pLastUpdateObject, uint nObjectUpdatesRequired, uint nObjectAppearanceUpdatesRequired);

    private delegate int SendServerToPlayerExamineGuiCreatureDataHook(void* pMessage, void* pPlayer, uint oidCreatureID);

    private delegate int SendServerToPlayerPlayModuleCharacterListResponseHook(void* pMessage, uint nPlayerId, uint nCharacterId, int bAdd);

    private delegate int SendServerToPlayerPlayerListAllHook(void* pMessage, void* pPlayer);

    private delegate int SendServerToPlayerPlayerListAddHook(void* pMessage, uint nPlayerId, void* pNewPlayer);

    private delegate int SendServerToPlayerPlayerListDeleteHook(void* pMessage, uint nPlayerId, void* pNewPlayer);

    private delegate int SendServerToPlayerDungeonMasterUpdatePartyListHook(void* pMessage, uint nPlayerID);

    private delegate int SendServerToPlayerPopUpGUIPanelHook(void* pMessage, uint observerOid, int nGuiPanel, int bGUIOption1, int bGUIOption2, int nStringReference, void* psStringReference);

    private delegate int SendServerToPlayerChatPartyHook(void* pMessage, uint nPlayerId, uint oidSpeaker, void* sSpeakerMessage);

    private delegate int SendServerToPlayerChatShoutHook(void* pMessage, uint nPlayerId, uint oidSpeaker, void* sSpeakerMessage);

    private delegate int SendServerToPlayerChatTellHook(void* pMessage, uint nPlayerId, uint oidSpeaker, void* sSpeakerMessage);

    private readonly FunctionHook<WriteGameObjUpdateUpdateObjectHook> writeGameObjUpdateUpdateObjectHook;
    private readonly FunctionHook<SendServerToPlayerExamineGuiCreatureDataHook> sendServerToPlayerExamineGuiCreatureDataHook;
    private readonly FunctionHook<SendServerToPlayerPlayModuleCharacterListResponseHook> sendServerToPlayerPlayModuleCharacterListResponseHook;
    private readonly FunctionHook<SendServerToPlayerPlayerListAllHook> sendServerToPlayerPlayerListAllHook;
    private readonly FunctionHook<SendServerToPlayerPlayerListAddHook> sendServerToPlayerPlayerListAddHook;
    private readonly FunctionHook<SendServerToPlayerPlayerListDeleteHook> sendServerToPlayerPlayerListDeleteHook;
    private readonly FunctionHook<SendServerToPlayerDungeonMasterUpdatePartyListHook> sendServerToPlayerDungeonMasterUpdatePartyListHook;
    private readonly FunctionHook<SendServerToPlayerPopUpGUIPanelHook> sendServerToPlayerPopUpGUIPanelHook;

    private readonly FunctionHook<SendServerToPlayerChatPartyHook> sendServerToPlayerChatPartyHook;
    private readonly FunctionHook<SendServerToPlayerChatShoutHook> sendServerToPlayerChatShoutHook;
    private readonly FunctionHook<SendServerToPlayerChatTellHook> sendServerToPlayerChatTellHook;

    private readonly Dictionary<uint, Dictionary<uint, RenamePlayerName>> renamePlayerNames = new Dictionary<uint, Dictionary<uint, RenamePlayerName>>();
    private readonly Dictionary<uint, RenameOriginalName> renameOriginalNames = new Dictionary<uint, RenameOriginalName>();
    private readonly Dictionary<uint, string> obfuscatedNames = new Dictionary<uint, string>();
    private readonly HashSet<uint> renameAddedToPlayerList = new HashSet<uint>();

    /// <summary>
    /// This is the listing of players from the character selection screen before entering the server. Setting the value to true overrides their names if a global rename has been set.
    /// </summary>
    public bool RenameOnModuleCharList { get; set; } = false;

    /// <summary>
    /// Renames the player name on the player list as well.
    /// </summary>
    public bool RenameOnPlayerList { get; set; } = true;

    /// <summary>
    /// DM observers will see global or personal overrides as well as being able to have their own name overridden for other observers.
    /// </summary>
    public bool RenameAllowDM { get; set; } = true;

    /// <summary>
    /// When using <see cref="SetPlayerNameOverride"/> with <see cref="PlayerNameState.Anonymous"/>, this is the string used for the player name.
    /// </summary>
    public string AnonymousName { get; set; } = "Someone";

    /// <summary>
    /// When set to true, global overrides change the display name globally - scripts and DMs included.<br/>
    /// When set to false, then name is only changed for players.<br/>
    /// Scripts and DMs see the original names (unless <see cref="RenameAllowDM"/> is set).
    /// </summary>
    public bool RenameOverwriteDisplayName { get; set; } = false;

    public PlayerNameOverrideService(HookService hookService)
    {
      writeGameObjUpdateUpdateObjectHook = hookService.RequestHook<WriteGameObjUpdateUpdateObjectHook>(OnWriteGameObjUpdateUpdateObject, FunctionsLinux._ZN11CNWSMessage31WriteGameObjUpdate_UpdateObjectEP10CNWSPlayerP10CNWSObjectP17CLastUpdateObjectjj, HookOrder.Early);
      sendServerToPlayerExamineGuiCreatureDataHook = hookService.RequestHook<SendServerToPlayerExamineGuiCreatureDataHook>(OnSendServerToPlayerExamineGuiCreatureData, FunctionsLinux._ZN11CNWSMessage41SendServerToPlayerExamineGui_CreatureDataEP10CNWSPlayerj, HookOrder.Early);
      sendServerToPlayerPlayModuleCharacterListResponseHook = hookService.RequestHook<SendServerToPlayerPlayModuleCharacterListResponseHook>(OnSendServerToPlayerPlayModuleCharacterListResponse, FunctionsLinux._ZN11CNWSMessage49SendServerToPlayerPlayModuleCharacterListResponseEjji, HookOrder.Early);
      sendServerToPlayerPlayerListAllHook = hookService.RequestHook<SendServerToPlayerPlayerListAllHook>(OnSendServerToPlayerPlayerListAll, FunctionsLinux._ZN11CNWSMessage32SendServerToPlayerPlayerList_AllEP10CNWSPlayer, HookOrder.Early);
      sendServerToPlayerPlayerListAddHook = hookService.RequestHook<SendServerToPlayerPlayerListAddHook>(OnSendServerToPlayerPlayerListAdd, FunctionsLinux._ZN11CNWSMessage32SendServerToPlayerPlayerList_AddEjP10CNWSPlayer, HookOrder.Early);
      sendServerToPlayerPlayerListDeleteHook = hookService.RequestHook<SendServerToPlayerPlayerListDeleteHook>(OnSendServerToPlayerPlayerListDelete, FunctionsLinux._ZN11CNWSMessage35SendServerToPlayerPlayerList_DeleteEjP10CNWSPlayer, HookOrder.Early);
      sendServerToPlayerDungeonMasterUpdatePartyListHook = hookService.RequestHook<SendServerToPlayerDungeonMasterUpdatePartyListHook>(OnSendServerToPlayerDungeonMasterUpdatePartyList, FunctionsLinux._ZN11CNWSMessage46SendServerToPlayerDungeonMasterUpdatePartyListEj, HookOrder.Early);
      sendServerToPlayerPopUpGUIPanelHook = hookService.RequestHook<SendServerToPlayerPopUpGUIPanelHook>(OnSendServerToPlayerPopUpGUIPanel, FunctionsLinux._ZN11CNWSMessage31SendServerToPlayerPopUpGUIPanelEjiiii10CExoString, HookOrder.Early);

      sendServerToPlayerChatPartyHook = hookService.RequestHook<SendServerToPlayerChatPartyHook>(OnSendServerToPlayerChatParty, FunctionsLinux._ZN11CNWSMessage28SendServerToPlayerChat_PartyEjj10CExoString, HookOrder.Early);
      sendServerToPlayerChatShoutHook = hookService.RequestHook<SendServerToPlayerChatShoutHook>(OnSendServerToPlayerChatShout, FunctionsLinux._ZN11CNWSMessage28SendServerToPlayerChat_ShoutEjj10CExoString, HookOrder.Early);
      sendServerToPlayerChatTellHook = hookService.RequestHook<SendServerToPlayerChatTellHook>(OnSendServerToPlayerChatTell, FunctionsLinux._ZN11CNWSMessage27SendServerToPlayerChat_TellEjj10CExoString, HookOrder.Early);
    }

    /// <summary>
    /// Gets the current name override for the specified player.
    /// </summary>
    /// <param name="target">The player whose name to query.</param>
    /// <param name="observer">The specific observer.</param>
    public void GetPlayerNameOverride(NwPlayer target, NwPlayer observer = null)
    {
    }

    /// <summary>
    /// Sets a player character name and community name on the player list. Is not persistent.
    /// </summary>
    /// <param name="target">The PC whose name is being overridden.</param>
    /// <param name="newName">The new name.</param>
    /// <param name="prefix">The prefix for their character name, sometimes used for a color code.</param>
    /// <param name="suffix">The suffix for their character name.</param>
    /// <param name="playerNameState">How to change the Community Name.</param>
    /// <param name="observer">If specified, the character name will appear to that specific observer as set, this overrides a global setting.</param>
    public void SetPlayerNameOverride(NwPlayer target, string newName, string prefix = "", string suffix = "", PlayerNameState playerNameState = PlayerNameState.Default, NwPlayer observer = null)
    {

    }

    /// <summary>
    /// Clears an overridden player character name.
    /// </summary>
    /// <param name="target">The player whose overridden name to clear, use null if you're clearing all overrides for an observer.</param>
    /// <param name="observer">The observer whose overriden name of target is being cleared. If oTarget is null then all overrides are cleared.</param>
    /// <param name="clearAll">If true, both the global and personal overrides will be cleared for that target player. Requires observer to be null.</param>
    public void ClearPlayerNameOverride(NwPlayer target, NwPlayer observer = null, bool clearAll = false)
    {

    }

    private void OnWriteGameObjUpdateUpdateObject(void* pMessage, void* pPlayer, void* pAreaObject, void* pLastUpdateObject, uint nObjectUpdatesRequired, uint nObjectAppearanceUpdatesRequired)
    {
      CNWSObject areaObject = CNWSObject.FromPointer(pAreaObject);

      CNWSPlayer targetPlayer = LowLevel.ServerExoApp.GetClientObjectByObjectId(areaObject.m_idSelf);
      CNWSPlayer observerPlayer = CNWSPlayer.FromPointer(pPlayer);

      SetOrRestorePlayerName(true, targetPlayer, observerPlayer);
      writeGameObjUpdateUpdateObjectHook.CallOriginal(pMessage, pPlayer, pAreaObject, pLastUpdateObject, nObjectUpdatesRequired, nObjectAppearanceUpdatesRequired);
      SetOrRestorePlayerName(false, targetPlayer, observerPlayer);
    }

    private int OnSendServerToPlayerExamineGuiCreatureData(void* pMessage, void* pPlayer, uint oidCreatureID)
    {
      CNWSPlayer targetPlayer = LowLevel.ServerExoApp.GetClientObjectByObjectId(oidCreatureID);
      CNWSPlayer observerPlayer = CNWSPlayer.FromPointer(pPlayer);

      SetOrRestorePlayerName(true, targetPlayer, observerPlayer);
      int retVal = sendServerToPlayerExamineGuiCreatureDataHook.CallOriginal(pMessage, pPlayer, oidCreatureID);
      SetOrRestorePlayerName(false, targetPlayer, observerPlayer);
      return retVal;
    }

    private int OnSendServerToPlayerPlayModuleCharacterListResponse(void* pMessage, uint nPlayerId, uint nCharacterId, int bAdd)
    {
      CNWSPlayer targetPlayer = LowLevel.ServerExoApp.GetClientObjectByPlayerId(nCharacterId).AsNWSPlayer();
      CNWSPlayer observerPlayer = LowLevel.ServerExoApp.GetClientObjectByPlayerId(nPlayerId, 0).AsNWSPlayer();

      SetOrRestorePlayerName(true, targetPlayer, observerPlayer);
      int retVal = sendServerToPlayerPlayModuleCharacterListResponseHook.CallOriginal(pMessage, nPlayerId, nCharacterId, bAdd);
      SetOrRestorePlayerName(false, targetPlayer, observerPlayer);

      return retVal;
    }

    private int OnSendServerToPlayerPlayerListAll(void* pMessage, void* pPlayer)
    {
      CNWSPlayer player = CNWSPlayer.FromPointer(pPlayer);

      GlobalNameChange(true, player.m_nPlayerID, PlayerIdConstants.AllPlayers);
      int retVal = sendServerToPlayerPlayerListAllHook.CallOriginal(pMessage, pPlayer);
      GlobalNameChange(false, player.m_nPlayerID, PlayerIdConstants.AllPlayers);

      return retVal;
    }

    private int OnSendServerToPlayerPlayerListAdd(void* pMessage, uint nPlayerId, void* pNewPlayer)
    {
      if (!RenameAllowDM && nPlayerId == PlayerIdConstants.AllGameMasters)
      {
        return sendServerToPlayerPlayerListAddHook.CallOriginal(pMessage, nPlayerId, pNewPlayer);
      }

      CNWSPlayer newPlayer = CNWSPlayer.FromPointer(pNewPlayer);

      GlobalNameChange(true, nPlayerId, newPlayer.m_nPlayerID);

      int retVal = sendServerToPlayerPlayerListAddHook.CallOriginal(pMessage, nPlayerId, pNewPlayer);
      renameAddedToPlayerList.Add(newPlayer.m_oidNWSObject);

      GlobalNameChange(false, nPlayerId, newPlayer.m_nPlayerID);

      return retVal;
    }

    private int OnSendServerToPlayerPlayerListDelete(void* pMessage, uint nPlayerId, void* pNewPlayer)
    {
      if (!RenameAllowDM && nPlayerId == PlayerIdConstants.AllGameMasters)
      {
        return sendServerToPlayerPlayerListDeleteHook.CallOriginal(pMessage, nPlayerId, pNewPlayer);
      }

      CNWSPlayer newPlayer = CNWSPlayer.FromPointer(pNewPlayer);
      renameAddedToPlayerList.Remove(newPlayer.m_oidNWSObject);

      return sendServerToPlayerPlayerListDeleteHook.CallOriginal(pMessage, nPlayerId, pNewPlayer);
    }

    private int OnSendServerToPlayerDungeonMasterUpdatePartyList(void* pMessage, uint nPlayerID)
    {
      GlobalNameChange(true, nPlayerID, PlayerIdConstants.AllPlayers);
      int retVal = sendServerToPlayerDungeonMasterUpdatePartyListHook.CallOriginal(pMessage, nPlayerID);
      GlobalNameChange(false, nPlayerID, PlayerIdConstants.AllPlayers);

      return retVal;
    }

    private int OnSendServerToPlayerPopUpGUIPanel(void* pMessage, uint observerOid, int nGuiPanel, int bGUIOption1, int bGUIOption2, int nStringReference, void* psStringReference)
    {
      if (nGuiPanel == 1) // Party invite popup
      {
        CNWSCreature observerCreature = LowLevel.ServerExoApp.GetCreatureByGameObjectID(observerOid);
        uint targetOid = observerCreature.m_oidInvitedToPartyBy;

        if (renamePlayerNames.TryGetValue(targetOid, out Dictionary<uint, RenamePlayerName> targetNames))
        {
          // This seems sketchy...
          if (targetNames.TryGetValue(observerOid, out RenamePlayerName name))
          {
            *psStringReference = *(void*)name.DisplayName.Pointer;
          }
          else if (targetNames.TryGetValue(NwObject.Invalid, out name))
          {
            *psStringReference = *(void*)name.DisplayName.Pointer;
          }
        }
      }

      return sendServerToPlayerPopUpGUIPanelHook.CallOriginal(pMessage, observerOid, nGuiPanel, bGUIOption1, bGUIOption2, nStringReference, psStringReference);
    }

    private int OnSendServerToPlayerChatParty(void* pMessage, uint nPlayerId, uint oidSpeaker, void* sSpeakerMessage)
    {
      CNWSPlayer targetPlayer = LowLevel.ServerExoApp.GetClientObjectByObjectId(oidSpeaker);
      CNWSPlayer observerPlayer = LowLevel.ServerExoApp.GetClientObjectByPlayerId(nPlayerId, 0).AsNWSPlayer();

      SetOrRestorePlayerName(true, targetPlayer, observerPlayer);
      int retVal = sendServerToPlayerChatPartyHook.CallOriginal(pMessage, nPlayerId, oidSpeaker, sSpeakerMessage);
      SetOrRestorePlayerName(false, targetPlayer, observerPlayer);

      return retVal;
    }

    private int OnSendServerToPlayerChatShout(void* pMessage, uint nPlayerId, uint oidSpeaker, void* sSpeakerMessage)
    {
      CNWSPlayer targetPlayer = LowLevel.ServerExoApp.GetClientObjectByObjectId(oidSpeaker);
      CNWSPlayer observerPlayer = LowLevel.ServerExoApp.GetClientObjectByPlayerId(nPlayerId, 0).AsNWSPlayer();

      SetOrRestorePlayerName(true, targetPlayer, observerPlayer);
      int retVal = sendServerToPlayerChatShoutHook.CallOriginal(pMessage, nPlayerId, oidSpeaker, sSpeakerMessage);
      SetOrRestorePlayerName(false, targetPlayer, observerPlayer);

      return retVal;
    }

    private int OnSendServerToPlayerChatTell(void* pMessage, uint nPlayerId, uint oidSpeaker, void* sSpeakerMessage)
    {
      CNWSPlayer targetPlayer = LowLevel.ServerExoApp.GetClientObjectByObjectId(oidSpeaker);
      CNWSPlayer observerPlayer = LowLevel.ServerExoApp.GetClientObjectByPlayerId(nPlayerId, 0).AsNWSPlayer();

      SetOrRestorePlayerName(true, targetPlayer, observerPlayer);
      int retVal = sendServerToPlayerChatTellHook.CallOriginal(pMessage, nPlayerId, oidSpeaker, sSpeakerMessage);
      SetOrRestorePlayerName(false, targetPlayer, observerPlayer);

      return retVal;
    }

    private void GlobalNameChange(bool before, uint observerPlayerId, uint targetPlayerId)
    {
      List<uint> observersToNotify = GetPlayersToNotify(observerPlayerId);
      List<uint> targetsToNotify = GetPlayersToNotify(targetPlayerId);

      CNetLayer netLayer = LowLevel.ServerExoApp.GetNetLayer();

      foreach (uint observerPid in observersToNotify)
      {
        foreach (uint targetPid in targetsToNotify)
        {
          CNWSPlayer targetPlayer = LowLevel.ServerExoApp.GetClientObjectByPlayerId(targetPid, 0).AsNWSPlayer();
          uint targetOid = targetPlayer.m_oidNWSObject;
          CNWSCreature targetCreature = LowLevel.ServerExoApp.GetCreatureByGameObjectID(targetOid);

          if (targetCreature != null && renamePlayerNames.TryGetValue(targetOid, out Dictionary<uint, RenamePlayerName> targetNames) && targetNames.TryGetValue(NwObject.Invalid, out RenamePlayerName name))
          {
            if (name.PlayerNameState != PlayerNameState.Default)
            {
              CNetLayerPlayerInfo playerInfo = netLayer.GetPlayerInfo(targetPid);
              if (!before)
              {
                playerInfo.m_sPlayerName = renameOriginalNames[targetOid].PlayerName;
              }
              else
              {
                switch (name.PlayerNameState)
                {
                  case PlayerNameState.Obfuscate:
                    playerInfo.m_sPlayerName = GenerateRandomPlayerName(7, targetOid).ToExoString();
                    break;
                  case PlayerNameState.Override:
                    playerInfo.m_sPlayerName = name.OverrideName;
                    break;
                  case PlayerNameState.Anonymous:
                    playerInfo.m_sPlayerName = AnonymousName.ToExoString();
                    break;
                  default:
                    playerInfo.m_sPlayerName = renameOriginalNames[targetOid].PlayerName;
                    break;
                }
              }
            }
          }

          CNWSPlayer observerPlayer = LowLevel.ServerExoApp.GetClientObjectByPlayerId(observerPid, 0).AsNWSPlayer();
          SetOrRestorePlayerName(before, targetPlayer, observerPlayer, true);
        }
      }
    }

    private List<uint> GetPlayersToNotify(uint playerId)
    {
      CExoLinkedListInternal playerList = LowLevel.ServerExoApp.m_pcExoAppInternal.m_pNWSPlayerList.m_pcExoLinkedListInternal;
      List<uint> playersToNotify = new List<uint>();

      if (playerId is PlayerIdConstants.AllPlayers or PlayerIdConstants.AllGameMasters)
      {
        for (CExoLinkedListNode node = playerList.pHead; node != null; node = node.pNext)
        {
          CNWSPlayer player = CNWSPlayer.FromPointer(node.pObject);
          if (playerId == PlayerIdConstants.AllGameMasters && player.m_nCharacterType == (byte)CharacterType.DM ||
            playerId == PlayerIdConstants.AllPlayers && player.m_nCharacterType != (byte)CharacterType.DM)
          {
            playersToNotify.Add(player.m_nPlayerID);
          }
        }
      }
      else
      {
        playersToNotify.Add(playerId);
      }

      return playersToNotify;
    }

    private void SetOrRestorePlayerName(bool before, CNWSPlayer targetPlayer, CNWSPlayer observerPlayer, bool playerList = false)
    {
      if (targetPlayer == null || observerPlayer == null)
      {
        return;
      }

      CNWSCreature targetCreature = targetPlayer.m_oidNWSObject.ToNwObject<NwCreature>();
      CNWSCreature observerCreature = observerPlayer.m_oidNWSObject.ToNwObject<NwCreature>();

      uint observerOid = observerCreature != null ? observerPlayer.m_oidNWSObject : NwObject.Invalid;
      if (!IsValidCreature(targetCreature) || !IsValidCreature(observerCreature))
      {
        return;
      }

      if (before)
      {
        SetPlayerNameAsObservedBy(targetCreature, observerOid, playerList);
      }
      else
      {
        RestorePlayerName(targetCreature, playerList);
      }
    }

    private bool IsValidCreature(CNWSCreature creature)
    {
      return creature != null && creature.m_pStats != null && (RenameAllowDM ||
        !creature.m_pStats.GetIsDM().ToBool() &&
        creature.m_nAssociateType != 7 &&
        creature.m_nAssociateType != 8);
    }

    private void SetPlayerNameAsObservedBy(CNWSCreature targetCreature, uint observerOid, bool playerList)
    {
      uint targetOid = targetCreature.m_idSelf;
      if (!renamePlayerNames.ContainsKey(targetOid))
      {
        return;
      }

      if (renamePlayerNames[targetOid].TryGetValue(observerOid, out RenamePlayerName name))
      {
        ApplyOverride(targetCreature, name, playerList);
      }
      else if (renamePlayerNames[targetOid].TryGetValue(NwObject.Invalid, out name))
      {
        ApplyOverride(targetCreature, name, playerList);
      }
    }

    private void ApplyOverride(CNWSCreature creature, RenamePlayerName name, bool playerList)
    {
      if (playerList)
      {
        creature.m_pStats.m_lsFirstName = name.OverrideName.ToExoLocString();
        creature.m_pStats.m_lsLastName = null;
      }

      creature.m_sDisplayName = name.DisplayName;
    }

    private void RestorePlayerName(CNWSCreature targetCreature, bool playerList)
    {
      if (renameOriginalNames.TryGetValue(targetCreature.m_idSelf, out RenameOriginalName originalName))
      {
        if (playerList)
        {
          targetCreature.m_pStats.m_lsFirstName = originalName.FirstName;
          targetCreature.m_pStats.m_lsLastName = originalName.LastName;
        }

        if (RenameOverwriteDisplayName && renamePlayerNames[targetCreature.m_idSelf].TryGetValue(NwObject.Invalid, out RenamePlayerName name))
        {
          targetCreature.m_sDisplayName = name.DisplayName;
        }
        else
        {
          targetCreature.m_sDisplayName = null;
        }
      }
    }

    private string GenerateRandomPlayerName(int length, uint targetOid)
    {

    }
  }
}
