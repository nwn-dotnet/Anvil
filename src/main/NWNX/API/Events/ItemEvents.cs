using NWN.API;
using NWN.API.Constants;
using NWN.Core.NWNX;

namespace NWNX.API.Events
{
  public static partial class ItemEvents
  {
    [NWNXEvent("NWNX_ON_VALIDATE_USE_ITEM_AFTER")]
    public sealed class OnValidateUseItemAfter : NWNXEvent<OnValidateUseItemAfter>
    {
      public NwCreature Creature { get; private set; }

      public NwItem Item { get; private set; }

      public bool Result { get; set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Creature = (NwCreature)objSelf;
        Item = EventsPlugin.GetEventData("ITEM_OBJECT_ID").ParseObject<NwItem>();
        Result = EventsPlugin.GetEventData("BEFORE_RESULT").ParseInt().ToBool();
      }

      protected override void InvokeCallbacks()
      {
        base.InvokeCallbacks();
        EventsPlugin.SetEventResult(Result.ToInt().ToString());
      }
    }

    [NWNXEvent("NWNX_ON_VALIDATE_ITEM_EQUIP_AFTER")]
    public sealed class OnValidateEquipItemAfter : NWNXEvent<OnValidateEquipItemAfter>
    {
      public NwCreature Creature { get; private set; }

      public NwItem Item { get; private set; }

      public InventorySlot Slot { get; private set; }

      public bool Result { get; set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Creature = (NwCreature)objSelf;
        Item = EventsPlugin.GetEventData("ITEM_OBJECT_ID").ParseObject<NwItem>();
        Slot = (InventorySlot)EventsPlugin.GetEventData("SLOT").ParseInt();
        Result = EventsPlugin.GetEventData("BEFORE_RESULT").ParseInt().ToBool();
      }

      protected override void InvokeCallbacks()
      {
        base.InvokeCallbacks();
        EventsPlugin.SetEventResult(Result.ToInt().ToString());
      }
    }

    [NWNXEvent("NWNX_ON_USE_ITEM_BEFORE")]
    public sealed class OnItemUseBefore : NWNXEventSkippable<OnItemUseBefore>
    {
      public NwCreature Creature { get; private set; }

      public NwItem Item { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Creature = (NwCreature)objSelf;
        Item = EventsPlugin.GetEventData("ITEM_OBJECT_ID").ParseObject<NwItem>();
      }
    }

    [NWNXEvent("NWNX_ON_ITEM_EQUIP_BEFORE")]
    public sealed class OnItemEquipBefore : NWNXEventSkippable<OnItemEquipBefore>
    {
      public NwCreature Creature { get; private set; }

      public NwItem Item { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Creature = (NwCreature)objSelf;
        Item = EventsPlugin.GetEventData("ITEM").ParseObject<NwItem>();
      }
    }

    [NWNXEvent("NWNX_ON_ITEM_PAY_TO_IDENTIFY_BEFORE")]
    public sealed class OnItemPayToIdentifyBefore : NWNXEventSkippable<OnItemPayToIdentifyBefore>
    {
      public NwCreature Creature { get; private set; }

      public NwItem Item { get; private set; }

      public NwStore Store { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Creature = (NwCreature)objSelf;
        Item = EventsPlugin.GetEventData("ITEM").ParseObject<NwItem>();
        Store = EventsPlugin.GetEventData("STORE").ParseObject<NwStore>();
      }
    }

    [NWNXEvent("NWNX_ON_ITEM_PAY_TO_IDENTIFY_AFTER")]
    public sealed class OnItemPayToIdentifyAfter : NWNXEventSkippable<OnItemPayToIdentifyAfter>
    {
      public NwCreature Creature { get; private set; }

      public NwItem Item { get; private set; }

      public NwStore Store { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Creature = (NwCreature)objSelf;
        Item = EventsPlugin.GetEventData("ITEM").ParseObject<NwItem>();
        Store = EventsPlugin.GetEventData("STORE").ParseObject<NwStore>();
      }
    }
  }
}
