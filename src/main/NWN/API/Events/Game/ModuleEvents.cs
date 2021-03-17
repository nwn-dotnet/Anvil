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
    public sealed class OnAcquireItem : IEvent
    {
      public NwItem Item { get; }

      public NwGameObject AcquiredBy { get; }

      public NwGameObject AcquiredFrom { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Item;

      public OnAcquireItem()
      {
        Item = NWScript.GetModuleItemAcquired().ToNwObject<NwItem>();
        AcquiredBy = NWScript.GetModuleItemAcquiredBy().ToNwObject<NwGameObject>();
        AcquiredFrom = NWScript.GetModuleItemAcquiredFrom().ToNwObject<NwGameObject>();
      }
    }

    [NativeEvent(EventScriptType.ModuleOnActivateItem)]
    public sealed class OnActivateItem : IEvent
    {
      public NwItem ActivatedItem { get; }

      public NwCreature ItemActivator { get; }

      public NwGameObject TargetObject { get; }

      public Location TargetLocation { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => ActivatedItem;

      public OnActivateItem()
      {
        ActivatedItem = NWScript.GetItemActivated().ToNwObject<NwItem>();
        ItemActivator = NWScript.GetItemActivator().ToNwObject<NwCreature>();
        TargetObject = NWScript.GetItemActivatedTarget().ToNwObject<NwGameObject>();
        TargetLocation = NWScript.GetItemActivatedTargetLocation();
      }
    }

    [NativeEvent(EventScriptType.ModuleOnClientEnter)]
    public sealed class OnClientEnter : IEvent
    {
      public NwPlayer Player { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Player;

      public OnClientEnter()
      {
        Player = NWScript.GetEnteringObject().ToNwObject<NwPlayer>();
      }
    }

    [NativeEvent(EventScriptType.ModuleOnClientExit)]
    public sealed class OnClientLeave : IEvent
    {
      public NwPlayer Player { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Player;

      public OnClientLeave()
      {
        Player = NWScript.GetExitingObject().ToNwObject<NwPlayer>();
      }
    }

    [NativeEvent(EventScriptType.ModuleOnPlayerCancelCutscene)]
    public sealed class OnCutsceneAbort : IEvent
    {
      public NwPlayer Player { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Player;

      public OnCutsceneAbort()
      {
        Player = NWScript.GetLastPCToCancelCutscene().ToNwObject<NwPlayer>();
      }
    }

    [NativeEvent(EventScriptType.ModuleOnHeartbeat)]
    public sealed class OnHeartbeat : IEvent
    {
      bool IEvent.HasContext => false;

      NwObject IEvent.Context => null;
    }

    [NativeEvent(EventScriptType.ModuleOnModuleLoad)]
    public sealed class OnModuleLoad : IEvent
    {
      bool IEvent.HasContext => false;

      NwObject IEvent.Context => null;
    }

    [NativeEvent(EventScriptType.ModuleOnModuleStart)]
    public sealed class OnModuleStart : IEvent
    {
      bool IEvent.HasContext => false;

      NwObject IEvent.Context => null;
    }

    [NativeEvent(EventScriptType.ModuleOnPlayerChat)]
    public sealed class OnPlayerChat : IEvent
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

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Sender;
    }

    [NativeEvent(EventScriptType.ModuleOnPlayerTarget)]
    public sealed class OnPlayerTarget : IEvent
    {
      /// <summary>
      /// Gets the player that has targeted something.
      /// </summary>
      public NwPlayer Player { get; }

      /// <summary>
      /// Gets the object that has been targeted by <see cref="Player"/>, otherwise the area if a position was selected.
      /// </summary>
      public NwObject TargetObject { get; }

      /// <summary>
      /// Gets the position targeted by the player.
      /// </summary>
      public Vector3 TargetPosition { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Player;

      public OnPlayerTarget()
      {
        Player = NWScript.GetLastPlayerToSelectTarget().ToNwObject<NwPlayer>();
        TargetObject = NWScript.GetTargetingModeSelectedObject().ToNwObject();
        TargetPosition = NWScript.GetTargetingModeSelectedPosition();
      }
    }

    [NativeEvent(EventScriptType.ModuleOnPlayerDeath)]
    public sealed class OnPlayerDeath : IEvent
    {
      public NwPlayer DeadPlayer { get; }

      public NwGameObject Killer { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => DeadPlayer;

      public OnPlayerDeath()
      {
        DeadPlayer = NWScript.GetLastPlayerDied().ToNwObject<NwPlayer>();
        Killer = NWScript.GetLastHostileActor().ToNwObject<NwGameObject>();
      }
    }

    [NativeEvent(EventScriptType.ModuleOnPlayerDying)]
    public sealed class OnPlayerDying : IEvent
    {
      public NwPlayer Player { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Player;

      public OnPlayerDying()
      {
        Player = NWScript.GetLastPlayerDying().ToNwObject<NwPlayer>();
      }
    }

    [NativeEvent(EventScriptType.ModuleOnEquipItem)]
    public sealed class OnPlayerEquipItem : IEvent
    {
      public NwCreature Player { get; }

      public NwItem Item { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Player;

      public OnPlayerEquipItem()
      {
        Player = NWScript.GetPCItemLastEquippedBy().ToNwObject<NwCreature>();
        Item = NWScript.GetPCItemLastEquipped().ToNwObject<NwItem>();
      }
    }

    [NativeEvent(EventScriptType.ModuleOnPlayerLevelUp)]
    public sealed class OnPlayerLevelUp : IEvent
    {
      public NwPlayer Player { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Player;

      public OnPlayerLevelUp()
      {
        Player = NWScript.GetPCLevellingUp().ToNwObject<NwPlayer>();
      }
    }

    [NativeEvent(EventScriptType.ModuleOnRespawnButtonPressed)]
    public sealed class OnPlayerRespawn : IEvent
    {
      public NwPlayer Player { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Player;

      public OnPlayerRespawn()
      {
        Player = NWScript.GetLastRespawnButtonPresser().ToNwObject<NwPlayer>();
      }
    }

    [NativeEvent(EventScriptType.ModuleOnPlayerRest)]
    public sealed class OnPlayerRest : IEvent
    {
      public NwPlayer Player { get; }

      public RestEventType RestEventType { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Player;

      public OnPlayerRest()
      {
        Player = NWScript.GetLastPCRested().ToNwObject<NwPlayer>();
        RestEventType = (RestEventType) NWScript.GetLastRestEventType();
      }
    }

    [NativeEvent(EventScriptType.ModuleOnUnequipItem)]
    public sealed class OnPlayerUnequipItem : IEvent
    {
      public NwCreature UnequippedBy { get; }

      public NwItem Item { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => UnequippedBy;

      public OnPlayerUnequipItem()
      {
        UnequippedBy = NWScript.GetPCItemLastUnequippedBy().ToNwObject<NwCreature>();
        Item = NWScript.GetPCItemLastUnequipped().ToNwObject<NwItem>();
      }
    }

    [NativeEvent(EventScriptType.ModuleOnLoseItem)]
    public sealed class OnUnacquireItem : IEvent
    {
      public NwCreature LostBy { get; }

      public NwItem Item { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => LostBy;

      public OnUnacquireItem()
      {
        LostBy = NWScript.GetModuleItemLostBy().ToNwObject<NwCreature>();
        Item = NWScript.GetModuleItemLost().ToNwObject<NwItem>();
      }
    }

    [NativeEvent(EventScriptType.ModuleOnUserDefinedEvent)]
    public sealed class OnUserDefined : IEvent
    {
      public int EventNumber { get; }

      bool IEvent.HasContext => false;

      NwObject IEvent.Context => null;

      public OnUserDefined()
      {
        EventNumber = NWScript.GetUserDefinedEventNumber();
      }
    }
  }
}
