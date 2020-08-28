using System.Numerics;
using NWN.API;
using NWN.API.Constants;
using NWN.Core;
using NWN.Core.NWNX;

namespace NWNX.API.Events
{
  public static class FeatUseEvents
  {
    [NWNXEvent("NWNX_ON_USE_FEAT_BEFORE")]
    public class OnUseFeatBefore : EventSkippable<OnUseFeatBefore>
    {
      public NwCreature FeatUser { get; private set; }
      public Feat Feat { get; private set; }
      public NwGameObject TargetGameObject { get; private set; }
      public Vector3 TargetPosition { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        FeatUser = (NwCreature) objSelf;
        Feat = (Feat) EventsPlugin.GetEventData("FEAT_ID").ParseInt();
        TargetGameObject = NWScript.StringToObject(EventsPlugin.GetEventData("TARGET_OBJECT_ID")).ToNwObject<NwGameObject>();

        Vector3 position;
        position.X = EventsPlugin.GetEventData("TARGET_POSITION_X").ParseFloat();
        position.Y = EventsPlugin.GetEventData("TARGET_POSITION_Y").ParseFloat();
        position.Z = EventsPlugin.GetEventData("TARGET_POSITION_Z").ParseFloat();

        TargetPosition = position;
      }
    }
  }
}