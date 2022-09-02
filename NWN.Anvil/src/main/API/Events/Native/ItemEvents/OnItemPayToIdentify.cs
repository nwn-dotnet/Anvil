using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnItemPayToIdentify : IEvent
  {
    public NwCreature Creature { get; private init; } = null!;

    public NwItem Item { get; private init; } = null!;
    public bool PreventPayToIdentify { get; set; }

    public NwStore Store { get; private init; } = null!;

    NwObject IEvent.Context => Creature;

    internal sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<PayToIdentifyItemHook> Hook { get; set; } = null!;

      private delegate void PayToIdentifyItemHook(void* pCreature, uint oidItem, uint oidStore);

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, uint, uint, void> pHook = &OnPayToIdentifyItem;
        Hook = HookService.RequestHook<PayToIdentifyItemHook>(pHook, FunctionsLinux._ZN12CNWSCreature17PayToIdentifyItemEjj, HookOrder.Early);
        return new IDisposable[] { Hook };
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
