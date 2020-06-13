using System;
using NWM.API;
using NWM.API.Events;
using NWN.Core.NWNX;

namespace NWMX.API.Events
{
  [NWNXAttackEvent]
  public sealed class AttackEvent : IEvent<AttackEvent>
  {
    public NwObject Attacker { get; private set; }
    public NwObject Target { get; private set; }

    public AttackData AttackData { get; set; }
    public DamageData DamageData { get; set; }

    public void BroadcastEvent(NwObject objSelf)
    {
      AttackEventData attackEventData = DamagePlugin.GetAttackEventData();
      AttackData = AttackData.FromNative(attackEventData);
      DamageData = DamageData.FromNative(attackEventData);

      Attacker = objSelf;
      Target = attackEventData.oTarget.ToNwObject();

      Callbacks?.Invoke(this);

      DamagePlugin.SetAttackEventData(AttackData.ToNative(Target, DamageData));
    }

    public event Action<AttackEvent> Callbacks;
  }
}