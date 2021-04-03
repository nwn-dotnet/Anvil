using System;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public class OnExamineObject : IEvent
  {
    public NwPlayer ExaminedBy { get; private init; }

    public NwGameObject ExaminedObject { get; private init; }

    NwObject IEvent.Context => ExaminedBy;

    [NativeFunction(NWNXLib.Functions._ZN11CNWSMessage41SendServerToPlayerExamineGui_CreatureDataEP10CNWSPlayerj)]
    internal delegate void CreatureExamineHook(IntPtr pMessage, IntPtr pPlayer, uint creature);

    [NativeFunction(NWNXLib.Functions._ZN11CNWSMessage37SendServerToPlayerExamineGui_DoorDataEP10CNWSPlayerj)]
    internal delegate void DoorExamineHook(IntPtr pMessage, IntPtr pPlayer, uint door);

    [NativeFunction(NWNXLib.Functions._ZN11CNWSMessage37SendServerToPlayerExamineGui_ItemDataEP10CNWSPlayerj)]
    internal delegate void ItemExamineHook(IntPtr pMessage, IntPtr pPlayer, uint item);

    [NativeFunction(NWNXLib.Functions._ZN11CNWSMessage42SendServerToPlayerExamineGui_PlaceableDataEP10CNWSPlayerj)]
    internal delegate void PlaceableExamineHook(IntPtr pMessage, IntPtr pPlayer, uint placeable);

    public static Type[] FactoryTypes { get; } = {typeof(CreatureEventFactory), typeof(DoorEventFactory), typeof(ItemEventFactory), typeof(PlaceableEventFactory)};

    internal class CreatureEventFactory : NativeEventFactory<CreatureExamineHook>
    {
      public CreatureEventFactory(Lazy<EventService> eventService, HookService hookService) : base(eventService, hookService) {}

      protected override FunctionHook<CreatureExamineHook> RequestHook(HookService hookService)
        => hookService.RequestHook<CreatureExamineHook>(OnCreatureExamine, HookOrder.Earliest);

      private void OnCreatureExamine(IntPtr pMessage, IntPtr pPlayer, uint creature)
      {
        ProcessEvent(new OnExamineObject()
        {
          ExaminedBy = new NwPlayer(new CNWSPlayer(pPlayer, false)),
          ExaminedObject = creature.ToNwObject<NwCreature>()
        });

        Hook.CallOriginal(pMessage, pPlayer, creature);
      }
    }

    internal class DoorEventFactory : NativeEventFactory<DoorExamineHook>
    {
      public DoorEventFactory(Lazy<EventService> eventService, HookService hookService) : base(eventService, hookService) {}

      protected override FunctionHook<DoorExamineHook> RequestHook(HookService hookService)
        => hookService.RequestHook<DoorExamineHook>(OnDoorExamine, HookOrder.Earliest);

      private void OnDoorExamine(IntPtr pMessage, IntPtr pPlayer, uint door)
      {
        ProcessEvent(new OnExamineObject()
        {
          ExaminedBy = new NwPlayer(new CNWSPlayer(pPlayer, false)),
          ExaminedObject = door.ToNwObject<NwDoor>()
        });

        Hook.CallOriginal(pMessage, pPlayer, door);
      }
    }

    internal class ItemEventFactory : NativeEventFactory<ItemExamineHook>
    {
      public ItemEventFactory(Lazy<EventService> eventService, HookService hookService) : base(eventService, hookService) {}

      protected override FunctionHook<ItemExamineHook> RequestHook(HookService hookService)
        => hookService.RequestHook<ItemExamineHook>(OnItemExamine, HookOrder.Earliest);

      private void OnItemExamine(IntPtr pMessage, IntPtr pPlayer, uint item)
      {
        ProcessEvent(new OnExamineObject()
        {
          ExaminedBy = new NwPlayer(new CNWSPlayer(pPlayer, false)),
          ExaminedObject = item.ToNwObject<NwItem>()
        });

        Hook.CallOriginal(pMessage, pPlayer, item);
      }
    }

    internal class PlaceableEventFactory : NativeEventFactory<PlaceableExamineHook>
    {
      public PlaceableEventFactory(Lazy<EventService> eventService, HookService hookService) : base(eventService, hookService) {}

      protected override FunctionHook<PlaceableExamineHook> RequestHook(HookService hookService)
        => hookService.RequestHook<PlaceableExamineHook>(OnPlaceableExamine, HookOrder.Earliest);

      private void OnPlaceableExamine(IntPtr pMessage, IntPtr pPlayer, uint placeable)
      {
        ProcessEvent(new OnExamineObject()
        {
          ExaminedBy = new NwPlayer(new CNWSPlayer(pPlayer, false)),
          ExaminedObject = placeable.ToNwObject<NwPlaceable>()
        });

        Hook.CallOriginal(pMessage, pPlayer, placeable);
      }
    }
  }
}
