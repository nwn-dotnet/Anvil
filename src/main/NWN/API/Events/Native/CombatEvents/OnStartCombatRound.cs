using System;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public class OnStartCombatRound : IEvent
  {
    public NwCreature Creature { get; private init; }

    public NwGameObject Target { get; private init; }

    NwObject IEvent.Context => Creature;

    [NativeFunction(NWNXLib.Functions._ZN15CNWSCombatRound16StartCombatRoundEj)]
    internal delegate void StartCombatRoundHook(IntPtr pCombatRound, uint oidTarget);

    internal class Factory : NativeEventFactory<StartCombatRoundHook>
    {
      public Factory(Lazy<EventService> eventService, HookService hookService) : base(eventService, hookService) {}

      protected override FunctionHook<StartCombatRoundHook> RequestHook(HookService hookService)
        => hookService.RequestHook<StartCombatRoundHook>(OnStartCombatRound, HookOrder.Earliest);

      private void OnStartCombatRound(IntPtr pCombatRound, uint oidTarget)
      {
        CNWSCombatRound combatRound = new CNWSCombatRound(pCombatRound, false);

        ProcessEvent(new OnStartCombatRound
        {
          Creature = combatRound.m_pBaseCreature.m_idSelf.ToNwObject<NwCreature>(),
          Target = oidTarget.ToNwObject<NwGameObject>()
        });
      }
    }
  }
}
