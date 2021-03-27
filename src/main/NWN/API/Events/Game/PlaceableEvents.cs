using NWN.API.Constants;
using NWN.Core;

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
      /// <summary>
      /// Gets the NwPlaceable that was closed.
      /// </summary>
      public NwPlaceable Placeable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      /// <summary>
      /// Gets the NwCreature that closed the NwPlaceable.
      /// </summary>
      public NwCreature ClosedBy { get; } = NWScript.GetLastClosedBy().ToNwObject<NwCreature>();

      NwObject IEvent.Context => Placeable;
    }

    [GameEvent(EventScriptType.PlaceableOnDamaged)]
    public sealed class OnDamaged : IEvent
    {
      /// <summary>
      /// Gets the NwPlaceable that was damaged.
      /// </summary>
      public NwPlaceable DamagedObject { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      /// <summary>
      /// Gets the NwCreature that damaged the NwPlaceable.
      /// </summary>
      public NwGameObject Damager { get; } = NWScript.GetLastDamager().ToNwObject<NwGameObject>();

      /// <summary>
      /// Gets the total damage dealt to the NwDoor.
      /// </summary>
      public int TotalDamageDealt { get; } = NWScript.GetTotalDamageDealt();

      /// <summary>
      /// Gets damage damage dealt to NwPlaceable, by DamageType.
      /// </summary>
      public int GetDamageDealtByType(DamageType damageType)
        => NWScript.GetDamageDealtByType((int) damageType);

      NwObject IEvent.Context => DamagedObject;
    }

    [GameEvent(EventScriptType.PlaceableOnDeath)]
    public sealed class OnDeath : IEvent
    {
      /// <summary>
      /// Gets the NwPlaceable that was destroyed.
      /// </summary>
      public NwPlaceable KilledObject { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      /// <summary>
      /// Gets the NwCreature that destroyed the NwPlaceable.
      /// </summary>
      public NwCreature Killer { get; } = NWScript.GetLastKiller().ToNwObject<NwCreature>();

      NwObject IEvent.Context => KilledObject;
    }

    [GameEvent(EventScriptType.PlaceableOnDisarm)]
    public sealed class OnDisarm : IEvent
    {
      /// <summary>
      /// Gets the NwPlaceable that was disarmed.
      /// </summary>
      public NwPlaceable Placeable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      NwObject IEvent.Context => Placeable;
    }

    [GameEvent(EventScriptType.PlaceableOnHeartbeat)]
    public sealed class OnHeartbeat : IEvent
    {
      /// <summary>
      /// Gets the NwDoor that had a heartbeat.
      /// </summary>
      public NwPlaceable Placeable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      NwObject IEvent.Context => Placeable;
    }

    [GameEvent(EventScriptType.PlaceableOnInventoryDisturbed)]
    public sealed class OnDisturbed : IEvent
    {
      /// <summary>
      /// Gets the inventory disturb type.
      /// </summary>
      public InventoryDisturbType DisturbType { get; } = (InventoryDisturbType) NWScript.GetInventoryDisturbType();

      /// <summary>
      /// Gets the NwPlaceable that was disturbed.
      /// </summary>
      public NwPlaceable Placeable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      /// <summary>
      /// Gets the NwCreature that disturbed NwPlaceable.
      /// </summary>
      public NwCreature Disturber { get; } = NWScript.GetLastDisturbed().ToNwObject<NwCreature>();

      /// <summary>
      /// Gets the NwItem that triggered the disturb event on NwPlaceable.
      /// </summary>
      public NwItem DisturbedItem { get; } = NWScript.GetInventoryDisturbItem().ToNwObject<NwItem>();

      NwObject IEvent.Context => Placeable;
    }

    [GameEvent(EventScriptType.PlaceableOnLock)]
    public sealed class OnLock : IEvent
    {
      /// <summary>
      /// Gets the NwPlaceable that was locked.
      /// </summary>
      public NwPlaceable LockedPlaceable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      /// <summary>
      /// Gets the LockDC on NwPlaceable.
      /// </summary>
      public int LockDC { get; } = NWScript.GetLockLockDC(NWScript.OBJECT_SELF);

      NwObject IEvent.Context => LockedPlaceable;
    }

    [GameEvent(EventScriptType.PlaceableOnMeleeAttacked)]
    public sealed class OnPhysicalAttacked : IEvent
    {
      /// <summary>
      /// Gets the NwPlaceable that was physically attacked.
      /// </summary>
      public NwPlaceable Placeable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      /// <summary>
      /// Gets the NwCreature that attacked the NwPlaceable.
      /// </summary>
      public NwCreature Attacker { get; } = NWScript.GetLastAttacker().ToNwObject<NwCreature>();

      /// <summary>
      /// Gets the NwItem used to damage NwPlaceable.
      /// </summary>
      public NwItem WeaponUsed(NwCreature attacker) => NWScript.GetLastWeaponUsed(attacker).ToNwObject<NwItem>();

      /// <summary>
      /// Gets the AttackType used to damage NwPlaceable.
      /// </summary>
      public SpecialAttack AttackType { get; } = (SpecialAttack) NWScript.GetLastAttackType();

      /// <summary>
      /// Gets the AttackMode used to damage NwPlaceable.
      /// </summary>
      public ActionMode AttackMode(NwCreature attacker) => (ActionMode) NWScript.GetLastAttackMode(attacker);

      NwObject IEvent.Context => Placeable;
    }

    [GameEvent(EventScriptType.PlaceableOnOpen)]
    public sealed class OnOpen : IEvent
    {
      /// <summary>
      /// Gets the NwPlaceable that was opened.
      /// </summary>
      public NwPlaceable Placeable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      /// <summary>
      /// Gets the NwCreature that opened the NwPlaceable.
      /// </summary>
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
      /// <summary>
      /// Gets the NwPlaceable that had a trap triggered.
      /// </summary>
      public NwPlaceable Placeable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      NwObject IEvent.Context => Placeable;
    }

    [GameEvent(EventScriptType.PlaceableOnUnlock)]
    public sealed class OnUnlock : IEvent
    {
      /// <summary>
      /// Gets the NwPlaceable that was unlocked.
      /// </summary>
      public NwPlaceable Placeable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      /// <summary>
      /// Gets the NwCreature that unlocked NwPlaceable.
      /// </summary>
      public NwCreature UnlockedBy { get; } = NWScript.GetLastUnlocked().ToNwObject<NwCreature>();

      NwObject IEvent.Context => Placeable;
    }

    [GameEvent(EventScriptType.PlaceableOnUsed)]
    public sealed class OnUsed : IEvent
    {
      /// <summary>
      /// Gets the NwPlaceable that was used.
      /// </summary>
      public NwPlaceable Placeable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      /// <summary>
      /// Gets the NwCreature that used NwPlaceable.
      /// </summary>
      public NwCreature UsedBy { get; } = NWScript.GetLastUsedBy().ToNwObject<NwCreature>();

      NwObject IEvent.Context => Placeable;
    }

    [GameEvent(EventScriptType.PlaceableOnUserDefinedEvent)]
    public sealed class OnUserDefined : IEvent
    {
      /// <summary>
      /// Gets the NwPlaceable that is running a user defined event.
      /// </summary>
      public NwPlaceable Placeable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      /// <summary>
      /// Get the specific event number used to trigger this user-defined event.
      /// </summary>
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
      /// <summary>
      /// Gets the NwPlaceable that is in dialog.
      /// </summary>
      public NwPlaceable Placeable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      NwObject IEvent.Context => Placeable;
    }

    [GameEvent(EventScriptType.PlaceableOnLeftClick)]
    public sealed class OnLeftClick : IEvent
    {
      /// <summary>
      /// Gets the NwPlaceable that was left clicked.
      /// </summary>
      public NwPlaceable Placeable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      NwObject IEvent.Context => Placeable;
    }
  }
}
