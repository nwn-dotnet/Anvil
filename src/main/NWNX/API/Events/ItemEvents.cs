using NWN.API;
using NWN.API.Constants;
using NWN.API.Events;
using NWN.Core;
using NWN.Core.NWNX;

namespace NWNX.API.Events
{
  public static class ItemEvents
  {
    [NWNXEvent("NWNX_ON_VALIDATE_USE_ITEM_AFTER")]
    public sealed class OnValidateUseItemAfter : Event<OnValidateUseItemAfter>
    {
      public NwCreature Creature { get; private set; }
      public NwItem Item { get; private set; }

      public bool Result { get; set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Creature = (NwCreature) objSelf;
        Item = NWScript.StringToObject(EventsPlugin.GetEventData("ITEM_OBJECT_ID")).ToNwObject<NwItem>();
        Result = EventsPlugin.GetEventData("BEFORE_RESULT").ParseInt().ToBool();
      }

      protected override void InvokeCallbacks()
      {
        base.InvokeCallbacks();
        EventsPlugin.SetEventResult(Result.ToInt().ToString());
      }
    }

    [NWNXEvent("NWNX_ON_VALIDATE_ITEM_EQUIP_AFTER")]
    public sealed class OnValidateEquipItemAfter : Event<OnValidateEquipItemAfter>
    {
      public NwCreature Creature { get; private set; }
      public NwItem Item { get; private set; }
      public InventorySlot Slot { get; private set; }

      public bool Result { get; set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Creature = (NwCreature) objSelf;
        Item = NWScript.StringToObject(EventsPlugin.GetEventData("ITEM_OBJECT_ID")).ToNwObject<NwItem>();
        Slot = (InventorySlot) EventsPlugin.GetEventData("SLOT").ParseInt();
        Result = EventsPlugin.GetEventData("BEFORE_RESULT").ParseInt().ToBool();
      }

      protected override void InvokeCallbacks()
      {
        base.InvokeCallbacks();
        EventsPlugin.SetEventResult(Result.ToInt().ToString());
      }
    }

    [NWNXEvent("NWNX_ON_USE_ITEM_BEFORE")]
    public sealed class OnItemUseBefore : EventSkippable<OnItemUseBefore>
    {
      public NwCreature Creature { get; private set; }
      public NwItem Item { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Creature = (NwCreature) objSelf;
        Item = NWScript.StringToObject(EventsPlugin.GetEventData("ITEM_OBJECT_ID")).ToNwObject<NwItem>();
      }
    }

    [NWNXEvent("NWNX_ON_ITEM_EQUIP_BEFORE")]
    public sealed class OnItemEquipBefore : EventSkippable<OnItemEquipBefore>
    {
      public NwCreature Creature { get; private set; }
      public NwItem Item { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Creature = (NwCreature) objSelf;
        Item = NWScript.StringToObject(EventsPlugin.GetEventData("ITEM")).ToNwObject<NwItem>();
      }
    }
  }
}