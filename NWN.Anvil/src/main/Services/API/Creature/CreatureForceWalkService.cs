using System.Linq;
using Anvil.API;
using NWN.Native.API;

namespace Anvil.Services
{
  [ServiceBinding(typeof(CreatureForceWalkService))]
  [ServiceBindingOptions(InternalBindingPriority.API)]
  internal sealed unsafe class CreatureForceWalkService
  {
    public const string AlwaysWalkVariable = "ALWAYS_WALK";

    private readonly FunctionHook<RemoveLimitMovementSpeedHook> removeLimitMovementSpeedHook;

    public CreatureForceWalkService(HookService hookService)
    {
      removeLimitMovementSpeedHook = hookService.RequestHook<RemoveLimitMovementSpeedHook>(OnRemoveLimitMovementSpeed, FunctionsLinux._ZN21CNWSEffectListHandler26OnRemoveLimitMovementSpeedEP10CNWSObjectP11CGameEffect, HookOrder.Late);
    }

    private delegate int RemoveLimitMovementSpeedHook(void* pEffectListHandler, void* pObject, void* pEffect);

    public bool GetAlwaysWalk(NwCreature creature)
    {
      return creature.GetObjectVariable<PersistentVariableBool.Internal>(AlwaysWalkVariable);
    }

    public void SetAlwaysWalk(NwCreature creature, bool forceWalk)
    {
      CNWSCreature nativeCreature = creature.Creature;
      PersistentVariableBool.Internal alwaysWalk = creature.GetObjectVariable<PersistentVariableBool.Internal>(AlwaysWalkVariable);

      if (!forceWalk)
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

        ((CNWSCreature)creature).m_bForcedWalk = false.ToInt();
      }
    }

    private int OnRemoveLimitMovementSpeed(void* pEffectListHandler, void* pObject, void* pEffect)
    {
      NwObject nwObject = CNWSObject.FromPointer(pObject).ToNwObject();
      if (nwObject != null && nwObject.GetObjectVariable<PersistentVariableBool.Internal>(AlwaysWalkVariable))
      {
        return 1;
      }

      return removeLimitMovementSpeedHook.CallOriginal(pEffectListHandler, pObject, pEffect);
    }
  }
}
