using System.Collections.Generic;
using System.Linq;
using Anvil.API;
using Anvil.API.Events;
using Anvil.Internal;
using Anvil.Native;
using NLog;
using NWN.Native.API;

namespace Anvil.Services
{
  [ServiceBinding(typeof(PlayerNameOverrideService))]
  [ServiceBindingOptions(InternalBindingPriority.API, Lazy = true)]
  public sealed unsafe class PlayerNameOverrideService
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private readonly Dictionary<NwPlayer, PlayerNameOverride> globalNameOverrides = new Dictionary<NwPlayer, PlayerNameOverride>();
    private readonly Dictionary<NwPlayer, Dictionary<NwPlayer, PlayerNameOverride>> perPlayerOverrides = new Dictionary<NwPlayer, Dictionary<NwPlayer, PlayerNameOverride>>();
    private readonly HashSet<NwPlayer> renameAddedToPlayerList = new HashSet<NwPlayer>();
    private readonly Dictionary<NwPlayer, OriginalNames> renameOriginalNames = new Dictionary<NwPlayer, OriginalNames>();

    private readonly FunctionHook<Functions.CNWSMessage.SendServerToPlayerChat_Party> sendServerToPlayerChatPartyHook;
    private readonly FunctionHook<Functions.CNWSMessage.SendServerToPlayerChat_Shout> sendServerToPlayerChatShoutHook;
    private readonly FunctionHook<Functions.CNWSMessage.SendServerToPlayerChat_Tell> sendServerToPlayerChatTellHook;
    private readonly FunctionHook<Functions.CNWSMessage.SendServerToPlayerDungeonMasterUpdatePartyList> sendServerToPlayerDungeonMasterUpdatePartyListHook;
    private readonly FunctionHook<Functions.CNWSMessage.SendServerToPlayerExamineGui_CreatureData> sendServerToPlayerExamineGuiCreatureDataHook;
    private readonly FunctionHook<Functions.CNWSMessage.SendServerToPlayerPlayerList_Add> sendServerToPlayerPlayerListAddHook;
    private readonly FunctionHook<Functions.CNWSMessage.SendServerToPlayerPlayerList_All> sendServerToPlayerPlayerListAllHook;
    private readonly FunctionHook<Functions.CNWSMessage.SendServerToPlayerPlayerList_Delete> sendServerToPlayerPlayerListDeleteHook;
    private readonly FunctionHook<Functions.CNWSMessage.SendServerToPlayerPopUpGUIPanel> sendServerToPlayerPopUpGUIPanelHook;

    private readonly FunctionHook<Functions.CNWSMessage.WriteGameObjUpdate_UpdateObject> writeGameObjUpdateUpdateObjectHook;

