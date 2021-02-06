using System.Numerics;
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

      protected override void ProcessEvent()
      {
        InvokeCallbacks();
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

      protected override void ProcessEvent()
      {
        InvokeCallbacks();
        EventsPlugin.SetEventResult(Result.ToInt().ToString());
      }
    }

    [NWNXEvent("NWNX_ON_USE_ITEM_BEFORE")]
    public sealed class OnItemUseBefore : NWNXEventSkippable<OnItemUseBefore>
    {
      public NwCreature Creature { get; private set; }

      public NwItem Item { get; private set; }

      public NwGameObject TargetObject { get; private set; }

      public int ItemPropertyIndex { get; private set; }

      public int ItemSubPropertyIndex { get; private set; }

      public Vector3 TargetPosition { get; private set; }

      public int UseCharges { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Creature = (NwCreature)objSelf;
        Item = EventsPlugin.GetEventData("ITEM_OBJECT_ID").ParseObject<NwItem>();
        TargetObject = EventsPlugin.GetEventData("TARGET_OBJECT_ID").ParseObject<NwGameObject>();
        ItemPropertyIndex = EventsPlugin.GetEventData("ITEM_PROPERTY_INDEX").ParseInt();
        ItemSubPropertyIndex = EventsPlugin.GetEventData("ITEM_SUB_PROPERTY_INDEX").ParseInt();
        float targetX = EventsPlugin.GetEventData("TARGET_POSITION_X").ParseFloat();
        float targetY = EventsPlugin.GetEventData("TARGET_POSITION_Y").ParseFloat();
        float targetZ = EventsPlugin.GetEventData("TARGET_POSITION_Z").ParseFloat();
        TargetPosition = new Vector3(targetX, targetY, targetZ);
        UseCharges = EventsPlugin.GetEventData("USE_CHARGES").ParseInt();
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
