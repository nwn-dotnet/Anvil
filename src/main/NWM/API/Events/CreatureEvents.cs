using System;
using NWM.API.Constants;
using NWN;

namespace NWM.API.Events
{
  public static class CreatureEvents
  {
    [ScriptEvent(EventScriptType.CreatureOnBlockedByDoor)]
    public sealed class OnBlocked : IEvent<NwCreature, OnBlocked>
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

    [ScriptEvent(EventScriptType.CreatureOnEndCombatRound)]
    public sealed class OnCombatRoundEnd : IEvent<NwCreature, OnCombatRoundEnd>
    {
      public NwCreature Creature { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Creature = (NwCreature) objSelf;
        Callbacks?.Invoke(this);
      }

      public event Action<OnCombatRoundEnd> Callbacks;
    }

    [ScriptEvent(EventScriptType.CreatureOnDialogue)]
    public sealed class OnConversation : IEvent<NwCreature, OnConversation>
    {
      public NwCreature Creature { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Creature = (NwCreature) objSelf;
        Callbacks?.Invoke(this);
      }

      public event Action<OnConversation> Callbacks;
    }

    [ScriptEvent(EventScriptType.CreatureOnDamaged)]
    public class OnDamaged : IEvent<NwCreature, OnDamaged>
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

    [ScriptEvent(EventScriptType.CreatureOnDeath)]
    public class OnDeath : IEvent<NwCreature, OnDeath>
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

    [ScriptEvent(EventScriptType.CreatureOnDisturbed)]
    public class OnDisturbed : IEvent<NwCreature, OnDisturbed>
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

    [ScriptEvent(EventScriptType.CreatureOnHeartbeat)]
    public class OnHeartbeat : IEvent<NwCreature, OnHeartbeat>
    {
      public NwCreature Creature { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Creature = (NwCreature) objSelf;
        Callbacks?.Invoke(this);
      }

      public event Action<OnHeartbeat> Callbacks;
    }

    [ScriptEvent(EventScriptType.CreatureOnNotice)]
    public class OnPerception : IEvent<NwCreature, OnPerception>
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

    [ScriptEvent(EventScriptType.CreatureOnMeleeAttacked)]
    public class OnPhysicalAttacked : IEvent<NwCreature, OnPhysicalAttacked>
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

    [ScriptEvent(EventScriptType.CreatureOnRested)]
    public class OnRested : IEvent<NwCreature, OnRested>
    {
      public NwCreature Creature { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Creature = (NwCreature) objSelf;
        Callbacks?.Invoke(this);
      }

      public event Action<OnRested> Callbacks;
    }

    [ScriptEvent(EventScriptType.CreatureOnSpawnIn)]
    public class OnSpawn : IEvent<NwCreature, OnSpawn>
    {
      public NwCreature Creature { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Creature = (NwCreature) objSelf;
        Callbacks?.Invoke(this);
      }

      public event Action<OnSpawn> Callbacks;
    }

    [ScriptEvent(EventScriptType.CreatureOnSpellCastAt)]
    public class OnSpellCastAt : IEvent<NwCreature, OnSpellCastAt>
    {
      public NwCreature Creature { get; private set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Creature = (NwCreature) objSelf;
        Callbacks?.Invoke(this);
      }

      public event Action<OnSpellCastAt> Callbacks;
    }

    [ScriptEvent(EventScriptType.CreatureOnUserDefinedEvent)]
    public class OnUserDefined : IEvent<NwCreature, OnUserDefined>
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