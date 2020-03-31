using System;
using NWM.API.Constants;
using NWN;

namespace NWM.API.Events
{
  public static class CreatureEvents
  {
    [EventInfo(EventType.Native, DefaultScriptSuffix = "blo")]
    public sealed class OnBlocked : IEvent<OnBlocked>
    {
      public NwCreature Creature { get; private set; }
      public NwDoor BlockingDoor { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Creature = (NwCreature) objSelf;
        BlockingDoor = NWScript.GetBlockingDoor().ToNwObject<NwDoor>();
        Callbacks?.Invoke(this);
      }

      public event Action<OnBlocked> Callbacks;
    }

    [EventInfo(EventType.Native, DefaultScriptSuffix = "com")]
    public sealed class OnCombatRoundEnd : IEvent<OnCombatRoundEnd>
    {
      public NwCreature Creature { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Creature = (NwCreature) objSelf;
        Callbacks?.Invoke(this);
      }

      public event Action<OnCombatRoundEnd> Callbacks;
    }

    [EventInfo(EventType.Native, DefaultScriptSuffix = "con")]
    public sealed class OnConversation : IEvent<OnConversation>
    {
      public NwCreature Creature { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Creature = (NwCreature) objSelf;
        Callbacks?.Invoke(this);
      }

      public event Action<OnConversation> Callbacks;
    }

    [EventInfo(EventType.Native, DefaultScriptSuffix = "dam")]
    public class OnDamaged : IEvent<OnDamaged>
    {
      public NwGameObject Damager { get; private set; }
      public int DamageAmount { get; private set; }


      public void BroadcastEvent(NwObject objSelf)
      {
        Damager = NWScript.GetLastDamager().ToNwObject<NwGameObject>();
        DamageAmount = NWScript.GetTotalDamageDealt();
        Callbacks?.Invoke(this);
      }

      public event Action<OnDamaged> Callbacks;
    }

    [EventInfo(EventType.Native, DefaultScriptSuffix = "dea")]
    public class OnDeath : IEvent<OnDeath>
    {
      public NwCreature KilledCreature { get; private set; }
      public NwGameObject Killer { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        KilledCreature = (NwCreature) objSelf;
        Killer = NWScript.GetLastKiller().ToNwObject<NwGameObject>();
        Callbacks?.Invoke(this);
      }

      public event Action<OnDeath> Callbacks;
    }

    [EventInfo(EventType.Native, DefaultScriptSuffix = "dis")]
    public class OnDisturbed : IEvent<OnDisturbed>
    {
      public InventoryDisturbType DisturbType { get; private set; }
      public NwCreature CreatureDisturbed { get; private set; }
      public NwCreature Disturber { get; private set; }
      public NwItem DisturbedItem { get; private set; }


      public void BroadcastEvent(NwObject objSelf)
      {
        DisturbType = (InventoryDisturbType) NWScript.GetInventoryDisturbType();
        CreatureDisturbed = (NwCreature) objSelf;
        Disturber = NWScript.GetLastDisturbed().ToNwObject<NwCreature>();
        DisturbedItem = NWScript.GetInventoryDisturbItem().ToNwObject<NwItem>();
        Callbacks?.Invoke(this);
      }

      public event Action<OnDisturbed> Callbacks;
    }

    [EventInfo(EventType.Native, DefaultScriptSuffix = "hea")]
    public class OnHeartbeat : IEvent<OnHeartbeat>
    {
      public NwCreature Creature { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Creature = (NwCreature) objSelf;
        Callbacks?.Invoke(this);
      }

      public event Action<OnHeartbeat> Callbacks;
    }

    [EventInfo(EventType.Native, DefaultScriptSuffix = "per")]
    public class OnPerception : IEvent<OnPerception>
    {
      public NwCreature Creature { get; private set; }
      public PerceptionEventType PerceptionEventType { get; private set; }
      public NwCreature PerceivedCreature { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Creature = (NwCreature) objSelf;
        PerceptionEventType = GetPerceptionEventType();
        PerceivedCreature = NWScript.GetLastPerceived().ToNwObject<NwCreature>();
        Callbacks?.Invoke(this);
      }

      private PerceptionEventType GetPerceptionEventType()
      {
        if (NWScript.GetLastPerceptionSeen().ToBool())
        {
          return PerceptionEventType.Seen;
        }

        if (NWScript.GetLastPerceptionVanished().ToBool())
        {
          return PerceptionEventType.Vanished;
        }

        if (NWScript.GetLastPerceptionHeard().ToBool())
        {
          return PerceptionEventType.Heard;
        }

        if (NWScript.GetLastPerceptionInaudible().ToBool())
        {
          return PerceptionEventType.Inaudible;
        }

        return PerceptionEventType.Unknown;
      }

      public event Action<OnPerception> Callbacks;
    }

    [EventInfo(EventType.Native, DefaultScriptSuffix = "phy")]
    public class OnPhysicalAttacked : IEvent<OnPhysicalAttacked>
    {
      public NwCreature Creature { get; private set; }
      public NwCreature Attacker { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Creature = (NwCreature) objSelf;
        Attacker = NWScript.GetLastAttacker().ToNwObject<NwCreature>();
        Callbacks?.Invoke(this);
      }

      public event Action<OnPhysicalAttacked> Callbacks;
    }

    [EventInfo(EventType.Native, DefaultScriptSuffix = "res")]
    public class OnRested : IEvent<OnRested>
    {
      public NwCreature Creature { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Creature = (NwCreature) objSelf;
        Callbacks?.Invoke(this);
      }

      public event Action<OnRested> Callbacks;
    }

    [EventInfo(EventType.Native, DefaultScriptSuffix = "spa")]
    public class OnSpawn : IEvent<OnSpawn>
    {
      public NwCreature Creature { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Creature = (NwCreature) objSelf;
        Callbacks?.Invoke(this);
      }

      public event Action<OnSpawn> Callbacks;
    }

    [EventInfo(EventType.Native, DefaultScriptSuffix = "spe")]
    public class OnSpellCastAt : IEvent<OnSpellCastAt>
    {
      public NwCreature Creature { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Creature = (NwCreature) objSelf;
        Callbacks?.Invoke(this);
      }

      public event Action<OnSpellCastAt> Callbacks;
    }

    [EventInfo(EventType.Native, DefaultScriptSuffix = "use")]
    public class OnUserDefined : IEvent<OnUserDefined>
    {
      public int EventNumber { get; private set; }
      public NwCreature Creature { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        EventNumber = NWScript.GetUserDefinedEventNumber();
        Creature = (NwCreature) objSelf;
        Callbacks?.Invoke(this);
      }

      public event Action<OnUserDefined> Callbacks;
    }
  }
}