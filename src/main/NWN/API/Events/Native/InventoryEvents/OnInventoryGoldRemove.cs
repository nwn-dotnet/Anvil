using System;
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

    [NativeFunction(NWNXLib.Functions._ZN12CNWSCreature10RemoveGoldEii)]
    internal delegate void RemoveGoldHook(IntPtr pCreature, int nGold, int bDisplayFeedback);

    internal class Factory : NativeEventFactory<RemoveGoldHook>
    {
      public Factory(Lazy<EventService> eventService, HookService hookService) : base(eventService, hookService) {}

      protected override FunctionHook<RemoveGoldHook> RequestHook(HookService hookService)
        => hookService.RequestHook<RemoveGoldHook>(OnRemoveGold, HookOrder.Early);

      private void OnRemoveGold(IntPtr pCreature, int nGold, int bDisplayFeedback)
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
