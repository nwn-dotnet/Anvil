using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Native;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  /// <summary>
  /// Called when a player adds an item to a barter.
  /// </summary>
  public sealed class OnBarterAddItem : IEvent
  {
    /// <summary>
    /// Gets the player adding the item.
    /// </summary>
    public NwPlayer Initiator { get; private init; } = null!;

    /// <summary>
    /// Gets the item being added.
    /// </summary>
    public NwItem Item { get; private init; } = null!;

    /// <summary>
    /// Gets or sets whether adding the item should be skipped.
    /// </summary>
    public bool Skip { get; set; }

    /// <summary>
    /// Gets the other player in the barter.
    /// </summary>
    public NwPlayer Target { get; private init; } = null!;

    NwObject? IEvent.Context => Initiator.ControlledCreature;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<Functions.CNWSMessage.HandlePlayerToServerBarter_AddItem> Hook { get; set; } = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void*, int> pHook = &OnHandleBarterAddItem;
        Hook = HookService.RequestHook<Functions.CNWSMessage.HandlePlayerToServerBarter_AddItem>(pHook, HookOrder.Early);
        return [Hook];
      }

      [UnmanagedCallersOnly]
      private static int OnHandleBarterAddItem(void* pMessage, void* pPlayer)
      {
        CNWSMessage message = CNWSMessage.FromPointer(pMessage);
        NwPlayer? initiator = CNWSPlayer.FromPointer(pPlayer).ToNwPlayer();
        CNWSBarter? barter = initiator?.ControlledCreature?.Creature.GetBarterInfo(0);
        NwPlayer? target = barter?.m_oidBarrator.ToNwObject<NwCreature>()?.ControllingPlayer;
        NwItem? item = (message.PeekMessage<uint>(0) & 0x7FFFFFFF).ToNwObject<NwItem>();

        if (initiator == null || target == null || item == null)
        {
          return Hook.CallOriginal(pMessage, pPlayer);
        }

        OnBarterAddItem eventData = ProcessEvent(EventCallbackType.Before, new OnBarterAddItem
        {
          Initiator = initiator,
          Item = item,
          Target = target,
        });

        int retVal = eventData.Skip ? false.ToInt() : Hook.CallOriginal(pMessage, pPlayer);
        if (eventData.Skip)
        {
          message.ClearReadMessage();
        }

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
    /// <inheritdoc cref="Events.OnBarterAddItem"/>
    public event Action<OnBarterAddItem> OnBarterAddItem
    {
      add => EventService.Subscribe<OnBarterAddItem, OnBarterAddItem.Factory>(ControlledCreature, value);
      remove => EventService.Unsubscribe<OnBarterAddItem, OnBarterAddItem.Factory>(ControlledCreature, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnBarterAddItem"/>
    public event Action<OnBarterAddItem> OnBarterAddItem
    {
      add => EventService.SubscribeAll<OnBarterAddItem, OnBarterAddItem.Factory>(value);
      remove => EventService.UnsubscribeAll<OnBarterAddItem, OnBarterAddItem.Factory>(value);
    }
  }
}
