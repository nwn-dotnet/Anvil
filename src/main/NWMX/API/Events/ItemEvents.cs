using System;
using NWM.API;
using NWM.API.Events;
using NWNX;

namespace NWMX.API.Events
{
  public sealed class ItemEvents
  {
    [NWNXEvent("NWNX_CAN_USE_ITEM_BEFORE")]
    public class ItemCanUseEventBefore : IEvent<ItemCanUseEventBefore>
    {
      public NwCreature Creature { get; private set; }
      public NwItem Item { get; private set; }
      public bool IgnoreIdentifyFlag { get; private set; }
      public bool Skip { get; set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Creature = (NwCreature) objSelf;
        Item = ObjectPlugin.StringToObject(EventsPlugin.GetEventData("ITEM_OBJECT_ID")).ToNwObject<NwItem>();
        IgnoreIdentifyFlag = EventsPlugin.GetEventData("IGNORE_IDENTIFY_FLAG").ToInt().ToBool();
        Skip = false;

        Callbacks?.Invoke(this);

        if (Skip)
        {
          EventsPlugin.SkipEvent();
        }
      }

      public event Action<ItemCanUseEventBefore> Callbacks;
    }
  }
}