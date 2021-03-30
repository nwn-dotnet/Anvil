using NWN.API.Constants;
using NWN.Core;

namespace NWN.API.Events
{
  /// <summary>
  /// Events for door objects.
  /// </summary>
  public static class DoorEvents
  {
    [GameEvent(EventScriptType.DoorOnOpen)]
    public sealed class OnOpen : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwDoor"/> that was opened.
      /// </summary>
      public NwDoor Door { get; } = NWScript.OBJECT_SELF.ToNwObject<NwDoor>();

      /// <summary>
      /// Gets the <see cref="NwCreature"/> that opened the <see cref="NwDoor"/>.
      /// </summary>
      public NwCreature OpenedBy { get; } = NWScript.GetLastOpenedBy().ToNwObject<NwCreature>();

      NwObject IEvent.Context => Door;
    }

    [GameEvent(EventScriptType.DoorOnClose)]
    public sealed class OnClose : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwDoor"/> that was closed.
      /// </summary>
      public NwDoor Door { get; } = NWScript.OBJECT_SELF.ToNwObject<NwDoor>();

      /// <summary>
      /// Gets the <see cref="NwCreature"/> that closed the <see cref="NwDoor"/>.
      /// </summary>
      public NwCreature ClosedBy { get; } = NWScript.GetLastClosedBy().ToNwObject<NwCreature>();

