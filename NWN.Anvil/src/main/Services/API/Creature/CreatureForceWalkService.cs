using System.Linq;
using Anvil.API;
using NLog;
using NWN.Native.API;

namespace Anvil.Services
{
  [ServiceBinding(typeof(CreatureForceWalkService))]
  [ServiceBindingOptions(InternalBindingPriority.API, Lazy = true)]
  internal sealed unsafe class CreatureForceWalkService
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private readonly FunctionHook<RemoveLimitMovementSpeedHook> removeLimitMovementSpeedHook;

    public CreatureForceWalkService(HookService hookService)
    {
      Log.Info($"Initialising optional service {nameof(CreatureForceWalkService)}");
      removeLimitMovementSpeedHook = hookService.RequestHook<RemoveLimitMovementSpeedHook>(OnRemoveLimitMovementSpeed, HookOrder.Late);
    }

    [NativeFunction("_ZN21CNWSEffectListHandler26OnRemoveLimitMovementSpeedEP10CNWSObjectP11CGameEffect", "")]
    private delegate int RemoveLimitMovementSpeedHook(void* pEffectListHandler, void* pObject, void* pEffect);

    public bool GetAlwaysWalk(NwCreature creature)
    {
      return InternalVariables.AlwaysWalk(creature);
    }

    public void SetAlwaysWalk(NwCreature creature, bool forceWalk)
    {
      CNWSCreature nativeCreature = creature.Creature;
      InternalVariableBool alwaysWalk = InternalVariables.AlwaysWalk(creature);

      if (forceWalk)
      {
        nativeCreature.m_bForcedWalk = true.ToInt();
        alwaysWalk.Value = true;
      }
      else
      {
        alwaysWalk.Delete();

        if (creature.ActiveEffects.Any(activeEffect => activeEffect.EffectType == EffectType.MovementSpeedDecrease))
        {
          return;
        }

        if (!nativeCreature.m_bForcedWalk.ToBool())
        {
          nativeCreature.UpdateEncumbranceState(false.ToInt());
          nativeCreature.m_bForcedWalk = (nativeCreature.m_nEncumbranceState != 0).ToInt();
        }

        nativeCreature.m_bForcedWalk = false.ToInt();
      }
    }

    private int OnRemoveLimitMovementSpeed(void* pEffectListHandler, void* pObject, void* pEffect)
    {
      NwObject? nwObject = CNWSObject.FromPointer(pObject).ToNwObject();
      if (nwObject != null && InternalVariables.AlwaysWalk(nwObject))
      {
        return 1;
      }

      return removeLimitMovementSpeedHook.CallOriginal(pEffectListHandler, pObject, pEffect);
    }
  }
}
