using NWN.API;
using NWN.API.Events;
using NWN.Core;
using NWN.Core.NWNX;

namespace NWNX.API.Events {
  public class AssociateEvents 
  {
    [NWNXEvent("NWNX_ON_ADD_ASSOCIATE_BEFORE")]
    public class OnAddAssociateBefore : Event<OnAddAssociateBefore>
    {
      public NwPlayer Player { get; private set; }
      public NwCreature Associate { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Player = (NwPlayer) objSelf;
        Associate = NWScript.StringToObject(EventsPlugin.GetEventData("ASSOCIATE_OBJECT_ID")).ToNwObject<NwCreature>();
      }
    }

    [NWNXEvent("NWNX_ON_ADD_ASSOCIATE_AFTER")]
    public class OnAddAssociateAfter : Event<OnAddAssociateAfter>
    {
      public NwPlayer Player { get; private set; }
      public NwCreature Associate { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Player = (NwPlayer) objSelf;
        Associate = NWScript.StringToObject(EventsPlugin.GetEventData("ASSOCIATE_OBJECT_ID")).ToNwObject<NwCreature>();
      }
    }
    
    [NWNXEvent("NWNX_ON_REMOVE_ASSOCIATE_BEFORE")]
    public class OnRemoveAssociateBefore : Event<OnRemoveAssociateBefore>
    {
      public NwPlayer Player { get; private set; }
      public NwCreature Associate { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Player = (NwPlayer) objSelf;
        Associate = NWScript.StringToObject(EventsPlugin.GetEventData("ASSOCIATE_OBJECT_ID")).ToNwObject<NwCreature>();
      }
    }

    [NWNXEvent("NWNX_ON_REMOVE_ASSOCIATE_AFTER")]
    public class OnRemoveAssociateAfter : Event<OnRemoveAssociateAfter>
    {
      public NwPlayer Player { get; private set; }
      public NwCreature Associate { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Player = (NwPlayer) objSelf;
        Associate = NWScript.StringToObject(EventsPlugin.GetEventData("ASSOCIATE_OBJECT_ID")).ToNwObject<NwCreature>();
      }
    }
  }
}


