using System.Runtime.InteropServices;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public sealed class OnAssociateRemove : IEvent
  {
    public NwCreature Owner { get; private init; }

    public NwCreature Associate { get; private init; }

    NwObject IEvent.Context
    {
      get => Owner;
    }

    internal sealed unsafe class Factory : SingleHookEventFactory<Factory.RemoveAssociateHook>
    {
      internal delegate void RemoveAssociateHook(void* pCreature, uint oidAssociate);

      protected override FunctionHook<RemoveAssociateHook> RequestHook()
      {
        delegate* unmanaged<void*, uint, void> pHook = &OnRemoveAssociate;
        return HookService.RequestHook<RemoveAssociateHook>(pHook, FunctionsLinux._ZN12CNWSCreature15RemoveAssociateEj, HookOrder.Earliest);
      }

      [UnmanagedCallersOnly]
      private static void OnRemoveAssociate(void* pCreature, uint oidAssociate)
      {
        ProcessEvent(new OnAssociateRemove
        {
          Owner = CNWSCreature.FromPointer(pCreature).ToNwObject<NwCreature>(),
          Associate = oidAssociate.ToNwObject<NwCreature>(),
        });

        Hook.CallOriginal(pCreature, oidAssociate);
      }
    }
  }
}
