using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public class OnBarterEnd : IEvent
  {
    public NwPlayer Initiator { get; private init; }

    public NwPlayer Target { get; private init; }

    public IReadOnlyList<NwItem> InitiatorItems { get; private init; }

    public IReadOnlyList<NwItem> TargetItems { get; private init; }

    public bool Complete { get; private init; }

    NwObject IEvent.Context => Initiator;

    [NativeFunction(NWNXLib.Functions._ZN10CNWSBarter15SetListAcceptedEi)]
    internal delegate int SetListAcceptedHook(IntPtr pBarter, int bAccepted);

    [NativeFunction(NWNXLib.Functions._ZN11CNWSMessage35SendServerToPlayerBarterCloseBarterEjji)]
    internal delegate int SendServerToPlayerBarterCloseBarterHook(IntPtr pMessage, uint nInitiatorId, uint nRecipientId, int bAccepted);

    public static Type[] FactoryTypes { get; } = {typeof(SetListAcceptedFactory), typeof(SendServerToPlayerBarterCloseBarterFactory)};

    internal class SetListAcceptedFactory : NativeEventFactory<SetListAcceptedHook>
    {
      public SetListAcceptedFactory(Lazy<EventService> eventService, HookService hookService) : base(eventService, hookService) {}

      protected override FunctionHook<SetListAcceptedHook> RequestHook(HookService hookService)
        => hookService.RequestHook<SetListAcceptedHook>(OnSetListAccepted, HookOrder.Earliest);

      private int OnSetListAccepted(IntPtr pBarter, int bAccepted)
      {
        if (pBarter != IntPtr.Zero && bAccepted.ToBool())
        {
          OnBarterEnd eventData = GetBarterEventData(new CNWSBarter(pBarter, false), bAccepted.ToBool());

          if (eventData != null)
          {
            ProcessEvent(eventData);
          }
        }

        return Hook.CallOriginal(pBarter, bAccepted);
      }
    }

    internal class SendServerToPlayerBarterCloseBarterFactory : NativeEventFactory<SendServerToPlayerBarterCloseBarterHook>
    {
      public SendServerToPlayerBarterCloseBarterFactory(Lazy<EventService> eventService, HookService hookService) : base(eventService, hookService) {}

      protected override FunctionHook<SendServerToPlayerBarterCloseBarterHook> RequestHook(HookService hookService)
        => hookService.RequestHook<SendServerToPlayerBarterCloseBarterHook>(OnSendServerToPlayerBarterCloseBarter, HookOrder.Earliest);

      private int OnSendServerToPlayerBarterCloseBarter(IntPtr pMessage, uint nInitiatorId, uint nRecipientId, int bAccepted)
      {
        NwPlayer player = new NwPlayer(LowLevel.ServerExoApp.GetClientObjectByPlayerId(nInitiatorId).AsNWSPlayer());
        CNWSBarter barter = player.Creature.GetBarterInfo(0);

        // We only need to run the END on a CANCEL BARTER for the initiator
        if (barter != null && barter.m_bInitiator.ToBool() && !bAccepted.ToBool())
        {
          OnBarterEnd eventData = GetBarterEventData(barter, bAccepted.ToBool());

          if (eventData != null)
          {
            ProcessEvent(eventData);
          }
        }

        return Hook.CallOriginal(pMessage, nInitiatorId, nRecipientId, bAccepted);
      }
    }

    private static OnBarterEnd GetBarterEventData(CNWSBarter barter, bool accepted)
    {
      NwCreature other = barter.m_oidBarrator.ToNwObject<NwCreature>();
      if (other == null)
      {
        return null;
      }

      CNWSBarter otherBarter = other.Creature.GetBarterInfo(0);
      CNWSBarter initiatorBarter = barter.m_bInitiator.ToBool() ? barter : otherBarter;
      CNWSBarter targetBarter = barter.m_bInitiator.ToBool() ? otherBarter : barter;

      return accepted ? GetBarterAcceptedEventData(otherBarter, initiatorBarter, targetBarter) : GetBarterCancelledEventData(initiatorBarter, targetBarter);
    }

    private static OnBarterEnd GetBarterAcceptedEventData(CNWSBarter other, CNWSBarter initiator, CNWSBarter target)
    {
      // We only handle a completed barter when the other player has already accepted
      if (!other.m_bListAccepted.ToBool())
      {
        return null;
      }

      return new OnBarterEnd
      {
        Initiator = initiator.m_pOwner.m_idSelf.ToNwObject<NwPlayer>(),
        Target = target.m_pOwner.m_idSelf.ToNwObject<NwPlayer>(),
        Complete = true,
        InitiatorItems = GetBarterItems(initiator),
        TargetItems = GetBarterItems(target)
      };
    }

    private static OnBarterEnd GetBarterCancelledEventData(CNWSBarter initiator, CNWSBarter target)
    {
      return new OnBarterEnd
      {
        Initiator = initiator.m_pOwner.m_idSelf.ToNwObject<NwPlayer>(),
        Target = target.m_pOwner.m_idSelf.ToNwObject<NwPlayer>(),
        Complete = false,
        InitiatorItems = new NwItem[0],
        TargetItems = new NwItem[0]
      };
    }

    private static unsafe IReadOnlyList<NwItem> GetBarterItems(CNWSBarter barter)
    {
      List<NwItem> items = new List<NwItem>();
      if (barter.m_pBarterList == null)
      {
        return items;
      }

      CExoLinkedListInternal itemList = barter.m_pBarterList.m_oidItems.m_pcExoLinkedListInternal;
      for (CExoLinkedListNode node = itemList.pHead; node != null; node = node.pNext)
      {
        NwItem item = (*(uint*)node.pObject).ToNwObject<NwItem>();
        if (item != null)
        {
          items.Add(item);
        }
      }

      return items.AsReadOnly();
    }
  }
}
