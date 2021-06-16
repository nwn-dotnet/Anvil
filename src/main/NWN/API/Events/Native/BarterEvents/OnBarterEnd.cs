using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Anvil.Internal;
using NWN.API.Events;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public sealed class OnBarterEnd : IEvent
  {
    public NwPlayer Initiator { get; private init; }

    public NwPlayer Target { get; private init; }

    public IReadOnlyList<NwItem> InitiatorItems { get; private init; }

    public IReadOnlyList<NwItem> TargetItems { get; private init; }

    public bool Complete { get; private init; }

    NwObject IEvent.Context
    {
      get => Initiator.ControlledCreature;
    }

    internal sealed unsafe class Factory : MultiHookEventFactory
    {
      internal delegate int SetListAcceptedHook(void* pBarter, int bAccepted);

      internal delegate int SendServerToPlayerBarterCloseBarterHook(void* pMessage, uint nInitiatorId, uint nRecipientId, int bAccepted);

      private static FunctionHook<SetListAcceptedHook> setListAcceptedHook;
      private static FunctionHook<SendServerToPlayerBarterCloseBarterHook> sendServerToPlayerBarterCloseBarterHook;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, int, int> pSetListAcceptedHook = &OnSetListAccepted;
        setListAcceptedHook = HookService.RequestHook<SetListAcceptedHook>(pSetListAcceptedHook, FunctionsLinux._ZN10CNWSBarter15SetListAcceptedEi, HookOrder.Earliest);

        delegate* unmanaged<void*, uint, uint, int, int> pSendServerToPlayerBarterCloseBarterHook = &OnSendServerToPlayerBarterCloseBarter;
        sendServerToPlayerBarterCloseBarterHook = HookService.RequestHook<SendServerToPlayerBarterCloseBarterHook>(pSendServerToPlayerBarterCloseBarterHook, FunctionsLinux._ZN11CNWSMessage35SendServerToPlayerBarterCloseBarterEjji, HookOrder.Earliest);

        return new IDisposable[] { setListAcceptedHook, sendServerToPlayerBarterCloseBarterHook };
      }

      [UnmanagedCallersOnly]
      private static int OnSetListAccepted(void* pBarter, int bAccepted)
      {
        if (pBarter != null && bAccepted.ToBool())
        {
          OnBarterEnd eventData = GetBarterEventData(CNWSBarter.FromPointer(pBarter), bAccepted.ToBool());

          if (eventData != null)
          {
            ProcessEvent(eventData);
          }
        }

        return setListAcceptedHook.CallOriginal(pBarter, bAccepted);
      }

      [UnmanagedCallersOnly]
      private static int OnSendServerToPlayerBarterCloseBarter(void* pMessage, uint nInitiatorId, uint nRecipientId, int bAccepted)
      {
        NwPlayer player = LowLevel.ServerExoApp.GetClientObjectByPlayerId(nInitiatorId).AsNWSPlayer().ToNwPlayer();
        if (player == null)
        {
          return sendServerToPlayerBarterCloseBarterHook.CallOriginal(pMessage, nInitiatorId, nRecipientId, bAccepted);
        }

        CNWSBarter barter = player.ControlledCreature?.Creature?.GetBarterInfo(0);

        // We only need to run the END on a CANCEL BARTER for the initiator
        if (barter != null && barter.m_bInitiator.ToBool() && !bAccepted.ToBool())
        {
          OnBarterEnd eventData = GetBarterEventData(barter, bAccepted.ToBool());

          if (eventData != null)
          {
            ProcessEvent(eventData);
          }
        }

        return sendServerToPlayerBarterCloseBarterHook.CallOriginal(pMessage, nInitiatorId, nRecipientId, bAccepted);
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
          Initiator = initiator.m_pOwner.m_idSelf.ToNwPlayer(),
          Target = target.m_pOwner.m_idSelf.ToNwPlayer(),
          Complete = true,
          InitiatorItems = GetBarterItems(initiator),
          TargetItems = GetBarterItems(target),
        };
      }

      private static OnBarterEnd GetBarterCancelledEventData(CNWSBarter initiator, CNWSBarter target)
      {
        return new OnBarterEnd
        {
          Initiator = initiator.m_pOwner.m_idSelf.ToNwPlayer(),
          Target = target.m_pOwner.m_idSelf.ToNwPlayer(),
          Complete = false,
          InitiatorItems = new NwItem[0],
          TargetItems = new NwItem[0],
        };
      }

      private static IReadOnlyList<NwItem> GetBarterItems(CNWSBarter barter)
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
}

namespace NWN.API
{
  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="NWN.API.Events.OnBarterEnd"/>
    public event Action<OnBarterEnd> OnBarterEnd
    {
      add => EventService.Subscribe<OnBarterEnd, OnBarterEnd.Factory>(ControlledCreature, value);
      remove => EventService.Unsubscribe<OnBarterEnd, OnBarterEnd.Factory>(ControlledCreature, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="NWN.API.Events.OnBarterEnd"/>
    public event Action<OnBarterEnd> OnBarterEnd
    {
      add => EventService.SubscribeAll<OnBarterEnd, OnBarterEnd.Factory>(value);
      remove => EventService.UnsubscribeAll<OnBarterEnd, OnBarterEnd.Factory>(value);
    }
  }
}
