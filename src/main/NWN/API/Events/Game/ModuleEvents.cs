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
    /// Triggered whenever an <see cref="NwItem"/> is added to <see cref="NwPlayer"/> inventory.
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnAcquireItem)]
    public sealed class OnAcquireItem : IEvent
    {
      public NwItem Item { get; } = NWScript.GetModuleItemAcquired().ToNwObject<NwItem>();

      /// <summary>
      /// Gets the <see cref="NwGameObject"/> that acquired the <see cref="NwItem"/>.
      /// </summary>
      public NwGameObject AcquiredBy { get; } = NWScript.GetModuleItemAcquiredBy().ToNwObject<NwGameObject>();

      /// <summary>
      /// Gets the <see cref="NwGameObject"/> that the <see cref="NwItem"/> was taken from.
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
    /// Triggered when a <see cref="NwPlayer"/> selects a character and logged into the module.
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnClientEnter)]
    public sealed class OnClientEnter : IEvent
    {
      public NwPlayer Player { get; } = NWScript.GetEnteringObject().ToNwObject<NwPlayer>();

      NwObject IEvent.Context => Player;
    }

    /// <summary>
    /// Triggered when a <see cref="NwPlayer"/> character leaves the server.
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnClientExit)]
    public sealed class OnClientLeave : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwPlayer"/>  that is leaving.<br/>
      /// Note! This will be a <see cref="NwCreature"/> if the leaving player is possessing a creature.
      /// </summary>
      public NwCreature Player { get; } = NWScript.GetExitingObject().ToNwObject<NwPlayer>();

      NwObject IEvent.Context => Player;
    }

    /// <summary>
    /// Triggered when a <see cref="NwPlayer"/> tries to cancel a cutscene (ESC).
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
    /// Triggered when any <see cref="NwPlayer"/> sends a non-tell based chat message.
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnPlayerChat)]
    public sealed class OnPlayerChat : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwPlayer"/> that sent this message.
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

      /// <summary>
      /// Triggered when a <see cref="NwPlayer"/> that has targeted something.
      /// </summary>
    [GameEvent(EventScriptType.ModuleOnPlayerTarget)]
    public sealed class OnPlayerTarget : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwPlayer"/> that has targeted something.
      /// </summary>
      public NwPlayer Player { get; } = NWScript.GetLastPlayerToSelectTarget().ToNwObject<NwPlayer>();

      /// <summary>
      /// Gets the <see cref="NwObject"/> that has been targeted by <see cref="Player"/>, otherwise the area if a position was selected.
      /// </summary>
      public NwObject TargetObject { get; } = NWScript.GetTargetingModeSelectedObject().ToNwObject();

      /// <summary>
      /// Gets the position targeted by the <see cref="NwPlayer"/>.
      /// </summary>
      public Vector3 TargetPosition { get; } = NWScript.GetTargetingModeSelectedPosition();

      NwObject IEvent.Context => Player;
    }

    /// <summary>
    /// Triggered when a <see cref="NwPlayer"/> dies.
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnPlayerDeath)]
    public sealed class OnPlayerDeath : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwPlayer"/> that has triggered the event.
      /// </summary>
      public NwPlayer DeadPlayer { get; } = NWScript.GetLastPlayerDied().ToNwObject<NwPlayer>();

      /// <summary>
      /// Gets the <see cref="NwGameObject"/> that caused <see cref="NwPlayer"/> to trigger the event.
      /// </summary>
      public NwGameObject Killer { get; } = NWScript.GetLastHostileActor().ToNwObject<NwGameObject>();

      NwObject IEvent.Context => DeadPlayer;
    }

    /// <summary>
    /// Triggered when a <see cref="NwPlayer"/> enters a dying state (&lt; 0 HP).
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnPlayerDying)]
    public sealed class OnPlayerDying : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwPlayer"/> that has triggered the event.
      /// </summary>
      public NwPlayer Player { get; } = NWScript.GetLastPlayerDying().ToNwObject<NwPlayer>();

      NwObject IEvent.Context => Player;
    }

    /// <summary>
    /// Triggered when a <see cref="NwCreature"/> equips an <see cref="NwItem"/>.
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnEquipItem)]
    public sealed class OnPlayerEquipItem : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwCreature"/> that last equipped <see cref="NwItem"/>.
      /// </summary>
      public NwCreature Player { get; } = NWScript.GetPCItemLastEquippedBy().ToNwObject<NwCreature>();

      /// <summary>
      /// Gets the last equipped <see cref="NwItem"/> that triggered the event.
      /// </summary>
      public NwItem Item { get; } = NWScript.GetPCItemLastEquipped().ToNwObject<NwItem>();

      NwObject IEvent.Context => Player;
    }

    /// <summary>
    /// Triggered when a <see cref="NwPlayer"/> levels up.
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnPlayerLevelUp)]
    public sealed class OnPlayerLevelUp : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwPlayer"/> that has triggered the event.
      /// </summary>
      public NwPlayer Player { get; } = NWScript.GetPCLevellingUp().ToNwObject<NwPlayer>();

      NwObject IEvent.Context => Player;
    }

    /// <summary>
    /// Triggered when a <see cref="NwPlayer"/> clicks the respawn button on the death screen.
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnRespawnButtonPressed)]
    public sealed class OnPlayerRespawn : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwPlayer"/> that clicked the respawn button on the death screen.
      /// </summary>
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
