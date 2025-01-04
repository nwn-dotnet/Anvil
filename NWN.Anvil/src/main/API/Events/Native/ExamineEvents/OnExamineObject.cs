using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Native;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnExamineObject : IEvent
  {
    public NwPlayer ExaminedBy { get; private init; } = null!;

    public NwGameObject ExaminedObject { get; private init; } = null!;

    NwObject? IEvent.Context => ExaminedBy.ControlledCreature;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<Functions.CNWSMessage.SendServerToPlayerExamineGui_CreatureData> creatureExamineHook = null!;
      private static FunctionHook<Functions.CNWSMessage.SendServerToPlayerExamineGui_DoorData> doorExamineHook = null!;
      private static FunctionHook<Functions.CNWSMessage.SendServerToPlayerExamineGui_ItemData> itemExamineHook = null!;
      private static FunctionHook<Functions.CNWSMessage.SendServerToPlayerExamineGui_PlaceableData> placeableExamineHook = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void*, uint, int> pCreatureExamineHook = &OnCreatureExamine;
        creatureExamineHook = HookService.RequestHook<Functions.CNWSMessage.SendServerToPlayerExamineGui_CreatureData>(pCreatureExamineHook, HookOrder.Earliest);

        delegate* unmanaged<void*, void*, uint, int> pDoorExamineHook = &OnDoorExamine;
        doorExamineHook = HookService.RequestHook<Functions.CNWSMessage.SendServerToPlayerExamineGui_DoorData>(pDoorExamineHook, HookOrder.Earliest);

        delegate* unmanaged<void*, void*, uint, int> pItemExamineHook = &OnItemExamine;
        itemExamineHook = HookService.RequestHook<Functions.CNWSMessage.SendServerToPlayerExamineGui_ItemData>(pItemExamineHook, HookOrder.Earliest);

        delegate* unmanaged<void*, void*, uint, int> pPlaceableExamineHook = &OnPlaceableExamine;
        placeableExamineHook = HookService.RequestHook<Functions.CNWSMessage.SendServerToPlayerExamineGui_PlaceableData>(pPlaceableExamineHook, HookOrder.Earliest);

        return [creatureExamineHook, doorExamineHook, itemExamineHook, placeableExamineHook];
      }

      [UnmanagedCallersOnly]
      private static int OnCreatureExamine(void* pMessage, void* pPlayer, uint oidCreature)
      {
        OnExamineObject eventData = ProcessEvent(EventCallbackType.Before, new OnExamineObject
        {
          ExaminedBy = CNWSPlayer.FromPointer(pPlayer).ToNwPlayer()!,
          ExaminedObject = oidCreature.ToNwObject<NwCreature>()!,
        });

        int retVal = creatureExamineHook.CallOriginal(pMessage, pPlayer, oidCreature);
        ProcessEvent(EventCallbackType.After, eventData);

        return retVal;
      }

      [UnmanagedCallersOnly]
      private static int OnDoorExamine(void* pMessage, void* pPlayer, uint oidDoor)
      {
        OnExamineObject eventData = ProcessEvent(EventCallbackType.Before, new OnExamineObject
        {
          ExaminedBy = CNWSPlayer.FromPointer(pPlayer).ToNwPlayer()!,
          ExaminedObject = oidDoor.ToNwObject<NwDoor>()!,
        });

        int retVal = doorExamineHook.CallOriginal(pMessage, pPlayer, oidDoor);
        ProcessEvent(EventCallbackType.After, eventData);

        return retVal;
      }

      [UnmanagedCallersOnly]
      private static int OnItemExamine(void* pMessage, void* pPlayer, uint oidItem)
      {
        OnExamineObject eventData = ProcessEvent(EventCallbackType.Before, new OnExamineObject
        {
          ExaminedBy = CNWSPlayer.FromPointer(pPlayer).ToNwPlayer()!,
          ExaminedObject = oidItem.ToNwObject<NwItem>()!,
        });

        int retVal = itemExamineHook.CallOriginal(pMessage, pPlayer, oidItem);
        ProcessEvent(EventCallbackType.After, eventData);

        return retVal;
      }

      [UnmanagedCallersOnly]
      private static int OnPlaceableExamine(void* pMessage, void* pPlayer, uint oidPlaceable)
      {
        OnExamineObject eventData = ProcessEvent(EventCallbackType.Before, new OnExamineObject
        {
          ExaminedBy = CNWSPlayer.FromPointer(pPlayer).ToNwPlayer()!,
          ExaminedObject = oidPlaceable.ToNwObject<NwPlaceable>()!,
        });

        int retVal = placeableExamineHook.CallOriginal(pMessage, pPlayer, oidPlaceable);
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
    /// <inheritdoc cref="Events.OnExamineObject"/>
    public event Action<OnExamineObject> OnExamineObject
    {
      add => EventService.Subscribe<OnExamineObject, OnExamineObject.Factory>(ControlledCreature, value);
      remove => EventService.Unsubscribe<OnExamineObject, OnExamineObject.Factory>(ControlledCreature, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnExamineObject"/>
    public event Action<OnExamineObject> OnExamineObject
    {
      add => EventService.SubscribeAll<OnExamineObject, OnExamineObject.Factory>(value);
      remove => EventService.UnsubscribeAll<OnExamineObject, OnExamineObject.Factory>(value);
    }
  }
}
