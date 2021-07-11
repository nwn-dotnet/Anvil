using System;
using System.Runtime.InteropServices;
using Anvil.API;
using Anvil.Services;
using NWN.API.Events;
using NWN.Native.API;

namespace NWN.API.Events
{
  public sealed class OnItemValidateUse : IEvent
  {
    public NwCreature UsedBy { get; private init; }

    public NwItem Item { get; private init; }

    public bool CanUse { get; set; }

    NwObject IEvent.Context
    {
      get => UsedBy;
    }

    internal sealed unsafe class Factory : SingleHookEventFactory<Factory.CanUseItemHook>
    {
      internal delegate int CanUseItemHook(void* pCreature, void* pItem, int bIgnoreIdentifiedFlag);

      protected override FunctionHook<CanUseItemHook> RequestHook()
      {
        delegate* unmanaged<void*, void*, int, int> pHook = &OnCanUseItem;
        return HookService.RequestHook<CanUseItemHook>(pHook, FunctionsLinux._ZN12CNWSCreature10CanUseItemEP8CNWSItemi, HookOrder.Early);
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

namespace NWN.API
{
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="NWN.API.Events.OnItemValidateUse"/>
    public event Action<OnItemValidateUse> OnItemValidateUse
    {
      add => EventService.Subscribe<OnItemValidateUse, OnItemValidateUse.Factory>(this, value);
      remove => EventService.Unsubscribe<OnItemValidateUse, OnItemValidateUse.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="NWN.API.Events.OnItemValidateUse"/>
    public event Action<OnItemValidateUse> OnItemValidateUse
    {
      add => EventService.SubscribeAll<OnItemValidateUse, OnItemValidateUse.Factory>(value);
      remove => EventService.UnsubscribeAll<OnItemValidateUse, OnItemValidateUse.Factory>(value);
    }
  }
}
