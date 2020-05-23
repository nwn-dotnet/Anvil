using System;
using NWM.API;
using NWM.API.Events;
using NWNX;

namespace NWMX.API.Events
{
  [NWNXDamageEvent]
  public sealed class DamageEvent : IEvent<DamageEvent>
  {
    public NwObject Damager { get; private set; }
    public NwObject Target { get; private set; }
    public DamageEventData Data { get; set; }

    public void BroadcastEvent(NwObject objSelf)
    {
      Data = DamagePlugin.GetDamageEventData();
      Damager = Data.oDamager.ToNwObject();
      Target = objSelf;

      Callbacks?.Invoke(this);

      DamagePlugin.SetDamageEventData(Data);
    }

    public event Action<DamageEvent> Callbacks;
  }
}