      NwObject IEvent.Context => Door;
    }

    [GameEvent(EventScriptType.DoorOnDamage)]
    public sealed class OnDamaged : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwDoor"/> that was damaged.
      /// </summary>
      public NwDoor Door { get; } = NWScript.OBJECT_SELF.ToNwObject<NwDoor>();

      /// <summary>
      /// Gets the <see cref="NwCreature"/> that damaged the <see cref="NwDoor"/>.
      /// </summary>
      public NwCreature DamagedBy { get; } = NWScript.GetLastDamager().ToNwObject<NwCreature>();

      /// <summary>
      /// Gets the total damage dealt to the <see cref="NwDoor"/>.
      /// </summary>
      public int TotalDamageDealt { get; } = NWScript.GetTotalDamageDealt();

      /// <summary>
      /// Gets damage dealt to <see cref="NwDoor"/>, by <see cref="DamageType"/>.
      /// </summary>
      public int GetDamageDealtByType(DamageType damageType)
        => NWScript.GetDamageDealtByType((int) damageType);

      NwObject IEvent.Context => Door;
    }

    [GameEvent(EventScriptType.DoorOnDeath)]
    public sealed class OnDeath : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwDoor"/> that was destroy.
      /// </summary>
      public NwDoor Door { get; } = NWScript.OBJECT_SELF.ToNwObject<NwDoor>();

      /// <summary>
      /// Gets the <see cref="NwCreature"/> that killed the <see cref="NwDoor"/>.
      /// </summary>
      public NwCreature Killer { get; } = NWScript.GetLastKiller().ToNwObject<NwCreature>();

      NwObject IEvent.Context => Door;
    }

    [GameEvent(EventScriptType.DoorOnDisarm)]
    public sealed class OnDisarm : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwDoor"/> that was disarmed.
      /// </summary>
      public NwDoor Door { get; } = NWScript.OBJECT_SELF.ToNwObject<NwDoor>();

      NwObject IEvent.Context => Door;
    }

    [GameEvent(EventScriptType.DoorOnHeartbeat)]
    public sealed class OnHeartbeat : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwDoor"/> that had a heartbeat.
      /// </summary>
      public NwDoor Door { get; } = NWScript.OBJECT_SELF.ToNwObject<NwDoor>();

      NwObject IEvent.Context => Door;
    }

    [GameEvent(EventScriptType.DoorOnLock)]
    public sealed class OnLock : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwDoor"/> that was locked.
      /// </summary>
      public NwDoor Door { get; } = NWScript.OBJECT_SELF.ToNwObject<NwDoor>();

      /// <summary>
      /// Gets the <see cref="NwCreature"/> that locked this <see cref="NwDoor"/>.
      /// </summary>
      public NwCreature LockedBy { get; } = NWScript.GetLastLocked().ToNwObject<NwCreature>();

      NwObject IEvent.Context => Door;
    }

    [GameEvent(EventScriptType.DoorOnMeleeAttacked)]
    public sealed class OnPhysicalAttacked : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwDoor"/> that was physically attacked.
      /// </summary>
      public NwDoor Door { get; } = NWScript.OBJECT_SELF.ToNwObject<NwDoor>();

      /// <summary>
      /// Gets the <see cref="NwCreature"/> that attacked the <see cref="NwDoor"/>.
      /// </summary>
      public NwCreature Attacker { get; } = NWScript.GetLastAttacker().ToNwObject<NwCreature>();

      /// <summary>
      /// Gets the <see cref="NwItem"/> used to damage <see cref="NwDoor"/>.
      /// </summary>
      public NwItem WeaponUsed(NwCreature attacker) => NWScript.GetLastWeaponUsed(attacker).ToNwObject<NwItem>();

      /// <summary>
      /// Gets the <see cref="SpecialAttack"/> used to damage <see cref="NwDoor"/>.
      /// </summary>
      public SpecialAttack AttackType { get; } = (SpecialAttack) NWScript.GetLastAttackType();

      /// <summary>
      /// Gets the <see cref="ActionMode"/> used to damage <see cref="NwDoor"/>.
      /// </summary>
      public ActionMode AttackMode(NwCreature attacker) => (ActionMode) NWScript.GetLastAttackMode(attacker);

      NwObject IEvent.Context => Door;
    }

    [GameEvent(EventScriptType.DoorOnSpellCastAt)]
    public sealed class OnSpellCastAt : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwDoor"/> targeted by this <see cref="Spell"/>.
      /// </summary>
      public NwDoor Door { get; } = NWScript.OBJECT_SELF.ToNwObject<NwDoor>();

      /// <summary>
      /// Gets the caster of this <see cref="Spell"/> (<see cref="NwCreature"/>, <see cref="NwPlaceable"/>, <see cref="NwDoor"/>). Returns null from an <see cref="NwAreaOfEffect"/>.
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

      NwObject IEvent.Context => Door;

      public static void Signal(NwObject caster, NwDoor target, Spell spell, bool harmful = true)
      {
        Event nwEvent = NWScript.EventSpellCastAt(caster, (int)spell, harmful.ToInt());
        NWScript.SignalEvent(target, nwEvent);
      }
    }

    [GameEvent(EventScriptType.DoorOnTrapTriggered)]
    public sealed class OnTrapTriggered : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwDoor"/> that has had a trap triggered.
      /// </summary>
      public NwDoor Door { get; } = NWScript.OBJECT_SELF.ToNwObject<NwDoor>();

      NwObject IEvent.Context => Door;
    }

    [GameEvent(EventScriptType.DoorOnUnlock)]
    public sealed class OnUnlock : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwDoor"/> that was unlocked.
      /// </summary>
      public NwDoor Door { get; } = NWScript.OBJECT_SELF.ToNwObject<NwDoor>();

      /// <summary>
      /// Gets the <see cref="NwCreature"/> that unlocked <see cref="NwDoor"/>.
      /// </summary>
      public NwCreature UnlockedBy { get; } = NWScript.GetLastUnlocked().ToNwObject<NwCreature>();

      NwObject IEvent.Context => Door;
    }

    [GameEvent(EventScriptType.DoorOnUserDefined)]
    public sealed class OnUserDefined : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwDoor"/> that is running a user defined event.
      /// </summary>
      public NwDoor Door { get; } = NWScript.OBJECT_SELF.ToNwObject<NwDoor>();

      /// <summary>
      /// Gets the specific event number used to trigger this user-defined event.
      /// </summary>
      public int EventNumber { get; } = NWScript.GetUserDefinedEventNumber();

      NwObject IEvent.Context => Door;

      public static void Signal(NwDoor door, int eventId)
      {
        Event nwEvent = NWScript.EventUserDefined(eventId);
        NWScript.SignalEvent(door, nwEvent);
      }
    }

    [GameEvent(EventScriptType.DoorOnClicked)]
    public sealed class OnAreaTransitionClick : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwDoor"/> that has the transition.
      /// </summary>
      public NwDoor Door { get; } = NWScript.OBJECT_SELF.ToNwObject<NwDoor>();

      /// <summary>
      /// Gets the <see cref="NwCreature"/> that clicked the transition.
      /// </summary>
      public NwPlayer ClickedBy { get; } = NWScript.GetClickingObject().ToNwObject<NwPlayer>();

      /// <summary>
      /// Gets the transition target for this <see cref="NwDoor"/>.
      /// </summary>
      public NwStationary TransitionTarget { get; } = NWScript.GetTransitionTarget(NWScript.OBJECT_SELF).ToNwObject<NwStationary>();

      NwObject IEvent.Context => Door;
    }

    [GameEvent(EventScriptType.DoorOnDialogue)]
    public sealed class OnDialogue : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwDoor"/> that is in dialog.
      /// </summary>
      public NwDoor Door { get; } = NWScript.OBJECT_SELF.ToNwObject<NwDoor>();

      NwObject IEvent.Context => Door;
    }

    [GameEvent(EventScriptType.DoorOnFailToOpen)]
    public sealed class OnFailToOpen : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwDoor"/> that failed to open.
      /// </summary>
      public NwDoor Door { get; } = NWScript.OBJECT_SELF.ToNwObject<NwDoor>();

      /// <summary>
      /// Gets the <see cref="NwCreature"/> that failed to unlock this <see cref="NwDoor"/>.
      /// </summary>
      public NwCreature WhoFailed { get; } = NWScript.GetClickingObject().ToNwObject<NwCreature>();

      NwObject IEvent.Context => Door;
    }
  }
}
