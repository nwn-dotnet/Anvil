using System;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public sealed class OnInventoryGoldAdd : IEvent
  {
    public NwCreature Creature { get; private init; }

    public int Gold { get; private init; }

    public bool PreventGoldAdd { get; set; }

    NwObject IEvent.Context => null;

    [NativeFunction(NWNXLib.Functions._ZN12CNWSCreature7AddGoldEii)]
    internal delegate void AddGoldHook(IntPtr pCreature, int nGold, int bDisplayFeedback);

    internal class Factory : NativeEventFactory<AddGoldHook>
    {
      public Factory(Lazy<EventService> eventService, HookService hookService) : base(eventService, hookService) {}

      protected override FunctionHook<AddGoldHook> RequestHook(HookService hookService)
        => hookService.RequestHook<AddGoldHook>(OnAddGold, HookOrder.Early);

      private void OnAddGold(IntPtr pCreature, int nGold, int bDisplayFeedback)
      {
        CNWSCreature creature = new CNWSCreature(pCreature, false);

        OnInventoryGoldAdd eventData = ProcessEvent(new OnInventoryGoldAdd
        {
          Creature = creature.m_idSelf.ToNwObject<NwCreature>(),
          Gold = nGold,
        });

        if (!eventData.PreventGoldAdd)
        {
          Hook.CallOriginal(pCreature, nGold, bDisplayFeedback);
        }
      }
    }
  }
}
