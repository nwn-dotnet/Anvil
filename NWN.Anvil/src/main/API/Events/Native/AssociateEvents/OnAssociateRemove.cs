using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnAssociateRemove : IEvent
  {
    public NwCreature Associate { get; private init; } = null!;
    public NwCreature Owner { get; private init; } = null!;

    NwObject IEvent.Context => Owner;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<RemoveAssociateHook> Hook { get; set; } = null!;

      [NativeFunction("_ZN12CNWSCreature15RemoveAssociateEj", "")]
      private delegate void RemoveAssociateHook(void* pCreature, uint oidAssociate);

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, uint, void> pHook = &OnRemoveAssociate;
        Hook = HookService.RequestHook<RemoveAssociateHook>(pHook, HookOrder.Earliest);
        return new IDisposable[] { Hook };
      }

      [UnmanagedCallersOnly]
      private static void OnRemoveAssociate(void* pCreature, uint oidAssociate)
      {
        OnAssociateRemove eventData = ProcessEvent(EventCallbackType.Before, new OnAssociateRemove
        {
          Owner = CNWSCreature.FromPointer(pCreature).ToNwObject<NwCreature>()!,
          Associate = oidAssociate.ToNwObject<NwCreature>()!,
        });

        Hook.CallOriginal(pCreature, oidAssociate);
        ProcessEvent(EventCallbackType.After, eventData);
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
