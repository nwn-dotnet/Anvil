using NLog;
using NWN;

namespace NWM.API
{
  public sealed class ModuleEvents : EventHandler<ModuleEventType>
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

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

    protected override void HandleEvent(ModuleEventType eventType, NwObject objSelf)
    {
      switch (eventType)
      {
        case ModuleEventType.AcquireItem:
        {
          OnAcquireItem?.Invoke(NWScript.GetModuleItemAcquired().ToNwObject<NwItem>(),
            NWScript.GetModuleItemAcquiredBy().ToNwObject<NwGameObject>(),
            NWScript.GetModuleItemAcquiredFrom().ToNwObject<NwGameObject>());
          break;
        }
        case ModuleEventType.ActivateItem:
        {
          OnActivateItem?.Invoke(NWScript.GetItemActivated().ToNwObject<NwItem>(),
            NWScript.GetItemActivator().ToNwObject<NwCreature>(),
            NWScript.GetItemActivatedTarget().ToNwObject<NwGameObject>(),
            NWScript.GetItemActivatedTargetLocation());
          break;
        }
        case ModuleEventType.ClientEnter:
        {
          OnClientEnter?.Invoke((NwPlayer) EnteringObject);
          break;
        }
        case ModuleEventType.ClientLeave:
        {
          OnClientLeave?.Invoke((NwPlayer) ExitingObject);
          break;
        }
        case ModuleEventType.CutsceneAbort:
        {
          OnCutsceneAbort?.Invoke(NWScript.GetLastPCToCancelCutscene().ToNwObject<NwPlayer>());
          break;
        }
        case ModuleEventType.Heartbeat:
        {
          OnHeartbeat?.Invoke();
          break;
        }
        case ModuleEventType.ModuleLoad:
        {
          OnModuleLoad?.Invoke();
          break;
        }
        case ModuleEventType.PlayerChat:
        {
          ProcessPlayerChatEvent();
          break;
        }
        case ModuleEventType.PlayerDeath:
          OnPlayerDeath?.Invoke(NWScript.GetLastPlayerDied().ToNwObject<NwPlayer>(),
            NWScript.GetLastHostileActor().ToNwObject<NwCreature>());
          break;
        case ModuleEventType.PlayerDying:
          OnPlayerDying?.Invoke(NWScript.GetLastPlayerDying().ToNwObject<NwPlayer>());
          break;
        case ModuleEventType.PlayerEquipItem:
          OnPlayerEquipItem?.Invoke(NWScript.GetPCItemLastEquippedBy().ToNwObject<NwPlayer>(),
            NWScript.GetPCItemLastEquipped().ToNwObject<NwItem>());
          break;
        case ModuleEventType.PlayerLevelUp:
          OnPlayerLevelUp?.Invoke(NWScript.GetPCLevellingUp().ToNwObject<NwPlayer>());
          break;
        case ModuleEventType.PlayerRespawn:
          OnPlayerRespawn?.Invoke(NWScript.GetLastRespawnButtonPresser().ToNwObject<NwPlayer>());
          break;
        case ModuleEventType.PlayerRest:
          OnPlayerRest?.Invoke(NWScript.GetLastPCRested().ToNwObject<NwPlayer>(),
            (RestEventType) NWScript.GetLastRestEventType());
          break;
        case ModuleEventType.PlayerUnEquipItem:
          OnPlayerUnequipItem?.Invoke(NWScript.GetPCItemLastUnequippedBy().ToNwObject<NwPlayer>(),
            NWScript.GetPCItemLastUnequipped().ToNwObject<NwItem>());
          break;
        case ModuleEventType.UnAcquireItem:
          OnUnacquireItem?.Invoke(NWScript.GetModuleItemLostBy().ToNwObject<NwCreature>(),
            NWScript.GetModuleItemLost().ToNwObject<NwItem>());
          break;
        case ModuleEventType.UserDefined:
          OnUserDefined?.Invoke(NWScript.GetUserDefinedEventNumber());
          break;
      }
    }

    private void ProcessPlayerChatEvent()
    {
      if (callingChatHandlers || OnPlayerChat == null) return;

      callingChatHandlers = true; // Prevent recursive calls.

      NwPlayer sender = NWScript.GetPCChatSpeaker().ToNwObject<NwPlayer>();
      ChatMessage chatMessage = new ChatMessage(NWScript.GetPCChatMessage(), (TalkVolume) NWScript.GetPCChatVolume());
      OnPlayerChat(sender, chatMessage);

      NWScript.SetPCChatMessage(chatMessage.Message);
      NWScript.SetPCChatVolume((int) chatMessage.Volume);

      callingChatHandlers = false;
    }
  }
}