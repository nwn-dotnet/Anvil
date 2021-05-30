using System.Runtime.InteropServices;
using NWN.Native.API;
using NWN.Services;
using CombatMode = NWN.API.Constants.CombatMode;

namespace NWN.API.Events
{
  public sealed class OnCombatModeToggle : IEvent
  {
    public NwCreature Creature { get; private init; }

    public CombatMode NewMode { get; set; }

    public bool ForceNewMode { get; private init; }

    public ForceNewModeOverride ForceNewModeOverride { get; set; }

    public bool PreventToggle { get; set; }

    NwObject IEvent.Context
    {
      get => Creature;
    }

    internal sealed unsafe class Factory : SingleHookEventFactory<Factory.SetCombatModeHook>
    {
      internal delegate void SetCombatModeHook(void* pCreature, byte nNewMode, int bForceNewMode);

      protected override FunctionHook<SetCombatModeHook> RequestHook()
      {
        delegate* unmanaged<void*, byte, int, void> pHook = &OnSetCombatMode;
        return HookService.RequestHook<SetCombatModeHook>(pHook, FunctionsLinux._ZN12CNWSCreature13SetCombatModeEhi, HookOrder.Early);
      }

      [UnmanagedCallersOnly]
      private static void OnSetCombatMode(void* pCreature, byte nNewMode, int bForceNewMode)
      {
        CNWSCreature creature = CNWSCreature.FromPointer(pCreature);
        if (creature == null)
        {
          Hook.CallOriginal(pCreature, nNewMode, bForceNewMode);
          return;
        }

        OnCombatModeToggle eventData = ProcessEvent(new OnCombatModeToggle
        {
          Creature = creature.ToNwObject<NwCreature>(),
          NewMode = (CombatMode)nNewMode,
          ForceNewMode = bForceNewMode.ToBool(),
        });

        bForceNewMode = eventData.ForceNewModeOverride switch
        {
          ForceNewModeOverride.Force => true.ToInt(),
          ForceNewModeOverride.DontForce => false.ToInt(),
          _ => bForceNewMode,
        };

        if (!eventData.PreventToggle)
        {
          Hook.CallOriginal(pCreature, (byte)eventData.NewMode, bForceNewMode);
        }
      }
    }
  }
}
