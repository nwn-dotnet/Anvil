using System;
using NWN.API;
using NWN.API.Events;
using NWN.Core;
using NWN.Core.NWNX;

namespace NWNX.API.Events
{
  [Obsolete("Use NwModule/NwCreature.OnCreatureAttack instead.")]
  public sealed class AttackEvent : IEvent
  {
    public NwObject Attacker { get; }

    public NwObject Target { get; }

    public AttackData AttackData { get; set; }

    public DamageData DamageData { get; set; }

    public AttackEvent()
    {
      AttackEventData attackEventData = DamagePlugin.GetAttackEventData();
      AttackData = AttackData.FromNative(attackEventData);
      DamageData = DamageData.FromNative(attackEventData);

      Attacker = NWScript.OBJECT_SELF.ToNwObject();
      Target = attackEventData.oTarget.ToNwObject();
    }

    NwObject IEvent.Context => Attacker;
  }
}
