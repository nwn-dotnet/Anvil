using NWM.API;

namespace NWM.Core
{
  public class CreatureEvents : EventHandler
  {
    public delegate void BlockedEvent(NwCreature creature, NwDoor blockingDoor);
    public delegate void CombatRoundEndEvent(NwCreature creature);
    public delegate void ConversationEvent(NwCreature creature);
    public delegate void DamagedEvent(NwCreature creature);
    public delegate void DeathEvent(NwCreature creature);
    public delegate void DisturbedEvent(NwCreature creature);
    public delegate void HeartbeatEvent(NwCreature creature);
    public delegate void PerceptionEvent(NwCreature creature);
    public delegate void PhysicalAttackedEvent(NwCreature creature);
    public delegate void RestedEvent(NwCreature creature);
    public delegate void SpawnEvent(NwCreature creature);
    public delegate void SpellCastAtEvent(NwCreature creature);
    public delegate void UserDefinedEvent(NwCreature creature, int eventId);

    internal override bool HandleScriptEvent(string scriptName, NwObject objSelf)
    {
      switch (scriptName)
      {

      }

      return false;
    }
  }
}