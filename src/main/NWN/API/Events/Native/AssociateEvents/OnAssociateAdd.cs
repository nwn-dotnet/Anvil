using System.Runtime.InteropServices;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public sealed class OnAssociateAdd : IEvent
  {
    public NwCreature Owner { get; private init; }

    public NwCreature Associate { get; private init; }

    public AssociateType AssociateType { get; private init; }

    NwObject IEvent.Context => Owner;

    internal sealed unsafe class Factory : SingleHookEventFactory<Factory.AddAssociateHook>
    {
      internal delegate void AddAssociateHook(void* pCreature, uint oidAssociate, ushort associateType);

      protected override FunctionHook<AddAssociateHook> RequestHook()
      {
        delegate* unmanaged<void*, uint, ushort, void> pHook = &OnAddAssociate;
        return HookService.RequestHook<AddAssociateHook>(pHook, FunctionsLinux._ZN12CNWSCreature12AddAssociateEjt, HookOrder.Earliest);
      }

      [UnmanagedCallersOnly]
      private static void OnAddAssociate(void* pCreature, uint oidAssociate, ushort associateType)
      {
        ProcessEvent(new OnAssociateAdd
        {
          Owner = new CNWSCreature(pCreature, false).ToNwObject<NwCreature>(),
          Associate = oidAssociate.ToNwObject<NwCreature>(),
          AssociateType = (AssociateType)associateType,
        });

        Hook.CallOriginal(pCreature, oidAssociate, associateType);
      }
    }
  }
}
