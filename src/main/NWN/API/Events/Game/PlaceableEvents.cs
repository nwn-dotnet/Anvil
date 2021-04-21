using NWN.API.Constants;
using NWN.Core;

namespace NWN.API.Events
{
  /// <summary>
  /// Events for <see cref="NwPlaceable"/>.
  /// </summary>
  public static class PlaceableEvents
  {
    /// <summary>
    /// Called when <see cref="NwCreature"/> has closed a <see cref="NwPlaceable"/>.
    /// </summary>
    [GameEvent(EventScriptType.PlaceableOnClosed)]
    public sealed class OnClose : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwPlaceable"/> that was closed.
      /// </summary>
      public NwPlaceable Placeable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      /// <summary>
      /// Gets the <see cref="NwCreature"/> that closed the <see cref="NwPlaceable"/>.
      /// </summary>
      public NwCreature ClosedBy { get; } = NWScript.GetLastClosedBy().ToNwObject<NwCreature>();

      NwObject IEvent.Context => Placeable;
    }

    /// <summary>
    /// Called when <see cref="NwGameObject"/> has damaged <see cref="NwPlaceable"/>.
    /// </summary>
    [GameEvent(EventScriptType.PlaceableOnDamaged)]
    public sealed class OnDamaged : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwPlaceable"/> that was damaged.
      /// </summary>
      public NwPlaceable DamagedObject { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      /// <summary>
      /// Gets the <see cref="NwGameObject"/> that damaged the <see cref="NwPlaceable"/>.
      /// </summary>
      public NwGameObject Damager { get; } = NWScript.GetLastDamager().ToNwObject<NwGameObject>();

      /// <summary>
      /// Gets the total damage dealt to <see cref="NwPlaceable"/>.
      /// </summary>
      public int TotalDamageDealt { get; } = NWScript.GetTotalDamageDealt();

      /// <summary>
      /// Gets <see cref="DamageType"/> dealt to <see cref="NwPlaceable"/>.
      /// </summary>
      public int GetDamageDealtByType(DamageType damageType)
        => NWScript.GetDamageDealtByType((int) damageType);

      NwObject IEvent.Context => DamagedObject;
    }

    /// <summary>
    /// Called when <see cref="NwCreature"/> has destroyed <see cref="NwPlaceable"/>.
    /// </summary>
    [GameEvent(EventScriptType.PlaceableOnDeath)]
    public sealed class OnDeath : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwPlaceable"/> that was destroyed.
      /// </summary>
      public NwPlaceable KilledObject { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      /// <summary>
      /// Gets the <see cref="NwCreature"/> that destroyed the <see cref="NwPlaceable"/>.
      /// </summary>
      public NwCreature Killer { get; } = NWScript.GetLastKiller().ToNwObject<NwCreature>();

      NwObject IEvent.Context => KilledObject;
    }

