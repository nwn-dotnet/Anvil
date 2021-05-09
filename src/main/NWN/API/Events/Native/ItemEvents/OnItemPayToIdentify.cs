using System.Runtime.InteropServices;
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

    NwObject IEvent.Context => Creature;

    internal sealed unsafe class Factory : NativeEventFactory<Factory.PayToIdentifyItemHook>
    {
      internal delegate void PayToIdentifyItemHook(void* pCreature, uint oidItem, uint oidStore);

      protected override FunctionHook<PayToIdentifyItemHook> RequestHook()
      {
        delegate* unmanaged<void*, uint, uint, void> pHook = &OnPayToIdentifyItem;
        return HookService.RequestHook<PayToIdentifyItemHook>(pHook, NWNXLib.Functions._ZN12CNWSCreature17PayToIdentifyItemEjj, HookOrder.Early);
      }

      [UnmanagedCallersOnly]
      private static void OnPayToIdentifyItem(void* pCreature, uint oidItem, uint oidStore)
      {
        OnItemPayToIdentify eventData = ProcessEvent(new OnItemPayToIdentify
        {
          Creature = new CNWSCreature(pCreature, false).ToNwObject<NwCreature>(),
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
