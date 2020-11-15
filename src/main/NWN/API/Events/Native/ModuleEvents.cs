using System.Numerics;
using NWN.API.Constants;
using NWN.Core;

// TODO Populate event data.
namespace NWN.API.Events
{
  /// <summary>
  /// Global module events.
  /// </summary>
  public static class ModuleEvents
  {
    [NativeEvent(EventScriptType.ModuleOnAcquireItem)]
    public sealed class OnAcquireItem : NativeEvent<NwModule, OnAcquireItem>
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

    [NativeEvent(EventScriptType.ModuleOnActivateItem)]
    public sealed class OnActivateItem : NativeEvent<NwModule, OnActivateItem>
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

    [NativeEvent(EventScriptType.ModuleOnClientEnter)]
    public sealed class OnClientEnter : NativeEvent<NwModule, OnClientEnter>
    {
      public NwPlayer Player { get; private set; }

      protected override void PrepareEvent(NwModule objSelf)
      {
        Player = NWScript.GetEnteringObject().ToNwObject<NwPlayer>();
      }
    }

    [NativeEvent(EventScriptType.ModuleOnClientExit)]
    public sealed class OnClientLeave : NativeEvent<NwModule, OnClientLeave>
    {
      public NwPlayer Player { get; private set; }

      protected override void PrepareEvent(NwModule objSelf)
      {
        Player = NWScript.GetExitingObject().ToNwObject<NwPlayer>();
      }
    }

    [NativeEvent(EventScriptType.ModuleOnPlayerCancelCutscene)]
    public sealed class OnCutsceneAbort : NativeEvent<NwModule, OnCutsceneAbort>
    {
      public NwPlayer Player { get; private set; }

      protected override void PrepareEvent(NwModule objSelf)
      {
        Player = NWScript.GetLastPCToCancelCutscene().ToNwObject<NwPlayer>();
      }
    }

    [NativeEvent(EventScriptType.ModuleOnHeartbeat)]
    public sealed class OnHeartbeat : NativeEvent<NwModule, OnHeartbeat>
    {
      protected override void PrepareEvent(NwModule objSelf) {}
    }

    [NativeEvent(EventScriptType.ModuleOnModuleLoad)]
    public sealed class OnModuleLoad : NativeEvent<NwModule, OnModuleLoad>
    {
      protected override void PrepareEvent(NwModule objSelf) {}
    }

    [NativeEvent(EventScriptType.ModuleOnModuleStart)]
    public sealed class OnModuleStart : NativeEvent<NwModule, OnModuleStart>
    {
      protected override void PrepareEvent(NwModule objSelf) {}
    }

    [NativeEvent(EventScriptType.ModuleOnPlayerChat)]
    public sealed class OnPlayerChat : NativeEvent<NwModule, OnPlayerChat>
    {
      public NwPlayer Sender { get; private set; }

      public static string Message
      {
        get => NWScript.GetPCChatMessage();
        set => NWScript.SetPCChatMessage(value);
      }

      public static TalkVolume Volume
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

    [NativeEvent(EventScriptType.ModuleOnPlayerTarget)]
    public sealed class OnPlayerTarget : NativeEvent<NwModule, OnPlayerTarget>
    {
      /// <summary>
      /// Gets the player that has targeted something.
      /// </summary>
      public NwPlayer Player { get; private set; }

      /// <summary>
      /// Gets the object that has been targeted by <see cref="Player"/>, otherwise the area if a position was selected.
      /// </summary>
      public NwObject TargetObject { get; private set; }

      /// <summary>
      /// Gets the position targeted by the player.
      /// </summary>
      public Vector3 TargetPosition { get; private set; }

      protected override void PrepareEvent(NwModule objSelf)
      {
        Player = NWScript.GetLastPlayerToSelectTarget().ToNwObject<NwPlayer>();
        TargetObject = NWScript.GetTargetingModeSelectedObject().ToNwObject();
        TargetPosition = NWScript.GetTargetingModeSelectedPosition();
      }
    }

    [NativeEvent(EventScriptType.ModuleOnPlayerDeath)]
    public sealed class OnPlayerDeath : NativeEvent<NwModule, OnPlayerDeath>
    {
      public NwPlayer DeadPlayer { get; private set; }

