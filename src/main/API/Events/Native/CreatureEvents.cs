using NWM.API.Constants;
using NWN;

namespace NWM.API
{
  public enum CreatureEventType
  {
    [DefaultScriptSuffix("blo")] Blocked,
    [DefaultScriptSuffix("com")] CombatRoundEnd,
    [DefaultScriptSuffix("con")] Conversation,
    [DefaultScriptSuffix("dam")] Damaged,
    [DefaultScriptSuffix("dea")] Death,
    [DefaultScriptSuffix("dis")] Disturbed,
    [DefaultScriptSuffix("hea")] Heartbeat,
    [DefaultScriptSuffix("per")] Perception,
    [DefaultScriptSuffix("phy")] PhysicalAttacked,
    [DefaultScriptSuffix("res")] Rested,
    [DefaultScriptSuffix("spa")] Spawn,
    [DefaultScriptSuffix("spe")] SpellCastAt,
    [DefaultScriptSuffix("use")] UserDefined
  }

  public class CreatureEvents : NativeEventHandler<CreatureEventType>
  {
    public event BlockedEvent OnBlocked;
    public event CombatRoundEndEvent OnCombatRoundEnd;
    public event ConversationEvent OnConversation;
    public event DamagedEvent OnDamaged;
    public event DeathEvent OnDeath;
    public event DisturbedEvent OnDisturbed;
    public event HeartbeatEvent OnHeartbeat;
    public event PerceptionEvent OnPerception;
    public event PhysicalAttackedEvent OnPhysicalAttacked;
    public event RestedEvent OnRested;
    public event SpawnEvent OnSpawn;
    public event SpellCastAtEvent OnSpellCastAt;
    public event UserDefinedEvent OnUserDefined;

    public delegate void BlockedEvent(NwCreature creature, NwDoor blockingDoor);

    public delegate void CombatRoundEndEvent(NwCreature creature);

    public delegate void ConversationEvent(NwCreature creature);

    public delegate void DamagedEvent(NwCreature creature, NwObject damager, int damage);

    public delegate void DeathEvent(NwCreature creature, NwObject killer);

    public delegate void DisturbedEvent(NwCreature creature, InventoryDisturbType disturbType, NwCreature disturber, NwItem disturbedItem);

    public delegate void HeartbeatEvent(NwCreature creature);

    public delegate void PerceptionEvent(NwCreature creature, PerceptionEventType perceptionType, NwCreature perceived);

    public delegate void PhysicalAttackedEvent(NwCreature creature, NwCreature attacker);

    public delegate void RestedEvent(NwCreature creature);

    public delegate void SpawnEvent(NwCreature creature);

    public delegate void SpellCastAtEvent(NwCreature creature);

    public delegate void UserDefinedEvent(NwCreature creature, int eventId);

    protected override void HandleEvent(CreatureEventType eventType, NwObject objSelf)
    {
      NwCreature creatureSelf = (NwCreature) objSelf;

      switch (eventType)
      {
        case CreatureEventType.Blocked:
        {
          OnBlocked?.Invoke(creatureSelf, NWScript.GetBlockingDoor().ToNwObject<NwDoor>());
          break;
        }
        case CreatureEventType.CombatRoundEnd:
        {
          OnCombatRoundEnd?.Invoke(creatureSelf);
          break;
        }
        case CreatureEventType.Conversation:
        {
          OnConversation?.Invoke(creatureSelf);
          break;
        }
        case CreatureEventType.Damaged:
        {
          OnDamaged?.Invoke(creatureSelf, NWScript.GetLastDamager().ToNwObject(), NWScript.GetTotalDamageDealt());
          break;
        }
        case CreatureEventType.Death:
        {
          OnDeath?.Invoke(creatureSelf, NWScript.GetLastKiller().ToNwObject());
          break;
        }
        case CreatureEventType.Disturbed:
        {
          OnDisturbed?.Invoke(creatureSelf,
            (InventoryDisturbType) NWScript.GetInventoryDisturbType(),
            NWScript.GetLastDisturbed().ToNwObject<NwCreature>(),
            NWScript.GetInventoryDisturbItem().ToNwObject<NwItem>());
          break;
        }
        case CreatureEventType.Heartbeat:
        {
          OnHeartbeat?.Invoke(creatureSelf);
          break;
        }
        case CreatureEventType.Perception:
        {
          OnPerception?.Invoke(creatureSelf,
            GetPerceptionEventType(),
            NWScript.GetLastPerceived().ToNwObject<NwCreature>());
          break;
        }
        case CreatureEventType.PhysicalAttacked:
        {
          OnPhysicalAttacked?.Invoke(creatureSelf, NWScript.GetLastAttacker().ToNwObject<NwCreature>());
          break;
        }
        case CreatureEventType.Rested:
        {
          OnRested?.Invoke(creatureSelf);
          break;
        }
        case CreatureEventType.Spawn:
        {
          OnSpawn?.Invoke(creatureSelf);
          break;
        }
        case CreatureEventType.SpellCastAt:
        {
          OnSpellCastAt?.Invoke(creatureSelf);
          break;
        }
        case CreatureEventType.UserDefined:
        {
          OnUserDefined?.Invoke(creatureSelf, NWScript.GetUserDefinedEventNumber());
          break;
        }
      }
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
}