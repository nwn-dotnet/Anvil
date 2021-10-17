using System;
using System.Collections.Generic;
using Anvil.API;
using NWN.Native.API;

namespace Anvil.Services
{
  [ServiceBinding(typeof(RestDurationOverrideService))]
  [ServiceBindingOptions(BindingOrder.API)]
  internal sealed unsafe class RestDurationOverrideService
  {
    private static readonly CExoString DurationTableKey = "Duration".ToExoString();

    private delegate uint AIActionRestHook(void* pCreature, void* pNode);

    private readonly FunctionHook<AIActionRestHook> aiActionRestHook;
    private readonly Dictionary<NwCreature, int> restDurationOverrides = new Dictionary<NwCreature, int>();

    public RestDurationOverrideService(HookService hookService)
    {
      aiActionRestHook = hookService.RequestHook<AIActionRestHook>(OnAIActionRest, FunctionsLinux._ZN12CNWSCreature12AIActionRestEP20CNWSObjectActionNode, HookOrder.Late);
    }

    public TimeSpan? GetDurationOverride(NwCreature creature)
    {
      return restDurationOverrides.TryGetValue(creature, out int retVal) ? TimeSpan.FromMilliseconds(retVal) : null;
    }

    public void SetDurationOverride(NwCreature creature, TimeSpan duration)
    {
      restDurationOverrides[creature] = (int)Math.Round(duration.TotalMilliseconds);
    }

    public void ClearDurationOverride(NwCreature creature)
    {
      restDurationOverrides.Remove(creature);
    }

    private uint OnAIActionRest(void* pCreature, void* pNode)
    {
      CNWSCreature creature = CNWSCreature.FromPointer(pCreature);
      NwCreature nwCreature = creature.ToNwObject<NwCreature>();

      if (nwCreature != null && restDurationOverrides.TryGetValue(nwCreature, out int durationOverride))
      {
        byte creatureLevel = creature.m_pStats.GetLevel(0);
        int originalValue;

        C2DA durationTable = NWNXLib.Rules().m_p2DArrays.m_pRestDurationTable;

        durationTable.GetINTEntry(creatureLevel, DurationTableKey, &originalValue);
        durationTable.SetINTEntry(creatureLevel, DurationTableKey, durationOverride);
        uint retVal = aiActionRestHook.CallOriginal(pCreature, pNode);
        durationTable.SetINTEntry(creatureLevel, DurationTableKey, originalValue);

        return retVal;
      }

      return aiActionRestHook.CallOriginal(pCreature, pNode);
    }
  }
}
