using System;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public sealed class OnExamineTrap : IEvent
  {
    public NwPlayer ExaminedBy { get; private init; }

    public NwGameObject ExaminedObject { get; private init; }

    public bool Success { get; private init; }

    NwObject IEvent.Context => ExaminedBy;

    [NativeFunction(NWNXLib.Functions._ZN11CNWSMessage37SendServerToPlayerExamineGui_TrapDataEP10CNWSPlayerjP12CNWSCreaturei)]
    internal delegate void TrapExamineHook(IntPtr pMessage, IntPtr pPlayer, uint trap, IntPtr pCreature, int bSuccess);

    internal class Factory : NativeEventFactory<TrapExamineHook>
    {
      public Factory(Lazy<EventService> eventService, HookService hookService) : base(eventService, hookService) {}

      protected override FunctionHook<TrapExamineHook> RequestHook(HookService hookService)
        => hookService.RequestHook<TrapExamineHook>(OnExamineTrap, HookOrder.Earliest);

      private void OnExamineTrap(IntPtr pMessage, IntPtr pPlayer, uint trap, IntPtr pCreature, int bSuccess)
      {
        ProcessEvent(new OnExamineTrap
        {
          ExaminedBy = new NwPlayer(new CNWSPlayer(pPlayer, false)),
          ExaminedObject = trap.ToNwObject<NwGameObject>(),
          Success = bSuccess.ToBool()
        });

        Hook.CallOriginal(pMessage, pPlayer, trap, pCreature, bSuccess);
      }
    }
  }
}
