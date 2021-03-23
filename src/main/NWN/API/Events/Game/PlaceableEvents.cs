using NWN.API.Constants;
using NWN.Core;

// TODO Populate event data.
namespace NWN.API.Events
{
  /// <summary>
  /// Events for Placeables.
  /// </summary>
  public static class PlaceableEvents
  {
    [GameEvent(EventScriptType.PlaceableOnClosed)]
    public sealed class OnClose : IEvent
    {
      public NwPlaceable Placeable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      public NwCreature LastClosedBy { get; } = NWScript.GetLastClosedBy().ToNwObject<NwCreature>();

      NwObject IEvent.Context => Placeable;
    }

    [GameEvent(EventScriptType.PlaceableOnDamaged)]
    public sealed class OnDamaged : IEvent
    {
      public NwPlaceable DamagedObject { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      public NwGameObject Damager { get; } = NWScript.GetLastDamager().ToNwObject<NwGameObject>();

      public int DamageAmount { get; } = NWScript.GetTotalDamageDealt();

      NwObject IEvent.Context => DamagedObject;
    }

    [GameEvent(EventScriptType.PlaceableOnDeath)]
    public sealed class OnDeath : IEvent
    {
      public NwPlaceable KilledObject { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      public NwGameObject Killer { get; } = NWScript.GetLastKiller().ToNwObject<NwGameObject>();

      NwObject IEvent.Context => KilledObject;
    }

    [GameEvent(EventScriptType.PlaceableOnDisarm)]
    public sealed class OnDisarm : IEvent
    {
      public NwPlaceable Placeable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      NwObject IEvent.Context => Placeable;
    }

    [GameEvent(EventScriptType.PlaceableOnHeartbeat)]
    public sealed class OnHeartbeat : IEvent
    {
      public NwPlaceable Placeable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      NwObject IEvent.Context => Placeable;
    }

    [GameEvent(EventScriptType.PlaceableOnInventoryDisturbed)]
    public sealed class OnDisturbed : IEvent
    {
      public InventoryDisturbType DisturbType { get; } = (InventoryDisturbType) NWScript.GetInventoryDisturbType();

      public NwPlaceable Placeable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      public NwCreature Disturber { get; } = NWScript.GetLastDisturbed().ToNwObject<NwCreature>();

      public NwItem DisturbedItem { get; } = NWScript.GetInventoryDisturbItem().ToNwObject<NwItem>();

      NwObject IEvent.Context => Placeable;
    }

    [GameEvent(EventScriptType.PlaceableOnLock)]
    public sealed class OnLock : IEvent
    {
      public NwPlaceable LockedPlaceable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      public int LockDC { get; } = NWScript.GetLockLockDC(NWScript.OBJECT_SELF);

      NwObject IEvent.Context => LockedPlaceable;
    }

    [GameEvent(EventScriptType.PlaceableOnMeleeAttacked)]
    public sealed class OnPhysicalAttacked : IEvent
    {
      public NwPlaceable Placeable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      public NwCreature Attacker { get; } = NWScript.GetLastAttacker().ToNwObject<NwCreature>();

      NwObject IEvent.Context => Placeable;
    }

    [GameEvent(EventScriptType.PlaceableOnOpen)]
    public sealed class OnOpen : IEvent
    {
      public NwPlaceable Placeable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      public NwCreature OpenedBy { get; } = NWScript.GetLastOpenedBy().ToNwObject<NwCreature>();

      NwObject IEvent.Context => Placeable;
    }

    [GameEvent(EventScriptType.PlaceableOnSpellCastAt)]
    public sealed class OnSpellCastAt : IEvent
    {
      /// <summary>
      /// Gets the placeable targeted by this spell.
      /// </summary>
      public NwPlaceable Placeable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      /// <summary>
      /// Gets the caster of this spell (creature, placeable, door). Returns null from an area of effect.
      /// </summary>
      public NwGameObject Caster { get; } = NWScript.GetLastSpellCaster().ToNwObject<NwGameObject>();

      /// <summary>
      /// Gets the spell that was cast.
      /// </summary>
      public Spell Spell { get; } = (Spell)NWScript.GetLastSpell();

      /// <summary>
      /// Gets a value indicating whether this spell is considered harmful.
      /// </summary>
      public bool Harmful { get; } = NWScript.GetLastSpellHarmful().ToBool();

      NwObject IEvent.Context => Placeable;

      public static void Signal(NwObject caster, NwPlaceable target, Spell spell, bool harmful = true)
      {
        Event nwEvent = NWScript.EventSpellCastAt(caster, (int)spell, harmful.ToInt());
        NWScript.SignalEvent(target, nwEvent);
      }
    }

    [GameEvent(EventScriptType.PlaceableOnTrapTriggered)]
    public sealed class OnTrapTriggered : IEvent
    {
      public NwPlaceable Placeable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      NwObject IEvent.Context => Placeable;
    }

    [GameEvent(EventScriptType.PlaceableOnUnlock)]
    public sealed class OnUnlock : IEvent
    {
      public NwPlaceable Placeable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      public NwCreature UnlockedBy { get; } = NWScript.GetLastUnlocked().ToNwObject<NwCreature>();

      NwObject IEvent.Context => Placeable;
    }

    [GameEvent(EventScriptType.PlaceableOnUsed)]
    public sealed class OnUsed : IEvent
    {
      public NwPlaceable Placeable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      public NwCreature UsedBy { get; } = NWScript.GetLastUsedBy().ToNwObject<NwCreature>();

      NwObject IEvent.Context => Placeable;
    }

    [GameEvent(EventScriptType.PlaceableOnUserDefinedEvent)]
    public sealed class OnUserDefined : IEvent
    {
      public NwPlaceable Placeable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      public int EventNumber { get; } = NWScript.GetUserDefinedEventNumber();

      NwObject IEvent.Context => Placeable;

      public static void Signal(NwPlaceable placeable, int eventId)
      {
        Event nwEvent = NWScript.EventUserDefined(eventId);
        NWScript.SignalEvent(placeable, nwEvent);
      }
    }

    [GameEvent(EventScriptType.PlaceableOnDialogue)]
    public sealed class OnDialogue : IEvent
    {
      public NwPlaceable Placeable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      NwObject IEvent.Context => Placeable;
    }

    [GameEvent(EventScriptType.PlaceableOnLeftClick)]
    public sealed class OnLeftClick : IEvent
    {
      public NwPlaceable Placeable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      NwObject IEvent.Context => Placeable;
    }
  }
}
