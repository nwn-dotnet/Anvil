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
    [GameEvent(EventScriptType.CreatureOnBlockedByDoor)]
    public sealed class OnBlocked : IEvent
    {
      /// <summary>
      /// Gets the blocked creature.
      /// </summary>
      public NwCreature Creature { get; } = NWScript.OBJECT_SELF.ToNwObject<NwCreature>();

      /// <summary>
      /// Gets the door that is blocking the creature.
      /// </summary>
      public NwDoor BlockingDoor { get; } = NWScript.GetBlockingDoor().ToNwObject<NwDoor>();

      NwObject IEvent.Context => Creature;
    }

    /// <summary>
    /// Called at the end of the creature's combat round.
    /// </summary>
    [GameEvent(EventScriptType.CreatureOnEndCombatRound)]
    public sealed class OnCombatRoundEnd : IEvent
    {
      /// <summary>
      /// Gets the creature whose combat round is ending.
      /// </summary>
      public NwCreature Creature { get; } = NWScript.OBJECT_SELF.ToNwObject<NwCreature>();

      NwObject IEvent.Context => Creature;
    }

    [GameEvent(EventScriptType.CreatureOnDialogue)]
    public sealed class OnConversation : IEvent
    {
      /// <summary>
      /// Gets the creature/object currently speaking.
      /// </summary>
      public NwCreature CurrentSpeaker { get; } = NWScript.OBJECT_SELF.ToNwObject<NwCreature>();

      /// <summary>
      /// Gets the player speaker in this conversation.
      /// </summary>
      public NwPlayer PlayerSpeaker { get; } = NWScript.GetPCSpeaker().ToNwObject<NwPlayer>();

      /// <summary>
      /// Gets the last creature/object that spoke in this conversation.
      /// </summary>
      public NwGameObject LastSpeaker { get; } = NWScript.GetLastSpeaker().ToNwObject<NwGameObject>();

      NwObject IEvent.Context => CurrentSpeaker;

      public static void Signal(NwCreature creature)
      {
        Event nwEvent = NWScript.EventConversation();
        NWScript.SignalEvent(creature, nwEvent);
      }

      public void PauseConversation()
        => NWScript.ActionPauseConversation();

      public void ResumeConversation()
        => NWScript.ActionResumeConversation();
    }

    [GameEvent(EventScriptType.CreatureOnDamaged)]
    public sealed class OnDamaged : IEvent
    {
      public NwCreature Creature { get; } = NWScript.OBJECT_SELF.ToNwObject<NwCreature>();

      public NwGameObject Damager { get; } = NWScript.GetLastDamager().ToNwObject<NwGameObject>();

      public int DamageAmount { get; } = NWScript.GetTotalDamageDealt();

      NwObject IEvent.Context => Creature;
    }

    [GameEvent(EventScriptType.CreatureOnDeath)]
    public sealed class OnDeath : IEvent
    {
      public NwCreature KilledCreature { get; } = NWScript.OBJECT_SELF.ToNwObject<NwCreature>();

      public NwGameObject Killer { get; } = NWScript.GetLastKiller().ToNwObject<NwGameObject>();

      NwObject IEvent.Context => KilledCreature;
    }

    [GameEvent(EventScriptType.CreatureOnDisturbed)]
    public sealed class OnDisturbed : IEvent
    {
      public InventoryDisturbType DisturbType { get; } = (InventoryDisturbType) NWScript.GetInventoryDisturbType();

      public NwCreature CreatureDisturbed { get; } = NWScript.OBJECT_SELF.ToNwObject<NwCreature>();

      public NwCreature Disturber { get; } = NWScript.GetLastDisturbed().ToNwObject<NwCreature>();

      public NwItem DisturbedItem { get; } = NWScript.GetInventoryDisturbItem().ToNwObject<NwItem>();

      NwObject IEvent.Context => CreatureDisturbed;
    }

    [GameEvent(EventScriptType.CreatureOnHeartbeat)]
    public sealed class OnHeartbeat : IEvent
    {
      public NwCreature Creature { get; } = NWScript.OBJECT_SELF.ToNwObject<NwCreature>();

      NwObject IEvent.Context => Creature;
    }

    [GameEvent(EventScriptType.CreatureOnNotice)]
    public sealed class OnPerception : IEvent
    {
      public NwCreature Creature { get; } = NWScript.OBJECT_SELF.ToNwObject<NwCreature>();

      public PerceptionEventType PerceptionEventType { get; } = GetPerceptionEventType();

      public NwCreature PerceivedCreature { get; } = NWScript.GetLastPerceived().ToNwObject<NwCreature>();

      NwObject IEvent.Context => Creature;

      private static PerceptionEventType GetPerceptionEventType()
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

    [GameEvent(EventScriptType.CreatureOnMeleeAttacked)]
    public sealed class OnPhysicalAttacked : IEvent
    {
      public NwCreature Creature { get; } = NWScript.OBJECT_SELF.ToNwObject<NwCreature>();

      public NwCreature Attacker { get; } = NWScript.GetLastAttacker().ToNwObject<NwCreature>();

      NwObject IEvent.Context => Creature;
    }

    [GameEvent(EventScriptType.CreatureOnRested)]
    public sealed class OnRested : IEvent
    {
      public NwCreature Creature { get; } = NWScript.OBJECT_SELF.ToNwObject<NwCreature>();

      NwObject IEvent.Context => Creature;
    }

    [GameEvent(EventScriptType.CreatureOnSpawnIn)]
    public sealed class OnSpawn : IEvent
    {
      public NwCreature Creature { get; } = NWScript.OBJECT_SELF.ToNwObject<NwCreature>();

      NwObject IEvent.Context => Creature;
    }

    [GameEvent(EventScriptType.CreatureOnSpellCastAt)]
    public sealed class OnSpellCastAt : IEvent
    {
      /// <summary>
      /// Gets the creature targeted by this spell.
      /// </summary>
      public NwCreature Creature { get; } = NWScript.OBJECT_SELF.ToNwObject<NwCreature>();

      /// <summary>
      /// Gets the caster of this spell (creature, placeable, door). Returns null from an area of effect.
      /// </summary>
      public NwGameObject Caster { get; } = NWScript.GetLastSpellCaster().ToNwObject<NwGameObject>();

      /// <summary>
      /// Gets the spell that was cast.
      /// </summary>
      public Spell Spell { get; } = (Spell)NWScript.GetLastSpell();

      /// <summary>
      /// Gets a value indicating whether this spell is considered harmful.
      /// </summary>
      public bool Harmful { get; } = NWScript.GetLastSpellHarmful().ToBool();

      NwObject IEvent.Context => Creature;

      public static void Signal(NwObject caster, NwCreature target, Spell spell, bool harmful = true)
      {
        Event nwEvent = NWScript.EventSpellCastAt(caster, (int)spell, harmful.ToInt());
        NWScript.SignalEvent(target, nwEvent);
      }
    }

    [GameEvent(EventScriptType.CreatureOnUserDefinedEvent)]
    public sealed class OnUserDefined : IEvent
    {
      public int EventNumber { get; } = NWScript.GetUserDefinedEventNumber();

      public NwCreature Creature { get; } = NWScript.OBJECT_SELF.ToNwObject<NwCreature>();

      NwObject IEvent.Context => Creature;

      public static void Signal(NwCreature creature, int eventId)
      {
        Event nwEvent = NWScript.EventUserDefined(eventId);
        NWScript.SignalEvent(creature, nwEvent);
      }
    }
  }
}
