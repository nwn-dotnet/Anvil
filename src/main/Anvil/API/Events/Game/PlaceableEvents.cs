using System;
using Anvil.API;
using NWN.API.Constants;
using NWN.API.Events;
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

      NwObject IEvent.Context
      {
        get => Placeable;
      }
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
      {
        return NWScript.GetDamageDealtByType((int)damageType);
      }

      NwObject IEvent.Context
      {
        get => DamagedObject;
      }
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

      NwObject IEvent.Context
      {
        get => KilledObject;
      }
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

      NwObject IEvent.Context
      {
        get => Placeable;
      }
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

      NwObject IEvent.Context
      {
        get => Placeable;
      }
    }

    /// <summary>
    /// Called when <see cref="NwPlaceable"/> inventory has been disturbed.
    /// </summary>
    [GameEvent(EventScriptType.PlaceableOnInventoryDisturbed)]
    public sealed class OnDisturbed : IEvent
    {
      /// <summary>
      /// Gets the <see cref="InventoryDisturbType"/>.
      /// </summary>
      public InventoryDisturbType DisturbType { get; } = (InventoryDisturbType)NWScript.GetInventoryDisturbType();

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

      NwObject IEvent.Context
      {
        get => Placeable;
      }
    }

    /// <summary>
    /// Called when <see cref="NwPlaceable"/> has been locked.
    /// </summary>
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

      NwObject IEvent.Context
      {
        get => LockedPlaceable;
      }
    }

    /// <summary>
    /// Called when <see cref="NwPlaceable"/> has been physically attacked.
    /// </summary>
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
      public NwItem WeaponUsed(NwCreature attacker)
      {
        return NWScript.GetLastWeaponUsed(attacker).ToNwObject<NwItem>();
      }

      /// <summary>
      /// Gets the <see cref="SpecialAttack"/> used to damage <see cref="NwPlaceable"/>.
      /// </summary>
      public SpecialAttack AttackType { get; } = (SpecialAttack)NWScript.GetLastAttackType();

      /// <summary>
      /// Gets the <see cref="ActionMode"/> used to damage <see cref="NwPlaceable"/>.
      /// </summary>
      public ActionMode AttackMode(NwCreature attacker)
      {
        return (ActionMode)NWScript.GetLastAttackMode(attacker);
      }

      NwObject IEvent.Context
      {
        get => Placeable;
      }
    }

    /// <summary>
    /// Called when <see cref="NwPlaceable"/> has been opened.
    /// </summary>
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

      NwObject IEvent.Context
      {
        get => Placeable;
      }
    }

    /// <summary>
    /// Called when <see cref="Spell"/> has been casted on <see cref="NwPlaceable"/>.
    /// </summary>
    [GameEvent(EventScriptType.PlaceableOnSpellCastAt)]
    public sealed class OnSpellCastAt : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwPlaceable"/> targeted by this spell.
      /// </summary>
      public NwPlaceable Placeable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      /// <summary>
      /// Gets the <see cref="NwGameObject"/> who cast <see cref="Spell"/> (<see cref="NwCreature"/>, <see cref="NwPlaceable"/>, <see cref="NwDoor"/>). Returns null from an <see cref="NwAreaOfEffect"/>.
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
        get => Placeable;
      }

      public static void Signal(NwObject caster, NwPlaceable target, Spell spell, bool harmful = true)
      {
        Event nwEvent = NWScript.EventSpellCastAt(caster, (int)spell, harmful.ToInt());
        NWScript.SignalEvent(target, nwEvent);
      }
    }

    /// <summary>
    /// Called when <see cref="NwPlaceable"/> has a trap triggered.
    /// </summary>
    [GameEvent(EventScriptType.PlaceableOnTrapTriggered)]
    public sealed class OnTrapTriggered : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwPlaceable"/> that had a trap triggered.
      /// </summary>
      public NwPlaceable Placeable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      NwObject IEvent.Context
      {
        get => Placeable;
      }
    }

    /// <summary>
    /// Called when <see cref="NwPlaceable"/> has been unlocked.
    /// </summary>
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

      NwObject IEvent.Context
      {
        get => Placeable;
      }
    }

    /// <summary>
    /// Called when <see cref="NwPlaceable"/> is being used.
    /// </summary>
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

      NwObject IEvent.Context
      {
        get => Placeable;
      }
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

      NwObject IEvent.Context
      {
        get => Placeable;
      }

      public static void Signal(NwPlaceable placeable, int eventId)
      {
        Event nwEvent = NWScript.EventUserDefined(eventId);
        NWScript.SignalEvent(placeable, nwEvent);
      }
    }

    /// <summary>
    /// Called when <see cref="NwPlaceable"/> starts a conversation.
    /// </summary>
    [GameEvent(EventScriptType.PlaceableOnDialogue)]
    public sealed class OnDialogue : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwPlaceable"/> that is in dialog.
      /// </summary>
      public NwPlaceable Placeable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      NwObject IEvent.Context
      {
        get => Placeable;
      }
    }

    /// <summary>
    /// Called when <see cref="NwPlaceable"/> has been mousepad (left) clicked.
    /// </summary>
    [GameEvent(EventScriptType.PlaceableOnLeftClick)]
    public sealed class OnLeftClick : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwPlaceable"/> that was left clicked.
      /// </summary>
      public NwPlaceable Placeable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      /// <summary>
      /// Gets the <see cref="NwPlayer"/> that clicked on the <see cref="NwPlaceable"/>.
      /// </summary>
      public NwPlayer ClickedBy { get; } = NWScript.GetPlaceableLastClickedBy().ToNwPlayer();

      NwObject IEvent.Context
      {
        get => Placeable;
      }
    }
  }
}