    /// <summary>
    /// Called when <see cref="NwPlaceable"/> has been disarmed.
    /// </summary>
    [GameEvent(EventScriptType.PlaceableOnDisarm)]
    public sealed class OnDisarm : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwPlaceable"/> that was disarmed.
      /// </summary>
      public NwPlaceable Placeable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      NwObject IEvent.Context => Placeable;
    }

    /// <summary>
    /// Called every 6 seconds on <see cref="NwPlaceable"/>.
    /// </summary>
    [GameEvent(EventScriptType.PlaceableOnHeartbeat)]
    public sealed class OnHeartbeat : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwPlaceable"/> heartbeat.
      /// </summary>
      public NwPlaceable Placeable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      NwObject IEvent.Context => Placeable;
    }

    [GameEvent(EventScriptType.PlaceableOnInventoryDisturbed)]
    public sealed class OnDisturbed : IEvent
    {
      /// <summary>
      /// Gets the <see cref="InventoryDisturbType"/>.
      /// </summary>
      public InventoryDisturbType DisturbType { get; } = (InventoryDisturbType) NWScript.GetInventoryDisturbType();

      /// <summary>
      /// Gets the <see cref="NwPlaceable"/> that was disturbed.
      /// </summary>
      public NwPlaceable Placeable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      /// <summary>
      /// Gets the <see cref="NwCreature"/> that disturbed <see cref="NwPlaceable"/>.
      /// </summary>
      public NwCreature Disturber { get; } = NWScript.GetLastDisturbed().ToNwObject<NwCreature>();

      /// <summary>
      /// Gets the <see cref="NwItem"/> that triggered the disturb event on <see cref="NwPlaceable"/>.
      /// </summary>
      public NwItem DisturbedItem { get; } = NWScript.GetInventoryDisturbItem().ToNwObject<NwItem>();

      NwObject IEvent.Context => Placeable;
    }

    [GameEvent(EventScriptType.PlaceableOnLock)]
    public sealed class OnLock : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwPlaceable"/> that was locked.
      /// </summary>
      public NwPlaceable LockedPlaceable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      /// <summary>
      /// Gets the <see cref="NwCreature"/> that locked this <see cref="NwPlaceable"/>.
      /// </summary>
      public NwCreature LockedBy { get; } = NWScript.GetLastLocked().ToNwObject<NwCreature>();

      NwObject IEvent.Context => LockedPlaceable;
    }

    [GameEvent(EventScriptType.PlaceableOnMeleeAttacked)]
    public sealed class OnPhysicalAttacked : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwPlaceable"/> that was physically attacked.
      /// </summary>
      public NwPlaceable Placeable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      /// <summary>
      /// Gets the <see cref="NwCreature"/> that attacked the <see cref="NwPlaceable"/>.
      /// </summary>
      public NwCreature Attacker { get; } = NWScript.GetLastAttacker().ToNwObject<NwCreature>();

      /// <summary>
      /// Gets the <see cref="NwItem"/> used to damage <see cref="NwPlaceable"/>.
      /// </summary>
      public NwItem WeaponUsed(NwCreature attacker) => NWScript.GetLastWeaponUsed(attacker).ToNwObject<NwItem>();

      /// <summary>
      /// Gets the <see cref="SpecialAttack"/> used to damage <see cref="NwPlaceable"/>.
      /// </summary>
      public SpecialAttack AttackType { get; } = (SpecialAttack) NWScript.GetLastAttackType();

      /// <summary>
      /// Gets the <see cref="ActionMode"/> used to damage <see cref="NwPlaceable"/>.
      /// </summary>
      public ActionMode AttackMode(NwCreature attacker) => (ActionMode) NWScript.GetLastAttackMode(attacker);

      NwObject IEvent.Context => Placeable;
    }

    [GameEvent(EventScriptType.PlaceableOnOpen)]
    public sealed class OnOpen : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwPlaceable"/> that was opened.
      /// </summary>
      public NwPlaceable Placeable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      /// <summary>
      /// Gets the <see cref="NwCreature"/> that opened the <see cref="NwPlaceable"/>.
      /// </summary>
      public NwCreature OpenedBy { get; } = NWScript.GetLastOpenedBy().ToNwObject<NwCreature>();

      NwObject IEvent.Context => Placeable;
    }

    [GameEvent(EventScriptType.PlaceableOnSpellCastAt)]
    public sealed class OnSpellCastAt : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwPlaceable"/> targeted by this spell.
      /// </summary>
      public NwPlaceable Placeable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      /// <summary>
      /// Gets the <see cref="NwGameObject"/> who cast this spell (<see cref="NwCreature"/>, <see cref="NwPlaceable"/>, <see cref="NwDoor"/>). Returns null from an <see cref="NwAreaOfEffect"/>.
      /// </summary>
      public NwGameObject Caster { get; } = NWScript.GetLastSpellCaster().ToNwObject<NwGameObject>();

      /// <summary>
      /// Gets the <see cref="Spell"/> that was cast.
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
      /// Gets the <see cref="NwPlaceable"/> that had a trap triggered.
      /// </summary>
      public NwPlaceable Placeable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      NwObject IEvent.Context => Placeable;
    }

    [GameEvent(EventScriptType.PlaceableOnUnlock)]
    public sealed class OnUnlock : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwPlaceable"/> that was unlocked.
      /// </summary>
      public NwPlaceable Placeable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      /// <summary>
      /// Gets the <see cref="NwCreature"/> that unlocked <see cref="NwPlaceable"/>.
      /// </summary>
      public NwCreature UnlockedBy { get; } = NWScript.GetLastUnlocked().ToNwObject<NwCreature>();

      NwObject IEvent.Context => Placeable;
    }

    [GameEvent(EventScriptType.PlaceableOnUsed)]
    public sealed class OnUsed : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwPlaceable"/> that was used.
      /// </summary>
      public NwPlaceable Placeable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      /// <summary>
      /// Gets the <see cref="NwCreature"/> that used <see cref="NwPlaceable"/>.
      /// </summary>
      public NwCreature UsedBy { get; } = NWScript.GetLastUsedBy().ToNwObject<NwCreature>();

      NwObject IEvent.Context => Placeable;
    }

    [GameEvent(EventScriptType.PlaceableOnUserDefinedEvent)]
    public sealed class OnUserDefined : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwPlaceable"/> that is running a user defined event.
      /// </summary>
      public NwPlaceable Placeable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      /// <summary>
      /// Gets the specific event number used to trigger this user-defined event.
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
      /// Gets the <see cref="NwPlaceable"/> that is in dialog.
      /// </summary>
      public NwPlaceable Placeable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      NwObject IEvent.Context => Placeable;
    }

    [GameEvent(EventScriptType.PlaceableOnLeftClick)]
    public sealed class OnLeftClick : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwPlaceable"/> that was left clicked.
      /// </summary>
      public NwPlaceable Placeable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      NwObject IEvent.Context => Placeable;
    }
  }
}
