using System;
using NWM.API;
using NWM.API.Events;
using NWNX;

namespace NWMX.API.Events
{
  [NWNXDamageEvent]
  public sealed class DamageEvent : IEvent<DamageEvent>
  {
    public DamageEventData Data;

    public void BroadcastEvent(NwObject objSelf)
    {
      Data = DamagePlugin.GetDamageEventData();
      Callbacks?.Invoke(this);
      DamagePlugin.SetDamageEventData(Data);
    }

    public event Action<DamageEvent> Callbacks;
  }
}