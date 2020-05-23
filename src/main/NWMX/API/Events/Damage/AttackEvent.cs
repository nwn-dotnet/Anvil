using System;
using NWM.API;
using NWM.API.Events;
using NWNX;

namespace NWMX.API.Events
{
  [NWNXAttackEvent]
  public sealed class AttackEvent : IEvent<AttackEvent>
  {
    public AttackEventData Data;

    public void BroadcastEvent(NwObject objSelf)
    {
      Data = DamagePlugin.GetAttackEventData();
      Callbacks?.Invoke(this);
      DamagePlugin.SetAttackEventData(Data);
    }

    public event Action<AttackEvent> Callbacks;
  }
}