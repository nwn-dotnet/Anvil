using System;
using System.Numerics;
using Anvil.API.Events;
using NWN.Core;

namespace Anvil.API.Events
{
  /// <summary>
  /// Global module events.
  /// </summary>
  public static class ModuleEvents
  {
    /// <summary>
    /// Triggered whenever an <see cref="NwItem"/> is added to <see cref="NwGameObject"/> inventory.
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnAcquireItem)]
    public sealed class OnAcquireItem : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwItem"/> that triggered the event.
      /// </summary>
      public NwItem Item { get; } = NWScript.GetModuleItemAcquired().ToNwObject<NwItem>();

      /// <summary>
      /// Gets the <see cref="NwGameObject"/> that acquired the <see cref="NwItem"/>.
      /// </summary>
      public NwGameObject AcquiredBy { get; } = NWScript.GetModuleItemAcquiredBy().ToNwObject<NwGameObject>();

      /// <summary>
      /// Gets the <see cref="NwGameObject"/> that the <see cref="NwItem"/> was taken from.
      /// </summary>
      public NwGameObject AcquiredFrom { get; } = NWScript.GetModuleItemAcquiredFrom().ToNwObject<NwGameObject>();

      /// <summary>
      /// Gets the number of items in the item stack that were just acquired.
      /// </summary>
      public int AmountAcquired { get; } = NWScript.GetModuleItemAcquiredStackSize();

      NwObject IEvent.Context
      {
        get => AcquiredBy;
      }

      public OnAcquireItem()
      {
        // Patch player reference due to a reference bug during client enter context
        // See https://github.com/Beamdog/nwn-issues/issues/367
        if (AcquiredBy is null && Item?.Possessor is NwCreature creature)
        {
          AcquiredBy = creature;
        }
      }
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

      NwObject IEvent.Context
      {
        get => ItemActivator;
      }

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
      /// <summary>
      /// Gets the <see cref="NwPlayer"/> that triggered the event.
      /// </summary>
      public NwPlayer Player { get; } = NWScript.GetEnteringObject().ToNwPlayer();

      NwObject IEvent.Context
      {
        get => Player.ControlledCreature;
      }
    }

    /// <summary>
    /// Triggered when a <see cref="NwCreature"/> leaves the server.
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnClientExit)]
    public sealed class OnClientLeave : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwPlayer"/> that is leaving.
      /// </summary>
      public NwPlayer Player { get; } = NWScript.GetExitingObject().ToNwPlayer(PlayerSearch.Login);

      NwObject IEvent.Context
      {
        get => Player.LoginCreature;
      }
    }

    /// <summary>
    /// Triggered when a <see cref="NwPlayer"/> tries to cancel a cutscene (ESC).
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnPlayerCancelCutscene)]
    public sealed class OnCutsceneAbort : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwPlayer"/> that triggered the event.
      /// </summary>
      public NwPlayer Player { get; } = NWScript.GetLastPCToCancelCutscene().ToNwPlayer();

      NwObject IEvent.Context
      {
        get => Player.ControlledCreature;
      }
    }

    /// <summary>
    /// Triggered every server heartbeat (~6 seconds).
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnHeartbeat)]
    public sealed class OnHeartbeat : IEvent
    {
      NwObject IEvent.Context
      {
        get => null;
      }
    }

    /// <summary>
    /// Triggered when the module is initially loaded. This event must be hooked in your service constructor, otherwise it will be missed.
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnModuleLoad)]
    public sealed class OnModuleLoad : IEvent
    {
      NwObject IEvent.Context
      {
        get => null;
      }
    }

    [GameEvent(EventScriptType.ModuleOnModuleStart)]
    public sealed class OnModuleStart : IEvent
    {
      NwObject IEvent.Context
      {
        get => null;
      }
    }

    /// <summary>
    /// Triggered when any <see cref="NwPlayer"/> sends a chat message. Private channel not hooked.
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnPlayerChat)]
    public sealed class OnPlayerChat : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwPlayer"/> that sent this message.
      /// </summary>
      public NwPlayer Sender { get; } = NWScript.GetPCChatSpeaker().ToNwPlayer();

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
        get => (TalkVolume)NWScript.GetPCChatVolume();
        set => NWScript.SetPCChatVolume((int)value);
      }

      NwObject IEvent.Context
      {
        get => Sender.ControlledCreature;
      }
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
      public NwPlayer Player { get; } = NWScript.GetLastPlayerToSelectTarget().ToNwPlayer();

      /// <summary>
      /// Gets the <see cref="NwObject"/> that has been targeted by <see cref="Player"/>, otherwise the area if a position was selected.
      /// </summary>
      public NwObject TargetObject { get; } = NWScript.GetTargetingModeSelectedObject().ToNwObject();

      /// <summary>
      /// Gets the position targeted by the <see cref="NwPlayer"/>.
      /// </summary>
      public Vector3 TargetPosition { get; } = NWScript.GetTargetingModeSelectedPosition();

      /// <summary>
      /// Gets if the player cancelled target selection.
      /// </summary>
      public bool IsCancelled
      {
        get => TargetObject == null;
      }

      NwObject IEvent.Context
      {
        get => Player.ControlledCreature;
      }
    }

    /// <summary>
    /// Triggered when a player clicks on a particular GUI interface.
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnPlayerGuiEvent)]
    public sealed class OnPlayerGuiEvent : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwPlayer"/> that triggered this event.
      /// </summary>
      public NwPlayer Player { get; } = NWScript.GetLastGuiEventPlayer().ToNwPlayer();

      /// <summary>
      /// Gets the <see cref="GuiEventType"/> that was triggered.
      /// </summary>
      public GuiEventType EventType { get; } = (GuiEventType)NWScript.GetLastGuiEventType();

      /// <summary>
      /// Gets the object data associated with this GUI event.
      /// </summary>
      /// <remarks>
      /// <see cref="GuiEventType.ChatBarFocus"/>: The selected chat channel. Does not indicate the actual used channel. 0 = Shout, 1 = Whisper, 2 = Talk, 3 = Party, 4 = DM
      /// <see cref="GuiEventType.CharacterSheetSkillClick"/>: The <see cref="Skill"/>
      /// </remarks>
      public NwObject EventObject { get; } = NWScript.GetLastGuiEventObject().ToNwObject();

      NwObject IEvent.Context
      {
        get => Player.ControlledCreature;
      }
    }

    /// <summary>
    /// Triggered when a player performs an action on an area tile.
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnPlayerTileAction)]
    public sealed class OnPlayerTileAction : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwPlayer"/> that performed a tile action.
      /// </summary>
      public NwPlayer Player { get; } = NWScript.GetLastPlayerToDoTileAction().ToNwPlayer();

      /// <summary>
      /// Gets the position that was clicked.
      /// </summary>
      public Vector3 TargetPosition { get; } = NWScript.GetLastTileActionPosition();

      /// <summary>
      /// Gets the action ID (surfacemat.2da) that was selected by the player.
      /// </summary>
      public int ActionId { get; } = NWScript.GetLastTileActionId();

      NwObject IEvent.Context
      {
        get => Player.ControlledCreature;
      }
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
      public NwPlayer DeadPlayer { get; } = NWScript.GetLastPlayerDied().ToNwPlayer();

      /// <summary>
      /// Gets the <see cref="NwGameObject"/> that caused <see cref="NwPlayer"/> to trigger the event.
      /// </summary>
      public NwGameObject Killer { get; }

      NwObject IEvent.Context
      {
        get => DeadPlayer.ControlledCreature;
      }

      public OnPlayerDeath()
      {
        Killer = NWScript.GetLastHostileActor(DeadPlayer.ControlledCreature).ToNwObject<NwGameObject>();
      }
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
      public NwPlayer Player { get; } = NWScript.GetLastPlayerDying().ToNwPlayer();

      NwObject IEvent.Context
      {
        get => Player.ControlledCreature;
      }
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

      NwObject IEvent.Context
      {
        get => Player;
      }

      public OnPlayerEquipItem()
      {
        // Patch player reference due to a reference bug during client enter context
        // See https://github.com/Beamdog/nwn-issues/issues/367
        if (Player is null && Item.Possessor is NwCreature creature)
        {
          Player = creature;
        }
      }
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
      public NwPlayer Player { get; } = NWScript.GetPCLevellingUp().ToNwPlayer();

      NwObject IEvent.Context
      {
        get => Player.ControlledCreature;
      }
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
      public NwPlayer Player { get; } = NWScript.GetLastRespawnButtonPresser().ToNwPlayer();

      NwObject IEvent.Context
      {
        get => Player.ControlledCreature;
      }
    }

    /// <summary>
    /// Triggered when <see cref="NwPlayer"/> presses the rest button and begins to rest, cancelled rest, or finished rest.
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnPlayerRest)]
    public sealed class OnPlayerRest : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwPlayer"/> that triggered the event.
      /// </summary>
      public NwPlayer Player { get; } = NWScript.GetLastPCRested().ToNwPlayer();

      /// <summary>
      /// Gets the <see cref="RestEventType"/> that was triggered.
      /// </summary>
      public RestEventType RestEventType { get; } = (RestEventType)NWScript.GetLastRestEventType();

      NwObject IEvent.Context
      {
        get => Player.ControlledCreature;
      }
    }

    /// <summary>
    /// Triggered just before a <see cref="NwCreature"/> un-equips an <see cref="NwItem"/>.
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnUnequipItem)]
    public sealed class OnPlayerUnequipItem : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwCreature"/> that triggered the event.
      /// </summary>
      public NwCreature UnequippedBy { get; } = NWScript.GetPCItemLastUnequippedBy().ToNwObject<NwCreature>();

      /// <summary>
      /// Gets the <see cref="NwItem"/> that was last unequipped.
      /// </summary>
      public NwItem Item { get; } = NWScript.GetPCItemLastUnequipped().ToNwObject<NwItem>();

      NwObject IEvent.Context
      {
        get => UnequippedBy;
      }
    }

    /// <summary>
    /// Triggered when a <see cref="NwItem"/> is removed from a <see cref="NwCreature"/>'s inventory.
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnLoseItem)]
    public sealed class OnUnacquireItem : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwCreature"/> that lost the <see cref="NwItem"/>.
      /// </summary>
      public NwCreature LostBy { get; } = NWScript.GetModuleItemLostBy().ToNwObject<NwCreature>();

      /// <summary>
      /// Gets the <see cref="NwItem"/> that was lost by <see cref="NwCreature"/>.
      /// </summary>
      public NwItem Item { get; } = NWScript.GetModuleItemLost().ToNwObject<NwItem>();

      NwObject IEvent.Context
      {
        get => LostBy;
      }
    }

    [GameEvent(EventScriptType.ModuleOnUserDefinedEvent)]
    public sealed class OnUserDefined : IEvent
    {
      public int EventNumber { get; } = NWScript.GetUserDefinedEventNumber();

      NwObject IEvent.Context
      {
        get => null;
      }

      public static void Signal(int eventId)
      {
        Event nwEvent = NWScript.EventUserDefined(eventId);
        NWScript.SignalEvent(NwModule.Instance, nwEvent);
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwModule
  {
    /// <inheritdoc cref="ModuleEvents.OnAcquireItem"/>
    public event Action<ModuleEvents.OnAcquireItem> OnAcquireItem
    {
      add => EventService.SubscribeAll<ModuleEvents.OnAcquireItem, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnAcquireItem, GameEventFactory>(value);
    }

    /// <inheritdoc cref="ModuleEvents.OnActivateItem"/>
    public event Action<ModuleEvents.OnActivateItem> OnActivateItem
    {
      add => EventService.SubscribeAll<ModuleEvents.OnActivateItem, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnActivateItem, GameEventFactory>(value);
    }

    /// <inheritdoc cref="ModuleEvents.OnClientEnter"/>
    public event Action<ModuleEvents.OnClientEnter> OnClientEnter
    {
      add => EventService.SubscribeAll<ModuleEvents.OnClientEnter, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnClientEnter, GameEventFactory>(value);
    }

    /// <inheritdoc cref="ModuleEvents.OnClientLeave"/>
    public event Action<ModuleEvents.OnClientLeave> OnClientLeave
    {
      add => EventService.SubscribeAll<ModuleEvents.OnClientLeave, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnClientLeave, GameEventFactory>(value);
    }

    /// <inheritdoc cref="ModuleEvents.OnCutsceneAbort"/>
    public event Action<ModuleEvents.OnCutsceneAbort> OnCutsceneAbort
    {
      add => EventService.SubscribeAll<ModuleEvents.OnCutsceneAbort, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnCutsceneAbort, GameEventFactory>(value);
    }

    /// <inheritdoc cref="ModuleEvents.OnHeartbeat"/>
    public event Action<ModuleEvents.OnHeartbeat> OnHeartbeat
    {
      add => EventService.SubscribeAll<ModuleEvents.OnHeartbeat, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnHeartbeat, GameEventFactory>(value);
    }

    /// <inheritdoc cref="ModuleEvents.OnModuleLoad"/>
    public event Action<ModuleEvents.OnModuleLoad> OnModuleLoad
    {
      add => EventService.SubscribeAll<ModuleEvents.OnModuleLoad, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnModuleLoad, GameEventFactory>(value);
    }

    /// <inheritdoc cref="ModuleEvents.OnModuleStart"/>
    public event Action<ModuleEvents.OnModuleStart> OnModuleStart
    {
      add => EventService.SubscribeAll<ModuleEvents.OnModuleStart, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnModuleStart, GameEventFactory>(value);
    }

    /// <inheritdoc cref="ModuleEvents.OnPlayerChat"/>
    public event Action<ModuleEvents.OnPlayerChat> OnPlayerChat
    {
      add => EventService.SubscribeAll<ModuleEvents.OnPlayerChat, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnPlayerChat, GameEventFactory>(value);
    }

    /// <inheritdoc cref="ModuleEvents.OnPlayerTarget"/>
    public event Action<ModuleEvents.OnPlayerTarget> OnPlayerTarget
    {
      add => EventService.SubscribeAll<ModuleEvents.OnPlayerTarget, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnPlayerTarget, GameEventFactory>(value);
    }

    /// <inheritdoc cref="ModuleEvents.OnPlayerGuiEvent"/>
    public event Action<ModuleEvents.OnPlayerGuiEvent> OnPlayerGuiEvent
    {
      add => EventService.SubscribeAll<ModuleEvents.OnPlayerGuiEvent, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnPlayerGuiEvent, GameEventFactory>(value);
    }

    /// <inheritdoc cref="ModuleEvents.OnPlayerTileAction"/>
    public event Action<ModuleEvents.OnPlayerTileAction> OnPlayerTileAction
    {
      add => EventService.SubscribeAll<ModuleEvents.OnPlayerTileAction, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnPlayerTileAction, GameEventFactory>(value);
    }

    /// <inheritdoc cref="ModuleEvents.OnPlayerDeath"/>
    public event Action<ModuleEvents.OnPlayerDeath> OnPlayerDeath
    {
      add => EventService.SubscribeAll<ModuleEvents.OnPlayerDeath, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnPlayerDeath, GameEventFactory>(value);
    }

    /// <inheritdoc cref="ModuleEvents.OnPlayerDying"/>
    public event Action<ModuleEvents.OnPlayerDying> OnPlayerDying
    {
      add => EventService.SubscribeAll<ModuleEvents.OnPlayerDying, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnPlayerDying, GameEventFactory>(value);
    }

    /// <inheritdoc cref="ModuleEvents.OnPlayerEquipItem"/>
    public event Action<ModuleEvents.OnPlayerEquipItem> OnPlayerEquipItem
    {
      add => EventService.SubscribeAll<ModuleEvents.OnPlayerEquipItem, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnPlayerEquipItem, GameEventFactory>(value);
    }

    /// <inheritdoc cref="ModuleEvents.OnPlayerLevelUp"/>
    public event Action<ModuleEvents.OnPlayerLevelUp> OnPlayerLevelUp
    {
      add => EventService.SubscribeAll<ModuleEvents.OnPlayerLevelUp, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnPlayerLevelUp, GameEventFactory>(value);
    }

    /// <inheritdoc cref="ModuleEvents.OnPlayerRespawn"/>
    public event Action<ModuleEvents.OnPlayerRespawn> OnPlayerRespawn
    {
      add => EventService.SubscribeAll<ModuleEvents.OnPlayerRespawn, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnPlayerRespawn, GameEventFactory>(value);
    }

    /// <inheritdoc cref="ModuleEvents.OnPlayerRest"/>
    public event Action<ModuleEvents.OnPlayerRest> OnPlayerRest
    {
      add => EventService.SubscribeAll<ModuleEvents.OnPlayerRest, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnPlayerRest, GameEventFactory>(value);
    }

    /// <inheritdoc cref="ModuleEvents.OnPlayerUnequipItem"/>
    public event Action<ModuleEvents.OnPlayerUnequipItem> OnPlayerUnequipItem
    {
      add => EventService.SubscribeAll<ModuleEvents.OnPlayerUnequipItem, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnPlayerUnequipItem, GameEventFactory>(value);
    }

    /// <inheritdoc cref="ModuleEvents.OnUnacquireItem"/>
    public event Action<ModuleEvents.OnUnacquireItem> OnUnacquireItem
    {
      add => EventService.SubscribeAll<ModuleEvents.OnUnacquireItem, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnUnacquireItem, GameEventFactory>(value);
    }

    /// <inheritdoc cref="ModuleEvents.OnUserDefined"/>
    public event Action<ModuleEvents.OnUserDefined> OnUserDefined
    {
      add => EventService.SubscribeAll<ModuleEvents.OnUserDefined, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnUserDefined, GameEventFactory>(value);
    }
  }

  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="ModuleEvents.OnClientEnter"/>
    public event Action<ModuleEvents.OnClientEnter> OnClientEnter
    {
      add => EventService.Subscribe<ModuleEvents.OnClientEnter, GameEventFactory, GameEventFactory.RegistrationData>(ControlledCreature, new GameEventFactory.RegistrationData(NwModule.Instance), value);
      remove => EventService.Unsubscribe<ModuleEvents.OnClientEnter, GameEventFactory>(ControlledCreature, value);
    }

    /// <inheritdoc cref="ModuleEvents.OnClientLeave"/>
    public event Action<ModuleEvents.OnClientLeave> OnClientLeave
    {
      add => EventService.Subscribe<ModuleEvents.OnClientLeave, GameEventFactory, GameEventFactory.RegistrationData>(ControlledCreature, new GameEventFactory.RegistrationData(NwModule.Instance), value);
      remove => EventService.Unsubscribe<ModuleEvents.OnClientLeave, GameEventFactory>(ControlledCreature, value);
    }

    /// <inheritdoc cref="ModuleEvents.OnCutsceneAbort"/>
    public event Action<ModuleEvents.OnCutsceneAbort> OnCutsceneAbort
    {
      add => EventService.Subscribe<ModuleEvents.OnCutsceneAbort, GameEventFactory, GameEventFactory.RegistrationData>(ControlledCreature, new GameEventFactory.RegistrationData(NwModule.Instance), value);
      remove => EventService.Unsubscribe<ModuleEvents.OnCutsceneAbort, GameEventFactory>(ControlledCreature, value);
    }

    /// <inheritdoc cref="ModuleEvents.OnPlayerChat"/>
    public event Action<ModuleEvents.OnPlayerChat> OnPlayerChat
    {
      add => EventService.Subscribe<ModuleEvents.OnPlayerChat, GameEventFactory, GameEventFactory.RegistrationData>(ControlledCreature, new GameEventFactory.RegistrationData(NwModule.Instance), value);
      remove => EventService.Unsubscribe<ModuleEvents.OnPlayerChat, GameEventFactory>(ControlledCreature, value);
    }

    /// <inheritdoc cref="ModuleEvents.OnPlayerTarget"/>
    public event Action<ModuleEvents.OnPlayerTarget> OnPlayerTarget
    {
      add => EventService.Subscribe<ModuleEvents.OnPlayerTarget, GameEventFactory, GameEventFactory.RegistrationData>(ControlledCreature, new GameEventFactory.RegistrationData(NwModule.Instance), value);
      remove => EventService.Unsubscribe<ModuleEvents.OnPlayerTarget, GameEventFactory>(ControlledCreature, value);
    }

    /// <inheritdoc cref="ModuleEvents.OnPlayerGuiEvent"/>
    public event Action<ModuleEvents.OnPlayerGuiEvent> OnPlayerGuiEvent
    {
      add => EventService.Subscribe<ModuleEvents.OnPlayerGuiEvent, GameEventFactory, GameEventFactory.RegistrationData>(ControlledCreature, new GameEventFactory.RegistrationData(NwModule.Instance), value);
      remove => EventService.Unsubscribe<ModuleEvents.OnPlayerGuiEvent, GameEventFactory>(ControlledCreature, value);
    }

    /// <inheritdoc cref="ModuleEvents.OnPlayerTileAction"/>
    public event Action<ModuleEvents.OnPlayerTileAction> OnPlayerTileAction
    {
      add => EventService.Subscribe<ModuleEvents.OnPlayerTileAction, GameEventFactory, GameEventFactory.RegistrationData>(ControlledCreature, new GameEventFactory.RegistrationData(NwModule.Instance), value);
      remove => EventService.Unsubscribe<ModuleEvents.OnPlayerTileAction, GameEventFactory>(ControlledCreature, value);
    }

    /// <inheritdoc cref="ModuleEvents.OnPlayerDeath"/>
    public event Action<ModuleEvents.OnPlayerDeath> OnPlayerDeath
    {
      add => EventService.Subscribe<ModuleEvents.OnPlayerDeath, GameEventFactory, GameEventFactory.RegistrationData>(ControlledCreature, new GameEventFactory.RegistrationData(NwModule.Instance), value);
      remove => EventService.Unsubscribe<ModuleEvents.OnPlayerDeath, GameEventFactory>(ControlledCreature, value);
    }

    /// <inheritdoc cref="ModuleEvents.OnPlayerDying"/>
    public event Action<ModuleEvents.OnPlayerDying> OnPlayerDying
    {
      add => EventService.Subscribe<ModuleEvents.OnPlayerDying, GameEventFactory, GameEventFactory.RegistrationData>(ControlledCreature, new GameEventFactory.RegistrationData(NwModule.Instance), value);
      remove => EventService.Unsubscribe<ModuleEvents.OnPlayerDying, GameEventFactory>(ControlledCreature, value);
    }

    /// <inheritdoc cref="ModuleEvents.OnPlayerEquipItem"/>
    public event Action<ModuleEvents.OnPlayerEquipItem> OnPlayerEquipItem
    {
      add => EventService.Subscribe<ModuleEvents.OnPlayerEquipItem, GameEventFactory, GameEventFactory.RegistrationData>(ControlledCreature, new GameEventFactory.RegistrationData(NwModule.Instance), value);
      remove => EventService.Unsubscribe<ModuleEvents.OnPlayerEquipItem, GameEventFactory>(ControlledCreature, value);
    }

    /// <inheritdoc cref="ModuleEvents.OnPlayerLevelUp"/>
    public event Action<ModuleEvents.OnPlayerLevelUp> OnPlayerLevelUp
    {
      add => EventService.Subscribe<ModuleEvents.OnPlayerLevelUp, GameEventFactory, GameEventFactory.RegistrationData>(ControlledCreature, new GameEventFactory.RegistrationData(NwModule.Instance), value);
      remove => EventService.Unsubscribe<ModuleEvents.OnPlayerLevelUp, GameEventFactory>(ControlledCreature, value);
    }

    /// <inheritdoc cref="ModuleEvents.OnPlayerRespawn"/>
    public event Action<ModuleEvents.OnPlayerRespawn> OnPlayerRespawn
    {
      add => EventService.Subscribe<ModuleEvents.OnPlayerRespawn, GameEventFactory, GameEventFactory.RegistrationData>(ControlledCreature, new GameEventFactory.RegistrationData(NwModule.Instance), value);
      remove => EventService.Unsubscribe<ModuleEvents.OnPlayerRespawn, GameEventFactory>(ControlledCreature, value);
    }

    /// <inheritdoc cref="ModuleEvents.OnPlayerRest"/>
    public event Action<ModuleEvents.OnPlayerRest> OnPlayerRest
    {
      add => EventService.Subscribe<ModuleEvents.OnPlayerRest, GameEventFactory, GameEventFactory.RegistrationData>(ControlledCreature, new GameEventFactory.RegistrationData(NwModule.Instance), value);
      remove => EventService.Unsubscribe<ModuleEvents.OnPlayerRest, GameEventFactory>(ControlledCreature, value);
    }

    /// <inheritdoc cref="ModuleEvents.OnPlayerUnequipItem"/>
    public event Action<ModuleEvents.OnPlayerUnequipItem> OnPlayerUnequipItem
    {
      add => EventService.Subscribe<ModuleEvents.OnPlayerUnequipItem, GameEventFactory, GameEventFactory.RegistrationData>(ControlledCreature, new GameEventFactory.RegistrationData(NwModule.Instance), value);
      remove => EventService.Unsubscribe<ModuleEvents.OnPlayerUnequipItem, GameEventFactory>(ControlledCreature, value);
    }
  }

  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="ModuleEvents.OnActivateItem"/>
    public event Action<ModuleEvents.OnActivateItem> OnActivateItem
    {
      add => EventService.Subscribe<ModuleEvents.OnActivateItem, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(NwModule.Instance), value);
      remove => EventService.Unsubscribe<ModuleEvents.OnActivateItem, GameEventFactory>(this, value);
    }
  }

  public abstract partial class NwGameObject
  {
    /// <inheritdoc cref="ModuleEvents.OnAcquireItem"/>
    public event Action<ModuleEvents.OnAcquireItem> OnAcquireItem
    {
      add => EventService.Subscribe<ModuleEvents.OnAcquireItem, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(NwModule.Instance), value);
      remove => EventService.Unsubscribe<ModuleEvents.OnAcquireItem, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="ModuleEvents.OnUnacquireItem"/>
    public event Action<ModuleEvents.OnUnacquireItem> OnUnacquireItem
    {
      add => EventService.Subscribe<ModuleEvents.OnUnacquireItem, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(NwModule.Instance), value);
      remove => EventService.Unsubscribe<ModuleEvents.OnUnacquireItem, GameEventFactory>(this, value);
    }
  }
}
