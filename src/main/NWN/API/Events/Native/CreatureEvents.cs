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
    public sealed class OnBlocked : NativeEvent<NwCreature, OnBlocked>
    {
      /// <summary>
      /// Gets the blocked creature.
      /// </summary>
      public NwCreature Creature { get; private set; }

      /// <summary>
      /// Gets the door that is blocking the creature.
      /// </summary>
      public NwDoor BlockingDoor { get; private set; }

      protected override void PrepareEvent(NwCreature objSelf)
      {
        Creature = objSelf;
        BlockingDoor = NWScript.GetBlockingDoor().ToNwObject<NwDoor>();
      }
    }

    /// <summary>
    /// Called at the end of the creature's combat round.
    /// </summary>
    [NativeEvent(EventScriptType.CreatureOnEndCombatRound)]
    public sealed class OnCombatRoundEnd : NativeEvent<NwCreature, OnCombatRoundEnd>
    {
      /// <summary>
      /// Gets the creature whose combat round is ending.
      /// </summary>
      public NwCreature Creature { get; private set; }

      protected override void PrepareEvent(NwCreature objSelf)
      {
        Creature = objSelf;
      }
    }

    [NativeEvent(EventScriptType.CreatureOnDialogue)]
    public sealed class OnConversation : NativeEvent<NwCreature, OnConversation>
    {
      /// <summary>
      /// Gets the creature/object currently speaking.
      /// </summary>
      public NwCreature CurrentSpeaker { get; private set; }

      /// <summary>
      /// Gets the player speaker in this conversation.
      /// </summary>
      public NwPlayer PlayerSpeaker { get; private set; }

      /// <summary>
      /// Gets the last creature/object that spoke in this conversation.
      /// </summary>
      public NwGameObject LastSpeaker { get; private set; }

      protected override void PrepareEvent(NwCreature objSelf)
      {
        CurrentSpeaker = objSelf;
        PlayerSpeaker = NWScript.GetPCSpeaker().ToNwObject<NwPlayer>();
        LastSpeaker = NWScript.GetLastSpeaker().ToNwObject<NwGameObject>();
      }

      public static void PauseConversation()
        => NWScript.ActionPauseConversation();

      public static void ResumeConversation()
        => NWScript.ActionResumeConversation();
    }

    [NativeEvent(EventScriptType.CreatureOnDamaged)]
    public sealed class OnDamaged : NativeEvent<NwCreature, OnDamaged>
    {
      public NwGameObject Damager { get; private set; }

      public int DamageAmount { get; private set; }

      protected override void PrepareEvent(NwCreature objSelf)
      {
        Damager = NWScript.GetLastDamager().ToNwObject<NwGameObject>();
        DamageAmount = NWScript.GetTotalDamageDealt();
      }
    }

    [NativeEvent(EventScriptType.CreatureOnDeath)]
    public sealed class OnDeath : NativeEvent<NwCreature, OnDeath>
    {
      public NwCreature KilledCreature { get; private set; }

      public NwGameObject Killer { get; private set; }

      protected override void PrepareEvent(NwCreature objSelf)
      {
        KilledCreature = objSelf;
        Killer = NWScript.GetLastKiller().ToNwObject<NwGameObject>();
      }
    }

    [NativeEvent(EventScriptType.CreatureOnDisturbed)]
    public sealed class OnDisturbed : NativeEvent<NwCreature, OnDisturbed>
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

    [NativeEvent(EventScriptType.CreatureOnHeartbeat)]
    public sealed class OnHeartbeat : NativeEvent<NwCreature, OnHeartbeat>
    {
      public NwCreature Creature { get; private set; }

      protected override void PrepareEvent(NwCreature objSelf)
      {
        Creature = objSelf;
      }
    }

    [NativeEvent(EventScriptType.CreatureOnNotice)]
    public sealed class OnPerception : NativeEvent<NwCreature, OnPerception>
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

    [NativeEvent(EventScriptType.CreatureOnMeleeAttacked)]
    public sealed class OnPhysicalAttacked : NativeEvent<NwCreature, OnPhysicalAttacked>
    {
      public NwCreature Creature { get; private set; }

      public NwCreature Attacker { get; private set; }

      protected override void PrepareEvent(NwCreature objSelf)
      {
        Creature = objSelf;
        Attacker = NWScript.GetLastAttacker().ToNwObject<NwCreature>();
      }
    }

    [NativeEvent(EventScriptType.CreatureOnRested)]
    public sealed class OnRested : NativeEvent<NwCreature, OnRested>
    {
      public NwCreature Creature { get; private set; }

      protected override void PrepareEvent(NwCreature objSelf)
      {
        Creature = objSelf;
      }
    }

    [NativeEvent(EventScriptType.CreatureOnSpawnIn)]
    public sealed class OnSpawn : NativeEvent<NwCreature, OnSpawn>
    {
      public NwCreature Creature { get; private set; }

      protected override void PrepareEvent(NwCreature objSelf)
      {
        Creature = objSelf;
      }
    }

    [NativeEvent(EventScriptType.CreatureOnSpellCastAt)]
    public sealed class OnSpellCastAt : NativeEvent<NwCreature, OnSpellCastAt>
    {
      public NwCreature Creature { get; private set; }

      protected override void PrepareEvent(NwCreature objSelf)
      {
        Creature = objSelf;
      }
    }

    [NativeEvent(EventScriptType.CreatureOnUserDefinedEvent)]
    public sealed class OnUserDefined : NativeEvent<NwCreature, OnUserDefined>
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
