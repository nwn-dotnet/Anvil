using System;
using NWN.API;
using NWN.API.Events;
using NWN.Core.NWNX;

namespace NWNX.API.Events
{
  [NWNXDamageEvent]
  public sealed class DamageEvent : IEvent<DamageEvent>
  {
    public NwObject Attacker { get; private set; }
    public NwObject Target { get; private set; }
    public DamageData DamageData { get; set; }

    public void BroadcastEvent(NwObject objSelf)
    {
      DamageEventData eventData = DamagePlugin.GetDamageEventData();
      DamageData = DamageData.FromNative(eventData);
      Attacker = eventData.oDamager.ToNwObject();
      Target = objSelf;

      Callbacks?.Invoke(this);

      DamagePlugin.SetDamageEventData(DamageData.ToNative(Attacker));
    }

    public event Action<DamageEvent> Callbacks;
  }
}