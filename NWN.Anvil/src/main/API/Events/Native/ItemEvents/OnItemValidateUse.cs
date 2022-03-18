using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnItemValidateUse : IEvent
  {
    public bool CanUse { get; set; }

    public NwItem Item { get; private init; }
    public NwCreature UsedBy { get; private init; }

    NwObject IEvent.Context => UsedBy;

    internal sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<CanUseItemHook> Hook { get; set; }

      private delegate int CanUseItemHook(void* pCreature, void* pItem, int bIgnoreIdentifiedFlag);

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void*, int, int> pHook = &OnCanUseItem;
        Hook = HookService.RequestHook<CanUseItemHook>(pHook, FunctionsLinux._ZN12CNWSCreature10CanUseItemEP8CNWSItemi, HookOrder.Early);
        return new IDisposable[] { Hook };
      }

      [UnmanagedCallersOnly]
      private static int OnCanUseItem(void* pCreature, void* pItem, int bIgnoreIdentifiedFlag)
      {
        OnItemValidateUse eventData = ProcessEvent(new OnItemValidateUse
        {
          UsedBy = CNWSCreature.FromPointer(pCreature).ToNwObject<NwCreature>(),
          Item = CNWSItem.FromPointer(pItem).ToNwObject<NwItem>(),
          CanUse = Hook.CallOriginal(pCreature, pItem, bIgnoreIdentifiedFlag).ToBool(),
        });

        return eventData.CanUse.ToInt();
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="Events.OnItemValidateUse"/>
    public event Action<OnItemValidateUse> OnItemValidateUse
    {
      add => EventService.Subscribe<OnItemValidateUse, OnItemValidateUse.Factory>(this, value);
      remove => EventService.Unsubscribe<OnItemValidateUse, OnItemValidateUse.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnItemValidateUse"/>
    public event Action<OnItemValidateUse> OnItemValidateUse
    {
      add => EventService.SubscribeAll<OnItemValidateUse, OnItemValidateUse.Factory>(value);
      remove => EventService.UnsubscribeAll<OnItemValidateUse, OnItemValidateUse.Factory>(value);
    }
  }
}
