using System;
using Anvil.API;
using NWN.API.Constants;
using NWN.API.Events;
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
      public NwGameObject OpenedBy { get; } = NWScript.GetLastOpenedBy().ToNwObject<NwGameObject>();

      NwObject IEvent.Context
      {
        get => Door;
      }
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
      public NwGameObject ClosedBy { get; } = NWScript.GetLastClosedBy().ToNwObject<NwGameObject>();

      NwObject IEvent.Context
      {
        get => Door;
      }
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
      {
        return NWScript.GetDamageDealtByType((int)damageType);
      }

      NwObject IEvent.Context
      {
        get => Door;
      }
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

      NwObject IEvent.Context
      {
        get => Door;
      }
    }

    [GameEvent(EventScriptType.DoorOnDisarm)]
    public sealed class OnDisarm : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwDoor"/> that was disarmed.
      /// </summary>
      public NwDoor Door { get; } = NWScript.OBJECT_SELF.ToNwObject<NwDoor>();

      NwObject IEvent.Context
      {
        get => Door;
      }
    }

    [GameEvent(EventScriptType.DoorOnHeartbeat)]
    public sealed class OnHeartbeat : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwDoor"/> that had a heartbeat.
      /// </summary>
      public NwDoor Door { get; } = NWScript.OBJECT_SELF.ToNwObject<NwDoor>();

      NwObject IEvent.Context
      {
        get => Door;
      }
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

      NwObject IEvent.Context
      {
        get => Door;
      }
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
      public NwItem WeaponUsed(NwCreature attacker)
      {
        return NWScript.GetLastWeaponUsed(attacker).ToNwObject<NwItem>();
      }

      /// <summary>
      /// Gets the <see cref="SpecialAttack"/> used to damage <see cref="NwDoor"/>.
      /// </summary>
      public SpecialAttack AttackType { get; } = (SpecialAttack)NWScript.GetLastAttackType();

      /// <summary>
      /// Gets the <see cref="ActionMode"/> used to damage <see cref="NwDoor"/>.
      /// </summary>
      public ActionMode AttackMode(NwCreature attacker)
      {
        return (ActionMode)NWScript.GetLastAttackMode(attacker);
      }

      NwObject IEvent.Context
      {
        get => Door;
      }
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

      NwObject IEvent.Context
      {
        get => Door;
      }

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

      NwObject IEvent.Context
      {
        get => Door;
      }
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

      NwObject IEvent.Context
      {
        get => Door;
      }
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

      NwObject IEvent.Context
      {
        get => Door;
      }

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
      public NwPlayer ClickedBy { get; } = NWScript.GetClickingObject().ToNwPlayer();

      /// <summary>
      /// Gets the transition target for this <see cref="NwDoor"/>.
      /// </summary>
      public NwStationary TransitionTarget { get; } = NWScript.GetTransitionTarget(NWScript.OBJECT_SELF).ToNwObject<NwStationary>();

      NwObject IEvent.Context
      {
        get => Door;
      }

      /// <summary>
      /// Sets the graphic shown when a PC moves between two different areas in a module.
      /// </summary>
      /// <param name="transition">The transition to use.</param>
      public void SetAreaTransitionBMP(AreaTransition transition)
      {
        if (transition == AreaTransition.UserDefined)
        {
          throw new ArgumentOutOfRangeException(nameof(transition), "Use the string overload instead if wanting to use a user defined transition.");
        }

        NWScript.SetAreaTransitionBMP((int)transition);
      }

      /// <summary>
      /// Sets the graphic shown when a PC moves between two different areas in a module.
      /// </summary>
      /// <param name="transition">The file name (.bmp) to use for the area transition bitmap.</param>
      public void SetAreaTransitionBMP(string transition)
      {
        NWScript.SetAreaTransitionBMP((int)AreaTransition.UserDefined, transition);
      }
    }

    [GameEvent(EventScriptType.DoorOnDialogue)]
    public sealed class OnDialogue : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwDoor"/> that is in dialog.
      /// </summary>
      public NwDoor Door { get; } = NWScript.OBJECT_SELF.ToNwObject<NwDoor>();

      NwObject IEvent.Context
      {
        get => Door;
      }
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

      NwObject IEvent.Context
      {
        get => Door;
      }
    }
  }
}

