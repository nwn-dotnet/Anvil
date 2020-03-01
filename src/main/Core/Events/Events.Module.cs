using NWM.API;
using NWN;

namespace NWM.Core
{
  public partial class EventManager
  {
    public event AcquireItemEvent OnAcquireItem;
    public event ActivateItemEvent OnActivateItem;
    public event ClientEnterEvent OnClientEnter;
    public event ClientLeaveEvent OnClientLeave;
    public event CutsceneAbortEvent OnCutsceneAbort;
    public event HeartbeatEvent OnHeartbeat;
    public event ModuleLoadEvent OnModuleLoad;
    public event PlayerChatEvent OnPlayerChat;
    public event PlayerDeathEvent OnPlayerDeath;
    public event PlayerDyingEvent OnPlayerDying;
    public event PlayerEquipItemEvent OnPlayerEquipItem;
    public event PlayerLevelUpEvent OnPlayerLevelUp;
    public event PlayerRespawnEvent OnPlayerRespawn;
    public event PlayerRestEvent OnPlayerRest;
    public event PlayerUnequipItemEvent OnPlayerUnequipItem;
    public event UnacquireItemEvent OnUnacquireItem;
    public event UserDefinedEvent OnUserDefined;

    public delegate void AcquireItemEvent(NwItem item, NwGameObject acquiredBy, NwGameObject acquiredFrom);
    public delegate void ActivateItemEvent(NwItem item, NwCreature activator, NwGameObject target, Location targetLocation);
    public delegate void ClientEnterEvent(NwPlayer entered);
    public delegate void ClientLeaveEvent(NwPlayer exiting);
    public delegate void CutsceneAbortEvent(NwPlayer aborter);
    public delegate void HeartbeatEvent();
    public delegate void ModuleLoadEvent();
    public delegate void PlayerChatEvent(NwPlayer sender, ChatMessage message);
    public delegate void PlayerDeathEvent(NwPlayer player, NwCreature lastHostile);
    public delegate void PlayerDyingEvent(NwPlayer player);
    public delegate void PlayerEquipItemEvent(NwPlayer player, NwItem item);
    public delegate void PlayerLevelUpEvent(NwPlayer player);
    public delegate void PlayerRespawnEvent(NwPlayer player);
    public delegate void PlayerRestEvent(NwPlayer player, RestEventType restEventType);
    public delegate void PlayerUnequipItemEvent(NwPlayer player, NwItem item);
    public delegate void UnacquireItemEvent(NwCreature lastOwner, NwItem item);
    public delegate void UserDefinedEvent(int eventNumber);

    private bool callingChatHandlers;

    [ScriptHandler("nwm_item_acq")]
    private void NWNOnAcquireItem()
    {
      if (OnAcquireItem == null) return;

      NwItem item = NWScript.GetModuleItemAcquired().ToNwObject<NwItem>();
      NwGameObject acquirer = NWScript.GetModuleItemAcquiredBy().ToNwObject<NwGameObject>();
      NwGameObject lostBy = NWScript.GetModuleItemAcquiredFrom().ToNwObject<NwGameObject>();
      OnAcquireItem(item, acquirer, lostBy);
    }

    [ScriptHandler("nwm_item_actv")]
    private void NWNOnActivateItem()
    {
      if (OnActivateItem == null) return;

      NwItem item = NWScript.GetItemActivated().ToNwObject<NwItem>();
      NwCreature activator = NWScript.GetItemActivator().ToNwObject<NwCreature>();
      NwGameObject target = NWScript.GetItemActivatedTarget().ToNwObject<NwGameObject>();
      Location targetLocation = NWScript.GetItemActivatedTargetLocation();
      OnActivateItem(item, activator, target, targetLocation);
    }

    [ScriptHandler("nwm_cli_ent")]
    private void NWNOnClientEnter()
    {
      if (OnClientEnter == null) return;

      NwPlayer entered = NWScript.GetEnteringObject().ToNwObject<NwPlayer>();
      OnClientEnter(entered);
    }

    [ScriptHandler("nwm_cli_exi")]
    private void NWNOnClientLeave()
    {
      if (OnClientLeave == null) return;

      NwPlayer exited = NWScript.GetExitingObject().ToNwObject<NwPlayer>();
      OnClientLeave(exited);
    }

