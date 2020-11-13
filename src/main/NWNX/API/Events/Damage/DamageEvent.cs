using NWN.API;
using NWN.Core.NWNX;

namespace NWNX.API.Events
{
  [NWNXDamageEvent]
  public sealed class DamageEvent : NWNXEvent<DamageEvent>
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

    protected override void ProcessEvent()
    {
      InvokeCallbacks();
      DamagePlugin.SetDamageEventData(DamageData.ToNative(Attacker));
    }
  }
}
