using System.Runtime.InteropServices;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public sealed class OnItemInventoryOpen : IEvent
  {
    public bool PreventOpen { get; set; }

    /// <summary>
    /// Gets the creature that is opening the container.
    /// </summary>
    public NwCreature OpenedBy { get; private init; }

    /// <summary>
    /// Gets the container being opened.
    /// </summary>
    public NwItem Container { get; private init; }

    NwObject IEvent.Context => OpenedBy;

    internal sealed unsafe class Factory : NativeEventFactory<Factory.OpenInventoryHook>
    {
      internal delegate void OpenInventoryHook(void* pItem, uint oidOpener);

      protected override FunctionHook<OpenInventoryHook> RequestHook()
      {
        delegate* unmanaged<void*, uint, void> pHook = &OnOpenInventory;
        return HookService.RequestHook<OpenInventoryHook>(NWNXLib.Functions._ZN8CNWSItem13OpenInventoryEj, pHook, HookOrder.Early);
      }

      [UnmanagedCallersOnly]
      private static void OnOpenInventory(void* pItem, uint oidOpener)
      {
        OnItemInventoryOpen eventData = ProcessEvent(new OnItemInventoryOpen
        {
          OpenedBy = oidOpener.ToNwObject<NwCreature>(),
          Container = new CNWSItem(pItem, false).ToNwObject<NwItem>(),
        });

        if (!eventData.PreventOpen)
        {
          Hook.CallOriginal(pItem, oidOpener);
        }
      }
    }
  }
}
