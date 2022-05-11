using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnExamineObject : IEvent
  {
    public NwPlayer ExaminedBy { get; private init; }

    public NwGameObject ExaminedObject { get; private init; }

    NwObject? IEvent.Context => ExaminedBy.ControlledCreature;

    internal sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<CreatureExamineHook> creatureExamineHook;
      private static FunctionHook<DoorExamineHook> doorExamineHook;
      private static FunctionHook<ItemExamineHook> itemExamineHook;
      private static FunctionHook<PlaceableExamineHook> placeableExamineHook;

      private delegate void CreatureExamineHook(void* pMessage, void* pPlayer, uint oidCreature);

      private delegate void DoorExamineHook(void* pMessage, void* pPlayer, uint oidDoor);

      private delegate void ItemExamineHook(void* pMessage, void* pPlayer, uint oidItem);

      private delegate void PlaceableExamineHook(void* pMessage, void* pPlayer, uint oidPlaceable);

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void*, uint, void> pCreatureExamineHook = &OnCreatureExamine;
        creatureExamineHook = HookService.RequestHook<CreatureExamineHook>(pCreatureExamineHook, FunctionsLinux._ZN11CNWSMessage41SendServerToPlayerExamineGui_CreatureDataEP10CNWSPlayerj, HookOrder.Earliest);

        delegate* unmanaged<void*, void*, uint, void> pDoorExamineHook = &OnDoorExamine;
        doorExamineHook = HookService.RequestHook<DoorExamineHook>(pDoorExamineHook, FunctionsLinux._ZN11CNWSMessage37SendServerToPlayerExamineGui_DoorDataEP10CNWSPlayerj, HookOrder.Earliest);

        delegate* unmanaged<void*, void*, uint, void> pItemExamineHook = &OnItemExamine;
        itemExamineHook = HookService.RequestHook<ItemExamineHook>(pItemExamineHook, FunctionsLinux._ZN11CNWSMessage37SendServerToPlayerExamineGui_ItemDataEP10CNWSPlayerj, HookOrder.Earliest);

        delegate* unmanaged<void*, void*, uint, void> pPlaceableExamineHook = &OnPlaceableExamine;
        placeableExamineHook = HookService.RequestHook<PlaceableExamineHook>(pPlaceableExamineHook, FunctionsLinux._ZN11CNWSMessage42SendServerToPlayerExamineGui_PlaceableDataEP10CNWSPlayerj, HookOrder.Earliest);

        return new IDisposable[] { creatureExamineHook, doorExamineHook, itemExamineHook, placeableExamineHook };
      }

      [UnmanagedCallersOnly]
      private static void OnCreatureExamine(void* pMessage, void* pPlayer, uint oidCreature)
      {
        ProcessEvent(new OnExamineObject
        {
          ExaminedBy = CNWSPlayer.FromPointer(pPlayer).ToNwPlayer()!,
          ExaminedObject = oidCreature.ToNwObject<NwCreature>()!,
        });

        creatureExamineHook.CallOriginal(pMessage, pPlayer, oidCreature);
      }

      [UnmanagedCallersOnly]
      private static void OnDoorExamine(void* pMessage, void* pPlayer, uint oidDoor)
      {
        ProcessEvent(new OnExamineObject
        {
          ExaminedBy = CNWSPlayer.FromPointer(pPlayer).ToNwPlayer()!,
          ExaminedObject = oidDoor.ToNwObject<NwDoor>()!,
        });

        doorExamineHook.CallOriginal(pMessage, pPlayer, oidDoor);
      }

      [UnmanagedCallersOnly]
      private static void OnItemExamine(void* pMessage, void* pPlayer, uint oidItem)
      {
        ProcessEvent(new OnExamineObject
        {
          ExaminedBy = CNWSPlayer.FromPointer(pPlayer).ToNwPlayer()!,
          ExaminedObject = oidItem.ToNwObject<NwItem>()!,
        });

        itemExamineHook.CallOriginal(pMessage, pPlayer, oidItem);
      }

      [UnmanagedCallersOnly]
      private static void OnPlaceableExamine(void* pMessage, void* pPlayer, uint oidPlaceable)
      {
        ProcessEvent(new OnExamineObject
        {
          ExaminedBy = CNWSPlayer.FromPointer(pPlayer).ToNwPlayer()!,
          ExaminedObject = oidPlaceable.ToNwObject<NwPlaceable>()!,
        });

        placeableExamineHook.CallOriginal(pMessage, pPlayer, oidPlaceable);
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
