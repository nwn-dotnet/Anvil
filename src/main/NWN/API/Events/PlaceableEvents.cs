using System;
using NWN.API.Constants;
using NWN.Core;

namespace NWN.API.Events
{
  public static class PlaceableEvents
  {
    [ScriptEvent(EventScriptType.PlaceableOnClosed)]
    public class OnClose : IEvent<NwPlaceable, OnClose>
    {
      public NwPlaceable Placeable { get; private set; }
      public NwCreature LastClosedBy { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Placeable = (NwPlaceable) objSelf;
        LastClosedBy = NWScript.GetLastClosedBy().ToNwObject<NwCreature>();
        Callbacks?.Invoke(this);
      }

      public event Action<OnClose> Callbacks;
    }

    [ScriptEvent(EventScriptType.PlaceableOnDamaged)]
    public class OnDamaged : IEvent<NwPlaceable, OnDamaged>
    {
      public NwPlaceable DamagedObject { get; private set; }
      public NwGameObject Damager { get; private set; }
      public int DamageAmount { get; private set; }


      public void BroadcastEvent(NwObject objSelf)
      {
        DamagedObject = (NwPlaceable) objSelf;
        Damager = NWScript.GetLastDamager().ToNwObject<NwGameObject>();
        DamageAmount = NWScript.GetTotalDamageDealt();
        Callbacks?.Invoke(this);
      }

      public event Action<OnDamaged> Callbacks;
    }

    [ScriptEvent(EventScriptType.PlaceableOnDeath)]
    public class OnDeath : IEvent<NwPlaceable, OnDeath>
    {
      public NwPlaceable KilledObject { get; private set; }
      public NwGameObject Killer { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        KilledObject = (NwPlaceable) objSelf;
        Killer = NWScript.GetLastKiller().ToNwObject<NwGameObject>();
        Callbacks?.Invoke(this);
      }

      public event Action<OnDeath> Callbacks;
    }

    [ScriptEvent(EventScriptType.PlaceableOnInventoryDisturbed)]
    public class OnDisturbed : IEvent<NwPlaceable, OnDisturbed>
    {
      public InventoryDisturbType DisturbType { get; private set; }
      public NwPlaceable Placeable { get; private set; }
      public NwCreature Disturber { get; private set; }
      public NwItem DisturbedItem { get; private set; }


      public void BroadcastEvent(NwObject objSelf)
      {
        DisturbType = (InventoryDisturbType) NWScript.GetInventoryDisturbType();
        Placeable = (NwPlaceable) objSelf;
        Disturber = NWScript.GetLastDisturbed().ToNwObject<NwCreature>();
        DisturbedItem = NWScript.GetInventoryDisturbItem().ToNwObject<NwItem>();
        Callbacks?.Invoke(this);
      }

      public event Action<OnDisturbed> Callbacks;
    }

    [ScriptEvent(EventScriptType.PlaceableOnHeartbeat)]
    public class OnHeartbeat : IEvent<NwPlaceable, OnHeartbeat>
    {
      public NwPlaceable Placeable { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Placeable = (NwPlaceable) objSelf;
        Callbacks?.Invoke(this);
      }

      public event Action<OnHeartbeat> Callbacks;
    }

    [ScriptEvent(EventScriptType.PlaceableOnLock)]
    public class OnLock : IEvent<NwPlaceable, OnLock>
    {
      public NwPlaceable LockedPlaceable { get; private set; }
      public int LockDC { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        LockedPlaceable = (NwPlaceable) objSelf;
        LockDC = NWScript.GetLockLockDC(objSelf);
        Callbacks?.Invoke(this);
      }

      public event Action<OnLock> Callbacks;
    }

    [ScriptEvent(EventScriptType.PlaceableOnOpen)]
    public class OnOpen : IEvent<NwPlaceable, OnOpen>
    {
      public NwPlaceable Placeable { get; private set; }
      public NwCreature OpenedBy { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Placeable = (NwPlaceable) objSelf;
        OpenedBy = NWScript.GetLastOpenedBy().ToNwObject<NwCreature>();
        Callbacks?.Invoke(this);
      }

      public event Action<OnOpen> Callbacks;
    }

    [ScriptEvent(EventScriptType.PlaceableOnMeleeAttacked)]
    public class OnPhysicalAttacked : IEvent<NwPlaceable, OnPhysicalAttacked>
    {
      public NwPlaceable Placeable { get; private set; }
      public NwCreature Attacker { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Placeable = (NwPlaceable) objSelf;
        Attacker = NWScript.GetLastAttacker().ToNwObject<NwCreature>();
        Callbacks?.Invoke(this);
      }

      public event Action<OnPhysicalAttacked> Callbacks;
    }

    [ScriptEvent(EventScriptType.PlaceableOnSpellCastAt)]
    public class OnSpellCastAt : IEvent<NwPlaceable, OnSpellCastAt>
    {
      public NwPlaceable Placeable { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Placeable = (NwPlaceable) objSelf;
        Callbacks?.Invoke(this);
      }

      public event Action<OnSpellCastAt> Callbacks;
    }

    [ScriptEvent(EventScriptType.PlaceableOnUnlock)]
    public class OnUnlock : IEvent<NwPlaceable, OnUnlock>
    {
      public NwPlaceable Placeable { get; private set; }
      public NwCreature UnlockedBy { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Placeable = (NwPlaceable) objSelf;
        UnlockedBy = NWScript.GetLastUnlocked().ToNwObject<NwCreature>();
        Callbacks?.Invoke(this);
      }

      public event Action<OnUnlock> Callbacks;
    }

    [ScriptEvent(EventScriptType.PlaceableOnUsed)]
    public class OnUsed : IEvent<NwPlaceable, OnUsed>
    {
      public NwPlaceable Placeable { get; private set; }
      public NwCreature UsedBy { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Placeable = (NwPlaceable) objSelf;
        UsedBy = NWScript.GetLastUsedBy().ToNwObject<NwCreature>();
        Callbacks?.Invoke(this);
      }

      public event Action<OnUsed> Callbacks;
    }

    [ScriptEvent(EventScriptType.PlaceableOnUserDefinedEvent)]
    public class OnUserDefined : IEvent<NwPlaceable, OnUserDefined>
    {
      public NwPlaceable Placeable { get; private set; }
      public int EventNumber { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Placeable = (NwPlaceable) objSelf;
        EventNumber = NWScript.GetUserDefinedEventNumber();
        Callbacks?.Invoke(this);
      }

      public event Action<OnUserDefined> Callbacks;
    }
  }
}