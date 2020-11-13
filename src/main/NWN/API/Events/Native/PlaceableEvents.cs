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
    public sealed class OnClose : NativeEvent<NwPlaceable, OnClose>
    {
      public NwPlaceable Placeable { get; private set; }

      public NwCreature LastClosedBy { get; private set; }

      protected override void PrepareEvent(NwPlaceable objSelf)
      {
        Placeable = objSelf;
        LastClosedBy = NWScript.GetLastClosedBy().ToNwObject<NwCreature>();
      }
    }

    [NativeEvent(EventScriptType.PlaceableOnDamaged)]
    public sealed class OnDamaged : NativeEvent<NwPlaceable, OnDamaged>
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

    [NativeEvent(EventScriptType.PlaceableOnDeath)]
    public sealed class OnDeath : NativeEvent<NwPlaceable, OnDeath>
    {
      public NwPlaceable KilledObject { get; private set; }

      public NwGameObject Killer { get; private set; }

      protected override void PrepareEvent(NwPlaceable objSelf)
      {
        KilledObject = objSelf;
        Killer = NWScript.GetLastKiller().ToNwObject<NwGameObject>();
      }
    }

    [NativeEvent(EventScriptType.PlaceableOnDisarm)]
    public sealed class OnDisarm : NativeEvent<NwPlaceable, OnDisarm>
    {
      public NwPlaceable Placeable { get; private set; }

      protected override void PrepareEvent(NwPlaceable objSelf)
      {
        Placeable = objSelf;
      }
    }

    [NativeEvent(EventScriptType.PlaceableOnHeartbeat)]
    public sealed class OnHeartbeat : NativeEvent<NwPlaceable, OnHeartbeat>
    {
      public NwPlaceable Placeable { get; private set; }

      protected override void PrepareEvent(NwPlaceable objSelf)
      {
        Placeable = objSelf;
      }
    }

    [NativeEvent(EventScriptType.PlaceableOnInventoryDisturbed)]
    public sealed class OnDisturbed : NativeEvent<NwPlaceable, OnDisturbed>
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

    [NativeEvent(EventScriptType.PlaceableOnLock)]
    public sealed class OnLock : NativeEvent<NwPlaceable, OnLock>
    {
      public NwPlaceable LockedPlaceable { get; private set; }

      public int LockDC { get; private set; }

      protected override void PrepareEvent(NwPlaceable objSelf)
      {
        LockedPlaceable = objSelf;
        LockDC = NWScript.GetLockLockDC(objSelf);
      }
    }

    [NativeEvent(EventScriptType.PlaceableOnMeleeAttacked)]
    public sealed class OnPhysicalAttacked : NativeEvent<NwPlaceable, OnPhysicalAttacked>
    {
      public NwPlaceable Placeable { get; private set; }

      public NwCreature Attacker { get; private set; }

      protected override void PrepareEvent(NwPlaceable objSelf)
      {
        Placeable = objSelf;
        Attacker = NWScript.GetLastAttacker().ToNwObject<NwCreature>();
      }
    }

    [NativeEvent(EventScriptType.PlaceableOnOpen)]
    public sealed class OnOpen : NativeEvent<NwPlaceable, OnOpen>
    {
      public NwPlaceable Placeable { get; private set; }

      public NwCreature OpenedBy { get; private set; }

      protected override void PrepareEvent(NwPlaceable objSelf)
      {
        Placeable = objSelf;
        OpenedBy = NWScript.GetLastOpenedBy().ToNwObject<NwCreature>();
      }
    }

    [NativeEvent(EventScriptType.PlaceableOnSpellCastAt)]
    public sealed class OnSpellCastAt : NativeEvent<NwPlaceable, OnSpellCastAt>
    {
      public NwPlaceable Placeable { get; private set; }

      protected override void PrepareEvent(NwPlaceable objSelf)
      {
        Placeable = objSelf;
      }
    }

    [NativeEvent(EventScriptType.PlaceableOnTrapTriggered)]
    public sealed class OnTrapTriggered : NativeEvent<NwPlaceable, OnTrapTriggered>
    {
      public NwPlaceable Placeable { get; private set; }

      protected override void PrepareEvent(NwPlaceable objSelf)
      {
        Placeable = objSelf;
      }
    }

    [NativeEvent(EventScriptType.PlaceableOnUnlock)]
    public sealed class OnUnlock : NativeEvent<NwPlaceable, OnUnlock>
    {
      public NwPlaceable Placeable { get; private set; }

      public NwCreature UnlockedBy { get; private set; }

      protected override void PrepareEvent(NwPlaceable objSelf)
      {
        Placeable = objSelf;
        UnlockedBy = NWScript.GetLastUnlocked().ToNwObject<NwCreature>();
      }
    }

    [NativeEvent(EventScriptType.PlaceableOnUsed)]
    public sealed class OnUsed : NativeEvent<NwPlaceable, OnUsed>
    {
      public NwPlaceable Placeable { get; private set; }

      public NwCreature UsedBy { get; private set; }

      protected override void PrepareEvent(NwPlaceable objSelf)
      {
        Placeable = objSelf;
        UsedBy = NWScript.GetLastUsedBy().ToNwObject<NwCreature>();
      }
    }

    [NativeEvent(EventScriptType.PlaceableOnUserDefinedEvent)]
    public sealed class OnUserDefined : NativeEvent<NwPlaceable, OnUserDefined>
    {
      public NwPlaceable Placeable { get; private set; }

      public int EventNumber { get; private set; }

      protected override void PrepareEvent(NwPlaceable objSelf)
      {
        Placeable = objSelf;
        EventNumber = NWScript.GetUserDefinedEventNumber();
      }
    }

    [NativeEvent(EventScriptType.PlaceableOnDialogue)]
    public sealed class OnDialogue : NativeEvent<NwPlaceable, OnDialogue>
    {
      public NwPlaceable Placeable { get; private set; }

      protected override void PrepareEvent(NwPlaceable objSelf)
      {
        Placeable = objSelf;
      }
    }

    [NativeEvent(EventScriptType.PlaceableOnLeftClick)]
    public sealed class OnLeftClick : NativeEvent<NwPlaceable, OnLeftClick>
    {
      public NwPlaceable Placeable { get; private set; }

      protected override void PrepareEvent(NwPlaceable objSelf)
      {
        Placeable = objSelf;
      }
    }
  }
}
