using NWN.API;
using NWN.Core.NWNX;

namespace NWNX.API.Events
{
  public class AssociateEvents
  {
    [NWNXEvent("NWNX_ON_ADD_ASSOCIATE_BEFORE")]
    public class OnAddAssociateBefore : NWNXEvent<OnAddAssociateBefore>
    {
      public NwPlayer Player { get; private set; }

      public NwCreature Associate { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Player = (NwPlayer) objSelf;
        Associate = EventsPlugin.GetEventData("ASSOCIATE_OBJECT_ID").ParseObject<NwCreature>();
      }
    }

    [NWNXEvent("NWNX_ON_ADD_ASSOCIATE_AFTER")]
    public class OnAddAssociateAfter : NWNXEvent<OnAddAssociateAfter>
    {
      public NwPlayer Player { get; private set; }

      public NwCreature Associate { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Player = (NwPlayer) objSelf;
        Associate = EventsPlugin.GetEventData("ASSOCIATE_OBJECT_ID").ParseObject<NwCreature>();
      }
    }

    [NWNXEvent("NWNX_ON_REMOVE_ASSOCIATE_BEFORE")]
    public class OnRemoveAssociateBefore : NWNXEvent<OnRemoveAssociateBefore>
    {
      public NwPlayer Player { get; private set; }

      public NwCreature Associate { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Player = (NwPlayer) objSelf;
        Associate = EventsPlugin.GetEventData("ASSOCIATE_OBJECT_ID").ParseObject<NwCreature>();
      }
    }

    [NWNXEvent("NWNX_ON_REMOVE_ASSOCIATE_AFTER")]
    public class OnRemoveAssociateAfter : NWNXEvent<OnRemoveAssociateAfter>
    {
      public NwPlayer Player { get; private set; }

      public NwCreature Associate { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Player = (NwPlayer) objSelf;
        Associate = EventsPlugin.GetEventData("ASSOCIATE_OBJECT_ID").ParseObject<NwCreature>();
      }
    }
  }
}
