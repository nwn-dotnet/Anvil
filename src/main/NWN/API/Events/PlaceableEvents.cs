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
    [ScriptEvent(EventScriptType.PlaceableOnClosed)]
    public sealed class OnClose : Event<NwPlaceable, OnClose>
    {
      public NwPlaceable Placeable { get; private set; }

      public NwCreature LastClosedBy { get; private set; }

      protected override void PrepareEvent(NwPlaceable objSelf)
      {
        Placeable = objSelf;
        LastClosedBy = NWScript.GetLastClosedBy().ToNwObject<NwCreature>();
      }
    }

    [ScriptEvent(EventScriptType.PlaceableOnDamaged)]
    public sealed class OnDamaged : Event<NwPlaceable, OnDamaged>
    {
      public NwPlaceable DamagedObject { get; private set; }

      public NwGameObject Damager { get; private set; }

      public int DamageAmount { get; private set; }

      protected override void PrepareEvent(NwPlaceable objSelf)
      {
        DamagedObject = objSelf;
        Damager = NWScript.GetLastDamager().ToNwObject<NwGameObject>();
        DamageAmount = NWScript.GetTotalDamageDealt();
      }
    }

    [ScriptEvent(EventScriptType.PlaceableOnDeath)]
    public sealed class OnDeath : Event<NwPlaceable, OnDeath>
    {
      public NwPlaceable KilledObject { get; private set; }

      public NwGameObject Killer { get; private set; }

      protected override void PrepareEvent(NwPlaceable objSelf)
      {
        KilledObject = objSelf;
        Killer = NWScript.GetLastKiller().ToNwObject<NwGameObject>();
      }
    }

    [ScriptEvent(EventScriptType.PlaceableOnDisarm)]
    public sealed class OnDisarm : Event<NwPlaceable, OnDisarm>
    {
      public NwPlaceable Placeable { get; private set; }

      protected override void PrepareEvent(NwPlaceable objSelf)
      {
        Placeable = objSelf;
      }
    }

    [ScriptEvent(EventScriptType.PlaceableOnHeartbeat)]
    public sealed class OnHeartbeat : Event<NwPlaceable, OnHeartbeat>
    {
      public NwPlaceable Placeable { get; private set; }

      protected override void PrepareEvent(NwPlaceable objSelf)
      {
        Placeable = objSelf;
      }
    }

    [ScriptEvent(EventScriptType.PlaceableOnInventoryDisturbed)]
    public sealed class OnDisturbed : Event<NwPlaceable, OnDisturbed>
    {
      public InventoryDisturbType DisturbType { get; private set; }

      public NwPlaceable Placeable { get; private set; }

      public NwCreature Disturber { get; private set; }

      public NwItem DisturbedItem { get; private set; }

      protected override void PrepareEvent(NwPlaceable objSelf)
      {
        DisturbType = (InventoryDisturbType) NWScript.GetInventoryDisturbType();
        Placeable = objSelf;
        Disturber = NWScript.GetLastDisturbed().ToNwObject<NwCreature>();
        DisturbedItem = NWScript.GetInventoryDisturbItem().ToNwObject<NwItem>();
      }
    }

    [ScriptEvent(EventScriptType.PlaceableOnLock)]
    public sealed class OnLock : Event<NwPlaceable, OnLock>
    {
      public NwPlaceable LockedPlaceable { get; private set; }

      public int LockDC { get; private set; }

      protected override void PrepareEvent(NwPlaceable objSelf)
      {
        LockedPlaceable = objSelf;
        LockDC = NWScript.GetLockLockDC(objSelf);
      }
    }

    [ScriptEvent(EventScriptType.PlaceableOnMeleeAttacked)]
    public sealed class OnPhysicalAttacked : Event<NwPlaceable, OnPhysicalAttacked>
    {
      public NwPlaceable Placeable { get; private set; }

      public NwCreature Attacker { get; private set; }

      protected override void PrepareEvent(NwPlaceable objSelf)
      {
        Placeable = objSelf;
        Attacker = NWScript.GetLastAttacker().ToNwObject<NwCreature>();
      }
    }

    [ScriptEvent(EventScriptType.PlaceableOnOpen)]
    public sealed class OnOpen : Event<NwPlaceable, OnOpen>
    {
      public NwPlaceable Placeable { get; private set; }

      public NwCreature OpenedBy { get; private set; }

      protected override void PrepareEvent(NwPlaceable objSelf)
      {
        Placeable = objSelf;
        OpenedBy = NWScript.GetLastOpenedBy().ToNwObject<NwCreature>();
      }
    }

    [ScriptEvent(EventScriptType.PlaceableOnSpellCastAt)]
    public sealed class OnSpellCastAt : Event<NwPlaceable, OnSpellCastAt>
    {
      public NwPlaceable Placeable { get; private set; }

      protected override void PrepareEvent(NwPlaceable objSelf)
      {
        Placeable = objSelf;
      }
    }

    [ScriptEvent(EventScriptType.PlaceableOnTrapTriggered)]
    public sealed class OnTrapTriggered : Event<NwPlaceable, OnTrapTriggered>
    {
      public NwPlaceable Placeable { get; private set; }

      protected override void PrepareEvent(NwPlaceable objSelf)
      {
        Placeable = objSelf;
      }
    }

    [ScriptEvent(EventScriptType.PlaceableOnUnlock)]
    public sealed class OnUnlock : Event<NwPlaceable, OnUnlock>
    {
      public NwPlaceable Placeable { get; private set; }

      public NwCreature UnlockedBy { get; private set; }

      protected override void PrepareEvent(NwPlaceable objSelf)
      {
        Placeable = objSelf;
        UnlockedBy = NWScript.GetLastUnlocked().ToNwObject<NwCreature>();
      }
    }

    [ScriptEvent(EventScriptType.PlaceableOnUsed)]
    public sealed class OnUsed : Event<NwPlaceable, OnUsed>
    {
      public NwPlaceable Placeable { get; private set; }

      public NwCreature UsedBy { get; private set; }

      protected override void PrepareEvent(NwPlaceable objSelf)
      {
        Placeable = objSelf;
        UsedBy = NWScript.GetLastUsedBy().ToNwObject<NwCreature>();
      }
    }

    [ScriptEvent(EventScriptType.PlaceableOnUserDefinedEvent)]
    public sealed class OnUserDefined : Event<NwPlaceable, OnUserDefined>
    {
      public NwPlaceable Placeable { get; private set; }

      public int EventNumber { get; private set; }

      protected override void PrepareEvent(NwPlaceable objSelf)
      {
        Placeable = objSelf;
        EventNumber = NWScript.GetUserDefinedEventNumber();
      }
    }

    [ScriptEvent(EventScriptType.PlaceableOnDialogue)]
    public sealed class OnDialogue : Event<NwPlaceable, OnDialogue>
    {
      public NwPlaceable Placeable { get; private set; }

      protected override void PrepareEvent(NwPlaceable objSelf)
      {
        Placeable = objSelf;
      }
    }

    [ScriptEvent(EventScriptType.PlaceableOnLeftClick)]
    public sealed class OnLeftClick : Event<NwPlaceable, OnLeftClick>
    {
      public NwPlaceable Placeable { get; private set; }

      protected override void PrepareEvent(NwPlaceable objSelf)
      {
        Placeable = objSelf;
      }
    }
  }
}
