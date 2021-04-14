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
    /// <summary>
    /// Triggered whenever an item is added to someone's inventory.
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnAcquireItem)]
    public sealed class OnAcquireItem : IEvent
    {
      public NwItem Item { get; } = NWScript.GetModuleItemAcquired().ToNwObject<NwItem>();

      /// <summary>
      /// Gets the object that acquired the item.
      /// </summary>
      public NwGameObject AcquiredBy { get; } = NWScript.GetModuleItemAcquiredBy().ToNwObject<NwGameObject>();

      /// <summary>
      /// Gets the object that the item was taken from.
      /// </summary>
      public NwGameObject AcquiredFrom { get; } = NWScript.GetModuleItemAcquiredFrom().ToNwObject<NwGameObject>();

      NwObject IEvent.Context => AcquiredBy;
    }

    /// <summary>
    /// Triggered when an item that has the item property spell "Unique Power" (targeted) or "Unique Power - Self Only" (self) casts its spell.
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnActivateItem)]
    public sealed class OnActivateItem : IEvent
    {
      public NwItem ActivatedItem { get; } = NWScript.GetItemActivated().ToNwObject<NwItem>();

      public NwCreature ItemActivator { get; } = NWScript.GetItemActivator().ToNwObject<NwCreature>();

      public NwGameObject TargetObject { get; } = NWScript.GetItemActivatedTarget().ToNwObject<NwGameObject>();

      public Location TargetLocation { get; } = NWScript.GetItemActivatedTargetLocation();

      NwObject IEvent.Context => ItemActivator;

      public static void Signal(NwItem item, Location targetLocation, NwGameObject targetObject = null)
      {
        Event nwEvent = NWScript.EventActivateItem(item, targetLocation, targetObject);
        NWScript.SignalEvent(NwModule.Instance, nwEvent);
      }
    }

    /// <summary>
    /// Triggered when a player selects a character, and loads into the module.
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnClientEnter)]
    public sealed class OnClientEnter : IEvent
    {
      public NwPlayer Player { get; } = NWScript.GetEnteringObject().ToNwObject<NwPlayer>();

      NwObject IEvent.Context => Player;
    }

    /// <summary>
    /// Triggered when a player character leaves the server.
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnClientExit)]
    public sealed class OnClientLeave : IEvent
    {
      /// <summary>
      /// Gets the player that is leaving.<br/>
      /// Note! This will be a <see cref="NwCreature"/> if the leaving player is possessing a creature.
      /// </summary>
      public NwCreature Player { get; } = NWScript.GetExitingObject().ToNwObject<NwPlayer>();

      NwObject IEvent.Context => Player;
    }

    /// <summary>
    /// Triggered when a player tries to cancel a cutscene (ESC).
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnPlayerCancelCutscene)]
    public sealed class OnCutsceneAbort : IEvent
    {
      public NwPlayer Player { get; } = NWScript.GetLastPCToCancelCutscene().ToNwObject<NwPlayer>();

      NwObject IEvent.Context => Player;
    }

    /// <summary>
    /// Triggered every server heartbeat (~6 seconds).
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnHeartbeat)]
    public sealed class OnHeartbeat : IEvent
    {
      NwObject IEvent.Context => null;
    }

    /// <summary>
    /// Triggered when the module is initially loaded. This event must be hooked in your service constructor, otherwise it will be missed.
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnModuleLoad)]
    public sealed class OnModuleLoad : IEvent
    {
      NwObject IEvent.Context => null;
    }

    [GameEvent(EventScriptType.ModuleOnModuleStart)]
    public sealed class OnModuleStart : IEvent
    {
      NwObject IEvent.Context => null;
    }

    /// <summary>
    /// Triggered when any player sends a non-tell based chat message.
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnPlayerChat)]
    public sealed class OnPlayerChat : IEvent
    {
      /// <summary>
      /// Gets the player that sent this message.
      /// </summary>
      public NwPlayer Sender { get; } = NWScript.GetPCChatSpeaker().ToNwObject<NwPlayer>();

      /// <summary>
      /// Gets or sets the message that is to be sent.
      /// </summary>
      public string Message
      {
        get => NWScript.GetPCChatMessage();
        set => NWScript.SetPCChatMessage(value);
      }

      /// <summary>
      /// Gets or sets the volume of this message.
      /// </summary>
      public TalkVolume Volume
      {
        get => (TalkVolume) NWScript.GetPCChatVolume();
        set => NWScript.SetPCChatVolume((int) value);
      }

      NwObject IEvent.Context => Sender;
    }

    [GameEvent(EventScriptType.ModuleOnPlayerTarget)]
    public sealed class OnPlayerTarget : IEvent
    {
      /// <summary>
      /// Gets the player that has targeted something.
      /// </summary>
      public NwPlayer Player { get; } = NWScript.GetLastPlayerToSelectTarget().ToNwObject<NwPlayer>();

      /// <summary>
      /// Gets the object that has been targeted by <see cref="Player"/>, otherwise the area if a position was selected.
      /// </summary>
      public NwObject TargetObject { get; } = NWScript.GetTargetingModeSelectedObject().ToNwObject();

      /// <summary>
      /// Gets the position targeted by the player.
      /// </summary>
      public Vector3 TargetPosition { get; } = NWScript.GetTargetingModeSelectedPosition();

      NwObject IEvent.Context => Player;
    }

    /// <summary>
    /// Triggered when a player dies.
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnPlayerDeath)]
    public sealed class OnPlayerDeath : IEvent
    {
      public NwPlayer DeadPlayer { get; } = NWScript.GetLastPlayerDied().ToNwObject<NwPlayer>();

      public NwGameObject Killer { get; } = NWScript.GetLastHostileActor().ToNwObject<NwGameObject>();

      NwObject IEvent.Context => DeadPlayer;
    }

    /// <summary>
    /// Triggered when a player enters a dying state (&lt; 0 HP).
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnPlayerDying)]
    public sealed class OnPlayerDying : IEvent
    {
      public NwPlayer Player { get; } = NWScript.GetLastPlayerDying().ToNwObject<NwPlayer>();

      NwObject IEvent.Context => Player;
    }

    /// <summary>
    /// Triggered when a player equips an item.
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnEquipItem)]
    public sealed class OnPlayerEquipItem : IEvent
    {
      public NwCreature Player { get; } = NWScript.GetPCItemLastEquippedBy().ToNwObject<NwCreature>();

      public NwItem Item { get; } = NWScript.GetPCItemLastEquipped().ToNwObject<NwItem>();

      NwObject IEvent.Context => Player;
    }

    /// <summary>
    /// Triggered when a player levels up.
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnPlayerLevelUp)]
    public sealed class OnPlayerLevelUp : IEvent
    {
      public NwPlayer Player { get; } = NWScript.GetPCLevellingUp().ToNwObject<NwPlayer>();

      NwObject IEvent.Context => Player;
    }

    /// <summary>
    /// Triggered when a player clicks the respawn button on the death screen.
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnRespawnButtonPressed)]
    public sealed class OnPlayerRespawn : IEvent
    {
      public NwPlayer Player { get; } = NWScript.GetLastRespawnButtonPresser().ToNwObject<NwPlayer>();

      NwObject IEvent.Context => Player;
    }

    /// <summary>
    /// Triggered when any player presses the rest button, or when the rest is finished.
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnPlayerRest)]
    public sealed class OnPlayerRest : IEvent
    {
      public NwPlayer Player { get; } = NWScript.GetLastPCRested().ToNwObject<NwPlayer>();

      public RestEventType RestEventType { get; } = (RestEventType) NWScript.GetLastRestEventType();

      NwObject IEvent.Context => Player;
    }

    /// <summary>
    /// Triggered just before a player un-equips an item.
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnUnequipItem)]
    public sealed class OnPlayerUnequipItem : IEvent
    {
      public NwCreature UnequippedBy { get; } = NWScript.GetPCItemLastUnequippedBy().ToNwObject<NwCreature>();

      public NwItem Item { get; } = NWScript.GetPCItemLastUnequipped().ToNwObject<NwItem>();

      NwObject IEvent.Context => UnequippedBy;
    }

    /// <summary>
    /// Triggered when an item stack is removed from a creature's inventory.
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnLoseItem)]
    public sealed class OnUnacquireItem : IEvent
    {
      public NwCreature LostBy { get; } = NWScript.GetModuleItemLostBy().ToNwObject<NwCreature>();

      public NwItem Item { get; } = NWScript.GetModuleItemLost().ToNwObject<NwItem>();

      NwObject IEvent.Context => LostBy;
    }

    [GameEvent(EventScriptType.ModuleOnUserDefinedEvent)]
    public sealed class OnUserDefined : IEvent
    {
      public int EventNumber { get; } = NWScript.GetUserDefinedEventNumber();

      NwObject IEvent.Context => null;

      public static void Signal(int eventId)
      {
        Event nwEvent = NWScript.EventUserDefined(eventId);
        NWScript.SignalEvent(NwModule.Instance, nwEvent);
      }
    }
  }
}