    public PlayerNameOverrideService(HookService hookService)
    {
      NwModule.Instance.OnClientLeave += OnClientLeave;

      writeGameObjUpdateUpdateObjectHook = hookService.RequestHook<Functions.CNWSMessage.WriteGameObjUpdate_UpdateObject>(OnWriteGameObjUpdateUpdateObject, HookOrder.Early);
      sendServerToPlayerExamineGuiCreatureDataHook = hookService.RequestHook<Functions.CNWSMessage.SendServerToPlayerExamineGui_CreatureData>(OnSendServerToPlayerExamineGuiCreatureData, HookOrder.Early);

      sendServerToPlayerPlayerListAllHook = hookService.RequestHook<Functions.CNWSMessage.SendServerToPlayerPlayerList_All>(OnSendServerToPlayerPlayerListAll, HookOrder.Early);
      sendServerToPlayerPlayerListAddHook = hookService.RequestHook<Functions.CNWSMessage.SendServerToPlayerPlayerList_Add>(OnSendServerToPlayerPlayerListAdd, HookOrder.Early);
      sendServerToPlayerPlayerListDeleteHook = hookService.RequestHook<Functions.CNWSMessage.SendServerToPlayerPlayerList_Delete>(OnSendServerToPlayerPlayerListDelete, HookOrder.Early);

      sendServerToPlayerDungeonMasterUpdatePartyListHook = hookService.RequestHook<Functions.CNWSMessage.SendServerToPlayerDungeonMasterUpdatePartyList>(OnSendServerToPlayerDungeonMasterUpdatePartyList, HookOrder.Early);
      sendServerToPlayerPopUpGUIPanelHook = hookService.RequestHook<Functions.CNWSMessage.SendServerToPlayerPopUpGUIPanel>(OnSendServerToPlayerPopUpGUIPanel, HookOrder.Early);

      sendServerToPlayerChatPartyHook = hookService.RequestHook<Functions.CNWSMessage.SendServerToPlayerChat_Party>(OnSendServerToPlayerChatParty, HookOrder.Early);
      sendServerToPlayerChatShoutHook = hookService.RequestHook<Functions.CNWSMessage.SendServerToPlayerChat_Shout>(OnSendServerToPlayerChatShout, HookOrder.Early);
      sendServerToPlayerChatTellHook = hookService.RequestHook<Functions.CNWSMessage.SendServerToPlayerChat_Tell>(OnSendServerToPlayerChatTell, HookOrder.Early);
    }

    /// <summary>
    /// Gets or sets if global overrides change the underlying display name value. This applies the name change globally to scripts, and DMs.<br/>
    /// When set to false, the override name is only visible to players - scripts and DMs see the original names unless <see cref="ShowOverridesToDM"/> is set to true.
    /// </summary>
    public bool OverwriteDisplayName { get; set; } = false;

    /// <summary>
    /// Gets or sets the type of name override to use from the in-game player list.
    /// </summary>
    public OverrideNameType PlayerListNameType { get; set; } = OverrideNameType.Player;

    /// <summary>
    /// DM observers will see global or personal overrides as well as being able to have their own name overridden for other observers.
    /// </summary>
    public bool ShowOverridesToDM { get; set; } = false;

    /// <summary>
    /// Clears an overridden player character name.
    /// </summary>
    /// <param name="target">The player whose name should be reset to default.</param>
    /// <param name="clearAll">If true, both global and any personal overrides will be cleared for that target player.</param>
    public void ClearPlayerNameOverride(NwPlayer target, bool clearAll = false)
    {
      globalNameOverrides.Remove(target);

      if (clearAll)
      {
        perPlayerOverrides.Remove(target);
      }

      SendNameUpdateToAllPlayers(target);
    }

    /// <summary>
    /// Clears an overridden player character name for a specific observer.
    /// </summary>
    /// <param name="target">The player whose overridden name to clear, use null if you're clearing all overrides for an observer.</param>
    /// <param name="observer">The observer whose overriden name of target is being cleared.</param>
    public void ClearPlayerNameOverride(NwPlayer target, NwPlayer observer)
    {
      if (perPlayerOverrides.TryGetValue(target, out Dictionary<NwPlayer, PlayerNameOverride>? playerOverrides))
      {
        playerOverrides.Remove(observer);
      }

      SendNameUpdate(target, observer);
    }

    /// <summary>
    /// Gets a list of all name overrides for the specified observer.
    /// </summary>
    /// <param name="observer">The observer to query.</param>
    /// <param name="includeGlobal">True if global overrides should be included in the returned map.</param>
    /// <returns>A dictionary containing the name overrides for the specified observer.</returns>
    public Dictionary<NwPlayer, PlayerNameOverride> GetOverridesForObserver(NwPlayer observer, bool includeGlobal = false)
    {
      Dictionary<NwPlayer, PlayerNameOverride> nameOverrides = includeGlobal ? new Dictionary<NwPlayer, PlayerNameOverride>(globalNameOverrides) : new Dictionary<NwPlayer, PlayerNameOverride>();
      foreach ((NwPlayer key, Dictionary<NwPlayer, PlayerNameOverride> value) in perPlayerOverrides)
      {
        if (value.TryGetValue(observer, out PlayerNameOverride? nameOverride))
        {
          nameOverrides[key] = nameOverride;
        }
      }

      return nameOverrides;
    }