      public NwGameObject Killer { get; private set; }

      protected override void PrepareEvent(NwModule objSelf)
      {
        DeadPlayer = NWScript.GetLastPlayerDied().ToNwObject<NwPlayer>();
        Killer = NWScript.GetLastHostileActor().ToNwObject<NwGameObject>();
      }
    }

    [NativeEvent(EventScriptType.ModuleOnPlayerDying)]
    public sealed class OnPlayerDying : NativeEvent<NwModule, OnPlayerDying>
    {
      public NwPlayer Player { get; private set; }

      protected override void PrepareEvent(NwModule objSelf)
      {
        Player = NWScript.GetLastPlayerDying().ToNwObject<NwPlayer>();
      }
    }

    [NativeEvent(EventScriptType.ModuleOnEquipItem)]
    public sealed class OnPlayerEquipItem : NativeEvent<NwModule, OnPlayerEquipItem>
    {
      public NwCreature Player { get; private set; }

      public NwItem Item { get; private set; }

      protected override void PrepareEvent(NwModule objSelf)
      {
        Player = NWScript.GetPCItemLastEquippedBy().ToNwObject<NwCreature>();
        Item = NWScript.GetPCItemLastEquipped().ToNwObject<NwItem>();
      }
    }

    [NativeEvent(EventScriptType.ModuleOnPlayerLevelUp)]
    public sealed class OnPlayerLevelUp : NativeEvent<NwModule, OnPlayerLevelUp>
    {
      public NwPlayer Player { get; private set; }

      protected override void PrepareEvent(NwModule objSelf)
      {
        Player = NWScript.GetPCLevellingUp().ToNwObject<NwPlayer>();
      }
    }

    [NativeEvent(EventScriptType.ModuleOnRespawnButtonPressed)]
    public sealed class OnPlayerRespawn : NativeEvent<NwModule, OnPlayerRespawn>
    {
      public NwPlayer Player { get; private set; }

      protected override void PrepareEvent(NwModule objSelf)
      {
        Player = NWScript.GetLastRespawnButtonPresser().ToNwObject<NwPlayer>();
      }
    }

    [NativeEvent(EventScriptType.ModuleOnPlayerRest)]
    public sealed class OnPlayerRest : NativeEvent<NwModule, OnPlayerRest>
    {
      public NwPlayer Player { get; private set; }

      public RestEventType RestEventType { get; private set; }

      protected override void PrepareEvent(NwModule objSelf)
      {
        Player = NWScript.GetLastPCRested().ToNwObject<NwPlayer>();
        RestEventType = (RestEventType) NWScript.GetLastRestEventType();
      }
    }

    [NativeEvent(EventScriptType.ModuleOnUnequipItem)]
    public sealed class OnPlayerUnequipItem : NativeEvent<NwModule, OnPlayerUnequipItem>
    {
      public NwCreature UnequippedBy { get; private set; }

      public NwItem Item { get; private set; }

      protected override void PrepareEvent(NwModule objSelf)
      {
        UnequippedBy = NWScript.GetPCItemLastUnequippedBy().ToNwObject<NwCreature>();
        Item = NWScript.GetPCItemLastUnequipped().ToNwObject<NwItem>();
      }
    }

    [NativeEvent(EventScriptType.ModuleOnLoseItem)]
    public sealed class OnUnacquireItem : NativeEvent<NwModule, OnUnacquireItem>
    {
      public NwCreature LostBy { get; private set; }

      public NwItem Item { get; private set; }

      protected override void PrepareEvent(NwModule objSelf)
      {
        LostBy = NWScript.GetModuleItemLostBy().ToNwObject<NwCreature>();
        Item = NWScript.GetModuleItemLost().ToNwObject<NwItem>();
      }
    }

    [NativeEvent(EventScriptType.ModuleOnUserDefinedEvent)]
    public sealed class OnUserDefined : NativeEvent<NwModule, OnUserDefined>
    {
      public int EventNumber { get; private set; }

      protected override void PrepareEvent(NwModule objSelf)
      {
        EventNumber = NWScript.GetUserDefinedEventNumber();
      }
    }
  }
}
