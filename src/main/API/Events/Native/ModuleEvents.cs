using System;
using NWM.API.Constants;
using NWN;

namespace NWM.API.Events
{
  public static class ModuleEvents
  {
    [EventInfo(EventType.Native, DefaultScriptSuffix = "acq_ite")]
    public sealed class OnAcquireItem : IEvent<OnAcquireItem>
    {
      public NwItem Item { get; private set; }
      public NwGameObject AcquiredBy { get; private set; }
      public NwGameObject AcquiredFrom { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Item = NWScript.GetModuleItemAcquired().ToNwObject<NwItem>();
        AcquiredBy = NWScript.GetModuleItemAcquiredBy().ToNwObject<NwGameObject>();
        AcquiredFrom = NWScript.GetModuleItemAcquiredFrom().ToNwObject<NwGameObject>();

        Callbacks?.Invoke(this);
      }

      public event Action<OnAcquireItem> Callbacks;
    }

    [EventInfo(EventType.Native, DefaultScriptSuffix = "act_ite")]
    public sealed class OnActivateItem : IEvent<OnActivateItem>
    {
      public NwItem ActivatedItem { get; private set; }
      public NwCreature ItemActivator { get; private set; }
      public NwGameObject TargetObject { get; private set; }
      public Location TargetLocation { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        ActivatedItem = NWScript.GetItemActivated().ToNwObject<NwItem>();
        ItemActivator = NWScript.GetItemActivator().ToNwObject<NwCreature>();
        TargetObject = NWScript.GetItemActivatedTarget().ToNwObject<NwGameObject>();
        TargetLocation = NWScript.GetItemActivatedTargetLocation();

        Callbacks?.Invoke(this);
      }

      public event Action<OnActivateItem> Callbacks;
    }

    [EventInfo(EventType.Native, DefaultScriptSuffix = "cli_ent")]
    public sealed class OnClientEnter : IEvent<OnClientEnter>
    {
      public NwPlayer Player { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Player = NWScript.GetEnteringObject().ToNwObject<NwPlayer>();
        Callbacks?.Invoke(this);
      }

      public event Action<OnClientEnter> Callbacks;
    }

    [EventInfo(EventType.Native, DefaultScriptSuffix = "cli_lea")]
    public sealed class OnClientLeave : IEvent<OnClientLeave>
    {
      public NwPlayer Player { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Player = NWScript.GetExitingObject().ToNwObject<NwPlayer>();
        Callbacks?.Invoke(this);
      }

      public event Action<OnClientLeave> Callbacks;
    }

    [EventInfo(EventType.Native, DefaultScriptSuffix = "cut_abo")]
    public sealed class OnCutsceneAbort : IEvent<OnCutsceneAbort>
    {
      public NwPlayer Player { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Player = NWScript.GetLastPCToCancelCutscene().ToNwObject<NwPlayer>();
        Callbacks?.Invoke(this);
      }

      public event Action<OnCutsceneAbort> Callbacks;
    }

    [EventInfo(EventType.Native, DefaultScriptSuffix = "mod_hea")]
    public sealed class OnHeartbeat : IEvent<OnHeartbeat>
    {
      public void BroadcastEvent(NwObject objSelf)
      {
        Callbacks?.Invoke(this);
      }

      public event Action<OnHeartbeat> Callbacks;
    }

    [EventInfo(EventType.Native, DefaultScriptSuffix = "mod_loa")]
    public sealed class OnModuleLoad : IEvent<OnModuleLoad>
    {
      public void BroadcastEvent(NwObject objSelf)
      {
        Callbacks?.Invoke(this);
      }

      public event Action<OnModuleLoad> Callbacks;
    }

    [EventInfo(EventType.Native, DefaultScriptSuffix = "pla_cha")]
    public sealed class OnPlayerChat : IEvent<OnPlayerChat>
    {
      public NwPlayer Sender { get; private set; }
      public string Message { get; set; }
      public TalkVolume Volume { get; set; }

      private bool callingChatHandlers;

      public void BroadcastEvent(NwObject objSelf)
      {
        if (callingChatHandlers || Callbacks == null) return;

        callingChatHandlers = true; // Prevent recursive calls.

        Sender = NWScript.GetPCChatSpeaker().ToNwObject<NwPlayer>();
        Message = NWScript.GetPCChatMessage();
        Volume = (TalkVolume) NWScript.GetPCChatVolume();
        Callbacks(this);

        NWScript.SetPCChatMessage(Message);
        NWScript.SetPCChatVolume((int) Volume);

        callingChatHandlers = false;
      }

