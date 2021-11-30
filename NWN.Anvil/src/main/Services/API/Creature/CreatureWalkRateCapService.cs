using Anvil.API;
using NWN.Native.API;

namespace Anvil.Services
{
  [ServiceBinding(typeof(CreatureWalkRateCapService))]
  [ServiceBindingOptions(InternalBindingPriority.API)]
  internal sealed unsafe class CreatureWalkRateCapService
  {
    private const string WalkCapVariable = "WALK_RATE_CAP";

    private readonly FunctionHook<GetWalkRateHook> walkRateHook;

    public CreatureWalkRateCapService(HookService hookService)
    {
      walkRateHook = hookService.RequestHook<GetWalkRateHook>(OnGetWalkRate, FunctionsLinux._ZN12CNWSCreature11GetWalkRateEv, HookOrder.Late);
    }

    private delegate float GetWalkRateHook(void* pCreature);

    public float? GetWalkRateCap(NwCreature creature)
    {
      PersistentVariableFloat.Internal overrideValue = creature.GetObjectVariable<PersistentVariableFloat.Internal>(WalkCapVariable);
      return overrideValue.HasValue ? overrideValue.Value : null;
    }

    public void SetWalkRateCap(NwCreature creature, float? newValue)
    {
      PersistentVariableFloat.Internal overrideValue = creature.GetObjectVariable<PersistentVariableFloat.Internal>(WalkCapVariable);
      if (newValue.HasValue)
      {
        overrideValue.Value = newValue.Value;
      }
      else
      {
        overrideValue.Delete();
      }
    }

    private float OnGetWalkRate(void* pCreature)
    {
      NwCreature creature = CNWSCreature.FromPointer(pCreature).ToNwObject<NwCreature>();
      if (creature == null)
      {
        return walkRateHook.CallOriginal(pCreature);
      }

      PersistentVariableFloat rateCap = creature.GetObjectVariable<PersistentVariableFloat>(WalkCapVariable);
      return rateCap.HasValue ? rateCap.Value : walkRateHook.CallOriginal(pCreature);
    }
  }
}