    [ScriptHandler("nwm_cuts_abrt")]
    private void NWNOnCutsceneAbort()
    {
      if (OnCutsceneAbort == null) return;

      NwPlayer canceller = NWScript.GetLastPCToCancelCutscene().ToNwObject<NwPlayer>();
      OnCutsceneAbort(canceller);
    }

    [ScriptHandler("nwm_heartbeat")]
    private void NWNOnHeartbeat()
    {
      OnHeartbeat?.Invoke();
    }

    [ScriptHandler("nwm_mod_load")]
    private void NWNOnModuleLoad()
    {
      OnModuleLoad?.Invoke();
    }

    [ScriptHandler("nwm_pc_onchat")]
    private void NWNOnPlayerChat()
    {
      if (callingChatHandlers || OnPlayerChat == null) return;

      callingChatHandlers = true; // Prevent recursive calls.

      NwPlayer sender = NWScript.GetPCChatSpeaker().ToNwObject<NwPlayer>();
      ChatMessage chatMessage = new ChatMessage(NWScript.GetPCChatMessage(), (TalkVolume)NWScript.GetPCChatVolume());
      OnPlayerChat(sender, chatMessage);

      NWScript.SetPCChatMessage(chatMessage.Message);
      NWScript.SetPCChatVolume((int)chatMessage.Volume);

      callingChatHandlers = false;
    }

    [ScriptHandler("nwm_pc_ondea")]
    private void NWNOnPlayerDeath()
    {
      if (OnPlayerDeath == null) return;

      NwPlayer player = NWScript.GetLastPlayerDied().ToNwObject<NwPlayer>();
      NwCreature lastHostile = NWScript.GetLastHostileActor().ToNwObject<NwCreature>();
      OnPlayerDeath(player, lastHostile);
    }

    [ScriptHandler("nwm_pc_ondying")]
    private void NWNOnPlayerDying()
    {
      if (OnPlayerDying == null) return;

      NwPlayer player = NWScript.GetLastPlayerDying().ToNwObject<NwPlayer>();
      OnPlayerDying(player);
    }

    [ScriptHandler("nwm_itempc_equi")]
    private void NWNOnPlayerEquipItem()
    {
      if (OnPlayerEquipItem == null) return;

      NwItem item = NWScript.GetPCItemLastEquipped().ToNwObject<NwItem>();
      NwPlayer player = NWScript.GetPCItemLastEquippedBy().ToNwObject<NwPlayer>();

      OnPlayerEquipItem(player, item);
    }

    [ScriptHandler("nwm_pc_lvl_up")]
    private void NWNOnPlayerLevelUp()
    {
      if (OnPlayerLevelUp == null) return;

      NwPlayer player = NWScript.GetPCLevellingUp().ToNwObject<NwPlayer>();
      OnPlayerLevelUp(player);
    }

    [ScriptHandler("nwm_pc_respawn")]
    private void NWNOnPlayerRespawn()
    {
      if (OnPlayerRespawn == null) return;

      NwPlayer player = NWScript.GetLastRespawnButtonPresser().ToNwObject<NwPlayer>();
      OnPlayerRespawn(player);
    }

    [ScriptHandler("nwm_pc_rest")]
    private void NWNOnPlayerRest()
    {
      if (OnPlayerRest == null) return;

      NwPlayer player = NWScript.GetLastPCRested().ToNwObject<NwPlayer>();
      RestEventType eventType = (RestEventType) NWScript.GetLastRestEventType();
      OnPlayerRest(player, eventType);
    }

    [ScriptHandler("nwm_itempc_uequi")]
    private void NWNOnPlayerUnequipItem()
    {
      if (OnPlayerUnequipItem == null) return;

      NwPlayer player = NWScript.GetPCItemLastUnequippedBy().ToNwObject<NwPlayer>();
      NwItem item = NWScript.GetPCItemLastUnequipped().ToNwObject<NwItem>();
      OnPlayerUnequipItem(player, item);
    }

    [ScriptHandler("nwm_item_uacq")]
    private void NWNOnUnacquireItem()
    {
      if (OnUnacquireItem == null) return;

      NwCreature creature = NWScript.GetModuleItemLostBy().ToNwObject<NwCreature>();
      NwItem item = NWScript.GetModuleItemLost().ToNwObject<NwItem>();
      OnUnacquireItem(creature, item);
    }

    [ScriptHandler("nwm_user_def")]
    private void NWNOnUserDefined()
    {
      if (OnUserDefined == null) return;

      int eventNum = NWScript.GetUserDefinedEventNumber();
      OnUserDefined(eventNum);
    }
  }
}