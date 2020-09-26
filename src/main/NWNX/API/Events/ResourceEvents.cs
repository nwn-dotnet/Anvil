using NWNX.API.Constants;
using NWN.API;
using NWN.Core.NWNX;

namespace NWNX.API.Events
{
  public class ResourceEvents
  {
    [NWNXEvent("NWNX_ON_RESOURCE_ADDED")]
    public class OnResourceAdded : EventSkippable<OnResourceAdded>
    {
      public NwModule Module { get; private set; }
      public string Alias { get; private set; }
      public string ResRef { get; private set; }
      public ResRefType Type { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Module = (NwModule)objSelf;
        Alias = EventsPlugin.GetEventData("ALIAS");
        ResRef = EventsPlugin.GetEventData("RESREF");
        Type = (ResRefType)EventsPlugin.GetEventData("TYPE").ParseInt();
      }
    }

    [NWNXEvent("NWNX_ON_RESOURCE_REMOVED")]
    public class OnResourceRemoved : EventSkippable<OnResourceRemoved>
    {
      public NwModule Module { get; private set; }
      public string Alias { get; private set; }
      public string ResRef { get; private set; }
      public ResRefType Type { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Module = (NwModule)objSelf;
        Alias = EventsPlugin.GetEventData("ALIAS");
        ResRef = EventsPlugin.GetEventData("RESREF");
        Type = (ResRefType)EventsPlugin.GetEventData("TYPE").ParseInt();
      }
    }

    [NWNXEvent("NWNX_ON_RESOURCE_MODIFIED")]
    public class OnResourceModified : EventSkippable<OnResourceModified>
    {
      public NwModule Module { get; private set; }
      public string Alias { get; private set; }
      public string ResRef { get; private set; }
      public ResRefType Type { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Module = (NwModule)objSelf;
        Alias = EventsPlugin.GetEventData("ALIAS");
        ResRef = EventsPlugin.GetEventData("RESREF");
        Type = (ResRefType)EventsPlugin.GetEventData("TYPE").ParseInt();
      }
    }
  }
}
