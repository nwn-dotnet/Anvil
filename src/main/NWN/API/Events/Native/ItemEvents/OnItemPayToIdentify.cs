using System;
using System.Runtime.InteropServices;
using NWN.API.Events;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public sealed class OnItemPayToIdentify : IEvent
  {
    public bool PreventPayToIdentify { get; set; }

    public NwCreature Creature { get; private init; }

    public NwStore Store { get; private init; }

    public NwItem Item { get; private init; }

    NwObject IEvent.Context
    {
      get => Creature;
    }

    internal sealed unsafe class Factory : SingleHookEventFactory<Factory.PayToIdentifyItemHook>
    {
      internal delegate void PayToIdentifyItemHook(void* pCreature, uint oidItem, uint oidStore);

      protected override FunctionHook<PayToIdentifyItemHook> RequestHook()
      {
        delegate* unmanaged<void*, uint, uint, void> pHook = &OnPayToIdentifyItem;
        return HookService.RequestHook<PayToIdentifyItemHook>(pHook, FunctionsLinux._ZN12CNWSCreature17PayToIdentifyItemEjj, HookOrder.Early);
      }

      [UnmanagedCallersOnly]
      private static void OnPayToIdentifyItem(void* pCreature, uint oidItem, uint oidStore)
      {
        OnItemPayToIdentify eventData = ProcessEvent(new OnItemPayToIdentify
        {
          Creature = CNWSCreature.FromPointer(pCreature).ToNwObject<NwCreature>(),
          Item = oidItem.ToNwObject<NwItem>(),
          Store = oidStore.ToNwObject<NwStore>(),
        });

        if (!eventData.PreventPayToIdentify)
        {
          Hook.CallOriginal(pCreature, oidItem, oidStore);
        }
      }
    }
  }
}

namespace NWN.API
{
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="NWN.API.Events.OnItemPayToIdentify"/>
    public event Action<OnItemPayToIdentify> OnItemPayToIdentify
    {
      add => EventService.Subscribe<OnItemPayToIdentify, OnItemPayToIdentify.Factory>(this, value);
      remove => EventService.Unsubscribe<OnItemPayToIdentify, OnItemPayToIdentify.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {

  }
}