    /// <summary>
    /// Gets the current name override for the specified player.
    /// </summary>
    /// <param name="target">The player whose name to query.</param>
    /// <param name="observer">The specific observer.</param>
    public PlayerNameOverride? GetPlayerNameOverride(NwPlayer? target, NwPlayer? observer = null)
    {
      if (target == null)
      {
        return null;
      }

      if (observer != null && perPlayerOverrides.TryGetValue(target, out Dictionary<NwPlayer, PlayerNameOverride>? playerOverrides) && playerOverrides.TryGetValue(observer, out PlayerNameOverride? retVal))
      {
        return retVal;
      }

      if (globalNameOverrides.TryGetValue(target, out retVal))
      {
        return retVal;
      }

      return null;
    }

    /// <summary>
    /// Sets an override player character name and community name on the player list for all players. Is not persistent.
    /// </summary>
    /// <param name="target">The player whose name is being overridden.</param>
    /// <param name="nameOverride">The new names for the player.</param>
    public void SetPlayerNameOverride(NwPlayer target, PlayerNameOverride nameOverride)
    {
      globalNameOverrides[target] = nameOverride;
      CacheOriginalNames(target);

      // If we've ran this before the PC has even been added to the other clients' player list then there's
      // nothing else we need to do, the hooks will take care of doing the renames. If we don't skip this
      // then the SendServerToPlayerPlayerList_All in the SendNameUpdate below runs before the server has even ran a
      // SendServerToPlayerPlayerList_Add and weird things happen(tm)
      if (PlayerListNameType != OverrideNameType.Original && renameAddedToPlayerList.Contains(target))
      {
        return;
      }

      SendNameUpdateToAllPlayers(target);
    }

    /// <summary>
    /// Sets an override player character name and community name on the player list as observed by a specific player. Is not persistent.
    /// </summary>
    /// <param name="target">The player whose name is being overridden.</param>
    /// <param name="nameOverride">The new names for the player.</param>
    /// <param name="observer">The observer to see the new names.</param>
    public void SetPlayerNameOverride(NwPlayer target, PlayerNameOverride nameOverride, NwPlayer observer)
    {
      if (!perPlayerOverrides.TryGetValue(target, out Dictionary<NwPlayer, PlayerNameOverride>? playerOverrides))
      {
        playerOverrides = new Dictionary<NwPlayer, PlayerNameOverride>();
        perPlayerOverrides[target] = playerOverrides;
      }

      playerOverrides[observer] = nameOverride;
      CacheOriginalNames(target);

      // If we've ran this before the PC has even been added to the other clients' player list then there's
      // nothing else we need to do, the hooks will take care of doing the renames. If we don't skip this
      // then the SendServerToPlayerPlayerList_All in the SendNameUpdate below runs before the server has even ran a
      // SendServerToPlayerPlayerList_Add and weird things happen(tm)
      if (PlayerListNameType != OverrideNameType.Original && renameAddedToPlayerList.Contains(target))
      {
        return;
      }

      SendNameUpdate(target, observer);
    }

    private void ApplyNameOverride(NwPlayer? targetPlayer, PlayerNameOverride? nameOverride, OverrideNameType nameType)
    {
      if (nameOverride == null || targetPlayer == null || !IsValidCreature(targetPlayer.ControlledCreature))
      {
        return;
      }

      CNWSCreature targetCreature = targetPlayer.ControlledCreature!;
      switch (nameType)
      {
        case OverrideNameType.Character:
          targetCreature.m_pStats.m_lsFirstName = nameOverride.CharacterNameInternal.ToExoLocString();
          targetCreature.m_pStats.m_lsLastName = "".ToExoLocString();
          break;
        case OverrideNameType.Player:
          targetCreature.m_pStats.m_lsFirstName = nameOverride.PlayerNameInternal.ToExoLocString();
          targetCreature.m_pStats.m_lsLastName = "".ToExoLocString();
          break;
      }

      targetCreature.m_sDisplayName = nameOverride.CharacterNameInternal;
    }