namespace NWN.API
{
  public sealed partial class NwDoor
  {
    /// <inheritdoc cref="NWN.API.Events.DoorEvents.OnOpen"/>
    public event Action<DoorEvents.OnOpen> OnOpen
    {
      add => EventService.Subscribe<DoorEvents.OnOpen, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<DoorEvents.OnOpen, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.DoorEvents.OnClose"/>
    public event Action<DoorEvents.OnClose> OnClose
    {
      add => EventService.Subscribe<DoorEvents.OnClose, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<DoorEvents.OnClose, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.DoorEvents.OnDamaged"/>
    public event Action<DoorEvents.OnDamaged> OnDamaged
    {
      add => EventService.Subscribe<DoorEvents.OnDamaged, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<DoorEvents.OnDamaged, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.DoorEvents.OnDeath"/>
    public event Action<DoorEvents.OnDeath> OnDeath
    {
      add => EventService.Subscribe<DoorEvents.OnDeath, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<DoorEvents.OnDeath, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.DoorEvents.OnDisarm"/>
    public event Action<DoorEvents.OnDisarm> OnDisarm
    {
      add => EventService.Subscribe<DoorEvents.OnDisarm, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<DoorEvents.OnDisarm, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.DoorEvents.OnHeartbeat"/>
    public event Action<DoorEvents.OnHeartbeat> OnHeartbeat
    {
      add => EventService.Subscribe<DoorEvents.OnHeartbeat, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<DoorEvents.OnHeartbeat, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.DoorEvents.OnLock"/>
    public event Action<DoorEvents.OnLock> OnLock
    {
      add => EventService.Subscribe<DoorEvents.OnLock, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<DoorEvents.OnLock, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.DoorEvents.OnPhysicalAttacked"/>
    public event Action<DoorEvents.OnPhysicalAttacked> OnPhysicalAttacked
    {
      add => EventService.Subscribe<DoorEvents.OnPhysicalAttacked, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<DoorEvents.OnPhysicalAttacked, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.DoorEvents.OnSpellCastAt"/>
    public event Action<DoorEvents.OnSpellCastAt> OnSpellCastAt
    {
      add => EventService.Subscribe<DoorEvents.OnSpellCastAt, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<DoorEvents.OnSpellCastAt, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.DoorEvents.OnTrapTriggered"/>
    public event Action<DoorEvents.OnTrapTriggered> OnTrapTriggered
    {
      add => EventService.Subscribe<DoorEvents.OnTrapTriggered, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<DoorEvents.OnTrapTriggered, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.DoorEvents.OnUnlock"/>
    public event Action<DoorEvents.OnUnlock> OnUnlock
    {
      add => EventService.Subscribe<DoorEvents.OnUnlock, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<DoorEvents.OnUnlock, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.DoorEvents.OnUserDefined"/>
    public event Action<DoorEvents.OnUserDefined> OnUserDefined
    {
      add => EventService.Subscribe<DoorEvents.OnUserDefined, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<DoorEvents.OnUserDefined, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.DoorEvents.OnAreaTransitionClick"/>
    public event Action<DoorEvents.OnAreaTransitionClick> OnAreaTransitionClick
    {
      add => EventService.Subscribe<DoorEvents.OnAreaTransitionClick, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<DoorEvents.OnAreaTransitionClick, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.DoorEvents.OnDialogue"/>
    public event Action<DoorEvents.OnDialogue> OnDialogue
    {
      add => EventService.Subscribe<DoorEvents.OnDialogue, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<DoorEvents.OnDialogue, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.DoorEvents.OnFailToOpen"/>
    public event Action<DoorEvents.OnFailToOpen> OnFailToOpen
    {
      add => EventService.Subscribe<DoorEvents.OnFailToOpen, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<DoorEvents.OnFailToOpen, GameEventFactory>(this, value);
    }
  }
}
