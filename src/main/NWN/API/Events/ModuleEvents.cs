using NWN.API.Constants;
using NWN.Core;

namespace NWN.API.Events
{
  // TODO Populate event data.
  /// <summary>
  /// Global module events.
  /// </summary>
  public static class ModuleEvents
  {
    [ScriptEvent(EventScriptType.ModuleOnAcquireItem)]
    public sealed class OnAcquireItem : Event<NwModule, OnAcquireItem>
    {
      public NwItem Item { get; private set; }
      public NwGameObject AcquiredBy { get; private set; }
      public NwGameObject AcquiredFrom { get; private set; }

      protected override void PrepareEvent(NwModule objSelf)
      {
        Item = NWScript.GetModuleItemAcquired().ToNwObject<NwItem>();
        AcquiredBy = NWScript.GetModuleItemAcquiredBy().ToNwObject<NwGameObject>();
        AcquiredFrom = NWScript.GetModuleItemAcquiredFrom().ToNwObject<NwGameObject>();
      }
    }

    [ScriptEvent(EventScriptType.ModuleOnActivateItem)]
    public sealed class OnActivateItem : Event<NwModule, OnActivateItem>
    {
      public NwItem ActivatedItem { get; private set; }
      public NwCreature ItemActivator { get; private set; }
      public NwGameObject TargetObject { get; private set; }
      public Location TargetLocation { get; private set; }

      protected override void PrepareEvent(NwModule objSelf)
      {
        ActivatedItem = NWScript.GetItemActivated().ToNwObject<NwItem>();
        ItemActivator = NWScript.GetItemActivator().ToNwObject<NwCreature>();
        TargetObject = NWScript.GetItemActivatedTarget().ToNwObject<NwGameObject>();
        TargetLocation = NWScript.GetItemActivatedTargetLocation();
      }
    }

    [ScriptEvent(EventScriptType.ModuleOnClientEnter)]
    public sealed class OnClientEnter : Event<NwModule, OnClientEnter>
    {
      public NwPlayer Player { get; private set; }

      protected override void PrepareEvent(NwModule objSelf)
      {
        Player = NWScript.GetEnteringObject().ToNwObject<NwPlayer>();
      }
    }

    [ScriptEvent(EventScriptType.ModuleOnClientExit)]
    public sealed class OnClientLeave : Event<NwModule, OnClientLeave>
    {
      public NwPlayer Player { get; private set; }

      protected override void PrepareEvent(NwModule objSelf)
      {
        Player = NWScript.GetExitingObject().ToNwObject<NwPlayer>();
      }
    }

    [ScriptEvent(EventScriptType.ModuleOnPlayerCancelCutscene)]
    public sealed class OnCutsceneAbort : Event<NwModule, OnCutsceneAbort>
    {
      public NwPlayer Player { get; private set; }

      protected override void PrepareEvent(NwModule objSelf)
      {
        Player = NWScript.GetLastPCToCancelCutscene().ToNwObject<NwPlayer>();
      }
    }

    [ScriptEvent(EventScriptType.ModuleOnHeartbeat)]
    public sealed class OnHeartbeat : Event<NwModule, OnHeartbeat>
    {
      protected override void PrepareEvent(NwModule objSelf) {}
    }

    [ScriptEvent(EventScriptType.ModuleOnModuleLoad)]
    public sealed class OnModuleLoad : Event<NwModule, OnModuleLoad>
    {
      protected override void PrepareEvent(NwModule objSelf) {}
    }

    [ScriptEvent(EventScriptType.ModuleOnModuleStart)]
    public sealed class OnModuleStart : Event<NwModule, OnModuleStart>
    {
      protected override void PrepareEvent(NwModule objSelf) {}
    }

    [ScriptEvent(EventScriptType.ModuleOnPlayerChat)]
    public sealed class OnPlayerChat : Event<NwModule, OnPlayerChat>
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

      protected override void PrepareEvent(NwModule objSelf)
      {
        Sender = NWScript.GetPCChatSpeaker().ToNwObject<NwPlayer>();
      }

      protected override void InvokeCallbacks()
      {
        // Prevent infinite recursion from use of send message in event.
        if (callingChatHandlers)
        {
          return;
        }

        callingChatHandlers = true;
        base.InvokeCallbacks();
        callingChatHandlers = false;
      }
    }

