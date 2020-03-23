using NWM.API;
using NWN;

namespace NWM.Core
{
  public class CreatureEvents : EventHandler
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

    internal override bool HandleScriptEvent(string scriptName, NwObject objSelf)
    {
      NwCreature creatureSelf = (NwCreature) objSelf;

      switch (scriptName)
      {
        case "blo":
          OnBlocked?.Invoke(creatureSelf, NWScript.GetBlockingDoor().ToNwObject<NwDoor>());
          return true;
        case "com":
          OnCombatRoundEnd?.Invoke(creatureSelf);
          return true;
        case "con":
          OnConversation?.Invoke(creatureSelf);
          return true;
        case "dam":
          OnDamaged?.Invoke(creatureSelf, NWScript.GetLastDamager().ToNwObject(), NWScript.GetTotalDamageDealt());
          return true;
        case "dea":
          OnDeath?.Invoke(creatureSelf, NWScript.GetLastKiller().ToNwObject());
          return true;
        case "dis":
          OnDisturbed?.Invoke(creatureSelf,
            (InventoryDisturbType)NWScript.GetInventoryDisturbType(),
            NWScript.GetLastDisturbed().ToNwObject<NwCreature>(),
            NWScript.GetInventoryDisturbItem().ToNwObject<NwItem>());
          return true;
        case "hea":
          OnHeartbeat?.Invoke(creatureSelf);
          return true;
        case "per":
          OnPerception?.Invoke(creatureSelf,
            GetPerceptionEventType(),
            NWScript.GetLastPerceived().ToNwObject<NwCreature>());
          return true;
        case "phy":
          OnPhysicalAttacked?.Invoke(creatureSelf, NWScript.GetLastAttacker().ToNwObject<NwCreature>());
          return true;
        case "res":
          OnRested?.Invoke(creatureSelf);
          return true;
        case "spa":
          OnSpawn?.Invoke(creatureSelf);
          return true;
        case "spe":
          OnSpellCastAt?.Invoke(creatureSelf);
          return true;
        case "use":
          OnUserDefined?.Invoke(creatureSelf, NWScript.GetUserDefinedEventNumber());
          return true;
      }

      return false;
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