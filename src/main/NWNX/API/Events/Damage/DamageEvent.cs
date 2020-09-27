using NWN.API;
using NWN.API.Events;
using NWN.Core.NWNX;

namespace NWNX.API.Events
{
  [NWNXDamageEvent]
  public sealed class DamageEvent : Event<DamageEvent>
  {
    public NwGameObject Attacker { get; private set; }

    public NwGameObject Target { get; private set; }

    public DamageData DamageData { get; set; }

    protected override void PrepareEvent(NwObject objSelf)
    {
      DamageEventData eventData = DamagePlugin.GetDamageEventData();
      DamageData = DamageData.FromNative(eventData);
      Attacker = eventData.oDamager.ToNwObject<NwGameObject>();
      Target = (NwGameObject) objSelf;
    }

    protected override void InvokeCallbacks()
    {
      base.InvokeCallbacks();
      DamagePlugin.SetDamageEventData(DamageData.ToNative(Attacker));
    }
  }
}
