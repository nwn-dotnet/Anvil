using NWN.API;
using NWN.API.Events;
using NWN.Core;
using NWN.Core.NWNX;

namespace NWNX.API.Events
{
  public static class AssociateEvents
  {
    [NWNXEvent("NWNX_ON_ADD_ASSOCIATE_BEFORE")]
    public sealed class OnAddAssociateBefore : IEvent
    {
      public NwPlayer Player { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlayer>();

      public NwCreature Associate { get; } = EventsPlugin.GetEventData("ASSOCIATE_OBJECT_ID").ParseObject<NwCreature>();

      NwObject IEvent.Context => Player;
    }

    [NWNXEvent("NWNX_ON_ADD_ASSOCIATE_AFTER")]
    public sealed class OnAddAssociateAfter : IEvent
    {
      public NwPlayer Player { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlayer>();

      public NwCreature Associate { get; } = EventsPlugin.GetEventData("ASSOCIATE_OBJECT_ID").ParseObject<NwCreature>();

      NwObject IEvent.Context => Player;
    }

    [NWNXEvent("NWNX_ON_REMOVE_ASSOCIATE_BEFORE")]
    public sealed class OnRemoveAssociateBefore : IEvent
    {
      public NwPlayer Player { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlayer>();

      public NwCreature Associate { get; } = EventsPlugin.GetEventData("ASSOCIATE_OBJECT_ID").ParseObject<NwCreature>();

      NwObject IEvent.Context => Player;
    }

    [NWNXEvent("NWNX_ON_REMOVE_ASSOCIATE_AFTER")]
    public sealed class OnRemoveAssociateAfter : IEvent
    {
      public NwPlayer Player { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlayer>();

      public NwCreature Associate { get; } = EventsPlugin.GetEventData("ASSOCIATE_OBJECT_ID").ParseObject<NwCreature>();

      NwObject IEvent.Context => Player;
    }
  }
}
