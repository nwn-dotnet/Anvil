using System;
using System.Numerics;
using NWM.API;
using NWM.API.Constants;
using NWM.API.Events;
using NWN.Core.NWNX;

namespace NWMX.API.Events
{
  public static class FeatUseEvents
  {
    [NWNXEvent("NWNX_ON_USE_FEAT_BEFORE")]
    public class OnUseFeatBefore : IEvent<OnUseFeatBefore>
    {
      public NwCreature FeatUser { get; private set; }
      public Feat Feat { get; private set; }
      public NwGameObject TargetGameObject { get; private set; }
      public Vector3 TargetPosition { get; private set; }
      public bool Skip { get; set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        FeatUser = (NwCreature) objSelf;
        Feat = (Feat) EventsPlugin.GetEventData("FEAT_ID").ParseInt();
        TargetGameObject = ObjectPlugin.StringToObject(EventsPlugin.GetEventData("TARGET_OBJECT_ID")).ToNwObject<NwGameObject>();
        Skip = false; // Reset before we call event handlers.

        Vector3 position;
        position.X = EventsPlugin.GetEventData("TARGET_POSITION_X").ParseFloat();
        position.Y = EventsPlugin.GetEventData("TARGET_POSITION_Y").ParseFloat();
        position.Z = EventsPlugin.GetEventData("TARGET_POSITION_Z").ParseFloat();

        TargetPosition = position;

        Callbacks?.Invoke(this);

        if (Skip)
        {
          EventsPlugin.SkipEvent();
        }
      }

      public event Action<OnUseFeatBefore> Callbacks;
    }
  }
}