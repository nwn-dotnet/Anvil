using Anvil.API;
using Anvil.Native;
using NLog;
using NWN.Native.API;

namespace Anvil.Services
{
  [ServiceBinding(typeof(CreatureWalkRateCapService))]
  [ServiceBindingOptions(InternalBindingPriority.API, Lazy = true)]
  internal sealed unsafe class CreatureWalkRateCapService
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private readonly FunctionHook<Functions.CNWSCreature.GetWalkRate> walkRateHook;

    public CreatureWalkRateCapService(HookService hookService)
    {
      Log.Info($"Initialising optional service {nameof(CreatureWalkRateCapService)}");
      walkRateHook = hookService.RequestHook<Functions.CNWSCreature.GetWalkRate>(OnGetWalkRate, HookOrder.Late);
    }

    public float? GetWalkRateCap(NwCreature creature)
    {
      InternalVariableFloat overrideValue = InternalVariables.WalkRateCap(creature);
      return overrideValue.HasValue ? overrideValue.Value : null;
    }

    public void SetWalkRateCap(NwCreature creature, float? newValue)
    {
      InternalVariableFloat overrideValue = InternalVariables.WalkRateCap(creature);
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
      NwCreature? creature = CNWSCreature.FromPointer(pCreature).ToNwObject<NwCreature>();
      if (creature == null)
      {
        return walkRateHook.CallOriginal(pCreature);
      }

      InternalVariableFloat rateCap = InternalVariables.WalkRateCap(creature);
      return rateCap.HasValue ? rateCap.Value : walkRateHook.CallOriginal(pCreature);
    }
  }
}