    [ScriptEvent(EventScriptType.ModuleOnPlayerDeath)]
    public sealed class OnPlayerDeath : Event<NwModule, OnPlayerDeath>
    {
      public NwPlayer DeadPlayer { get; private set; }
      public NwGameObject Killer { get; private set; }

      protected override void PrepareEvent(NwModule objSelf)
      {
        DeadPlayer = NWScript.GetLastPlayerDied().ToNwObject<NwPlayer>();
        Killer = NWScript.GetLastHostileActor().ToNwObject<NwGameObject>();
      }
    }

    [ScriptEvent(EventScriptType.ModuleOnPlayerDying)]
    public sealed class OnPlayerDying : Event<NwModule, OnPlayerDying>
    {
      public NwPlayer Player { get; private set; }

      protected override void PrepareEvent(NwModule objSelf)
      {
        Player = NWScript.GetLastPlayerDying().ToNwObject<NwPlayer>();
      }
    }

    [ScriptEvent(EventScriptType.ModuleOnEquipItem)]
    public sealed class OnPlayerEquipItem : Event<NwModule, OnPlayerEquipItem>
    {
      public NwCreature Player { get; private set; }
      public NwItem Item { get; private set; }

      protected override void PrepareEvent(NwModule objSelf)
      {
        Player = NWScript.GetPCItemLastEquippedBy().ToNwObject<NwCreature>();
        Item = NWScript.GetPCItemLastEquipped().ToNwObject<NwItem>();
      }
    }

    [ScriptEvent(EventScriptType.ModuleOnPlayerLevelUp)]
    public sealed class OnPlayerLevelUp : Event<NwModule, OnPlayerLevelUp>
    {
      public NwPlayer Player { get; private set; }

      protected override void PrepareEvent(NwModule objSelf)
      {
        Player = NWScript.GetPCLevellingUp().ToNwObject<NwPlayer>();
      }
    }

    [ScriptEvent(EventScriptType.ModuleOnRespawnButtonPressed)]
    public sealed class OnPlayerRespawn : Event<NwModule, OnPlayerRespawn>
    {
      public NwPlayer Player { get; private set; }

      protected override void PrepareEvent(NwModule objSelf)
      {
        Player = NWScript.GetLastRespawnButtonPresser().ToNwObject<NwPlayer>();
      }
    }

    [ScriptEvent(EventScriptType.ModuleOnPlayerRest)]
    public sealed class OnPlayerRest : Event<NwModule, OnPlayerRest>
    {
      public NwPlayer Player { get; private set; }
      public RestEventType RestEventType { get; private set; }

      protected override void PrepareEvent(NwModule objSelf)
      {
        Player = NWScript.GetLastPCRested().ToNwObject<NwPlayer>();
        RestEventType = (RestEventType) NWScript.GetLastRestEventType();
      }
    }

    [ScriptEvent(EventScriptType.ModuleOnUnequipItem)]
    public sealed class OnPlayerUnequipItem : Event<NwModule, OnPlayerUnequipItem>
    {
      public NwCreature UnequippedBy { get; private set; }
      public NwItem Item { get; private set; }

      protected override void PrepareEvent(NwModule objSelf)
      {
        UnequippedBy = NWScript.GetPCItemLastUnequippedBy().ToNwObject<NwCreature>();
        Item = NWScript.GetPCItemLastUnequipped().ToNwObject<NwItem>();
      }
    }

    [ScriptEvent(EventScriptType.ModuleOnLoseItem)]
    public sealed class OnUnacquireItem : Event<NwModule, OnUnacquireItem>
    {
      public NwCreature LostBy { get; private set; }
      public NwItem Item { get; private set; }

      protected override void PrepareEvent(NwModule objSelf)
      {
        LostBy = NWScript.GetModuleItemLostBy().ToNwObject<NwCreature>();
        Item = NWScript.GetModuleItemLost().ToNwObject<NwItem>();
      }
    }

    [ScriptEvent(EventScriptType.ModuleOnUserDefinedEvent)]
    public sealed class OnUserDefined : Event<NwModule, OnUserDefined>
    {
      public int EventNumber { get; private set; }

      protected override void PrepareEvent(NwModule objSelf)
      {
        EventNumber = NWScript.GetUserDefinedEventNumber();
      }
    }
  }
}