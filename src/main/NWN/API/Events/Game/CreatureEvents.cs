using NWN.API.Constants;
using NWN.Core;

// TODO Populate event data.
namespace NWN.API.Events
{
  /// <summary>
  /// Events for Creatures.
  /// </summary>
  public static class CreatureEvents
  {
    /// <summary>
    /// Called when the creature is blocked by a door.
    /// </summary>
    [NativeEvent(EventScriptType.CreatureOnBlockedByDoor)]
    public sealed class OnBlocked : IEvent
    {
      /// <summary>
      /// Gets the blocked creature.
      /// </summary>
      public NwCreature Creature { get; }

      /// <summary>
      /// Gets the door that is blocking the creature.
      /// </summary>
      public NwDoor BlockingDoor { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Creature;

      public OnBlocked()
      {
        Creature = NWScript.OBJECT_SELF.ToNwObject<NwCreature>();
        BlockingDoor = NWScript.GetBlockingDoor().ToNwObject<NwDoor>();
      }
    }

    /// <summary>
    /// Called at the end of the creature's combat round.
    /// </summary>
    [NativeEvent(EventScriptType.CreatureOnEndCombatRound)]
    public sealed class OnCombatRoundEnd : IEvent
    {
      /// <summary>
      /// Gets the creature whose combat round is ending.
      /// </summary>
      public NwCreature Creature { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Creature;

      public OnCombatRoundEnd()
      {
        Creature = NWScript.OBJECT_SELF.ToNwObject<NwCreature>();
      }
    }

    [NativeEvent(EventScriptType.CreatureOnDialogue)]
    public sealed class OnConversation : IEvent
    {
      /// <summary>
      /// Gets the creature/object currently speaking.
      /// </summary>
      public NwCreature CurrentSpeaker { get; }

      /// <summary>
      /// Gets the player speaker in this conversation.
      /// </summary>
      public NwPlayer PlayerSpeaker { get; }

      /// <summary>
      /// Gets the last creature/object that spoke in this conversation.
      /// </summary>
      public NwGameObject LastSpeaker { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => CurrentSpeaker;

      public OnConversation()
      {
        CurrentSpeaker = NWScript.OBJECT_SELF.ToNwObject<NwCreature>();
        PlayerSpeaker = NWScript.GetPCSpeaker().ToNwObject<NwPlayer>();
        LastSpeaker = NWScript.GetLastSpeaker().ToNwObject<NwGameObject>();
      }

      public void PauseConversation()
        => NWScript.ActionPauseConversation();

      public void ResumeConversation()
        => NWScript.ActionResumeConversation();
    }

    [NativeEvent(EventScriptType.CreatureOnDamaged)]
    public sealed class OnDamaged : IEvent
    {
      public NwCreature Creature { get; }

      public NwGameObject Damager { get; }

      public int DamageAmount { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Creature;

      public OnDamaged()
      {
        Creature = NWScript.OBJECT_SELF.ToNwObject<NwCreature>();
        Damager = NWScript.GetLastDamager().ToNwObject<NwGameObject>();
        DamageAmount = NWScript.GetTotalDamageDealt();
      }
    }

    [NativeEvent(EventScriptType.CreatureOnDeath)]
    public sealed class OnDeath : IEvent
    {
      public NwCreature KilledCreature { get; }

      public NwGameObject Killer { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => KilledCreature;

      public OnDeath()
      {
        KilledCreature = NWScript.OBJECT_SELF.ToNwObject<NwCreature>();
        Killer = NWScript.GetLastKiller().ToNwObject<NwGameObject>();
      }
    }

    [NativeEvent(EventScriptType.CreatureOnDisturbed)]
    public sealed class OnDisturbed : IEvent
    {
      public InventoryDisturbType DisturbType { get; }

      public NwCreature CreatureDisturbed { get; }

      public NwCreature Disturber { get; }

      public NwItem DisturbedItem { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => CreatureDisturbed;

      public OnDisturbed()
      {
        CreatureDisturbed = NWScript.OBJECT_SELF.ToNwObject<NwCreature>();
        DisturbType = (InventoryDisturbType) NWScript.GetInventoryDisturbType();
        Disturber = NWScript.GetLastDisturbed().ToNwObject<NwCreature>();
        DisturbedItem = NWScript.GetInventoryDisturbItem().ToNwObject<NwItem>();
      }
    }

    [NativeEvent(EventScriptType.CreatureOnHeartbeat)]
    public sealed class OnHeartbeat : IEvent
    {
      public NwCreature Creature { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Creature;

      public OnHeartbeat()
      {
        Creature = NWScript.OBJECT_SELF.ToNwObject<NwCreature>();
      }
    }

    [NativeEvent(EventScriptType.CreatureOnNotice)]
    public sealed class OnPerception : IEvent
    {
      public NwCreature Creature { get; }

      public PerceptionEventType PerceptionEventType { get; }

      public NwCreature PerceivedCreature { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Creature;

      public OnPerception()
      {
        Creature = NWScript.OBJECT_SELF.ToNwObject<NwCreature>();
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

    [NativeEvent(EventScriptType.CreatureOnMeleeAttacked)]
    public sealed class OnPhysicalAttacked : IEvent
    {
      public NwCreature Creature { get; }

      public NwCreature Attacker { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Creature;

      public OnPhysicalAttacked()
      {
        Creature = NWScript.OBJECT_SELF.ToNwObject<NwCreature>();
        Attacker = NWScript.GetLastAttacker().ToNwObject<NwCreature>();
      }
    }

    [NativeEvent(EventScriptType.CreatureOnRested)]
    public sealed class OnRested : IEvent
    {
      public NwCreature Creature { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Creature;

      public OnRested()
      {
        Creature = NWScript.OBJECT_SELF.ToNwObject<NwCreature>();
      }
    }

    [NativeEvent(EventScriptType.CreatureOnSpawnIn)]
    public sealed class OnSpawn : IEvent
    {
      public NwCreature Creature { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Creature;

      public OnSpawn()
      {
        Creature = NWScript.OBJECT_SELF.ToNwObject<NwCreature>();
      }
    }

    [NativeEvent(EventScriptType.CreatureOnSpellCastAt)]
    public sealed class OnSpellCastAt : IEvent
    {
      /// <summary>
      /// Gets the creature targeted by this spell.
      /// </summary>
      public NwCreature Creature { get; }

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

      NwObject IEvent.Context => Creature;

      public OnSpellCastAt()
      {
        Creature = NWScript.OBJECT_SELF.ToNwObject<NwCreature>();
        Caster = NWScript.GetLastSpellCaster().ToNwObject<NwGameObject>();
        Spell = (Spell)NWScript.GetLastSpell();
        Harmful = NWScript.GetLastSpellHarmful().ToBool();
      }
    }

    [NativeEvent(EventScriptType.CreatureOnUserDefinedEvent)]
    public sealed class OnUserDefined : IEvent
    {
      public int EventNumber { get; }

      public NwCreature Creature { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Creature;

      public OnUserDefined()
      {
        Creature = NWScript.OBJECT_SELF.ToNwObject<NwCreature>();
        EventNumber = NWScript.GetUserDefinedEventNumber();
      }
    }
  }
}
