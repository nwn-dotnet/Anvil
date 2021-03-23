using NWN.API.Events;
using NWN.Core.NWNX;
using NWN.Services;
using NWNX.API.Events;

namespace NWNX.Services
{
  [ServiceBinding(typeof(IEventFactory))]
  [ServiceBinding(typeof(IScriptDispatcher))]
  public class NWNXDamageEventFactory : IEventFactory, IScriptDispatcher
  {
    private const string AttackScriptName = "__nwnxatk_event";
    private const string DamageScriptName = "__nwnxdmg_event";

    private EventService eventService;

    void IEventFactory.Init(EventService eventService)
    {
      this.eventService = eventService;
      DamagePlugin.SetAttackEventScript(AttackScriptName);
      DamagePlugin.SetDamageEventScript(DamageScriptName);
    }

    void IEventFactory.Unregister<TEvent>() {}

    ScriptHandleResult IScriptDispatcher.ExecuteScript(string scriptName, uint oidSelf)
    {
      if (eventService == null)
      {
        return ScriptHandleResult.NotHandled;
      }

      switch (scriptName)
      {
        case AttackScriptName:
          AttackEvent attackEvent = eventService.ProcessEvent(new AttackEvent());
          DamagePlugin.SetAttackEventData(attackEvent.AttackData.ToNative(attackEvent.Target, attackEvent.DamageData));
          return ScriptHandleResult.Handled;
        case DamageScriptName:
          DamageEvent damageEvent = eventService.ProcessEvent(new DamageEvent());
          DamagePlugin.SetDamageEventData(damageEvent.DamageData.ToNative(damageEvent.Attacker));
          return ScriptHandleResult.Handled;
        default:
          return ScriptHandleResult.NotHandled;
      }
    }
  }
}
