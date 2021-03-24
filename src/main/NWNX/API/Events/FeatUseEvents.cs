using System.Numerics;
using NWN.API;
using NWN.API.Constants;
using NWN.API.Events;
using NWN.Core;
using NWN.Core.NWNX;

namespace NWNX.API.Events
{
  public static class FeatUseEvents
  {
    [NWNXEvent("NWNX_ON_USE_FEAT_BEFORE")]
    public sealed class OnUseFeatBefore : IEventSkippable
    {
      public NwCreature FeatUser { get; } = NWScript.OBJECT_SELF.ToNwObject<NwCreature>();

      public Feat Feat { get; } = (Feat) EventsPlugin.GetEventData("FEAT_ID").ParseInt();

      public NwGameObject TargetGameObject { get; } = EventsPlugin.GetEventData("TARGET_OBJECT_ID").ParseObject<NwGameObject>();

      public Vector3 TargetPosition { get; }

      public bool Skip { get; set; }

      NwObject IEvent.Context => FeatUser;

      public OnUseFeatBefore()
      {
        Vector3 position;
        position.X = EventsPlugin.GetEventData("TARGET_POSITION_X").ParseFloat();
        position.Y = EventsPlugin.GetEventData("TARGET_POSITION_Y").ParseFloat();
        position.Z = EventsPlugin.GetEventData("TARGET_POSITION_Z").ParseFloat();

        TargetPosition = position;
      }
    }
  }
}