    private void ApplyObserverOverrides(Dictionary<NwPlayer, PlayerNameOverride> nameOverrides, OverrideNameType nameType)
    {
      foreach ((NwPlayer? targetPlayer, PlayerNameOverride? nameOverride) in nameOverrides)
      {
        ApplyNameOverride(targetPlayer, nameOverride, nameType);
      }
    }

    private void CacheOriginalNames(NwPlayer player)
    {
      if (renameOriginalNames.ContainsKey(player))
      {
        return;
      }

      CNWSCreature? creature = player.LoginCreature?.Creature;
      if (creature == null)
      {
        return;
      }

      OriginalNames originalNames = new OriginalNames
      {
        FirstName = creature.m_pStats.m_lsFirstName,
        LastName = creature.m_pStats.m_lsLastName,
        PlayerName = LowLevel.ServerExoApp.GetNetLayer().GetPlayerInfo(player.Player.m_nPlayerID).m_sPlayerName,
      };

      renameOriginalNames[player] = originalNames;
    }

    private bool IsCreatureInLastUpdateObjectList(NwPlayer observer, NwPlayer target)
    {
      CNWSPlayer nwPlayer = observer.Player;
      CLastUpdateObject lastUpdateObj = nwPlayer.GetLastUpdateObject(target.ControlledCreature);
      if (lastUpdateObj != null)
      {
        return true;
      }

      return nwPlayer.m_lstActivePartyObjectsLastUpdate.Any(partyObject => partyObject.m_nPlayerId == target.ControlledCreature?.ObjectId);
    }

    private bool IsValidCreature(CNWSCreature? creature)
    {
      return creature != null && creature.m_pStats != null && (ShowOverridesToDM ||
        !creature.m_pStats.GetIsDM().ToBool() &&
        creature.m_nAssociateType != 7 &&
        creature.m_nAssociateType != 8);
    }

    private void OnClientLeave(ModuleEvents.OnClientLeave eventData)
    {
      ClearPlayerNameOverride(eventData.Player, true);
      renameOriginalNames.Remove(eventData.Player);
      renameAddedToPlayerList.Remove(eventData.Player);
    }

    private int OnSendServerToPlayerChatParty(void* pMessage, uint nPlayerId, uint oidSpeaker, void* sSpeakerMessage)
    {
      NwPlayer? targetPlayer = oidSpeaker.ToNwPlayer(PlayerSearch.Controlled);
      NwPlayer? observerPlayer = NwPlayer.FromPlayerId(nPlayerId);
      PlayerNameOverride? nameOverride = GetPlayerNameOverride(targetPlayer, observerPlayer);

      ApplyNameOverride(targetPlayer, nameOverride, OverrideNameType.Original);
      int retVal = sendServerToPlayerChatPartyHook.CallOriginal(pMessage, nPlayerId, oidSpeaker, sSpeakerMessage);
      RestoreNameOverride(targetPlayer, nameOverride, OverrideNameType.Original);

      return retVal;
    }

    private int OnSendServerToPlayerChatShout(void* pMessage, uint nPlayerId, uint oidSpeaker, void* sSpeakerMessage)
    {
      NwPlayer? targetPlayer = oidSpeaker.ToNwPlayer(PlayerSearch.Controlled);
      NwPlayer? observerPlayer = NwPlayer.FromPlayerId(nPlayerId);
      PlayerNameOverride? nameOverride = GetPlayerNameOverride(targetPlayer, observerPlayer);

      ApplyNameOverride(targetPlayer, nameOverride, OverrideNameType.Original);
      int retVal = sendServerToPlayerChatShoutHook.CallOriginal(pMessage, nPlayerId, oidSpeaker, sSpeakerMessage);
      RestoreNameOverride(targetPlayer, nameOverride, OverrideNameType.Original);

      return retVal;
    }

