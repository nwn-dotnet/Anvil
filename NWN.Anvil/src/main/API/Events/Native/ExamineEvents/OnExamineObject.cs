using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
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
      private static FunctionHook<CreatureExamineHook> creatureExamineHook = null!;
      private static FunctionHook<DoorExamineHook> doorExamineHook = null!;
      private static FunctionHook<ItemExamineHook> itemExamineHook = null!;
      private static FunctionHook<PlaceableExamineHook> placeableExamineHook = null!;

      [NativeFunction("_ZN11CNWSMessage41SendServerToPlayerExamineGui_CreatureDataEP10CNWSPlayerj", "")]
      private delegate void CreatureExamineHook(void* pMessage, void* pPlayer, uint oidCreature);

      [NativeFunction("_ZN11CNWSMessage37SendServerToPlayerExamineGui_DoorDataEP10CNWSPlayerj", "")]
      private delegate void DoorExamineHook(void* pMessage, void* pPlayer, uint oidDoor);

      [NativeFunction("_ZN11CNWSMessage37SendServerToPlayerExamineGui_ItemDataEP10CNWSPlayerj", "")]
      private delegate void ItemExamineHook(void* pMessage, void* pPlayer, uint oidItem);

      [NativeFunction("_ZN11CNWSMessage42SendServerToPlayerExamineGui_PlaceableDataEP10CNWSPlayerj", "")]
      private delegate void PlaceableExamineHook(void* pMessage, void* pPlayer, uint oidPlaceable);

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void*, uint, void> pCreatureExamineHook = &OnCreatureExamine;
        creatureExamineHook = HookService.RequestHook<CreatureExamineHook>(pCreatureExamineHook, HookOrder.Earliest);

        delegate* unmanaged<void*, void*, uint, void> pDoorExamineHook = &OnDoorExamine;
        doorExamineHook = HookService.RequestHook<DoorExamineHook>(pDoorExamineHook, HookOrder.Earliest);

        delegate* unmanaged<void*, void*, uint, void> pItemExamineHook = &OnItemExamine;
        itemExamineHook = HookService.RequestHook<ItemExamineHook>(pItemExamineHook, HookOrder.Earliest);

        delegate* unmanaged<void*, void*, uint, void> pPlaceableExamineHook = &OnPlaceableExamine;
        placeableExamineHook = HookService.RequestHook<PlaceableExamineHook>(pPlaceableExamineHook, HookOrder.Earliest);

        return new IDisposable[] { creatureExamineHook, doorExamineHook, itemExamineHook, placeableExamineHook };
      }

      [UnmanagedCallersOnly]
      private static void OnCreatureExamine(void* pMessage, void* pPlayer, uint oidCreature)
      {
        OnExamineObject eventData = ProcessEvent(EventCallbackType.Before, new OnExamineObject
        {
          ExaminedBy = CNWSPlayer.FromPointer(pPlayer).ToNwPlayer()!,
          ExaminedObject = oidCreature.ToNwObject<NwCreature>()!,
        });

        creatureExamineHook.CallOriginal(pMessage, pPlayer, oidCreature);
        ProcessEvent(EventCallbackType.After, eventData);
      }

      [UnmanagedCallersOnly]
      private static void OnDoorExamine(void* pMessage, void* pPlayer, uint oidDoor)
      {
        OnExamineObject eventData = ProcessEvent(EventCallbackType.Before, new OnExamineObject
        {
          ExaminedBy = CNWSPlayer.FromPointer(pPlayer).ToNwPlayer()!,
          ExaminedObject = oidDoor.ToNwObject<NwDoor>()!,
        });

        doorExamineHook.CallOriginal(pMessage, pPlayer, oidDoor);
        ProcessEvent(EventCallbackType.After, eventData);
      }

      [UnmanagedCallersOnly]
      private static void OnItemExamine(void* pMessage, void* pPlayer, uint oidItem)
      {
        OnExamineObject eventData = ProcessEvent(EventCallbackType.Before, new OnExamineObject
        {
          ExaminedBy = CNWSPlayer.FromPointer(pPlayer).ToNwPlayer()!,
          ExaminedObject = oidItem.ToNwObject<NwItem>()!,
        });

        itemExamineHook.CallOriginal(pMessage, pPlayer, oidItem);
        ProcessEvent(EventCallbackType.After, eventData);
      }

      [UnmanagedCallersOnly]
      private static void OnPlaceableExamine(void* pMessage, void* pPlayer, uint oidPlaceable)
      {
        OnExamineObject eventData = ProcessEvent(EventCallbackType.Before, new OnExamineObject
        {
          ExaminedBy = CNWSPlayer.FromPointer(pPlayer).ToNwPlayer()!,
          ExaminedObject = oidPlaceable.ToNwObject<NwPlaceable>()!,
        });

        placeableExamineHook.CallOriginal(pMessage, pPlayer, oidPlaceable);
        ProcessEvent(EventCallbackType.After, eventData);
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
