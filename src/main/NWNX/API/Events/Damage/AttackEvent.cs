using NWN.API;
using NWN.API.Events;
using NWN.Core.NWNX;

namespace NWNX.API.Events
{
  [NWNXAttackEvent]
  public sealed class AttackEvent : Event<AttackEvent>
  {
    public NwObject Attacker { get; private set; }

    public NwObject Target { get; private set; }

    public AttackData AttackData { get; set; }

    public DamageData DamageData { get; set; }

    protected override void PrepareEvent(NwObject objSelf)
    {
      AttackEventData attackEventData = DamagePlugin.GetAttackEventData();
      AttackData = AttackData.FromNative(attackEventData);
      DamageData = DamageData.FromNative(attackEventData);

      Attacker = objSelf;
      Target = attackEventData.oTarget.ToNwObject();
    }

    protected override void InvokeCallbacks()
    {
      base.InvokeCallbacks();
      DamagePlugin.SetAttackEventData(AttackData.ToNative(Target, DamageData));
    }
  }
}
