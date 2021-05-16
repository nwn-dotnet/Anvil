using System.Runtime.InteropServices;
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

    internal sealed unsafe class Factory : SingleHookEventFactory<Factory.TrapExamineHook>
    {
      internal delegate void TrapExamineHook(void* pMessage, void* pPlayer, uint oidTrap, void* pCreature, int bSuccess);

      protected override FunctionHook<TrapExamineHook> RequestHook()
      {
        delegate* unmanaged<void*, void*, uint, void*, int, void> pHook = &OnExamineTrap;
        return HookService.RequestHook<TrapExamineHook>(pHook, FunctionsLinux._ZN11CNWSMessage37SendServerToPlayerExamineGui_TrapDataEP10CNWSPlayerjP12CNWSCreaturei, HookOrder.Earliest);
      }

      [UnmanagedCallersOnly]
      private static void OnExamineTrap(void* pMessage, void* pPlayer, uint oidTrap, void* pCreature, int bSuccess)
      {
        ProcessEvent(new OnExamineTrap
        {
          ExaminedBy = new NwPlayer(new CNWSPlayer(pPlayer, false)),
          ExaminedObject = oidTrap.ToNwObject<NwGameObject>(),
          Success = bSuccess.ToBool()
        });

        Hook.CallOriginal(pMessage, pPlayer, oidTrap, pCreature, bSuccess);
      }
    }
  }
}
