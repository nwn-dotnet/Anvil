using System;
using NWM.API;
using NWM.API.Constants;
using NWM.API.Events;
using NWNX;

namespace NWMX.API.Events
{
  public sealed class ItemEvents
  {
    [NWNXEvent("NWNX_ON_VALIDATE_USE_ITEM_AFTER")]
    public class OnValidateUseItemAfter : IEvent<OnValidateUseItemAfter>
    {
      public NwCreature Creature { get; private set; }
      public NwItem Item { get; private set; }

      public bool Result { get; set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Creature = (NwCreature) objSelf;
        Item = ObjectPlugin.StringToObject(EventsPlugin.GetEventData("ITEM_OBJECT_ID")).ToNwObject<NwItem>();
        Result = EventsPlugin.GetEventData("BEFORE_RESULT").ToInt().ToBool();

        Callbacks?.Invoke(this);
        EventsPlugin.SetEventResult(Result.ToInt().ToString());
      }

      public event Action<OnValidateUseItemAfter> Callbacks;
    }

    [NWNXEvent("NWNX_ON_VALIDATE_ITEM_EQUIP_AFTER")]
    public class OnValidateEquipItemAfter : IEvent<OnValidateEquipItemAfter>
    {
      public NwCreature Creature { get; private set; }
      public NwItem Item { get; private set; }
      public InventorySlot Slot { get; private set; }

      public bool Result { get; set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Creature = (NwCreature) objSelf;
        Item = ObjectPlugin.StringToObject(EventsPlugin.GetEventData("ITEM_OBJECT_ID")).ToNwObject<NwItem>();
        Slot = (InventorySlot) EventsPlugin.GetEventData("SLOT").ToInt();
        Result = EventsPlugin.GetEventData("BEFORE_RESULT").ToInt().ToBool();

        Callbacks?.Invoke(this);
        EventsPlugin.SetEventResult(Result.ToInt().ToString());
      }

      public event Action<OnValidateEquipItemAfter> Callbacks;
    }

    [NWNXEvent("NWNX_ON_USE_ITEM_BEFORE")]
    public class OnItemUseBefore : IEvent<OnItemUseBefore>
    {
      public NwCreature Creature { get; private set; }
      public NwItem Item { get; private set; }
      public bool Skip { get; set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Creature = (NwCreature) objSelf;
        Item = ObjectPlugin.StringToObject(EventsPlugin.GetEventData("ITEM_OBJECT_ID")).ToNwObject<NwItem>();
        Skip = false;

        Callbacks?.Invoke(this);

        if (Skip)
        {
          EventsPlugin.SkipEvent();
        }
      }

      public event Action<OnItemUseBefore> Callbacks;
    }

    [NWNXEvent("NWNX_ON_ITEM_EQUIP_BEFORE")]
    public class OnItemEquipBefore : IEvent<OnItemEquipBefore>
    {
      public NwCreature Creature { get; private set; }
      public NwItem Item { get; private set; }
      public bool Skip { get; set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Creature = (NwCreature) objSelf;
        Item = ObjectPlugin.StringToObject(EventsPlugin.GetEventData("ITEM")).ToNwObject<NwItem>();
        Skip = false;

        Callbacks?.Invoke(this);

        if (Skip)
        {
          EventsPlugin.SkipEvent();
        }
      }

      public event Action<OnItemEquipBefore> Callbacks;
    }
  }
}