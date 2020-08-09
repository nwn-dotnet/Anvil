using NWN.API.Constants;
using NWN.Core;

namespace NWN.API.Events
{
  public static class CreatureEvents
  {
    [ScriptEvent(EventScriptType.CreatureOnBlockedByDoor)]
    public sealed class OnBlocked : Event<NwCreature, OnBlocked>
    {
      public NwCreature Creature { get; private set; }
      public NwDoor BlockingDoor { get; private set; }

      protected override void PrepareEvent(NwCreature objSelf)
      {
        Creature = objSelf;
        BlockingDoor = NWScript.GetBlockingDoor().ToNwObject<NwDoor>();
      }
    }

    [ScriptEvent(EventScriptType.CreatureOnEndCombatRound)]
    public sealed class OnCombatRoundEnd : Event<NwCreature, OnCombatRoundEnd>
    {
      public NwCreature Creature { get; private set; }

      protected override void PrepareEvent(NwCreature objSelf)
      {
        Creature = objSelf;
      }
    }

    [ScriptEvent(EventScriptType.CreatureOnDialogue)]
    public sealed class OnConversation : Event<NwCreature, OnConversation>
    {
      public NwCreature Creature { get; private set; }

      protected override void PrepareEvent(NwCreature objSelf)
      {
        Creature = objSelf;
      }
    }

    [ScriptEvent(EventScriptType.CreatureOnDamaged)]
    public class OnDamaged : Event<NwCreature, OnDamaged>
    {
      public NwGameObject Damager { get; private set; }
      public int DamageAmount { get; private set; }

      protected override void PrepareEvent(NwCreature objSelf)
      {
        Damager = NWScript.GetLastDamager().ToNwObject<NwGameObject>();
        DamageAmount = NWScript.GetTotalDamageDealt();
      }
    }

    [ScriptEvent(EventScriptType.CreatureOnDeath)]
    public class OnDeath : Event<NwCreature, OnDeath>
    {
      public NwCreature KilledCreature { get; private set; }
      public NwGameObject Killer { get; private set; }

      protected override void PrepareEvent(NwCreature objSelf)
      {
        KilledCreature = objSelf;
        Killer = NWScript.GetLastKiller().ToNwObject<NwGameObject>();
      }
    }

    [ScriptEvent(EventScriptType.CreatureOnDisturbed)]
    public class OnDisturbed : Event<NwCreature, OnDisturbed>
    {
      public InventoryDisturbType DisturbType { get; private set; }
      public NwCreature CreatureDisturbed { get; private set; }
      public NwCreature Disturber { get; private set; }
      public NwItem DisturbedItem { get; private set; }

      protected override void PrepareEvent(NwCreature objSelf)
      {
        DisturbType = (InventoryDisturbType) NWScript.GetInventoryDisturbType();
        CreatureDisturbed = objSelf;
        Disturber = NWScript.GetLastDisturbed().ToNwObject<NwCreature>();
        DisturbedItem = NWScript.GetInventoryDisturbItem().ToNwObject<NwItem>();
      }
    }

    [ScriptEvent(EventScriptType.CreatureOnHeartbeat)]
    public class OnHeartbeat : Event<NwCreature, OnHeartbeat>
    {
      public NwCreature Creature { get; private set; }

      protected override void PrepareEvent(NwCreature objSelf)
      {
        Creature = objSelf;
      }
    }

    [ScriptEvent(EventScriptType.CreatureOnNotice)]
    public class OnPerception : Event<NwCreature, OnPerception>
    {
      public NwCreature Creature { get; private set; }
      public PerceptionEventType PerceptionEventType { get; private set; }
      public NwCreature PerceivedCreature { get; private set; }

      protected override void PrepareEvent(NwCreature objSelf)
      {
        Creature = objSelf;
        PerceptionEventType = GetPerceptionEventType();
        PerceivedCreature = NWScript.GetLastPerceived().ToNwObject<NwCreature>();
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
    }

    [ScriptEvent(EventScriptType.CreatureOnMeleeAttacked)]
    public class OnPhysicalAttacked : Event<NwCreature, OnPhysicalAttacked>
    {
      public NwCreature Creature { get; private set; }
      public NwCreature Attacker { get; private set; }

      protected override void PrepareEvent(NwCreature objSelf)
      {
        Creature = objSelf;
        Attacker = NWScript.GetLastAttacker().ToNwObject<NwCreature>();
      }
    }

    [ScriptEvent(EventScriptType.CreatureOnRested)]
    public class OnRested : Event<NwCreature, OnRested>
    {
      public NwCreature Creature { get; private set; }

      protected override void PrepareEvent(NwCreature objSelf)
      {
        Creature = objSelf;
      }
    }

    [ScriptEvent(EventScriptType.CreatureOnSpawnIn)]
    public class OnSpawn : Event<NwCreature, OnSpawn>
    {
      public NwCreature Creature { get; private set; }

      protected override void PrepareEvent(NwCreature objSelf)
      {
        Creature = objSelf;
      }
    }

    [ScriptEvent(EventScriptType.CreatureOnSpellCastAt)]
    public class OnSpellCastAt : Event<NwCreature, OnSpellCastAt>
    {
      public NwCreature Creature { get; private set; }

      protected override void PrepareEvent(NwCreature objSelf)
      {
        Creature = objSelf;
      }
    }

    [ScriptEvent(EventScriptType.CreatureOnUserDefinedEvent)]
    public class OnUserDefined : Event<NwCreature, OnUserDefined>
    {
      public int EventNumber { get; private set; }
      public NwCreature Creature { get; private set; }

      protected override void PrepareEvent(NwCreature objSelf)
      {
        EventNumber = NWScript.GetUserDefinedEventNumber();
        Creature = objSelf;
      }
    }
  }
}