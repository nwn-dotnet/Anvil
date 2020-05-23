using System;
using NWM.API;
using NWM.API.Events;
using NWNX;

namespace NWMX.API.Events
{
  [NWNXAttackEvent]
  public sealed class AttackEvent : IEvent<AttackEvent>
  {
    public NwObject Attacker { get; private set; }
    public NwObject Target { get; private set; }
    public AttackEventData Data { get; set; }

    public void BroadcastEvent(NwObject objSelf)
    {
      Data = DamagePlugin.GetAttackEventData();
      Attacker = Data.oTarget.ToNwObject();
      Target = objSelf;

      Callbacks?.Invoke(this);

      DamagePlugin.SetAttackEventData(Data);
    }

    public event Action<AttackEvent> Callbacks;
  }
}