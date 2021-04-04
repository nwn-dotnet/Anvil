using System;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public sealed class OnExamineObject : IEvent
  {
    public NwPlayer ExaminedBy { get; private init; }

    public NwGameObject ExaminedObject { get; private init; }

    NwObject IEvent.Context => ExaminedBy;

    [NativeFunction(NWNXLib.Functions._ZN11CNWSMessage41SendServerToPlayerExamineGui_CreatureDataEP10CNWSPlayerj)]
    internal delegate void CreatureExamineHook(IntPtr pMessage, IntPtr pPlayer, uint oidCreature);

    [NativeFunction(NWNXLib.Functions._ZN11CNWSMessage37SendServerToPlayerExamineGui_DoorDataEP10CNWSPlayerj)]
    internal delegate void DoorExamineHook(IntPtr pMessage, IntPtr pPlayer, uint oidDoor);

    [NativeFunction(NWNXLib.Functions._ZN11CNWSMessage37SendServerToPlayerExamineGui_ItemDataEP10CNWSPlayerj)]
    internal delegate void ItemExamineHook(IntPtr pMessage, IntPtr pPlayer, uint oidItem);

    [NativeFunction(NWNXLib.Functions._ZN11CNWSMessage42SendServerToPlayerExamineGui_PlaceableDataEP10CNWSPlayerj)]
    internal delegate void PlaceableExamineHook(IntPtr pMessage, IntPtr pPlayer, uint oidPlaceable);

    public static Type[] FactoryTypes { get; } = {typeof(CreatureEventFactory), typeof(DoorEventFactory), typeof(ItemEventFactory), typeof(PlaceableEventFactory)};

    internal class CreatureEventFactory : NativeEventFactory<CreatureExamineHook>
    {
      public CreatureEventFactory(Lazy<EventService> eventService, HookService hookService) : base(eventService, hookService) {}

      protected override FunctionHook<CreatureExamineHook> RequestHook(HookService hookService)
        => hookService.RequestHook<CreatureExamineHook>(OnCreatureExamine, HookOrder.Earliest);

      private void OnCreatureExamine(IntPtr pMessage, IntPtr pPlayer, uint oidCreature)
      {
        ProcessEvent(new OnExamineObject
        {
          ExaminedBy = new NwPlayer(new CNWSPlayer(pPlayer, false)),
          ExaminedObject = oidCreature.ToNwObject<NwCreature>()
        });

        Hook.CallOriginal(pMessage, pPlayer, oidCreature);
      }
    }

    internal class DoorEventFactory : NativeEventFactory<DoorExamineHook>
    {
      public DoorEventFactory(Lazy<EventService> eventService, HookService hookService) : base(eventService, hookService) {}

      protected override FunctionHook<DoorExamineHook> RequestHook(HookService hookService)
        => hookService.RequestHook<DoorExamineHook>(OnDoorExamine, HookOrder.Earliest);

      private void OnDoorExamine(IntPtr pMessage, IntPtr pPlayer, uint oidDoor)
      {
        ProcessEvent(new OnExamineObject
        {
          ExaminedBy = new NwPlayer(new CNWSPlayer(pPlayer, false)),
          ExaminedObject = oidDoor.ToNwObject<NwDoor>()
        });

        Hook.CallOriginal(pMessage, pPlayer, oidDoor);
      }
    }

    internal class ItemEventFactory : NativeEventFactory<ItemExamineHook>
    {
      public ItemEventFactory(Lazy<EventService> eventService, HookService hookService) : base(eventService, hookService) {}

      protected override FunctionHook<ItemExamineHook> RequestHook(HookService hookService)
        => hookService.RequestHook<ItemExamineHook>(OnItemExamine, HookOrder.Earliest);

      private void OnItemExamine(IntPtr pMessage, IntPtr pPlayer, uint oidItem)
      {
        ProcessEvent(new OnExamineObject
        {
          ExaminedBy = new NwPlayer(new CNWSPlayer(pPlayer, false)),
          ExaminedObject = oidItem.ToNwObject<NwItem>()
        });

        Hook.CallOriginal(pMessage, pPlayer, oidItem);
      }
    }

    internal class PlaceableEventFactory : NativeEventFactory<PlaceableExamineHook>
    {
      public PlaceableEventFactory(Lazy<EventService> eventService, HookService hookService) : base(eventService, hookService) {}

      protected override FunctionHook<PlaceableExamineHook> RequestHook(HookService hookService)
        => hookService.RequestHook<PlaceableExamineHook>(OnPlaceableExamine, HookOrder.Earliest);

      private void OnPlaceableExamine(IntPtr pMessage, IntPtr pPlayer, uint oidPlaceable)
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
