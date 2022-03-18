using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnAssociateRemove : IEvent
  {
    public NwCreature Associate { get; private init; }
    public NwCreature Owner { get; private init; }

    NwObject IEvent.Context => Owner;

    internal sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<RemoveAssociateHook> Hook { get; set; }

      private delegate void RemoveAssociateHook(void* pCreature, uint oidAssociate);

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, uint, void> pHook = &OnRemoveAssociate;
        Hook = HookService.RequestHook<RemoveAssociateHook>(pHook, FunctionsLinux._ZN12CNWSCreature15RemoveAssociateEj, HookOrder.Earliest);
        return new IDisposable[] { Hook };
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

namespace Anvil.API
{
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="Events.OnAssociateRemove"/>
    public event Action<OnAssociateRemove> OnAssociateRemove
    {
      add => EventService.Subscribe<OnAssociateRemove, OnAssociateRemove.Factory>(this, value);
      remove => EventService.Unsubscribe<OnAssociateRemove, OnAssociateRemove.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnAssociateRemove"/>
    public event Action<OnAssociateRemove> OnAssociateRemove
    {
      add => EventService.SubscribeAll<OnAssociateRemove, OnAssociateRemove.Factory>(value);
      remove => EventService.UnsubscribeAll<OnAssociateRemove, OnAssociateRemove.Factory>(value);
    }
  }
}
