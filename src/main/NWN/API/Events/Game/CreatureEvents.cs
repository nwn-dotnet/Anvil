using NWN.API.Constants;
using NWN.Core;

// TODO Populate event data.
namespace NWN.API.Events
{
  /// <summary>
  /// Events for <see cref="NwCreature"/>.
  /// </summary>
  public static class CreatureEvents
  {
    /// <summary>
    /// Called when the <see cref="NwCreature"/> is blocked by a <see cref="NwDoor"/>.
    /// </summary>
    [GameEvent(EventScriptType.CreatureOnBlockedByDoor)]
    public sealed class OnBlocked : IEvent
    {
      /// <summary>
      /// Gets the blocked <see cref="NwCreature"/>.
      /// </summary>
      public NwCreature Creature { get; } = NWScript.OBJECT_SELF.ToNwObject<NwCreature>();

      /// <summary>
      /// Gets the <see cref="NwDoor"/> that is blocking the <see cref="NwCreature"/>.
      /// </summary>
      public NwDoor BlockingDoor { get; } = NWScript.GetBlockingDoor().ToNwObject<NwDoor>();

      NwObject IEvent.Context => Creature;
    }

    /// <summary>
    /// Called at the end of the <see cref="NwCreature"/> combat round.
    /// </summary>
    [GameEvent(EventScriptType.CreatureOnEndCombatRound)]
    public sealed class OnCombatRoundEnd : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwCreature"/> whose combat round is ending.
      /// </summary>
      public NwCreature Creature { get; } = NWScript.OBJECT_SELF.ToNwObject<NwCreature>();

      NwObject IEvent.Context => Creature;
    }

    [GameEvent(EventScriptType.CreatureOnDialogue)]
    public sealed class OnConversation : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwCreature"/> currently speaking.
      /// </summary>
      public NwCreature CurrentSpeaker { get; } = NWScript.OBJECT_SELF.ToNwObject<NwCreature>();

      /// <summary>
      /// Gets the <see cref="NwPlayer"/> speaker in this conversation.
      /// </summary>
      public NwPlayer PlayerSpeaker { get; } = NWScript.GetPCSpeaker().ToNwObject<NwPlayer>();

      /// <summary>
      /// Gets the last <see cref="NwGameObject"/> that spoke in this conversation.
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

    /// <summary>
    /// Called by <see cref="NwCreature"/> when taken damage from <see cref="NwGameObject"/>.
    /// </summary>
    [GameEvent(EventScriptType.CreatureOnDamaged)]
    public sealed class OnDamaged : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwCreature"/> that is taking damage.
      /// </summary>   
      public NwCreature Creature { get; } = NWScript.OBJECT_SELF.ToNwObject<NwCreature>();

      /// <summary>
      /// Gets the <see cref="NwGameObject"/> that damaged <see cref="NwCreature"/>.
      /// </summary>
      public NwGameObject Damager { get; } = NWScript.GetLastDamager().ToNwObject<NwGameObject>();

      /// <summary>
      /// Gets the amount of damage done by <see cref="NwGameObject"/> to <see cref="NwCreature"/>.
      /// </summary>
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
