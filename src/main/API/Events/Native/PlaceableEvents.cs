using System;
using NWM.API.Constants;
using NWN;

namespace NWM.API.Events
{
  public static class PlaceableEvents
  {
    [EventInfo(EventType.Native, DefaultScriptSuffix = "clo")]
    public class OnClose : IEvent<OnClose>
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

    [EventInfo(EventType.Native, DefaultScriptSuffix = "dam")]
    public class OnDamaged : IEvent<OnDamaged>
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

    [EventInfo(EventType.Native, DefaultScriptSuffix = "dea")]
    public class OnDeath : IEvent<OnDeath>
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

    [EventInfo(EventType.Native, DefaultScriptSuffix = "dis")]
    public class OnDisturbed : IEvent<OnDisturbed>
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

    [EventInfo(EventType.Native, DefaultScriptSuffix = "hea")]
    public class OnHeartbeat : IEvent<OnHeartbeat>
    {
      public NwPlaceable Placeable { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Placeable = (NwPlaceable) objSelf;
        Callbacks?.Invoke(this);
      }

      public event Action<OnHeartbeat> Callbacks;
    }

    [EventInfo(EventType.Native, DefaultScriptSuffix = "loc")]
    public class OnLock : IEvent<OnLock>
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

    [EventInfo(EventType.Native, DefaultScriptSuffix = "ope")]
    public class OnOpen : IEvent<OnOpen>
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

    [EventInfo(EventType.Native, DefaultScriptSuffix = "phy")]
    public class OnPhysicalAttacked : IEvent<OnPhysicalAttacked>
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

    [EventInfo(EventType.Native, DefaultScriptSuffix = "spe")]
    public class OnSpellCastAt : IEvent<OnSpellCastAt>
    {
      public NwPlaceable Placeable { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Placeable = (NwPlaceable) objSelf;
        Callbacks?.Invoke(this);
      }

      public event Action<OnSpellCastAt> Callbacks;
    }

    public class OnUnlock : IEvent<OnUnlock>
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

    public class OnUsed : IEvent<OnUsed>
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

    [EventInfo(EventType.Native, DefaultScriptSuffix = "use")]
    public class OnUserDefined : IEvent<OnUserDefined>
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