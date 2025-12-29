using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Native;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  /// <summary>
  /// Triggered when a creature closes a container item.
  /// </summary>
  public sealed class OnItemInventoryClose : IEvent
  {
    /// <summary>
    /// Gets the creature that is closing the container.
    /// </summary>
    public NwCreature ClosedBy { get; private init; } = null!;

    /// <summary>
    /// Gets the container being closed.
    /// </summary>
    public NwItem Container { get; private init; } = null!;

    /// <summary>
    /// Set to true to prevent the container from being closed.
    /// </summary>
    public bool PreventClose { get; set; }

    NwObject IEvent.Context => ClosedBy;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<Functions.CNWSItem.CloseInventory> Hook { get; set; } = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, uint, int, void> pHook = &OnCloseInventory;
        Hook = HookService.RequestHook<Functions.CNWSItem.CloseInventory>(pHook, HookOrder.Early);
        return [Hook];
      }

      [UnmanagedCallersOnly]
      private static void OnCloseInventory(void* pItem, uint oidCloser, int bUpdatePlayer)
      {
        OnItemInventoryClose eventData = ProcessEvent(EventCallbackType.Before, new OnItemInventoryClose
        {
          ClosedBy = oidCloser.ToNwObject<NwCreature>()!,
          Container = CNWSItem.FromPointer(pItem).ToNwObject<NwItem>()!,
        });

        if (!eventData.PreventClose)
        {
          Hook.CallOriginal(pItem, oidCloser, bUpdatePlayer);
        }

        ProcessEvent(EventCallbackType.After, eventData);
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