      public event Action<OnPlayerChat> Callbacks;
    }

    [EventInfo(EventType.Native, DefaultScriptSuffix = "pla_dea")]
    public sealed class OnPlayerDeath : IEvent<OnPlayerDeath>
    {
      public NwPlayer DeadPlayer { get; private set; }
      public NwGameObject Killer { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        DeadPlayer = NWScript.GetLastPlayerDied().ToNwObject<NwPlayer>();
        Killer = NWScript.GetLastHostileActor().ToNwObject<NwGameObject>();
        Callbacks?.Invoke(this);
      }

      public event Action<OnPlayerDeath> Callbacks;
    }

    [EventInfo(EventType.Native, DefaultScriptSuffix = "pla_dyi")]
    public sealed class OnPlayerDying : IEvent<OnPlayerDying>
    {
      public NwPlayer Player { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Player = NWScript.GetLastPlayerDying().ToNwObject<NwPlayer>();
        Callbacks?.Invoke(this);
      }

      public event Action<OnPlayerDying> Callbacks;
    }

    [EventInfo(EventType.Native, DefaultScriptSuffix = "pla_equ")]
    public sealed class OnPlayerEquipItem : IEvent<OnPlayerEquipItem>
    {
      public NwPlayer Player { get; private set; }
      public NwItem Item { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Player = NWScript.GetPCItemLastEquippedBy().ToNwObject<NwPlayer>();
        Item = NWScript.GetPCItemLastEquipped().ToNwObject<NwItem>();
        Callbacks?.Invoke(this);
      }

      public event Action<OnPlayerEquipItem> Callbacks;
    }

    [EventInfo(EventType.Native, DefaultScriptSuffix = "pla_lev")]
    public sealed class OnPlayerLevelUp : IEvent<OnPlayerLevelUp>
    {
      public NwPlayer Player { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Player = NWScript.GetPCLevellingUp().ToNwObject<NwPlayer>();
        Callbacks?.Invoke(this);
      }

      public event Action<OnPlayerLevelUp> Callbacks;
    }

    [EventInfo(EventType.Native, DefaultScriptSuffix = "pla_resp")]
    public sealed class OnPlayerRespawn : IEvent<OnPlayerRespawn>
    {
      public NwPlayer Player { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Player = NWScript.GetLastRespawnButtonPresser().ToNwObject<NwPlayer>();
        Callbacks?.Invoke(this);
      }

      public event Action<OnPlayerRespawn> Callbacks;
    }

    [EventInfo(EventType.Native, DefaultScriptSuffix = "pla_rest")]
    public sealed class OnPlayerRest : IEvent<OnPlayerRest>
    {
      public NwPlayer Player { get; private set; }
      public RestEventType RestEventType { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Player = NWScript.GetLastPCRested().ToNwObject<NwPlayer>();
        RestEventType = (RestEventType) NWScript.GetLastRestEventType();
        Callbacks?.Invoke(this);
      }

      public event Action<OnPlayerRest> Callbacks;
    }

    [EventInfo(EventType.Native, DefaultScriptSuffix = "pla_uneq")]
    public sealed class OnPlayerUnequipItem : IEvent<OnPlayerUnequipItem>
    {
      public NwPlayer UnequippedBy { get; private set; }
      public NwItem Item { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        UnequippedBy = NWScript.GetPCItemLastUnequippedBy().ToNwObject<NwPlayer>();
        Item = NWScript.GetPCItemLastUnequipped().ToNwObject<NwItem>();
        Callbacks?.Invoke(this);
      }

      public event Action<OnPlayerUnequipItem> Callbacks;
    }

    [EventInfo(EventType.Native, DefaultScriptSuffix = "una_ite")]
    public sealed class OnUnacquireItem : IEvent<OnUnacquireItem>
    {
      public NwCreature LostBy { get; private set; }
      public NwItem Item { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        LostBy = NWScript.GetModuleItemLostBy().ToNwObject<NwCreature>();
        Item = NWScript.GetModuleItemLost().ToNwObject<NwItem>();
        Callbacks?.Invoke(this);
      }

      public event Action<OnUnacquireItem> Callbacks;
    }

    [EventInfo(EventType.Native, DefaultScriptSuffix = "use")]
    public sealed class OnUserDefined : IEvent<OnUserDefined>
    {
      public int EventNumber { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        EventNumber = NWScript.GetUserDefinedEventNumber();
        Callbacks?.Invoke(this);
      }

      public event Action<OnUserDefined> Callbacks;
    }
  }
}