    private int OnSendServerToPlayerChatTell(void* pMessage, uint nPlayerId, uint oidSpeaker, void* sSpeakerMessage)
    {
      NwPlayer? targetPlayer = oidSpeaker.ToNwPlayer(PlayerSearch.Controlled);
      NwPlayer? observerPlayer = NwPlayer.FromPlayerId(nPlayerId);
      PlayerNameOverride? nameOverride = GetPlayerNameOverride(targetPlayer, observerPlayer);

      ApplyNameOverride(targetPlayer, nameOverride, OverrideNameType.Original);
      int retVal = sendServerToPlayerChatTellHook.CallOriginal(pMessage, nPlayerId, oidSpeaker, sSpeakerMessage);
      RestoreNameOverride(targetPlayer, nameOverride, OverrideNameType.Original);

      return retVal;
    }

    private int OnSendServerToPlayerDungeonMasterUpdatePartyList(void* pMessage, uint nPlayerID)
    {
      NwPlayer? observer = NwPlayer.FromPlayerId(nPlayerID);
      if (observer == null)
      {
        return sendServerToPlayerDungeonMasterUpdatePartyListHook.CallOriginal(pMessage, nPlayerID);
      }

      Dictionary<NwPlayer, PlayerNameOverride> nameOverrides = GetOverridesForObserver(observer, true);

      ApplyObserverOverrides(nameOverrides, PlayerListNameType);
      int retVal = sendServerToPlayerDungeonMasterUpdatePartyListHook.CallOriginal(pMessage, nPlayerID);
      RestoreObserverOverrides(nameOverrides, PlayerListNameType);

      return retVal;
    }

    private int OnSendServerToPlayerExamineGuiCreatureData(void* pMessage, void* pPlayer, uint oidCreatureID)
    {
      NwPlayer? targetPlayer = oidCreatureID.ToNwPlayer(PlayerSearch.Controlled);
      NwPlayer? observerPlayer = CNWSPlayer.FromPointer(pPlayer).ToNwPlayer();
      PlayerNameOverride? nameOverride = GetPlayerNameOverride(targetPlayer, observerPlayer);

      ApplyNameOverride(targetPlayer, nameOverride, OverrideNameType.Original);
      int retVal = sendServerToPlayerExamineGuiCreatureDataHook.CallOriginal(pMessage, pPlayer, oidCreatureID);
      RestoreNameOverride(targetPlayer, nameOverride, OverrideNameType.Original);
      return retVal;
    }

    private int OnSendServerToPlayerPlayerListAdd(void* pMessage, uint nPlayerId, void* pNewPlayer)
    {
      if (PlayerListNameType == OverrideNameType.Original)
      {
        return sendServerToPlayerPlayerListAddHook.CallOriginal(pMessage, nPlayerId, pNewPlayer);
      }

      if (!ShowOverridesToDM && nPlayerId == PlayerIdConstants.AllGameMasters)
      {
        return sendServerToPlayerPlayerListAddHook.CallOriginal(pMessage, nPlayerId, pNewPlayer);
      }

      NwPlayer? observer = NwPlayer.FromPlayerId(nPlayerId);
      NwPlayer? target = CNWSPlayer.FromPointer(pNewPlayer).ToNwPlayer();

      if (target == null)
      {
        return sendServerToPlayerPlayerListAddHook.CallOriginal(pMessage, nPlayerId, pNewPlayer);
      }

      PlayerNameOverride? nameOverride = GetPlayerNameOverride(target, observer);

      ApplyNameOverride(target, nameOverride, PlayerListNameType);
      int retVal = sendServerToPlayerPlayerListAddHook.CallOriginal(pMessage, nPlayerId, pNewPlayer);
      renameAddedToPlayerList.Add(target);
      RestoreNameOverride(target, nameOverride, PlayerListNameType);

      return retVal;
    }

