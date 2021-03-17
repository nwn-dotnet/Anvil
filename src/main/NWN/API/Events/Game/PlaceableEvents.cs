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
    [NativeEvent(EventScriptType.PlaceableOnClosed)]
    public sealed class OnClose : IEvent
    {
      public NwPlaceable Placeable { get; }

      public NwCreature LastClosedBy { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Placeable;

      public OnClose()
      {
        Placeable = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();
        LastClosedBy = NWScript.GetLastClosedBy().ToNwObject<NwCreature>();
      }
    }

    [NativeEvent(EventScriptType.PlaceableOnDamaged)]
    public sealed class OnDamaged : IEvent
    {
      public NwPlaceable DamagedObject { get; }

      public NwGameObject Damager { get; }

      public int DamageAmount { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => DamagedObject;

      public OnDamaged()
      {
        DamagedObject = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();
        Damager = NWScript.GetLastDamager().ToNwObject<NwGameObject>();
        DamageAmount = NWScript.GetTotalDamageDealt();
      }
    }

    [NativeEvent(EventScriptType.PlaceableOnDeath)]
    public sealed class OnDeath : IEvent
    {
      public NwPlaceable KilledObject { get; }

      public NwGameObject Killer { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => KilledObject;

      public OnDeath()
      {
        KilledObject = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();
        Killer = NWScript.GetLastKiller().ToNwObject<NwGameObject>();
      }
    }

    [NativeEvent(EventScriptType.PlaceableOnDisarm)]
    public sealed class OnDisarm : IEvent
    {
      public NwPlaceable Placeable { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Placeable;

      public OnDisarm()
      {
        Placeable = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();
      }
    }

    [NativeEvent(EventScriptType.PlaceableOnHeartbeat)]
    public sealed class OnHeartbeat : IEvent
    {
      public NwPlaceable Placeable { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Placeable;

      public OnHeartbeat()
      {
        Placeable = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();
      }
    }

    [NativeEvent(EventScriptType.PlaceableOnInventoryDisturbed)]
    public sealed class OnDisturbed : IEvent
    {
      public InventoryDisturbType DisturbType { get; }

      public NwPlaceable Placeable { get; }

      public NwCreature Disturber { get; }

      public NwItem DisturbedItem { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Placeable;

      public OnDisturbed()
      {
        Placeable = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();
        DisturbType = (InventoryDisturbType) NWScript.GetInventoryDisturbType();
        Disturber = NWScript.GetLastDisturbed().ToNwObject<NwCreature>();
        DisturbedItem = NWScript.GetInventoryDisturbItem().ToNwObject<NwItem>();
      }
    }

    [NativeEvent(EventScriptType.PlaceableOnLock)]
    public sealed class OnLock : IEvent
    {
      public NwPlaceable LockedPlaceable { get; }

      public int LockDC { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => LockedPlaceable;

      public OnLock()
      {
        LockedPlaceable = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();
        LockDC = NWScript.GetLockLockDC(NWScript.OBJECT_SELF);
      }
    }

    [NativeEvent(EventScriptType.PlaceableOnMeleeAttacked)]
    public sealed class OnPhysicalAttacked : IEvent
    {
      public NwPlaceable Placeable { get; }

      public NwCreature Attacker { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Placeable;

      public OnPhysicalAttacked()
      {
        Placeable = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();
        Attacker = NWScript.GetLastAttacker().ToNwObject<NwCreature>();
      }
    }

    [NativeEvent(EventScriptType.PlaceableOnOpen)]
    public sealed class OnOpen : IEvent
    {
      public NwPlaceable Placeable { get; }

      public NwCreature OpenedBy { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Placeable;

      public OnOpen()
      {
        Placeable = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();
        OpenedBy = NWScript.GetLastOpenedBy().ToNwObject<NwCreature>();
      }
    }

    [NativeEvent(EventScriptType.PlaceableOnSpellCastAt)]
    public sealed class OnSpellCastAt : IEvent
    {
      /// <summary>
      /// Gets the placeable targeted by this spell.
      /// </summary>
      public NwPlaceable Placeable { get; }

      /// <summary>
      /// Gets the caster of this spell (creature, placeable, door). Returns null from an area of effect.
      /// </summary>
      public NwGameObject Caster { get; }

      /// <summary>
      /// Gets the spell that was cast.
      /// </summary>
      public Spell Spell { get; }

      /// <summary>
      /// Gets a value indicating whether this spell is considered harmful.
      /// </summary>
      public bool Harmful { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Placeable;

      public OnSpellCastAt()
      {
        Placeable = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();
        Caster = NWScript.GetLastSpellCaster().ToNwObject<NwGameObject>();
        Spell = (Spell)NWScript.GetLastSpell();
        Harmful = NWScript.GetLastSpellHarmful().ToBool();
      }
    }

    [NativeEvent(EventScriptType.PlaceableOnTrapTriggered)]
    public sealed class OnTrapTriggered : IEvent
    {
      public NwPlaceable Placeable { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Placeable;

      public OnTrapTriggered()
      {
        Placeable = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();
      }
    }

    [NativeEvent(EventScriptType.PlaceableOnUnlock)]
    public sealed class OnUnlock : IEvent
    {
      public NwPlaceable Placeable { get; }

      public NwCreature UnlockedBy { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Placeable;

      public OnUnlock()
      {
        Placeable = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();
        UnlockedBy = NWScript.GetLastUnlocked().ToNwObject<NwCreature>();
      }
    }

    [NativeEvent(EventScriptType.PlaceableOnUsed)]
    public sealed class OnUsed : IEvent
    {
      public NwPlaceable Placeable { get; }

      public NwCreature UsedBy { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Placeable;

      public OnUsed()
      {
        Placeable = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();
        UsedBy = NWScript.GetLastUsedBy().ToNwObject<NwCreature>();
      }
    }

    [NativeEvent(EventScriptType.PlaceableOnUserDefinedEvent)]
    public sealed class OnUserDefined : IEvent
    {
      public NwPlaceable Placeable { get; }

      public int EventNumber { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Placeable;

      public OnUserDefined()
      {
        Placeable = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();
        EventNumber = NWScript.GetUserDefinedEventNumber();
      }
    }

    [NativeEvent(EventScriptType.PlaceableOnDialogue)]
    public sealed class OnDialogue : IEvent
    {
      public NwPlaceable Placeable { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Placeable;

      public OnDialogue()
      {
        Placeable = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();
      }
    }

    [NativeEvent(EventScriptType.PlaceableOnLeftClick)]
    public sealed class OnLeftClick : IEvent
    {
      public NwPlaceable Placeable { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Placeable;

      public OnLeftClick()
      {
        Placeable = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();
      }
    }
  }
}
