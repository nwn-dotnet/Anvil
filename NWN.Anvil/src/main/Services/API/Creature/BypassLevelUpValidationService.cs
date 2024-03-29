using Anvil.API;
using Anvil.Native;
using NWN.Native.API;

namespace Anvil.Services
{
  [ServiceBinding(typeof(BypassLevelUpValidationService))]
  [ServiceBindingOptions(InternalBindingPriority.API, Lazy = true)]
  internal sealed unsafe class BypassLevelUpValidationService
  {
    private readonly FunctionHook<Functions.CNWSCreatureStats.CanLevelUp> canLevelUpHook;
    private readonly FunctionHook<Functions.CNWSCreatureStats.ValidateLevelUp> validateLevelUpHook;

    public bool DisableValidation { get; set; }

    public BypassLevelUpValidationService(HookService hookService)
    {
      canLevelUpHook = hookService.RequestHook<Functions.CNWSCreatureStats.CanLevelUp>(OnCanLevelUp, HookOrder.Late);
      validateLevelUpHook = hookService.RequestHook<Functions.CNWSCreatureStats.ValidateLevelUp>(OnValidateLevelUp, HookOrder.Late);
    }

    private uint OnValidateLevelUp(void* pCreatureStats, void* pLevelUpStats, byte nDomain1, byte nDomain2, byte nSchool)
    {
      CNWSCreatureStats creatureStats = CNWSCreatureStats.FromPointer(pCreatureStats);
      CNWLevelStats levelStats = CNWLevelStats.FromPointer(pLevelUpStats);

      if (!DisableValidation || creatureStats.m_bIsPC.ToBool())
      {
        return validateLevelUpHook.CallOriginal(pCreatureStats, pLevelUpStats, nDomain1, nDomain2, nSchool);
      }

      creatureStats.LevelUp(levelStats, nDomain1, nDomain2, nSchool, true.ToInt());
      creatureStats.UpdateCombatInformation();
      return 0;
    }

    private int OnCanLevelUp(void* pCreatureStats)
    {
      CNWSCreatureStats creatureStats = CNWSCreatureStats.FromPointer(pCreatureStats);

      if (!DisableValidation || creatureStats.m_bIsPC.ToBool())
      {
        return canLevelUpHook.CallOriginal(pCreatureStats);
      }

      return (creatureStats.GetLevel(false.ToInt()) < 60).ToInt();
    }
  }
}
