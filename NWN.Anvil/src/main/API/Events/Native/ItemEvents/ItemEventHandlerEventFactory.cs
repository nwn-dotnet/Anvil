using System;
using System.Runtime.InteropServices;
using Anvil.Native;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed unsafe class ItemEventHandlerEventFactory : HookEventFactory
  {
    private static FunctionHook<Functions.CNWSItem.EventHandler> Hook { get; set; } = null!;

    protected override IDisposable[] RequestHooks()
    {
      delegate* unmanaged<void*, uint, uint, void*, uint, uint, void> pHook = &OnEventHandler;
      Hook = HookService.RequestHook<Functions.CNWSItem.EventHandler>(pHook, HookOrder.Early);
      return [Hook];
    }

    [UnmanagedCallersOnly]
    private static void OnEventHandler(void* pItem, uint nEventId, uint nCallerObjectId, void* pScript, uint nCalendarDay, uint nTimeOfDay)
    {
      CNWSItem item = CNWSItem.FromPointer(pItem);

      ItemHandlerEvent eventData = nEventId switch
      {
        11 => HandleEvent<OnItemDestroy>(item),
        16 => HandleEvent<OnItemDecrementStackSize>(item),
        _ => throw new ArgumentOutOfRangeException(nameof(nEventId), nEventId, null),
      };

      if (!eventData.Skip)
      {
        Hook.CallOriginal(pItem, nEventId, nCallerObjectId, pScript, nCalendarDay, nTimeOfDay);
      }

      ProcessEvent(EventCallbackType.After, eventData);
    }

    private static TEvent HandleEvent<TEvent>(CNWSItem item) where TEvent : ItemHandlerEvent, new()
    {
      return ProcessEvent(EventCallbackType.Before, new TEvent
      {
        Item = item.ToNwObject<NwItem>()!,
      });
    }
  }
}
