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
    [GameEvent(EventScriptType.ModuleOnAcquireItem)]
    public sealed class OnAcquireItem : IEvent
    {
      public NwItem Item { get; } = NWScript.GetModuleItemAcquired().ToNwObject<NwItem>();

      public NwGameObject AcquiredBy { get; } = NWScript.GetModuleItemAcquiredBy().ToNwObject<NwGameObject>();

      public NwGameObject AcquiredFrom { get; } = NWScript.GetModuleItemAcquiredFrom().ToNwObject<NwGameObject>();

      NwObject IEvent.Context => AcquiredBy;
    }

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

    [GameEvent(EventScriptType.ModuleOnClientEnter)]
    public sealed class OnClientEnter : IEvent
    {
      public NwPlayer Player { get; } = NWScript.GetEnteringObject().ToNwObject<NwPlayer>();

      NwObject IEvent.Context => Player;
    }

    [GameEvent(EventScriptType.ModuleOnClientExit)]
    public sealed class OnClientLeave : IEvent
    {
      public NwPlayer Player { get; } = NWScript.GetExitingObject().ToNwObject<NwPlayer>();

      NwObject IEvent.Context => Player;
    }

    [GameEvent(EventScriptType.ModuleOnPlayerCancelCutscene)]
    public sealed class OnCutsceneAbort : IEvent
    {
      public NwPlayer Player { get; } = NWScript.GetLastPCToCancelCutscene().ToNwObject<NwPlayer>();

      NwObject IEvent.Context => Player;
    }

    [GameEvent(EventScriptType.ModuleOnHeartbeat)]
    public sealed class OnHeartbeat : IEvent
    {
      NwObject IEvent.Context => null;
    }

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

    [GameEvent(EventScriptType.ModuleOnPlayerChat)]
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

    [GameEvent(EventScriptType.ModuleOnPlayerDeath)]
    public sealed class OnPlayerDeath : IEvent
    {
      public NwPlayer DeadPlayer { get; } = NWScript.GetLastPlayerDied().ToNwObject<NwPlayer>();

      public NwGameObject Killer { get; } = NWScript.GetLastHostileActor().ToNwObject<NwGameObject>();

      NwObject IEvent.Context => DeadPlayer;
    }

    [GameEvent(EventScriptType.ModuleOnPlayerDying)]
    public sealed class OnPlayerDying : IEvent
    {
      public NwPlayer Player { get; } = NWScript.GetLastPlayerDying().ToNwObject<NwPlayer>();

      NwObject IEvent.Context => Player;
    }

    [GameEvent(EventScriptType.ModuleOnEquipItem)]
    public sealed class OnPlayerEquipItem : IEvent
    {
      public NwCreature Player { get; } = NWScript.GetPCItemLastEquippedBy().ToNwObject<NwCreature>();

      public NwItem Item { get; } = NWScript.GetPCItemLastEquipped().ToNwObject<NwItem>();

      NwObject IEvent.Context => Player;
    }

    [GameEvent(EventScriptType.ModuleOnPlayerLevelUp)]
    public sealed class OnPlayerLevelUp : IEvent
    {
      public NwPlayer Player { get; } = NWScript.GetPCLevellingUp().ToNwObject<NwPlayer>();

      NwObject IEvent.Context => Player;
    }

    [GameEvent(EventScriptType.ModuleOnRespawnButtonPressed)]
    public sealed class OnPlayerRespawn : IEvent
    {
      public NwPlayer Player { get; } = NWScript.GetLastRespawnButtonPresser().ToNwObject<NwPlayer>();

      NwObject IEvent.Context => Player;
    }

    [GameEvent(EventScriptType.ModuleOnPlayerRest)]
    public sealed class OnPlayerRest : IEvent
    {
      public NwPlayer Player { get; } = NWScript.GetLastPCRested().ToNwObject<NwPlayer>();

      public RestEventType RestEventType { get; } = (RestEventType) NWScript.GetLastRestEventType();

      NwObject IEvent.Context => Player;
    }

    [GameEvent(EventScriptType.ModuleOnUnequipItem)]
    public sealed class OnPlayerUnequipItem : IEvent
    {
      public NwCreature UnequippedBy { get; } = NWScript.GetPCItemLastUnequippedBy().ToNwObject<NwCreature>();

      public NwItem Item { get; } = NWScript.GetPCItemLastUnequipped().ToNwObject<NwItem>();

      NwObject IEvent.Context => UnequippedBy;
    }

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
    }
  }
}
