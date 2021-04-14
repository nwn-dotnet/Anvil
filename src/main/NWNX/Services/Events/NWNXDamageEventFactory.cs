using System;
using NWN.API.Events;
using NWN.Core.NWNX;
using NWN.Services;
using NWNX.API.Events;

namespace NWNX.Services
{
  [ServiceBinding(typeof(IEventFactory))]
  [ServiceBinding(typeof(IScriptDispatcher))]
  [Obsolete("The NWNXDamageEventFactory is obsolete and will be removed in the next release.\n" +
    "Use NwModule/NwCreature.OnCreatureAttack/Damage instead.")]
  public class NWNXDamageEventFactory : IEventFactory, IScriptDispatcher
  {
    private const string AttackScriptName = "___nwnmxatk_evt";
    private const string DamageScriptName = "___nwnmxdmg_evt";

    private readonly Lazy<EventService> eventService;

    public NWNXDamageEventFactory(Lazy<EventService> eventService)
    {
      this.eventService = eventService;
    }

    void IEventFactory.Init()
    {
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
          AttackEvent attackEvent = eventService.Value.ProcessEvent(new AttackEvent());
          DamagePlugin.SetAttackEventData(attackEvent.AttackData.ToNative(attackEvent.Target, attackEvent.DamageData));
          return ScriptHandleResult.Handled;
        case DamageScriptName:
          DamageEvent damageEvent = eventService.Value.ProcessEvent(new DamageEvent());
          DamagePlugin.SetDamageEventData(damageEvent.DamageData.ToNative(damageEvent.Attacker));
          return ScriptHandleResult.Handled;
        default:
          return ScriptHandleResult.NotHandled;
      }
    }
  }
}