namespace NWN.API
{
  public sealed partial class NwPlaceable
  {
    /// <inheritdoc cref="NWN.API.Events.PlaceableEvents.OnClose"/>
    public event Action<PlaceableEvents.OnClose> OnClose
    {
      add => EventService.Subscribe<PlaceableEvents.OnClose, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<PlaceableEvents.OnClose, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.PlaceableEvents.OnDamaged"/>
    public event Action<PlaceableEvents.OnDamaged> OnDamaged
    {
      add => EventService.Subscribe<PlaceableEvents.OnDamaged, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<PlaceableEvents.OnDamaged, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.PlaceableEvents.OnDeath"/>
    public event Action<PlaceableEvents.OnDeath> OnDeath
    {
      add => EventService.Subscribe<PlaceableEvents.OnDeath, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<PlaceableEvents.OnDeath, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.PlaceableEvents.OnDisarm"/>
    public event Action<PlaceableEvents.OnDisarm> OnDisarm
    {
      add => EventService.Subscribe<PlaceableEvents.OnDisarm, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<PlaceableEvents.OnDisarm, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.PlaceableEvents.OnHeartbeat"/>
    public event Action<PlaceableEvents.OnHeartbeat> OnHeartbeat
    {
      add => EventService.Subscribe<PlaceableEvents.OnHeartbeat, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<PlaceableEvents.OnHeartbeat, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.PlaceableEvents.OnDisturbed"/>
    public event Action<PlaceableEvents.OnDisturbed> OnDisturbed
    {
      add => EventService.Subscribe<PlaceableEvents.OnDisturbed, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<PlaceableEvents.OnDisturbed, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.PlaceableEvents.OnLock"/>
    public event Action<PlaceableEvents.OnLock> OnLock
    {
      add => EventService.Subscribe<PlaceableEvents.OnLock, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<PlaceableEvents.OnLock, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.PlaceableEvents.OnPhysicalAttacked"/>
    public event Action<PlaceableEvents.OnPhysicalAttacked> OnPhysicalAttacked
    {
      add => EventService.Subscribe<PlaceableEvents.OnPhysicalAttacked, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<PlaceableEvents.OnPhysicalAttacked, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.PlaceableEvents.OnOpen"/>
    public event Action<PlaceableEvents.OnOpen> OnOpen
    {
      add => EventService.Subscribe<PlaceableEvents.OnOpen, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<PlaceableEvents.OnOpen, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.PlaceableEvents.OnSpellCastAt"/>
    public event Action<PlaceableEvents.OnSpellCastAt> OnSpellCastAt
    {
      add => EventService.Subscribe<PlaceableEvents.OnSpellCastAt, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<PlaceableEvents.OnSpellCastAt, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.PlaceableEvents.OnTrapTriggered"/>
    public event Action<PlaceableEvents.OnTrapTriggered> OnTrapTriggered
    {
      add => EventService.Subscribe<PlaceableEvents.OnTrapTriggered, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<PlaceableEvents.OnTrapTriggered, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.PlaceableEvents.OnUnlock"/>
    public event Action<PlaceableEvents.OnUnlock> OnUnlock
    {
      add => EventService.Subscribe<PlaceableEvents.OnUnlock, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<PlaceableEvents.OnUnlock, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.PlaceableEvents.OnUsed"/>
    public event Action<PlaceableEvents.OnUsed> OnUsed
    {
      add => EventService.Subscribe<PlaceableEvents.OnUsed, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<PlaceableEvents.OnUsed, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.PlaceableEvents.OnUserDefined"/>
    public event Action<PlaceableEvents.OnUserDefined> OnUserDefined
    {
      add => EventService.Subscribe<PlaceableEvents.OnUserDefined, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<PlaceableEvents.OnUserDefined, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.PlaceableEvents.OnDialogue"/>
    public event Action<PlaceableEvents.OnDialogue> OnDialogue
    {
      add => EventService.Subscribe<PlaceableEvents.OnDialogue, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<PlaceableEvents.OnDialogue, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.PlaceableEvents.OnLeftClick"/>
    public event Action<PlaceableEvents.OnLeftClick> OnLeftClick
    {
      add => EventService.Subscribe<PlaceableEvents.OnLeftClick, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<PlaceableEvents.OnLeftClick, GameEventFactory>(this, value);
    }
  }
}
