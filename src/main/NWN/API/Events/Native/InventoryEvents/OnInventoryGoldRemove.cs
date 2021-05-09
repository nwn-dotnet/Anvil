using System.Runtime.InteropServices;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public sealed class OnInventoryGoldRemove : IEvent
  {
    public NwCreature Creature { get; private init; }

    public int Gold { get; private init; }

    public bool PreventGoldRemove { get; set; }

    NwObject IEvent.Context => null;

    internal sealed unsafe class Factory : NativeEventFactory<Factory.RemoveGoldHook>
    {
      internal delegate void RemoveGoldHook(void* pCreature, int nGold, int bDisplayFeedback);

      protected override FunctionHook<RemoveGoldHook> RequestHook()
      {
        delegate* unmanaged<void*, int, int, void> pHook = &OnRemoveGold;
        return HookService.RequestHook<RemoveGoldHook>(pHook, NWNXLib.Functions._ZN12CNWSCreature10RemoveGoldEii, HookOrder.Early);
      }

      [UnmanagedCallersOnly]
      private static void OnRemoveGold(void* pCreature, int nGold, int bDisplayFeedback)
      {
        CNWSCreature creature = new CNWSCreature(pCreature, false);

        OnInventoryGoldRemove eventData = ProcessEvent(new OnInventoryGoldRemove
        {
          Creature = creature.m_idSelf.ToNwObject<NwCreature>(),
          Gold = nGold,
        });

        if (!eventData.PreventGoldRemove)
        {
          Hook.CallOriginal(pCreature, nGold, bDisplayFeedback);
        }
      }
    }
  }
}
