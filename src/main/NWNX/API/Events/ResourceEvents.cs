using NWN.API;
using NWN.API.Events;
using NWN.Core.NWNX;
using NWNX.API.Constants;

namespace NWNX.API.Events
{
  public static class ResourceEvents
  {
    [NWNXEvent("NWNX_ON_RESOURCE_ADDED")]
    public sealed class OnResourceAdded : IEventSkippable
    {
      public string Alias { get; } = EventsPlugin.GetEventData("ALIAS");

      public string ResRef { get; } = EventsPlugin.GetEventData("RESREF");

      public ResRefType Type { get; } = (ResRefType) EventsPlugin.GetEventData("TYPE").ParseInt();

      public bool Skip { get; set; }

      NwObject IEvent.Context => null;
    }

    [NWNXEvent("NWNX_ON_RESOURCE_REMOVED")]
    public sealed class OnResourceRemoved : IEventSkippable
    {
      public string Alias { get; } = EventsPlugin.GetEventData("ALIAS");

      public string ResRef { get; } = EventsPlugin.GetEventData("RESREF");

      public ResRefType Type { get; } = (ResRefType) EventsPlugin.GetEventData("TYPE").ParseInt();

      public bool Skip { get; set; }

      NwObject IEvent.Context => null;
    }

    [NWNXEvent("NWNX_ON_RESOURCE_MODIFIED")]
    public sealed class OnResourceModified : IEventSkippable
    {
      public string Alias { get; } = EventsPlugin.GetEventData("ALIAS");

      public string ResRef { get; } = EventsPlugin.GetEventData("RESREF");

      public ResRefType Type { get; } = (ResRefType) EventsPlugin.GetEventData("TYPE").ParseInt();

      public bool Skip { get; set; }

      NwObject IEvent.Context => null;
    }
  }
}
