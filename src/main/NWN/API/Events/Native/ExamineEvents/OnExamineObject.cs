using System;
using System.Runtime.InteropServices;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public sealed class OnExamineObject : IEvent
  {
    public NwPlayer ExaminedBy { get; private init; }

    public NwGameObject ExaminedObject { get; private init; }

    NwObject IEvent.Context => ExaminedBy;

    public static Type[] FactoryTypes { get; } = {typeof(CreatureEventFactory), typeof(DoorEventFactory), typeof(ItemEventFactory), typeof(PlaceableEventFactory)};

    internal sealed unsafe class CreatureEventFactory : NativeEventFactory<CreatureEventFactory.CreatureExamineHook>
    {
      internal delegate void CreatureExamineHook(void* pMessage, void* pPlayer, uint oidCreature);

      protected override FunctionHook<CreatureExamineHook> RequestHook()
      {
        delegate* unmanaged<void*, void*, uint, void> pHook = &OnCreatureExamine;
        return HookService.RequestHook<CreatureExamineHook>(pHook, FunctionsLinux._ZN11CNWSMessage41SendServerToPlayerExamineGui_CreatureDataEP10CNWSPlayerj, HookOrder.Earliest);
      }

      [UnmanagedCallersOnly]
      private static void OnCreatureExamine(void* pMessage, void* pPlayer, uint oidCreature)
      {
        ProcessEvent(new OnExamineObject
        {
          ExaminedBy = new NwPlayer(new CNWSPlayer(pPlayer, false)),
          ExaminedObject = oidCreature.ToNwObject<NwCreature>()
        });

        Hook.CallOriginal(pMessage, pPlayer, oidCreature);
      }
    }

    internal sealed unsafe class DoorEventFactory : NativeEventFactory<DoorEventFactory.DoorExamineHook>
    {
      internal delegate void DoorExamineHook(void* pMessage, void* pPlayer, uint oidDoor);

      protected override FunctionHook<DoorExamineHook> RequestHook()
      {
        delegate* unmanaged<void*, void*, uint, void> pHook = &OnDoorExamine;
        return HookService.RequestHook<DoorExamineHook>(pHook, FunctionsLinux._ZN11CNWSMessage37SendServerToPlayerExamineGui_DoorDataEP10CNWSPlayerj, HookOrder.Earliest);
      }

      [UnmanagedCallersOnly]
      private static void OnDoorExamine(void* pMessage, void* pPlayer, uint oidDoor)
      {
        ProcessEvent(new OnExamineObject
        {
          ExaminedBy = new NwPlayer(new CNWSPlayer(pPlayer, false)),
          ExaminedObject = oidDoor.ToNwObject<NwDoor>()
        });

        Hook.CallOriginal(pMessage, pPlayer, oidDoor);
      }
    }

    internal sealed unsafe class ItemEventFactory : NativeEventFactory<ItemEventFactory.ItemExamineHook>
    {
      internal delegate void ItemExamineHook(void* pMessage, void* pPlayer, uint oidItem);

      protected override FunctionHook<ItemExamineHook> RequestHook()
      {
        delegate* unmanaged<void*, void*, uint, void> pHook = &OnItemExamine;
        return HookService.RequestHook<ItemExamineHook>(pHook, FunctionsLinux._ZN11CNWSMessage37SendServerToPlayerExamineGui_ItemDataEP10CNWSPlayerj, HookOrder.Earliest);
      }

      [UnmanagedCallersOnly]
      private static void OnItemExamine(void* pMessage, void* pPlayer, uint oidItem)
      {
        ProcessEvent(new OnExamineObject
        {
          ExaminedBy = new NwPlayer(new CNWSPlayer(pPlayer, false)),
          ExaminedObject = oidItem.ToNwObject<NwItem>()
        });

        Hook.CallOriginal(pMessage, pPlayer, oidItem);
      }
    }

    internal sealed unsafe class PlaceableEventFactory : NativeEventFactory<PlaceableEventFactory.PlaceableExamineHook>
    {
      internal delegate void PlaceableExamineHook(void* pMessage, void* pPlayer, uint oidPlaceable);

      protected override FunctionHook<PlaceableExamineHook> RequestHook()
      {
        delegate* unmanaged<void*, void*, uint, void> pHook = &OnPlaceableExamine;
        return HookService.RequestHook<PlaceableExamineHook>(pHook, FunctionsLinux._ZN11CNWSMessage42SendServerToPlayerExamineGui_PlaceableDataEP10CNWSPlayerj, HookOrder.Earliest);
      }

      [UnmanagedCallersOnly]
      private static void OnPlaceableExamine(void* pMessage, void* pPlayer, uint oidPlaceable)
      {
        ProcessEvent(new OnExamineObject
        {
          ExaminedBy = new NwPlayer(new CNWSPlayer(pPlayer, false)),
          ExaminedObject = oidPlaceable.ToNwObject<NwPlaceable>()
        });

        Hook.CallOriginal(pMessage, pPlayer, oidPlaceable);
      }
    }
  }
}
