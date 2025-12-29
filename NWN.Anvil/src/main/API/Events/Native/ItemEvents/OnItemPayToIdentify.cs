using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Native;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  /// <summary>
  /// Triggered when a creature pays a store to identify an item.
  /// </summary>
  public sealed class OnItemPayToIdentify : IEvent
  {
    /// <summary>
    /// Gets the creature requesting item identification.
    /// </summary>
    public NwCreature Creature { get; private init; } = null!;

    /// <summary>
    /// Gets the item being identified.
    /// </summary>
    public NwItem Item { get; private init; } = null!;

    /// <summary>
    /// Set to true to prevent paying to identify the item.
    /// </summary>
    public bool PreventPayToIdentify { get; set; }

    /// <summary>
    /// Gets the store performing the identification.
    /// </summary>
    public NwStore Store { get; private init; } = null!;

    NwObject IEvent.Context => Creature;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<Functions.CNWSCreature.PayToIdentifyItem> Hook { get; set; } = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, uint, uint, void> pHook = &OnPayToIdentifyItem;
        Hook = HookService.RequestHook<Functions.CNWSCreature.PayToIdentifyItem>(pHook, HookOrder.Early);
        return [Hook];
      }

      [UnmanagedCallersOnly]
      private static void OnPayToIdentifyItem(void* pCreature, uint oidItem, uint oidStore)
      {
        OnItemPayToIdentify eventData = ProcessEvent(EventCallbackType.Before, new OnItemPayToIdentify
        {
          Creature = CNWSCreature.FromPointer(pCreature).ToNwObject<NwCreature>()!,
          Item = oidItem.ToNwObject<NwItem>()!,
          Store = oidStore.ToNwObject<NwStore>()!,
        });

        if (!eventData.PreventPayToIdentify)
        {
          Hook.CallOriginal(pCreature, oidItem, oidStore);
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
    /// <inheritdoc cref="Events.OnItemPayToIdentify"/>
    public event Action<OnItemPayToIdentify> OnItemPayToIdentify
    {
      add => EventService.Subscribe<OnItemPayToIdentify, OnItemPayToIdentify.Factory>(this, value);
      remove => EventService.Unsubscribe<OnItemPayToIdentify, OnItemPayToIdentify.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnItemPayToIdentify"/>
    public event Action<OnItemPayToIdentify> OnItemPayToIdentify
    {
      add => EventService.SubscribeAll<OnItemPayToIdentify, OnItemPayToIdentify.Factory>(value);
      remove => EventService.UnsubscribeAll<OnItemPayToIdentify, OnItemPayToIdentify.Factory>(value);
    }
  }
}
