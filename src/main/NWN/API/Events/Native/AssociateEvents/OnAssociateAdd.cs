using System;
using System.Runtime.InteropServices;
using NWN.API.Events;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public sealed class OnAssociateAdd : IEvent
  {
    public NwCreature Owner { get; private init; }

    public NwCreature Associate { get; private init; }

    public AssociateType AssociateType { get; private init; }

    NwObject IEvent.Context
    {
      get => Owner;
    }

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
          Owner = CNWSCreature.FromPointer(pCreature).ToNwObject<NwCreature>(),
          Associate = oidAssociate.ToNwObject<NwCreature>(),
          AssociateType = (AssociateType)associateType,
        });

        Hook.CallOriginal(pCreature, oidAssociate, associateType);
      }
    }
  }
}

namespace NWN.API
{
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="NWN.API.Events.OnAssociateAdd"/>
    public event Action<OnAssociateAdd> OnAssociateAdd
    {
      add => EventService.Subscribe<OnAssociateAdd, OnAssociateAdd.Factory>(this, value);
      remove => EventService.Unsubscribe<OnAssociateAdd, OnAssociateAdd.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {

  }
}
