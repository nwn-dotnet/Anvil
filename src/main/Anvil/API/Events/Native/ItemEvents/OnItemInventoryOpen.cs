using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
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

    NwObject IEvent.Context
    {
      get => OpenedBy;
    }

    internal sealed unsafe class Factory : SingleHookEventFactory<Factory.OpenInventoryHook>
    {
      internal delegate void OpenInventoryHook(void* pItem, uint oidOpener);

      protected override FunctionHook<OpenInventoryHook> RequestHook()
      {
        delegate* unmanaged<void*, uint, void> pHook = &OnOpenInventory;
        return HookService.RequestHook<OpenInventoryHook>(pHook, FunctionsLinux._ZN8CNWSItem13OpenInventoryEj, HookOrder.Early);
      }

      [UnmanagedCallersOnly]
      private static void OnOpenInventory(void* pItem, uint oidOpener)
      {
        OnItemInventoryOpen eventData = ProcessEvent(new OnItemInventoryOpen
        {
          OpenedBy = oidOpener.ToNwObject<NwCreature>(),
          Container = CNWSItem.FromPointer(pItem).ToNwObject<NwItem>(),
        });

        if (!eventData.PreventOpen)
        {
          Hook.CallOriginal(pItem, oidOpener);
        }
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
