using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Native;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnItemInventoryOpen : IEvent
  {
    /// <summary>
    /// Gets the container being opened.
    /// </summary>
    public NwItem Container { get; private init; } = null!;

    /// <summary>
    /// Gets the creature that is opening the container.
    /// </summary>
    public NwCreature OpenedBy { get; private init; } = null!;

    public bool PreventOpen { get; set; }

    NwObject IEvent.Context => OpenedBy;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<Functions.CNWSItem.OpenInventory> Hook { get; set; } = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, uint, void> pHook = &OnOpenInventory;
        Hook = HookService.RequestHook<Functions.CNWSItem.OpenInventory>(pHook, HookOrder.Early);
        return new IDisposable[] { Hook };
      }

      [UnmanagedCallersOnly]
      private static void OnOpenInventory(void* pItem, uint oidOpener)
      {
        OnItemInventoryOpen eventData = ProcessEvent(EventCallbackType.Before, new OnItemInventoryOpen
        {
          OpenedBy = oidOpener.ToNwObject<NwCreature>()!,
          Container = CNWSItem.FromPointer(pItem).ToNwObject<NwItem>()!,
        });

        if (!eventData.PreventOpen)
        {
          Hook.CallOriginal(pItem, oidOpener);
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
    /// <inheritdoc cref="Events.OnItemInventoryOpen"/>
    public event Action<OnItemInventoryOpen> OnItemInventoryOpen
    {
      add => EventService.Subscribe<OnItemInventoryOpen, OnItemInventoryOpen.Factory>(this, value);
      remove => EventService.Unsubscribe<OnItemInventoryOpen, OnItemInventoryOpen.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnItemInventoryOpen"/>
    public event Action<OnItemInventoryOpen> OnItemInventoryOpen
    {
      add => EventService.SubscribeAll<OnItemInventoryOpen, OnItemInventoryOpen.Factory>(value);
      remove => EventService.UnsubscribeAll<OnItemInventoryOpen, OnItemInventoryOpen.Factory>(value);
    }
  }
}
