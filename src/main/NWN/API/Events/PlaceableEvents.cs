using NWN.API.Constants;
using NWN.Core;

namespace NWN.API.Events
{
  public static class PlaceableEvents
  {
    [ScriptEvent(EventScriptType.PlaceableOnClosed)]
    public class OnClose : Event<NwPlaceable, OnClose>
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
    public class OnDamaged : Event<NwPlaceable, OnDamaged>
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
    public class OnDeath : Event<NwPlaceable, OnDeath>
    {
      public NwPlaceable KilledObject { get; private set; }
      public NwGameObject Killer { get; private set; }

      protected override void PrepareEvent(NwPlaceable objSelf)
      {
        KilledObject = objSelf;
        Killer = NWScript.GetLastKiller().ToNwObject<NwGameObject>();
      }
    }

    [ScriptEvent(EventScriptType.PlaceableOnInventoryDisturbed)]
    public class OnDisturbed : Event<NwPlaceable, OnDisturbed>
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

    [ScriptEvent(EventScriptType.PlaceableOnHeartbeat)]
    public class OnHeartbeat : Event<NwPlaceable, OnHeartbeat>
    {
      public NwPlaceable Placeable { get; private set; }

      protected override void PrepareEvent(NwPlaceable objSelf)
      {
        Placeable = objSelf;
      }
    }

    [ScriptEvent(EventScriptType.PlaceableOnLock)]
    public class OnLock : Event<NwPlaceable, OnLock>
    {
      public NwPlaceable LockedPlaceable { get; private set; }
      public int LockDC { get; private set; }

      protected override void PrepareEvent(NwPlaceable objSelf)
      {
        LockedPlaceable = objSelf;
        LockDC = NWScript.GetLockLockDC(objSelf);
      }
    }

    [ScriptEvent(EventScriptType.PlaceableOnOpen)]
    public class OnOpen : Event<NwPlaceable, OnOpen>
    {
      public NwPlaceable Placeable { get; private set; }
      public NwCreature OpenedBy { get; private set; }

      protected override void PrepareEvent(NwPlaceable objSelf)
      {
        Placeable = objSelf;
        OpenedBy = NWScript.GetLastOpenedBy().ToNwObject<NwCreature>();
      }
    }

    [ScriptEvent(EventScriptType.PlaceableOnMeleeAttacked)]
    public class OnPhysicalAttacked : Event<NwPlaceable, OnPhysicalAttacked>
    {
      public NwPlaceable Placeable { get; private set; }
      public NwCreature Attacker { get; private set; }

      protected override void PrepareEvent(NwPlaceable objSelf)
      {
        Placeable = objSelf;
        Attacker = NWScript.GetLastAttacker().ToNwObject<NwCreature>();
      }
    }

    [ScriptEvent(EventScriptType.PlaceableOnSpellCastAt)]
    public class OnSpellCastAt : Event<NwPlaceable, OnSpellCastAt>
    {
      public NwPlaceable Placeable { get; private set; }

      protected override void PrepareEvent(NwPlaceable objSelf)
      {
        Placeable = objSelf;
      }
    }

    [ScriptEvent(EventScriptType.PlaceableOnUnlock)]
    public class OnUnlock : Event<NwPlaceable, OnUnlock>
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
    public class OnUsed : Event<NwPlaceable, OnUsed>
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
    public class OnUserDefined : Event<NwPlaceable, OnUserDefined>
    {
      public NwPlaceable Placeable { get; private set; }
      public int EventNumber { get; private set; }

      protected override void PrepareEvent(NwPlaceable objSelf)
      {
        Placeable = objSelf;
        EventNumber = NWScript.GetUserDefinedEventNumber();
      }
    }
  }
}