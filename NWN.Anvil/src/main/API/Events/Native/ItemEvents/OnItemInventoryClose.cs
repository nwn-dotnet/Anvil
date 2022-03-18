using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnItemInventoryClose : IEvent
  {
    /// <summary>
    /// Gets the creature that is closing the container.
    /// </summary>
    public NwCreature ClosedBy { get; private init; }

    /// <summary>
    /// Gets the container being opened.
    /// </summary>
    public NwItem Container { get; private init; }

    public bool PreventClose { get; set; }

    NwObject IEvent.Context => ClosedBy;

    internal sealed unsafe class Factory : HookEventFactory
    {
      internal delegate void CloseInventoryHook(void* pItem, uint oidCloser, int bUpdatePlayer);

      private static FunctionHook<CloseInventoryHook> Hook { get; set; }

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, uint, int, void> pHook = &OnCloseInventory;
        Hook = HookService.RequestHook<CloseInventoryHook>(pHook, FunctionsLinux._ZN8CNWSItem14CloseInventoryEji, HookOrder.Early);
        return new IDisposable[] { Hook };
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

namespace Anvil.API
{
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="Events.OnItemInventoryClose"/>
    public event Action<OnItemInventoryClose> OnItemInventoryClose
    {
      add => EventService.Subscribe<OnItemInventoryClose, OnItemInventoryClose.Factory>(this, value);
      remove => EventService.Unsubscribe<OnItemInventoryClose, OnItemInventoryClose.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnItemInventoryClose"/>
    public event Action<OnItemInventoryClose> OnItemInventoryClose
    {
      add => EventService.SubscribeAll<OnItemInventoryClose, OnItemInventoryClose.Factory>(value);
      remove => EventService.UnsubscribeAll<OnItemInventoryClose, OnItemInventoryClose.Factory>(value);
    }
  }
}
