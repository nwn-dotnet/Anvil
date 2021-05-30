using System.Runtime.InteropServices;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public sealed class OnItemInventoryClose : IEvent
  {
    public bool PreventClose { get; set; }

    /// <summary>
    /// Gets the creature that is closing the container.
    /// </summary>
    public NwCreature ClosedBy { get; private init; }

    /// <summary>
    /// Gets the container being opened.
    /// </summary>
    public NwItem Container { get; private init; }

    NwObject IEvent.Context
    {
      get => ClosedBy;
    }

    internal sealed unsafe class Factory : SingleHookEventFactory<Factory.CloseInventoryHook>
    {
      internal delegate void CloseInventoryHook(void* pItem, uint oidCloser, int bUpdatePlayer);

      protected override FunctionHook<CloseInventoryHook> RequestHook()
      {
        delegate* unmanaged<void*, uint, int, void> pHook = &OnCloseInventory;
        return HookService.RequestHook<CloseInventoryHook>(pHook, FunctionsLinux._ZN8CNWSItem14CloseInventoryEji, HookOrder.Early);
      }

      [UnmanagedCallersOnly]
      private static void OnCloseInventory(void* pItem, uint oidCloser, int bUpdatePlayer)
      {
        OnItemInventoryClose eventData = ProcessEvent(new OnItemInventoryClose
        {
          ClosedBy = oidCloser.ToNwObject<NwCreature>(),
          Container = CNWSItem.FromPointer(pItem).ToNwObject<NwItem>(),
        });

        if (!eventData.PreventClose)
        {
          Hook.CallOriginal(pItem, oidCloser, bUpdatePlayer);
        }
      }
    }
  }
}
