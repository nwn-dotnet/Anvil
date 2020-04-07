using System;
using NWM.API.Constants;
using NWN;

namespace NWM.API.Events
{
  public static class ModuleEvents
  {
    [ScriptEvent(EventScriptType.ModuleOnAcquireItem)]
    public sealed class OnAcquireItem : IEvent<NwModule, OnAcquireItem>
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

    [ScriptEvent(EventScriptType.ModuleOnActivateItem)]
    public sealed class OnActivateItem : IEvent<NwModule, OnActivateItem>
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

    [ScriptEvent(EventScriptType.ModuleOnClientEnter)]
    public sealed class OnClientEnter : IEvent<NwModule, OnClientEnter>
    {
      public NwPlayer Player { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Player = NWScript.GetEnteringObject().ToNwObject<NwPlayer>();
        Callbacks?.Invoke(this);
      }

      public event Action<OnClientEnter> Callbacks;
    }

    [ScriptEvent(EventScriptType.ModuleOnClientExit)]
    public sealed class OnClientLeave : IEvent<NwModule, OnClientLeave>
    {
      public NwPlayer Player { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Player = NWScript.GetExitingObject().ToNwObject<NwPlayer>();
        Callbacks?.Invoke(this);
      }

      public event Action<OnClientLeave> Callbacks;
    }

    [ScriptEvent(EventScriptType.ModuleOnPlayerCancelCutscene)]
    public sealed class OnCutsceneAbort : IEvent<NwModule, OnCutsceneAbort>
    {
      public NwPlayer Player { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Player = NWScript.GetLastPCToCancelCutscene().ToNwObject<NwPlayer>();
        Callbacks?.Invoke(this);
      }

      public event Action<OnCutsceneAbort> Callbacks;
    }

    [ScriptEvent(EventScriptType.ModuleOnHeartbeat)]
    public sealed class OnHeartbeat : IEvent<NwModule, OnHeartbeat>
    {
      public void BroadcastEvent(NwObject objSelf)
      {
        Callbacks?.Invoke(this);
      }

      public event Action<OnHeartbeat> Callbacks;
    }

    [ScriptEvent(EventScriptType.ModuleOnModuleLoad)]
    public sealed class OnModuleLoad : IEvent<NwModule, OnModuleLoad>
    {
      public void BroadcastEvent(NwObject objSelf)
      {
        Callbacks?.Invoke(this);
      }

      public event Action<OnModuleLoad> Callbacks;
    }

    [ScriptEvent(EventScriptType.ModuleOnPlayerChat)]
    public sealed class OnPlayerChat : IEvent<NwModule, OnPlayerChat>
    {
      public NwPlayer Sender { get; private set; }
      public string Message
      {
        get => NWScript.GetPCChatMessage();
        set => NWScript.SetPCChatMessage(value);
      }
      public TalkVolume Volume
      {
        get => (TalkVolume) NWScript.GetPCChatVolume();
        set => NWScript.SetPCChatVolume((int) value);
      }

      private bool callingChatHandlers;

      public void BroadcastEvent(NwObject objSelf)
      {
        if (callingChatHandlers || Callbacks == null) return;

        callingChatHandlers = true; // Prevent recursive calls.

        Sender = NWScript.GetPCChatSpeaker().ToNwObject<NwPlayer>();
        Callbacks(this);

        callingChatHandlers = false;
      }

      public event Action<OnPlayerChat> Callbacks;
    }

    [ScriptEvent(EventScriptType.ModuleOnPlayerDeath)]
    public sealed class OnPlayerDeath : IEvent<NwModule, OnPlayerDeath>
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

    [ScriptEvent(EventScriptType.ModuleOnPlayerDying)]
    public sealed class OnPlayerDying : IEvent<NwModule, OnPlayerDying>
    {
      public NwPlayer Player { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Player = NWScript.GetLastPlayerDying().ToNwObject<NwPlayer>();
        Callbacks?.Invoke(this);
      }

      public event Action<OnPlayerDying> Callbacks;
    }

    [ScriptEvent(EventScriptType.ModuleOnEquipItem)]
    public sealed class OnPlayerEquipItem : IEvent<NwModule, OnPlayerEquipItem>
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

    [ScriptEvent(EventScriptType.ModuleOnPlayerLevelUp)]
    public sealed class OnPlayerLevelUp : IEvent<NwModule, OnPlayerLevelUp>
    {
      public NwPlayer Player { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Player = NWScript.GetPCLevellingUp().ToNwObject<NwPlayer>();
        Callbacks?.Invoke(this);
      }

      public event Action<OnPlayerLevelUp> Callbacks;
    }

    [ScriptEvent(EventScriptType.ModuleOnRespawnButtonPressed)]
    public sealed class OnPlayerRespawn : IEvent<NwModule, OnPlayerRespawn>
    {
      public NwPlayer Player { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Player = NWScript.GetLastRespawnButtonPresser().ToNwObject<NwPlayer>();
        Callbacks?.Invoke(this);
      }

      public event Action<OnPlayerRespawn> Callbacks;
    }

    [ScriptEvent(EventScriptType.ModuleOnPlayerRest)]
    public sealed class OnPlayerRest : IEvent<NwModule, OnPlayerRest>
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

    [ScriptEvent(EventScriptType.ModuleOnUnequipItem)]
    public sealed class OnPlayerUnequipItem : IEvent<NwModule, OnPlayerUnequipItem>
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

    [ScriptEvent(EventScriptType.ModuleOnLoseItem)]
    public sealed class OnUnacquireItem : IEvent<NwModule, OnUnacquireItem>
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

    [ScriptEvent(EventScriptType.ModuleOnUserDefinedEvent)]
    public sealed class OnUserDefined : IEvent<NwModule, OnUserDefined>
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