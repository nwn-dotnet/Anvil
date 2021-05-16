using System;
using System.Collections.Generic;
using System.Linq;
using Anvil.Internal;
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

    internal NwModule(CNWSModule module) : base(module)
    {
      this.Module = module;
    }

    public static implicit operator CNWSModule(NwModule module)
    {
      return module?.Module;
    }

    public static readonly NwModule Instance = new NwModule(LowLevel.ServerExoApp.GetModule());

    /// <inheritdoc cref="NWN.API.Events.ModuleEvents.OnAcquireItem"/>
    public event Action<ModuleEvents.OnAcquireItem> OnAcquireItem
    {
      add => EventService.SubscribeAll<ModuleEvents.OnAcquireItem, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnAcquireItem, GameEventFactory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.ModuleEvents.OnActivateItem"/>
    public event Action<ModuleEvents.OnActivateItem> OnActivateItem
    {
      add => EventService.SubscribeAll<ModuleEvents.OnActivateItem, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnActivateItem, GameEventFactory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.ModuleEvents.OnClientEnter"/>
    public event Action<ModuleEvents.OnClientEnter> OnClientEnter
    {
      add => EventService.SubscribeAll<ModuleEvents.OnClientEnter, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnClientEnter, GameEventFactory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.ModuleEvents.OnClientLeave"/>
    public event Action<ModuleEvents.OnClientLeave> OnClientLeave
    {
      add => EventService.SubscribeAll<ModuleEvents.OnClientLeave, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnClientLeave, GameEventFactory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.ModuleEvents.OnCutsceneAbort"/>
    public event Action<ModuleEvents.OnCutsceneAbort> OnCutsceneAbort
    {
      add => EventService.SubscribeAll<ModuleEvents.OnCutsceneAbort, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnCutsceneAbort, GameEventFactory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.ModuleEvents.OnHeartbeat"/>
    public event Action<ModuleEvents.OnHeartbeat> OnHeartbeat
    {
      add => EventService.SubscribeAll<ModuleEvents.OnHeartbeat, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnHeartbeat, GameEventFactory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.ModuleEvents.OnModuleLoad"/>
    public event Action<ModuleEvents.OnModuleLoad> OnModuleLoad
    {
      add => EventService.SubscribeAll<ModuleEvents.OnModuleLoad, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnModuleLoad, GameEventFactory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.ModuleEvents.OnModuleStart"/>
    public event Action<ModuleEvents.OnModuleStart> OnModuleStart
    {
      add => EventService.SubscribeAll<ModuleEvents.OnModuleStart, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnModuleStart, GameEventFactory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.ModuleEvents.OnPlayerChat"/>
    public event Action<ModuleEvents.OnPlayerChat> OnPlayerChat
    {
      add => EventService.SubscribeAll<ModuleEvents.OnPlayerChat, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnPlayerChat, GameEventFactory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.ModuleEvents.OnPlayerDeath"/>
    public event Action<ModuleEvents.OnPlayerDeath> OnPlayerDeath
    {
      add => EventService.SubscribeAll<ModuleEvents.OnPlayerDeath, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnPlayerDeath, GameEventFactory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.ModuleEvents.OnPlayerDying"/>
    public event Action<ModuleEvents.OnPlayerDying> OnPlayerDying
    {
      add => EventService.SubscribeAll<ModuleEvents.OnPlayerDying, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnPlayerDying, GameEventFactory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.ModuleEvents.OnPlayerEquipItem"/>
    public event Action<ModuleEvents.OnPlayerEquipItem> OnPlayerEquipItem
    {
      add => EventService.SubscribeAll<ModuleEvents.OnPlayerEquipItem, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnPlayerEquipItem, GameEventFactory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.ModuleEvents.OnPlayerLevelUp"/>
    public event Action<ModuleEvents.OnPlayerLevelUp> OnPlayerLevelUp
    {
      add => EventService.SubscribeAll<ModuleEvents.OnPlayerLevelUp, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnPlayerLevelUp, GameEventFactory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.ModuleEvents.OnPlayerRespawn"/>
    public event Action<ModuleEvents.OnPlayerRespawn> OnPlayerRespawn
    {
      add => EventService.SubscribeAll<ModuleEvents.OnPlayerRespawn, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnPlayerRespawn, GameEventFactory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.ModuleEvents.OnPlayerRest"/>
    public event Action<ModuleEvents.OnPlayerRest> OnPlayerRest
    {
      add => EventService.SubscribeAll<ModuleEvents.OnPlayerRest, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnPlayerRest, GameEventFactory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.ModuleEvents.OnPlayerUnequipItem"/>
    public event Action<ModuleEvents.OnPlayerUnequipItem> OnPlayerUnequipItem
    {
      add => EventService.SubscribeAll<ModuleEvents.OnPlayerUnequipItem, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnPlayerUnequipItem, GameEventFactory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.ModuleEvents.OnUnacquireItem"/>
    public event Action<ModuleEvents.OnUnacquireItem> OnUnacquireItem
    {
      add => EventService.SubscribeAll<ModuleEvents.OnUnacquireItem, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnUnacquireItem, GameEventFactory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.ModuleEvents.OnUserDefined"/>
    public event Action<ModuleEvents.OnUserDefined> OnUserDefined
    {
      add => EventService.SubscribeAll<ModuleEvents.OnUserDefined, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
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

    /// <inheritdoc cref="NWN.API.Events.OnServerSendArea"/>
    public event Action<OnServerSendArea> OnServerSendArea
    {
      add => EventService.SubscribeAll<OnServerSendArea, OnServerSendArea.Factory>(value);
      remove => EventService.UnsubscribeAll<OnServerSendArea, OnServerSendArea.Factory>(value);
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

    /// <inheritdoc cref="Events.OnDisarmWeapon"/>
    public event Action<OnDisarmWeapon> OnDisarmWeapon
    {
      add => EventService.SubscribeAll<OnDisarmWeapon, OnDisarmWeapon.Factory>(value);
      remove => EventService.UnsubscribeAll<OnDisarmWeapon, OnDisarmWeapon.Factory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnCreatureAttack"/>
    public event Action<OnCreatureAttack> OnCreatureAttack
    {
      add => EventService.SubscribeAll<OnCreatureAttack, OnCreatureDamage.Factory>(value);
      remove => EventService.UnsubscribeAll<OnCreatureAttack, OnCreatureDamage.Factory>(value);
    }

    /// <inheritdoc cref="Events.OnCreatureDamage"/>
    public event Action<OnCreatureDamage> OnCreatureDamage
    {
      add => EventService.SubscribeAll<OnCreatureDamage, OnCreatureDamage.Factory>(value);
      remove => EventService.UnsubscribeAll<OnCreatureDamage, OnCreatureDamage.Factory>(value);
    }

    /// <inheritdoc cref="Events.OnCombatRoundStart"/>
    public event Action<OnCombatRoundStart> OnCombatRoundStart
    {
      add => EventService.SubscribeAll<OnCombatRoundStart, OnCombatRoundStart.Factory>(value);
      remove => EventService.UnsubscribeAll<OnCombatRoundStart, OnCombatRoundStart.Factory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnAssociateAdd"/>
    public event Action<OnAssociateAdd> OnAssociateAdd
    {
      add => EventService.SubscribeAll<OnAssociateAdd, OnAssociateAdd.Factory>(value);
      remove => EventService.UnsubscribeAll<OnAssociateAdd, OnAssociateAdd.Factory>(value);
    }

    /// <inheritdoc cref="Events.OnAssociateRemove"/>
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

    /// <inheritdoc cref="NWN.API.Events.OnBarterEnd"/>
    public event Action<OnBarterEnd> OnBarterEnd
    {
      add => EventService.SubscribeAll<OnBarterEnd, OnBarterEnd.Factory>(value);
      remove => EventService.UnsubscribeAll<OnBarterEnd, OnBarterEnd.Factory>(value);
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
      add => EventService.SubscribeAll<OnExamineObject, OnExamineObject.Factory>(value);
      remove => EventService.UnsubscribeAll<OnExamineObject, OnExamineObject.Factory>(value);
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

    /// <inheritdoc cref="NWN.API.Events.OnHeal"/>
    public event Action<OnHeal> OnHeal
    {
      add => EventService.SubscribeAll<OnHeal, OnHeal.Factory>(value);
      remove => EventService.UnsubscribeAll<OnHeal, OnHeal.Factory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnInventoryGoldAdd"/>
    public event Action<OnInventoryGoldAdd> OnInventoryGoldAdd
    {
      add => EventService.SubscribeAll<OnInventoryGoldAdd, OnInventoryGoldAdd.Factory>(value);
      remove => EventService.UnsubscribeAll<OnInventoryGoldAdd, OnInventoryGoldAdd.Factory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnInventoryGoldRemove"/>
    public event Action<OnInventoryGoldRemove> OnInventoryGoldRemove
    {
      add => EventService.SubscribeAll<OnInventoryGoldRemove, OnInventoryGoldRemove.Factory>(value);
      remove => EventService.UnsubscribeAll<OnInventoryGoldRemove, OnInventoryGoldRemove.Factory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnInventoryItemAdd"/>
    public event Action<OnInventoryItemAdd> OnInventoryItemAdd
    {
      add => EventService.SubscribeAll<OnInventoryItemAdd, OnInventoryItemAdd.Factory>(value);
      remove => EventService.UnsubscribeAll<OnInventoryItemAdd, OnInventoryItemAdd.Factory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnInventoryItemRemove"/>
    public event Action<OnInventoryItemRemove> OnInventoryItemRemove
    {
      add => EventService.SubscribeAll<OnInventoryItemRemove, OnInventoryItemRemove.Factory>(value);
      remove => EventService.UnsubscribeAll<OnInventoryItemRemove, OnInventoryItemRemove.Factory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnItemEquip"/>
    public event Action<OnItemEquip> OnItemEquip
    {
      add => EventService.SubscribeAll<OnItemEquip, OnItemEquip.Factory>(value);
      remove => EventService.UnsubscribeAll<OnItemEquip, OnItemEquip.Factory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnItemInventoryClose"/>
    public event Action<OnItemInventoryClose> OnItemInventoryClose
    {
      add => EventService.SubscribeAll<OnItemInventoryClose, OnItemInventoryClose.Factory>(value);
      remove => EventService.UnsubscribeAll<OnItemInventoryClose, OnItemInventoryClose.Factory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnItemInventoryOpen"/>
    public event Action<OnItemInventoryOpen> OnItemInventoryOpen
    {
      add => EventService.SubscribeAll<OnItemInventoryOpen, OnItemInventoryOpen.Factory>(value);
      remove => EventService.UnsubscribeAll<OnItemInventoryOpen, OnItemInventoryOpen.Factory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnItemPayToIdentify"/>
    public event Action<OnItemPayToIdentify> OnItemPayToIdentify
    {
      add => EventService.SubscribeAll<OnItemPayToIdentify, OnItemPayToIdentify.Factory>(value);
      remove => EventService.UnsubscribeAll<OnItemPayToIdentify, OnItemPayToIdentify.Factory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnItemUse"/>
    public event Action<OnItemUse> OnItemUse
    {
      add => EventService.SubscribeAll<OnItemUse, OnItemUse.Factory>(value);
      remove => EventService.UnsubscribeAll<OnItemUse, OnItemUse.Factory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnItemValidateEquip"/>
    public event Action<OnItemValidateEquip> OnItemValidateEquip
    {
      add => EventService.SubscribeAll<OnItemValidateEquip, OnItemValidateEquip.Factory>(value);
      remove => EventService.UnsubscribeAll<OnItemValidateEquip, OnItemValidateEquip.Factory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnItemValidateUse"/>
    public event Action<OnItemValidateUse> OnItemValidateUse
    {
      add => EventService.SubscribeAll<OnItemValidateUse, OnItemValidateUse.Factory>(value);
      remove => EventService.UnsubscribeAll<OnItemValidateUse, OnItemValidateUse.Factory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnClientLevelUpBegin"/>
    public event Action<OnClientLevelUpBegin> OnClientLevelUpBegin
    {
      add => EventService.SubscribeAll<OnClientLevelUpBegin, OnClientLevelUpBegin.Factory>(value);
      remove => EventService.UnsubscribeAll<OnClientLevelUpBegin, OnClientLevelUpBegin.Factory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnLevelDown"/>
    public event Action<OnLevelDown> OnLevelDown
    {
      add => EventService.SubscribeAll<OnLevelDown, OnLevelDown.Factory>(value);
      remove => EventService.UnsubscribeAll<OnLevelDown, OnLevelDown.Factory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnLevelUp"/>
    public event Action<OnLevelUp> OnLevelUp
    {
      add => EventService.SubscribeAll<OnLevelUp, OnLevelUp.Factory>(value);
      remove => EventService.UnsubscribeAll<OnLevelUp, OnLevelUp.Factory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnLevelUpAutomatic"/>
    public event Action<OnLevelUpAutomatic> OnLevelUpAutomatic
    {
      add => EventService.SubscribeAll<OnLevelUpAutomatic, OnLevelUpAutomatic.Factory>(value);
      remove => EventService.UnsubscribeAll<OnLevelUpAutomatic, OnLevelUpAutomatic.Factory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnPartyEvent"/>
    public event Action<OnPartyEvent> OnPartyEvent
    {
      add => EventService.SubscribeAll<OnPartyEvent, OnPartyEvent.Factory>(value);
      remove => EventService.UnsubscribeAll<OnPartyEvent, OnPartyEvent.Factory>(value);
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

    /// <inheritdoc cref="NWN.API.Events.OnSpellAction"/>
    public event Action<OnSpellAction> OnSpellAction
    {
      add => EventService.SubscribeAll<OnSpellAction, OnSpellAction.Factory>(value);
      remove => EventService.UnsubscribeAll<OnSpellAction, OnSpellAction.Factory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnSpellBroadcast"/>
    public event Action<OnSpellBroadcast> OnSpellBroadcast
    {
      add => EventService.SubscribeAll<OnSpellBroadcast, OnSpellBroadcast.Factory>(value);
      remove => EventService.UnsubscribeAll<OnSpellBroadcast, OnSpellBroadcast.Factory>(value);
    }

    /// <inheritdoc cref="NWN.API.Events.OnSpellCast"/>
    public event Action<OnSpellCast> OnSpellCast
    {
      add => EventService.SubscribeAll<OnSpellCast, OnSpellCast.Factory>(value);
      remove => EventService.UnsubscribeAll<OnSpellCast, OnSpellCast.Factory>(value);
    }

    /// <inheritdoc cref="Events.OnSpellInterrupt"/>
    public event Action<OnSpellInterrupt> OnSpellInterrupt
    {
      add => EventService.SubscribeAll<OnSpellInterrupt, OnSpellInterrupt.Factory>(value);
      remove => EventService.UnsubscribeAll<OnSpellInterrupt, OnSpellInterrupt.Factory>(value);
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

    /// <inheritdoc cref="NWN.API.Events.OnCalendarTimeChange"/>
    public event Action<OnCalendarTimeChange> OnCalendarTimeChange
    {
      add => EventService.SubscribeAll<OnCalendarTimeChange, OnCalendarTimeChange.Factory>(value);
      remove => EventService.UnsubscribeAll<OnCalendarTimeChange, OnCalendarTimeChange.Factory>(value);
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
        CExoLinkedListCNWSClient playerList = LowLevel.ServerExoApp.m_pcExoAppInternal.m_pNWSPlayerList;

        for (CExoLinkedListNode node = playerList.GetHeadPos(); node != null; node = node.pNext)
        {
          CNWSPlayer player = playerList.GetAtPos(node).AsNWSPlayer();
          yield return new NwPlayer(player, player.m_oidPCObject.ToNwObject<NwCreature>());
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

    /// <summary>
    /// Gets the last objects that were created in the module. Use LINQ to skip or limit the query.
    /// </summary>
    /// <returns>An enumerable containing the last created objects.</returns>
    public IEnumerable<NwObject> GetLastCreatedObjects()
    {
      CGameObjectArray gameObjectArray = LowLevel.ServerExoApp.GetObjectArray();

      for (uint objectId = gameObjectArray.m_nNextObjectArrayID[0] - 1; objectId > 0; objectId--)
      {
        if (TryGetObject(objectId, out NwObject gameObject))
        {
          yield return gameObject;
        }
      }

      unsafe bool TryGetObject(uint objectId, out NwObject gameObject)
      {
        void* pObject;

        bool retVal = gameObjectArray.GetGameObject(objectId, &pObject) == 0;
        gameObject = retVal ? objectId.ToNwObject() : default;

        return retVal;
      }
    }
  }
}