    private int OnSendServerToPlayerPlayerListAll(void* pMessage, void* pPlayer)
    {
      if (PlayerListNameType == OverrideNameType.Original)
      {
        return sendServerToPlayerPlayerListAllHook.CallOriginal(pMessage, pPlayer);
      }

      NwPlayer? observer = CNWSPlayer.FromPointer(pPlayer).ToNwPlayer();
      if (observer == null)
      {
        return sendServerToPlayerPlayerListAllHook.CallOriginal(pMessage, pPlayer);
      }

      Dictionary<NwPlayer, PlayerNameOverride> nameOverrides = GetOverridesForObserver(observer, true);

      ApplyObserverOverrides(nameOverrides, PlayerListNameType);
      int retVal = sendServerToPlayerPlayerListAllHook.CallOriginal(pMessage, pPlayer);
      RestoreObserverOverrides(nameOverrides, PlayerListNameType);

      return retVal;
    }

    private int OnSendServerToPlayerPlayerListDelete(void* pMessage, uint nPlayerId, void* pNewPlayer)
    {
      if (PlayerListNameType == OverrideNameType.Original)
      {
        return sendServerToPlayerPlayerListDeleteHook.CallOriginal(pMessage, nPlayerId, pNewPlayer);
      }

      if (!ShowOverridesToDM && nPlayerId == PlayerIdConstants.AllGameMasters)
      {
        return sendServerToPlayerPlayerListDeleteHook.CallOriginal(pMessage, nPlayerId, pNewPlayer);
      }

      NwPlayer? newPlayer = CNWSPlayer.FromPointer(pNewPlayer).ToNwPlayer();
      if (newPlayer != null)
      {
        renameAddedToPlayerList.Remove(newPlayer);
      }

      return sendServerToPlayerPlayerListDeleteHook.CallOriginal(pMessage, nPlayerId, pNewPlayer);
    }

    private int OnSendServerToPlayerPopUpGUIPanel(void* pMessage, uint observerOid, int nGuiPanel, int bGUIOption1, int bGUIOption2, int nStringReference, void** psStringReference)
    {
      if (nGuiPanel == 1) // Party invite popup
      {
        CNWSCreature? observerCreature = LowLevel.ServerExoApp.GetCreatureByGameObjectID(observerOid);
        NwPlayer? observerPlayer = observerCreature?.ToNwObject<NwCreature>()?.ControllingPlayer;
        NwPlayer? targetPlayer = observerCreature?.m_oidInvitedToPartyBy.ToNwPlayer();

        if (targetPlayer != null)
        {
          PlayerNameOverride? name = GetPlayerNameOverride(targetPlayer, observerPlayer);
          if (name != null)
          {
            *psStringReference = (void*)name.CharacterNameInternal.Pointer;
          }
        }
      }

      return sendServerToPlayerPopUpGUIPanelHook.CallOriginal(pMessage, observerOid, nGuiPanel, bGUIOption1, bGUIOption2, nStringReference, psStringReference);
    }

    private void OnWriteGameObjUpdateUpdateObject(void* pMessage, void* pPlayer, void* pAreaObject, void* pLastUpdateObject, uint nObjectUpdatesRequired, uint nObjectAppearanceUpdatesRequired)
    {
      CNWSObject areaObject = CNWSObject.FromPointer(pAreaObject);

      NwPlayer? targetPlayer = areaObject.m_idSelf.ToNwPlayer(PlayerSearch.Controlled);
      NwPlayer? observerPlayer = CNWSPlayer.FromPointer(pPlayer).ToNwPlayer();
      PlayerNameOverride? nameOverride = GetPlayerNameOverride(targetPlayer, observerPlayer);

      ApplyNameOverride(targetPlayer, nameOverride, OverrideNameType.Original);
      writeGameObjUpdateUpdateObjectHook.CallOriginal(pMessage, pPlayer, pAreaObject, pLastUpdateObject, nObjectUpdatesRequired, nObjectAppearanceUpdatesRequired);
      RestoreNameOverride(targetPlayer, nameOverride, OverrideNameType.Original);
    }

