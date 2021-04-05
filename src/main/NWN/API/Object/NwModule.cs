using System;
using System.Collections.Generic;
using System.Linq;
using NWN.API.Constants;
using NWN.API.Events;
using NWN.Core;
using NWN.Native.API;

namespace NWN.API
{
  [NativeObjectInfo(0, ObjectType.Module)]
  public sealed class NwModule : NwObject
  {
    internal readonly CNWSModule Module;

    internal NwModule(uint objectId, CNWSModule module) : base(objectId)
    {
      this.Module = module;
    }

    public static implicit operator CNWSModule(NwModule module)
    {
      return module?.Module;
    }

    public static readonly NwModule Instance = new NwModule(NWScript.GetModule(), LowLevel.ServerExoApp.GetModule());

    /// <inheritdoc cref="NWN.API.Events.ModuleEvents.OnAcquireItem"/>
    public event Action<ModuleEvents.OnAcquireItem> OnAcquireItem
    {
      add => EventService.SubscribeAll<ModuleEvents.OnAcquireItem, GameEventFactory>(value)
        .Register<ModuleEvents.OnAcquireItem>(this);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnAcquireItem, GameEventFactory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.ModuleEvents.OnActivateItem"/>
    public event Action<ModuleEvents.OnActivateItem> OnActivateItem
    {
      add => EventService.SubscribeAll<ModuleEvents.OnActivateItem, GameEventFactory>(value)
        .Register<ModuleEvents.OnActivateItem>(this);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnActivateItem, GameEventFactory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.ModuleEvents.OnClientEnter"/>
    public event Action<ModuleEvents.OnClientEnter> OnClientEnter
    {
      add => EventService.SubscribeAll<ModuleEvents.OnClientEnter, GameEventFactory>(value)
        .Register<ModuleEvents.OnClientEnter>(this);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnClientEnter, GameEventFactory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.ModuleEvents.OnClientLeave"/>
    public event Action<ModuleEvents.OnClientLeave> OnClientLeave
    {
      add => EventService.SubscribeAll<ModuleEvents.OnClientLeave, GameEventFactory>(value)
        .Register<ModuleEvents.OnClientLeave>(this);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnClientLeave, GameEventFactory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.ModuleEvents.OnCutsceneAbort"/>
    public event Action<ModuleEvents.OnCutsceneAbort> OnCutsceneAbort
    {
      add => EventService.SubscribeAll<ModuleEvents.OnCutsceneAbort, GameEventFactory>(value)
        .Register<ModuleEvents.OnCutsceneAbort>(this);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnCutsceneAbort, GameEventFactory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.ModuleEvents.OnHeartbeat"/>
    public event Action<ModuleEvents.OnHeartbeat> OnHeartbeat
    {
      add => EventService.SubscribeAll<ModuleEvents.OnHeartbeat, GameEventFactory>(value)
        .Register<ModuleEvents.OnHeartbeat>(this);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnHeartbeat, GameEventFactory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.ModuleEvents.OnModuleLoad"/>
    public event Action<ModuleEvents.OnModuleLoad> OnModuleLoad
    {
      add => EventService.SubscribeAll<ModuleEvents.OnModuleLoad, GameEventFactory>(value)
        .Register<ModuleEvents.OnModuleLoad>(this);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnModuleLoad, GameEventFactory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.ModuleEvents.OnModuleStart"/>
    public event Action<ModuleEvents.OnModuleStart> OnModuleStart
    {
      add => EventService.SubscribeAll<ModuleEvents.OnModuleStart, GameEventFactory>(value)
        .Register<ModuleEvents.OnModuleStart>(this);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnModuleStart, GameEventFactory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.ModuleEvents.OnPlayerChat"/>
    public event Action<ModuleEvents.OnPlayerChat> OnPlayerChat
    {
      add => EventService.SubscribeAll<ModuleEvents.OnPlayerChat, GameEventFactory>(value)
        .Register<ModuleEvents.OnPlayerChat>(this);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnPlayerChat, GameEventFactory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.ModuleEvents.OnPlayerDeath"/>
    public event Action<ModuleEvents.OnPlayerDeath> OnPlayerDeath
    {
      add => EventService.SubscribeAll<ModuleEvents.OnPlayerDeath, GameEventFactory>(value)
        .Register<ModuleEvents.OnPlayerDeath>(this);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnPlayerDeath, GameEventFactory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.ModuleEvents.OnPlayerDying"/>
    public event Action<ModuleEvents.OnPlayerDying> OnPlayerDying
    {
      add => EventService.SubscribeAll<ModuleEvents.OnPlayerDying, GameEventFactory>(value)
        .Register<ModuleEvents.OnPlayerDying>(this);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnPlayerDying, GameEventFactory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.ModuleEvents.OnPlayerEquipItem"/>
    public event Action<ModuleEvents.OnPlayerEquipItem> OnPlayerEquipItem
    {
      add => EventService.SubscribeAll<ModuleEvents.OnPlayerEquipItem, GameEventFactory>(value)
        .Register<ModuleEvents.OnPlayerEquipItem>(this);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnPlayerEquipItem, GameEventFactory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.ModuleEvents.OnPlayerLevelUp"/>
    public event Action<ModuleEvents.OnPlayerLevelUp> OnPlayerLevelUp
    {
      add => EventService.SubscribeAll<ModuleEvents.OnPlayerLevelUp, GameEventFactory>(value)
        .Register<ModuleEvents.OnPlayerLevelUp>(this);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnPlayerLevelUp, GameEventFactory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.ModuleEvents.OnPlayerRespawn"/>
    public event Action<ModuleEvents.OnPlayerRespawn> OnPlayerRespawn
    {
      add => EventService.SubscribeAll<ModuleEvents.OnPlayerRespawn, GameEventFactory>(value)
        .Register<ModuleEvents.OnPlayerRespawn>(this);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnPlayerRespawn, GameEventFactory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.ModuleEvents.OnPlayerRest"/>
    public event Action<ModuleEvents.OnPlayerRest> OnPlayerRest
    {
      add => EventService.SubscribeAll<ModuleEvents.OnPlayerRest, GameEventFactory>(value)
        .Register<ModuleEvents.OnPlayerRest>(this);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnPlayerRest, GameEventFactory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.ModuleEvents.OnPlayerUnequipItem"/>
    public event Action<ModuleEvents.OnPlayerUnequipItem> OnPlayerUnequipItem
    {
      add => EventService.SubscribeAll<ModuleEvents.OnPlayerUnequipItem, GameEventFactory>(value)
        .Register<ModuleEvents.OnPlayerUnequipItem>(this);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnPlayerUnequipItem, GameEventFactory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.ModuleEvents.OnUnacquireItem"/>
    public event Action<ModuleEvents.OnUnacquireItem> OnUnacquireItem
    {
      add => EventService.SubscribeAll<ModuleEvents.OnUnacquireItem, GameEventFactory>(value)
        .Register<ModuleEvents.OnUnacquireItem>(this);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnUnacquireItem, GameEventFactory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.ModuleEvents.OnUserDefined"/>
    public event Action<ModuleEvents.OnUserDefined> OnUserDefined
    {
      add => EventService.SubscribeAll<ModuleEvents.OnUserDefined, GameEventFactory>(value)
        .Register<ModuleEvents.OnUserDefined>(this);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnUserDefined, GameEventFactory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnClientConnect"/>
    public event Action<OnClientConnect> OnClientConnect
    {
      add => EventService.SubscribeAll<OnClientConnect, OnClientConnect.Factory>(value);
      remove => EventService.UnsubscribeAll<OnClientConnect, OnClientConnect.Factory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnClientDisconnect"/>
    public event Action<OnClientDisconnect> OnClientDisconnect
    {
      add => EventService.SubscribeAll<OnClientDisconnect, OnClientDisconnect.Factory>(value);
      remove => EventService.UnsubscribeAll<OnClientDisconnect, OnClientDisconnect.Factory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnServerCharacterSave"/>
    public event Action<OnServerCharacterSave> OnServerCharacterSave
    {
      add => EventService.SubscribeAll<OnServerCharacterSave, OnServerCharacterSave.Factory>(value);
      remove => EventService.UnsubscribeAll<OnServerCharacterSave, OnServerCharacterSave.Factory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnCombatDRBroken"/>
    public event Action<OnCombatDRBroken> OnCombatDRBroken
    {
      add => EventService.SubscribeAll<OnCombatDRBroken, OnCombatDRBroken.Factory>(value);
      remove => EventService.UnsubscribeAll<OnCombatDRBroken, OnCombatDRBroken.Factory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnCombatStatusChange"/>
    public event Action<OnCombatStatusChange> OnCombatStatusChange
    {
      add => EventService.SubscribeAll<OnCombatStatusChange, OnCombatStatusChange.Factory>(value);
      remove => EventService.UnsubscribeAll<OnCombatStatusChange, OnCombatStatusChange.Factory>(value);
    }

    /// <inheritdoc cref="OnDisarmWeapon"/>
    public event Action<OnDisarmWeapon> OnDisarm
    {
      add => EventService.SubscribeAll<OnDisarmWeapon, OnDisarmWeapon.Factory>(value);
      remove => EventService.UnsubscribeAll<OnDisarmWeapon, OnDisarmWeapon.Factory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnStartCombatRound"/>
    public event Action<OnStartCombatRound> OnStartCombatRound
    {
      add => EventService.SubscribeAll<OnStartCombatRound, OnStartCombatRound.Factory>(value);
      remove => EventService.UnsubscribeAll<OnStartCombatRound, OnStartCombatRound.Factory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnAssociateAdd"/>
    public event Action<OnAssociateAdd> OnAssociateAdd
    {
      add => EventService.SubscribeAll<OnAssociateAdd, OnAssociateAdd.Factory>(value);
      remove => EventService.UnsubscribeAll<OnAssociateAdd, OnAssociateAdd.Factory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnAssociateRemove"/>
    public event Action<OnAssociateRemove> OnAssociateRemove
    {
      add => EventService.SubscribeAll<OnAssociateRemove, OnAssociateRemove.Factory>(value);
      remove => EventService.UnsubscribeAll<OnAssociateRemove, OnAssociateRemove.Factory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnFamiliarPossess"/>
    public event Action<OnFamiliarPossess> OnFamiliarPossess
    {
      add => EventService.SubscribeAll<OnFamiliarPossess, OnFamiliarPossess.Factory>(value);
      remove => EventService.UnsubscribeAll<OnFamiliarPossess, OnFamiliarPossess.Factory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnFamiliarUnpossess"/>
    public event Action<OnFamiliarUnpossess> OnFamiliarUnpossess
    {
      add => EventService.SubscribeAll<OnFamiliarUnpossess, OnFamiliarUnpossess.Factory>(value);
      remove => EventService.UnsubscribeAll<OnFamiliarUnpossess, OnFamiliarUnpossess.Factory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnBarterStart"/>
    public event Action<OnBarterStart> OnBarterStart
    {
      add => EventService.SubscribeAll<OnBarterStart, OnBarterStart.Factory>(value);
      remove => EventService.UnsubscribeAll<OnBarterStart, OnBarterStart.Factory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnExamineObject"/>
    public event Action<OnExamineObject> OnExamineObject
    {
      add => EventService.SubscribeAll(Events.OnExamineObject.FactoryTypes, value);
      remove => EventService.UnsubscribeAll(Events.OnExamineObject.FactoryTypes, value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnExamineTrap"/>
    public event Action<OnExamineTrap> OnExamineTrap
    {
      add => EventService.SubscribeAll<OnExamineTrap, OnExamineTrap.Factory>(value);
      remove => EventService.UnsubscribeAll<OnExamineTrap, OnExamineTrap.Factory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnUseFeat"/>
    public event Action<OnUseFeat> OnUseFeat
    {
      add => EventService.SubscribeAll<OnUseFeat, OnUseFeat.Factory>(value);
      remove => EventService.UnsubscribeAll<OnUseFeat, OnUseFeat.Factory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnDoListenDetection"/>
    public event Action<OnDoListenDetection> OnDoListenDetection
    {
      add => EventService.SubscribeAll<OnDoListenDetection, OnDoListenDetection.Factory>(value);
      remove => EventService.UnsubscribeAll<OnDoListenDetection, OnDoListenDetection.Factory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnDoSpotDetection"/>
    public event Action<OnDoSpotDetection> OnDoSpotDetection
    {
      add => EventService.SubscribeAll<OnDoSpotDetection, OnDoSpotDetection.Factory>(value);
      remove => EventService.UnsubscribeAll<OnDoSpotDetection, OnDoSpotDetection.Factory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnSpellCast"/>
    public event Action<OnSpellCast> OnSpellCast
    {
      add => EventService.SubscribeAll<OnSpellCast, OnSpellCast.Factory>(value);
      remove => EventService.UnsubscribeAll<OnSpellCast, OnSpellCast.Factory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnSpellInterrupted"/>
    public event Action<OnSpellInterrupted> OnSpellInterrupted
    {
      add => EventService.SubscribeAll<OnSpellInterrupted, OnSpellInterrupted.Factory>(value);
      remove => EventService.UnsubscribeAll<OnSpellInterrupted, OnSpellInterrupted.Factory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnSpellSlotClear"/>
    public event Action<OnSpellSlotClear> OnSpellSlotClear
    {
      add => EventService.SubscribeAll<OnSpellSlotClear, OnSpellSlotClear.Factory>(value);
      remove => EventService.UnsubscribeAll<OnSpellSlotClear, OnSpellSlotClear.Factory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnSpellSlotMemorize"/>
    public event Action<OnSpellSlotMemorize> OnSpellSlotMemorize
    {
      add => EventService.SubscribeAll<OnSpellSlotMemorize, OnSpellSlotMemorize.Factory>(value);
      remove => EventService.UnsubscribeAll<OnSpellSlotMemorize, OnSpellSlotMemorize.Factory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnStoreRequestBuy"/>
    public event Action<OnStoreRequestBuy> OnStoreRequestBuy
    {
      add => EventService.SubscribeAll<OnStoreRequestBuy, OnStoreRequestBuy.Factory>(value);
      remove => EventService.UnsubscribeAll<OnStoreRequestBuy, OnStoreRequestBuy.Factory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnStoreRequestSell"/>
    public event Action<OnStoreRequestSell> OnStoreRequestSell
    {
      add => EventService.SubscribeAll<OnStoreRequestSell, OnStoreRequestSell.Factory>(value);
      remove => EventService.UnsubscribeAll<OnStoreRequestSell, OnStoreRequestSell.Factory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnDetectModeUpdate"/>
    public event Action<OnDetectModeUpdate> OnDetectModeUpdate
    {
      add => EventService.SubscribeAll<OnDetectModeUpdate, OnDetectModeUpdate.Factory>(value);
      remove => EventService.UnsubscribeAll<OnDetectModeUpdate, OnDetectModeUpdate.Factory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnStealthModeUpdate"/>
    public event Action<OnStealthModeUpdate> OnStealthModeUpdate
    {
      add => EventService.SubscribeAll<OnStealthModeUpdate, OnStealthModeUpdate.Factory>(value);
      remove => EventService.UnsubscribeAll<OnStealthModeUpdate, OnStealthModeUpdate.Factory>(value);
    }

    internal override CNWSScriptVarTable ScriptVarTable
    {
      get => Module.m_ScriptVars;
    }

    /// <summary>
    /// Gets or sets the XP scale for this module. Must be a value between 0-200.
    /// </summary>
    public int XPScale
    {
      get => NWScript.GetModuleXPScale();
      set => NWScript.SetModuleXPScale(value);
    }

    /// <summary>
    /// Gets or sets the max possible attack bonus from temporary effects/items (Default: 20).
    /// </summary>
    public int AttackBonusLimit
    {
      get => NWScript.GetAttackBonusLimit();
      set => NWScript.SetAttackBonusLimit(value);
    }

    /// <summary>
    /// Gets or sets the max possible damage bonus from temporary effects/items (Default: 100).
    /// </summary>
    public int DamageBonusLimit
    {
      get => NWScript.GetDamageBonusLimit();
      set => NWScript.SetDamageBonusLimit(value);
    }

    /// <summary>
    /// Gets or sets the max possible saving throw bonus from temporary effects/items (Default: 20).
    /// </summary>
    public int SavingThrowBonusLimit
    {
      get => NWScript.GetSavingThrowBonusLimit();
      set => NWScript.SetSavingThrowBonusLimit(value);
    }

    /// <summary>
    /// Gets or sets the max possible ability score bonus from temporary effects/items (Default: 12).
    /// </summary>
    public int GetAbilityBonusLimit
    {
      get => NWScript.GetAbilityBonusLimit();
      set => NWScript.SetAbilityBonusLimit(value);
    }

    /// <summary>
    /// Gets or sets the max possible ability score penalty from temporary effects/items (Default: 30).
    /// </summary>
    public int AbilityPenaltyLimit
    {
      get => NWScript.GetAbilityPenaltyLimit();
      set => NWScript.SetAbilityPenaltyLimit(value);
    }

    /// <summary>
    /// Gets or sets the max possible skill bonus from temporary effects/items (Default: 50).
    /// </summary>
    public int SkillBonusLimit
    {
      get => NWScript.GetSkillBonusLimit();
      set => NWScript.SetSkillBonusLimit(value);
    }

    /// <summary>
    /// Gets or sets the maximum number of henchmen.
    /// </summary>
    public int MaxHenchmen
    {
      get => NWScript.GetMaxHenchmen();
      set => NWScript.SetMaxHenchmen(value);
    }

    /// <summary>
    /// Gets the current server difficulty setting.
    /// </summary>
    public GameDifficulty GameDifficulty
    {
      get => (GameDifficulty)NWScript.GetGameDifficulty();
    }

    /// <summary>
    /// Gets a value indicating whether it is currently dawn.
    /// </summary>
    public bool IsDawn
    {
      get => NWScript.GetIsDawn().ToBool();
    }

    /// <summary>
    /// Gets a value indicating whether it is currently day.
    /// </summary>
    public bool IsDay
    {
      get => NWScript.GetIsDay().ToBool();
    }

    /// <summary>
    /// Gets a value indicating whether it is currently dusk.
    /// </summary>
    public bool IsDusk
    {
      get => NWScript.GetIsDusk().ToBool();
    }

    /// <summary>
    /// Gets a value indicating whether it is currently night.
    /// </summary>
    public bool IsNight
    {
      get => NWScript.GetIsNight().ToBool();
    }

    /// <summary>
    /// Gets the starting location for new players.
    /// </summary>
    public Location StartingLocation
    {
      get => NWScript.GetStartingLocation();
    }

    /// <summary>
    /// Gets all active areas in the module.
    /// </summary>
    public IEnumerable<NwArea> Areas
    {
      get
      {
        for (uint area = NWScript.GetFirstArea(); area != INVALID; area = NWScript.GetNextArea())
        {
          yield return area.ToNwObject<NwArea>();
        }
      }
    }

    /// <summary>
    /// Gets all current online players.
    /// </summary>
    public IEnumerable<NwPlayer> Players
    {
      get
      {
        for (uint player = NWScript.GetFirstPC(); player != INVALID; player = NWScript.GetNextPC())
        {
          yield return player.ToNwObject<NwPlayer>();
        }
      }
    }

    public override Guid? PeekUUID()
    {
      return null;
    }

    /// <summary>
    /// Gets the specified global campaign variable.
    /// </summary>
    /// <param name="campaign">The name of the campaign.</param>
    /// <param name="name">The variable name.</param>
    /// <typeparam name="T">The variable type.</typeparam>
    /// <returns>A CampaignVariable instance for getting/setting the variable's value.</returns>
    public CampaignVariable<T> GetCampaignVariable<T>(string campaign, string name)
      => CampaignVariable<T>.Create(campaign, name);

    /// <summary>
    /// Deletes the entire campaign database, if it exists.
    /// </summary>
    /// <param name="campaign">The campaign DB to delete.</param>
    public void DestroyCampaignDatabase(string campaign)
      => NWScript.DestroyCampaignDatabase(campaign);

    /// <summary>
    /// Broadcasts a message to the DM channel, sending a message to all DMs on the server.
    /// </summary>
    /// <param name="message">The message to send.</param>
    /// <param name="color">A color to apply to the message.</param>
    public void SendMessageToAllDMs(string message, Color color)
      => NWScript.SendMessageToAllDMs(message.ColorString(color));

    /// <inheritdoc cref="SendMessageToAllDMs(string,NWN.API.Color)"/>
    public void SendMessageToAllDMs(string message)
      => NWScript.SendMessageToAllDMs(message);

    /// <summary>
    /// Ends the current running game, plays the specified movie then returns all players to the main menu.
    /// </summary>
    /// <param name="endMovie">The filename of the movie to play, without file extension.</param>
    public void EndGame(string endMovie)
      => NWScript.EndGame(endMovie);

    /// <summary>
    /// Forces all players who are currently game to have their characters
    /// exported to their respective directories i.e. LocalVault/ServerVault/ etc.
    /// </summary>
    public void ExportAllCharacters()
      => NWScript.ExportAllCharacters();

    /// <summary>
    /// Makes all online PCs load a new texture instead of another.
    /// </summary>
    /// <param name="oldTexName">The existing texture to replace.</param>
    /// <param name="newName">The new override texture.</param>
    public void SetTextureOverride(string oldTexName, string newName)
      => NWScript.SetTextureOverride(oldTexName, newName);

    /// <summary>
    /// Removes the override for the specified texture, reverting to the original texture.
    /// </summary>
    /// <param name="texName">The name of the original texture.</param>
    public void ClearTextureOverride(string texName)
      => NWScript.SetTextureOverride(texName, string.Empty);

    /// <summary>
    /// Finds the specified waypoint with the given tag.
    /// </summary>
    public NwWaypoint GetWaypointByTag(string tag)
      => NWScript.GetWaypointByTag(tag).ToNwObject<NwWaypoint>();

    /// <summary>
    /// Adds an entry to the journal of all players in the module.<br/>
    /// See <see cref="NwPlayer.AddJournalQuestEntry"/> to add a journal entry to a specific player/party.
    /// </summary>
    /// <param name="categoryTag">The tag of the Journal category (case-sensitive).</param>
    /// <param name="entryId">The ID of the Journal entry.</param>
    /// <param name="allowOverrideHigher">If true, disables the default restriction that requires journal entry numbers to increase.</param>
    public void AddJournalQuestEntry(string categoryTag, int entryId, bool allowOverrideHigher = false)
      => NWScript.AddJournalQuestEntry(categoryTag, entryId, Players.FirstOrDefault(), true.ToInt(), true.ToInt(), allowOverrideHigher.ToInt());
  }
}
