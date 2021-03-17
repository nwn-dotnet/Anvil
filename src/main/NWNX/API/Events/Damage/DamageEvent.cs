using NWN.API;
using NWN.API.Events;
using NWN.Core;
using NWN.Core.NWNX;

namespace NWNX.API.Events
{
  [NWNXDamageEvent]
  public sealed class DamageEvent : IEvent
  {
    public NwGameObject Attacker { get; }

    public NwGameObject Target { get; }

    public DamageData DamageData { get; set; }

    public DamageEvent()
    {
      DamageEventData eventData = DamagePlugin.GetDamageEventData();
      DamageData = DamageData.FromNative(eventData);
      Attacker = eventData.oDamager.ToNwObject<NwGameObject>();
      Target = NWScript.OBJECT_SELF.ToNwObject<NwGameObject>();
    }

    NwObject IEvent.Context => Attacker;
  }
}
