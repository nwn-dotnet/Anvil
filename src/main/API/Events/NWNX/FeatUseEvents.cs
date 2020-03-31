using System;
using System.Numerics;
using NWM.API.Constants;
using NWNX;

namespace NWM.API.Events.NWNX
{
  public static class FeatUseEvents
  {
    [EventInfo(EventType.NWNX, EventName = "NWNX_ON_USE_FEAT_BEFORE")]
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
        Feat = (Feat) EventsPlugin.GetEventData("FEAT_ID").ToInt();
        TargetGameObject = ObjectPlugin.StringToObject(EventsPlugin.GetEventData("TARGET_OBJECT_ID")).ToNwObject<NwGameObject>();
        Skip = false; // Reset before we call event handlers.

        Vector3 position;
        position.X = EventsPlugin.GetEventData("TARGET_POSITION_X").ToFloat();
        position.Y = EventsPlugin.GetEventData("TARGET_POSITION_Y").ToFloat();
        position.Z = EventsPlugin.GetEventData("TARGET_POSITION_Z").ToFloat();

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