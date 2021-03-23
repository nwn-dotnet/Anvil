using System.Numerics;
using NWN.API;
using NWN.API.Constants;
using NWN.API.Events;
using NWN.Core;
using NWN.Core.NWNX;

namespace NWNX.API.Events
{
  public static partial class ItemEvents
  {
    [NWNXEvent("NWNX_ON_VALIDATE_USE_ITEM_AFTER")]
    public sealed class OnValidateUseItemAfter : IEventNWNXResult
    {
      public NwCreature Creature { get; } = NWScript.OBJECT_SELF.ToNwObject<NwCreature>();

      public NwItem Item { get; } = EventsPlugin.GetEventData("ITEM_OBJECT_ID").ParseObject<NwItem>();

      public bool Result { get; set; } = EventsPlugin.GetEventData("BEFORE_RESULT").ParseInt().ToBool();

      NwObject IEvent.Context => Creature;

      string IEventNWNXResult.EventResult => Result.ToString();
    }

    [NWNXEvent("NWNX_ON_VALIDATE_ITEM_EQUIP_AFTER")]
    public sealed class OnValidateEquipItemAfter : IEventNWNXResult
    {
      public NwCreature Creature { get; } = NWScript.OBJECT_SELF.ToNwObject<NwCreature>();

      public NwItem Item { get; } = EventsPlugin.GetEventData("ITEM_OBJECT_ID").ParseObject<NwItem>();

      public InventorySlot Slot { get; } = (InventorySlot)EventsPlugin.GetEventData("SLOT").ParseInt();

      public bool Result { get; set; } = EventsPlugin.GetEventData("BEFORE_RESULT").ParseInt().ToBool();

      NwObject IEvent.Context => Creature;

      string IEventNWNXResult.EventResult => Result.ToString();
    }

    [NWNXEvent("NWNX_ON_USE_ITEM_BEFORE")]
    public sealed class OnItemUseBefore : IEventSkippable
    {
      public NwCreature Creature { get; } = NWScript.OBJECT_SELF.ToNwObject<NwCreature>();

      public NwItem Item { get; } = EventsPlugin.GetEventData("ITEM_OBJECT_ID").ParseObject<NwItem>();

      public NwGameObject TargetObject { get; } = EventsPlugin.GetEventData("TARGET_OBJECT_ID").ParseObject<NwGameObject>();

      public int ItemPropertyIndex { get; } = EventsPlugin.GetEventData("ITEM_PROPERTY_INDEX").ParseInt();

      public int ItemSubPropertyIndex { get; } = EventsPlugin.GetEventData("ITEM_SUB_PROPERTY_INDEX").ParseInt();

      public int UseCharges { get; } = EventsPlugin.GetEventData("USE_CHARGES").ParseInt();

      public Vector3 TargetPosition { get; }

      public bool Skip { get; set; }

      NwObject IEvent.Context => Creature;

      public OnItemUseBefore()
      {
        float targetX = EventsPlugin.GetEventData("TARGET_POSITION_X").ParseFloat();
        float targetY = EventsPlugin.GetEventData("TARGET_POSITION_Y").ParseFloat();
        float targetZ = EventsPlugin.GetEventData("TARGET_POSITION_Z").ParseFloat();
        TargetPosition = new Vector3(targetX, targetY, targetZ);
      }
    }

    [NWNXEvent("NWNX_ON_ITEM_EQUIP_BEFORE")]
    public sealed class OnItemEquipBefore : IEventSkippable
    {
      public NwCreature Creature { get; } = NWScript.OBJECT_SELF.ToNwObject<NwCreature>();

      public NwItem Item { get; } = EventsPlugin.GetEventData("ITEM").ParseObject<NwItem>();

      public bool Skip { get; set; }

      NwObject IEvent.Context => Creature;
    }

    [NWNXEvent("NWNX_ON_ITEM_PAY_TO_IDENTIFY_BEFORE")]
    public sealed class OnItemPayToIdentifyBefore : IEventSkippable
    {
      public NwCreature Creature { get; } = NWScript.OBJECT_SELF.ToNwObject<NwCreature>();

      public NwItem Item { get; } = EventsPlugin.GetEventData("ITEM").ParseObject<NwItem>();

      public NwStore Store { get; } = EventsPlugin.GetEventData("STORE").ParseObject<NwStore>();

      public bool Skip { get; set; }

      NwObject IEvent.Context => Creature;
    }

    [NWNXEvent("NWNX_ON_ITEM_PAY_TO_IDENTIFY_AFTER")]
    public sealed class OnItemPayToIdentifyAfter : IEventSkippable
    {
      public NwCreature Creature { get; } = NWScript.OBJECT_SELF.ToNwObject<NwCreature>();

      public NwItem Item { get; } = EventsPlugin.GetEventData("ITEM").ParseObject<NwItem>();

      public NwStore Store { get; } = EventsPlugin.GetEventData("STORE").ParseObject<NwStore>();

      public bool Skip { get; set; }

      NwObject IEvent.Context => Creature;
    }
  }
}
