using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Internal;
using Anvil.Native;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnBarterEnd : IEvent
  {
    public bool Complete { get; private init; }
    public NwPlayer Initiator { get; private init; } = null!;

    public IReadOnlyList<NwItem> InitiatorItems { get; private init; } = null!;

    public NwPlayer Target { get; private init; } = null!;

    public IReadOnlyList<NwItem> TargetItems { get; private init; } = null!;

    NwObject? IEvent.Context => Initiator.ControlledCreature;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<Functions.CNWSMessage.SendServerToPlayerBarterCloseBarter> sendServerToPlayerBarterCloseBarterHook = null!;
      private static FunctionHook<SetListAcceptedHook> setListAcceptedHook = null!;

      [NativeFunction("_ZN10CNWSBarter15SetListAcceptedEi", "")]
      private delegate int SetListAcceptedHook(void* pBarter, int bAccepted);

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, int, int> pSetListAcceptedHook = &OnSetListAccepted;
        setListAcceptedHook = HookService.RequestHook<SetListAcceptedHook>(pSetListAcceptedHook, HookOrder.Earliest);

        delegate* unmanaged<void*, uint, uint, int, int> pSendServerToPlayerBarterCloseBarterHook = &OnSendServerToPlayerBarterCloseBarter;
        sendServerToPlayerBarterCloseBarterHook = HookService.RequestHook<Functions.CNWSMessage.SendServerToPlayerBarterCloseBarter>(pSendServerToPlayerBarterCloseBarterHook, HookOrder.Earliest);

        return new IDisposable[] { setListAcceptedHook, sendServerToPlayerBarterCloseBarterHook };
      }

      private static OnBarterEnd? GetBarterAcceptedEventData(CNWSBarter other, CNWSBarter initiator, CNWSBarter target)
      {
        // We only handle a completed barter when the other player has already accepted
        if (!other.m_bListAccepted.ToBool())
        {
          return null;
        }

        return new OnBarterEnd
        {
          Initiator = initiator.m_pOwner.m_idSelf.ToNwPlayer()!,
          Target = target.m_pOwner.m_idSelf.ToNwPlayer()!,
          Complete = true,
          InitiatorItems = GetBarterItems(initiator),
          TargetItems = GetBarterItems(target),
        };
      }

      private static OnBarterEnd GetBarterCancelledEventData(CNWSBarter initiator, CNWSBarter target)
      {
        return new OnBarterEnd
        {
          Initiator = initiator.m_pOwner.m_idSelf.ToNwPlayer()!,
          Target = target.m_pOwner.m_idSelf.ToNwPlayer()!,
          Complete = false,
          InitiatorItems = ImmutableArray<NwItem>.Empty,
          TargetItems = ImmutableArray<NwItem>.Empty,
        };
      }

      private static OnBarterEnd? GetBarterEventData(CNWSBarter barter, bool accepted)
      {
        NwCreature? other = barter.m_oidBarrator.ToNwObject<NwCreature>();
        if (other == null)
        {
          return null;
        }

        CNWSBarter otherBarter = other.Creature.GetBarterInfo(0);
        CNWSBarter initiatorBarter = barter.m_bInitiator.ToBool() ? barter : otherBarter;
        CNWSBarter targetBarter = barter.m_bInitiator.ToBool() ? otherBarter : barter;

        return accepted ? GetBarterAcceptedEventData(otherBarter, initiatorBarter, targetBarter) : GetBarterCancelledEventData(initiatorBarter, targetBarter);
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
          NwItem? item = (*(uint*)node.pObject).ToNwObject<NwItem>();
          if (item != null)
          {
            items.Add(item);
          }
        }

        return items.AsReadOnly();
      }

      [UnmanagedCallersOnly]
      private static int OnSendServerToPlayerBarterCloseBarter(void* pMessage, uint nInitiatorId, uint nRecipientId, int bAccepted)
      {
        NwPlayer? player = LowLevel.ServerExoApp.GetClientObjectByPlayerId(nInitiatorId).AsNWSPlayer().ToNwPlayer();
        if (player == null)
        {
          return sendServerToPlayerBarterCloseBarterHook.CallOriginal(pMessage, nInitiatorId, nRecipientId, bAccepted);
        }

        CNWSBarter? barter = player.ControlledCreature?.Creature.GetBarterInfo(0);

        // We only need to run the END on a CANCEL BARTER for the initiator
        OnBarterEnd? eventData = null;
        if (barter != null && barter.m_bInitiator.ToBool() && !bAccepted.ToBool())
        {
          eventData = GetBarterEventData(barter, bAccepted.ToBool());
        }

        ProcessEvent(EventCallbackType.Before, eventData);
        int retVal = sendServerToPlayerBarterCloseBarterHook.CallOriginal(pMessage, nInitiatorId, nRecipientId, bAccepted);
        ProcessEvent(EventCallbackType.After, eventData);

        return retVal;
      }

      [UnmanagedCallersOnly]
      private static int OnSetListAccepted(void* pBarter, int bAccepted)
      {
        OnBarterEnd? eventData = null;
        if (pBarter != null && bAccepted.ToBool())
        {
          eventData = GetBarterEventData(CNWSBarter.FromPointer(pBarter), bAccepted.ToBool());
        }

        ProcessEvent(EventCallbackType.Before, eventData);
        int retVal = setListAcceptedHook.CallOriginal(pBarter, bAccepted);
        ProcessEvent(EventCallbackType.After, eventData);

        return retVal;
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="Events.OnBarterEnd"/>
    public event Action<OnBarterEnd> OnBarterEnd
    {
      add => EventService.Subscribe<OnBarterEnd, OnBarterEnd.Factory>(ControlledCreature, value);
      remove => EventService.Unsubscribe<OnBarterEnd, OnBarterEnd.Factory>(ControlledCreature, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnBarterEnd"/>
    public event Action<OnBarterEnd> OnBarterEnd
    {
      add => EventService.SubscribeAll<OnBarterEnd, OnBarterEnd.Factory>(value);
      remove => EventService.UnsubscribeAll<OnBarterEnd, OnBarterEnd.Factory>(value);
    }
  }
}
