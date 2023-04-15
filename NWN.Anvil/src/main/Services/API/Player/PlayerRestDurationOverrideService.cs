using System;
using System.Collections.Generic;
using Anvil.API;
using Anvil.Native;
using NLog;
using NWN.Native.API;

namespace Anvil.Services
{
  [ServiceBinding(typeof(PlayerRestDurationOverrideService))]
  [ServiceBindingOptions(InternalBindingPriority.API, Lazy = true)]
  internal sealed unsafe class PlayerRestDurationOverrideService
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private static readonly CExoString DurationTableKey = "Duration".ToExoString();

    private readonly FunctionHook<Functions.CNWSCreature.AIActionRest> aiActionRestHook;
    private readonly Dictionary<NwCreature, int> restDurationOverrides = new Dictionary<NwCreature, int>();

    public PlayerRestDurationOverrideService(HookService hookService)
    {
      Log.Info($"Initialising optional service {nameof(PlayerRestDurationOverrideService)}");
      aiActionRestHook = hookService.RequestHook<Functions.CNWSCreature.AIActionRest>(OnAIActionRest, HookOrder.Late);
    }

    public void ClearDurationOverride(NwCreature creature)
    {
      restDurationOverrides.Remove(creature);
    }

    public TimeSpan? GetDurationOverride(NwCreature creature)
    {
      return restDurationOverrides.TryGetValue(creature, out int retVal) ? TimeSpan.FromMilliseconds(retVal) : null;
    }

    public void SetDurationOverride(NwCreature creature, TimeSpan duration)
    {
      restDurationOverrides[creature] = (int)Math.Round(duration.TotalMilliseconds);
    }

    private uint OnAIActionRest(void* pCreature, void* pNode)
    {
      CNWSCreature creature = CNWSCreature.FromPointer(pCreature);
      NwCreature? nwCreature = creature.ToNwObject<NwCreature>();

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
