using NWN.API;
using NWN.Core;
using NWN.Core.NWNX;

namespace NWNX.API.Events {
  public class BarterEvents {
    [NWNXEvent("NWNX_ON_BARTER_START_BEFORE")]
    public class OnBarterStartBefore : EventSkippable<OnBarterStartBefore>
    {
      public NwPlayer Player { get; private set; }
      public NwPlayer Target { get; private set; }

      protected override void PrepareEvent(NwObject objSelf) {
        Player = (NwPlayer) objSelf;
        Target = NWScript.StringToObject(EventsPlugin.GetEventData("BARTER_TARGET")).ToNwObject<NwPlayer>();
      }
    }
    
    [NWNXEvent("NWNX_ON_BARTER_START_AFTER")]
    public class OnBarterStartAfter : EventSkippable<OnBarterStartAfter>
    {
      public NwPlayer Player { get; private set; }
      public NwPlayer Target { get; private set; }

      protected override void PrepareEvent(NwObject objSelf) {
        Player = (NwPlayer) objSelf;
        Target = NWScript.StringToObject(EventsPlugin.GetEventData("BARTER_TARGET")).ToNwObject<NwPlayer>();
      }
    }
    
    [NWNXEvent("NWNX_ON_BARTER_END_BEFORE")]
    public class OnBarterEndBefore : EventSkippable<OnBarterEndBefore>
    {
      public NwPlayer Player { get; private set; }
      public NwPlayer Target { get; private set; }
      public bool Completed { get; private set; }
      public int InitiatorItemCount { get; private set; }
      public int TargetItemCount { get; private set; }
      public NwObject[] PlayerItems { get; private set; }
      public NwObject[] TargetItems { get; private set; }
      
      protected override void PrepareEvent(NwObject objSelf) {
        Player = (NwPlayer) objSelf;
        Target = (NwPlayer) NWScript.StringToObject(EventsPlugin.GetEventData("BARTER_TARGET")).ToNwObject();
      }
    }
    
    [NWNXEvent("NWNX_ON_BARTER_END_AFTER")]
    public class OnBarterEndAfter : EventSkippable<OnBarterEndAfter>
    {
      public NwPlayer Player { get; private set; }
      public NwPlayer Target { get; private set; }

      protected override void PrepareEvent(NwObject objSelf) {
        Player = (NwPlayer) objSelf;
        Target = (NwPlayer) NWScript.StringToObject(EventsPlugin.GetEventData("BARTER_TARGET")).ToNwObject();
      }
    }
  }
}