using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnAssociateAdd : IEvent
  {
    public NwCreature Associate { get; private init; } = null!;

    public AssociateType AssociateType { get; private init; }

    public NwCreature Owner { get; private init; } = null!;

    NwObject IEvent.Context => Owner;

    internal sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<AddAssociateHook> Hook { get; set; } = null!;

      private delegate void AddAssociateHook(void* pCreature, uint oidAssociate, ushort associateType);

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, uint, ushort, void> pHook = &OnAddAssociate;
        Hook = HookService.RequestHook<AddAssociateHook>(pHook, FunctionsLinux._ZN12CNWSCreature12AddAssociateEjt, HookOrder.Earliest);
        return new IDisposable[] { Hook };
      }

      [UnmanagedCallersOnly]
      private static void OnAddAssociate(void* pCreature, uint oidAssociate, ushort associateType)
      {
        ProcessEvent(new OnAssociateAdd
        {
          Owner = CNWSCreature.FromPointer(pCreature).ToNwObject<NwCreature>()!,
          Associate = oidAssociate.ToNwObject<NwCreature>()!,
          AssociateType = (AssociateType)associateType,
        });

        Hook.CallOriginal(pCreature, oidAssociate, associateType);
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="Events.OnAssociateAdd"/>
    public event Action<OnAssociateAdd> OnAssociateAdd
    {
      add => EventService.Subscribe<OnAssociateAdd, OnAssociateAdd.Factory>(this, value);
      remove => EventService.Unsubscribe<OnAssociateAdd, OnAssociateAdd.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnAssociateAdd"/>
    public event Action<OnAssociateAdd> OnAssociateAdd
    {
      add => EventService.SubscribeAll<OnAssociateAdd, OnAssociateAdd.Factory>(value);
      remove => EventService.UnsubscribeAll<OnAssociateAdd, OnAssociateAdd.Factory>(value);
    }
  }
}
