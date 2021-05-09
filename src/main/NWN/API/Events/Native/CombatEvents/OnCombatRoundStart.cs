using System.Runtime.InteropServices;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public sealed class OnCombatRoundStart : IEvent
  {
    public NwCreature Creature { get; private init; }

    public NwGameObject Target { get; private init; }

    NwObject IEvent.Context => Creature;

    internal sealed unsafe class Factory : NativeEventFactory<Factory.StartCombatRoundHook>
    {
      internal delegate void StartCombatRoundHook(void* pCombatRound, uint oidTarget);

      protected override FunctionHook<StartCombatRoundHook> RequestHook()
      {
        delegate* unmanaged<void*, uint, void> pHook = &OnStartCombatRound;
        return HookService.RequestHook<StartCombatRoundHook>(pHook, NWNXLib.Functions._ZN15CNWSCombatRound16StartCombatRoundEj, HookOrder.Earliest);
      }

      [UnmanagedCallersOnly]
      private static void OnStartCombatRound(void* pCombatRound, uint oidTarget)
      {
        CNWSCombatRound combatRound = new CNWSCombatRound(pCombatRound, false);

        ProcessEvent(new OnCombatRoundStart
        {
          Creature = combatRound.m_pBaseCreature.m_idSelf.ToNwObject<NwCreature>(),
          Target = oidTarget.ToNwObject<NwGameObject>()
        });

        Hook.CallOriginal(pCombatRound, oidTarget);
      }
    }
  }
}