    private void RestoreNameOverride(NwPlayer? targetPlayer, PlayerNameOverride? nameOverride, OverrideNameType nameType)
    {
      if (nameOverride == null || targetPlayer == null || !IsValidCreature(targetPlayer.ControlledCreature))
      {
        return;
      }

      CNWSCreature? targetCreature = targetPlayer.ControlledCreature;
      if (targetCreature == null)
      {
        return;
      }

      if (renameOriginalNames.TryGetValue(targetPlayer, out OriginalNames? originalName))
      {
        if (nameType != OverrideNameType.Original)
        {
          targetCreature.m_pStats.m_lsFirstName = originalName.FirstName;
          targetCreature.m_pStats.m_lsLastName = originalName.LastName;
        }

        targetCreature.m_sDisplayName = OverwriteDisplayName ? nameOverride.CharacterNameInternal : "".ToExoString();
      }
    }

    private void RestoreObserverOverrides(Dictionary<NwPlayer, PlayerNameOverride> nameOverrides, OverrideNameType nameType)
    {
      foreach ((NwPlayer? targetPlayer, PlayerNameOverride? nameOverride) in nameOverrides)
      {
        RestoreNameOverride(targetPlayer, nameOverride, nameType);
      }
    }

    private void SendNameUpdate(NwPlayer targetPlayer, NwPlayer observerPlayer)
    {
      bool success = false;
      CNWSMessage message = LowLevel.ServerExoApp.GetNWSMessage();

      uint observerPlayerId = observerPlayer.PlayerId;

      // The client may crash if we send an object update for a creature that does not exist in its
      // last update object list
      if (!IsCreatureInLastUpdateObjectList(observerPlayer, targetPlayer))
      {
        return;
      }

      if (ShowOverridesToDM || !observerPlayer.IsDM)
      {
        // Write a message notifying an object update.
        message.CreateWriteMessage(0x400, observerPlayerId, 1);

        // We don't need one for our update.
        // However, the appearance update is contingent on receiving a pointer which isn't nullptr.
        CLastUpdateObject lastUpdateObj = CLastUpdateObject.FromPointer((void*)0xDEADBEEF);
        message.WriteGameObjUpdate_UpdateObject(observerPlayer, targetPlayer.ControlledCreature, lastUpdateObj, 0, 0x400);

        byte* data = null;
        uint size = 0;

        if (message.GetWriteMessage(&data, &size).ToBool() && size != 0)
        {
          message.SendServerToPlayerMessage(observerPlayerId, (byte)MessageMajor.GameObjectUpdate, (byte)MessageGameObjectUpdateMinor.ObjectList, data, size);
          success = true;
        }

        if (PlayerListNameType != OverrideNameType.Original)
        {
          message.SendServerToPlayerPlayerList_All(observerPlayer);
        }
      }

      if (success == false)
      {
        Log.Warn($"Sending name update message for observer {observerPlayer.PlayerName}, target {targetPlayer.PlayerName} failed.");
      }
    }

    private void SendNameUpdateToAllPlayers(NwPlayer targetPlayer)
    {
      Dictionary<NwPlayer, PlayerNameOverride>? playerOverrides = perPlayerOverrides.TryGetValue(targetPlayer, out playerOverrides) ? playerOverrides : null;
      foreach (NwPlayer observerPlayer in NwModule.Instance.Players)
      {
        // If the observer has a personal override of the target's name then skip
        if (playerOverrides == null || !playerOverrides.ContainsKey(observerPlayer))
        {
          SendNameUpdate(targetPlayer, observerPlayer);
        }
      }
    }
  }
}
