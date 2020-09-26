using NWN.API;
using NWN.API.Events;
using NWN.Core;
using NWN.Core.NWNX;

namespace NWNX.API.Events {
  public class ExamineEvents {
    [NWNXEvent("NWNX_ON_EXAMINE_OBJECT_BEFORE")]
    public class OnAddAssociateBefore : Event<OnAddAssociateBefore>
    {
      public NwPlayer Examiner { get; private set; }
      public NwObject Examinee { get; private set; }
      public bool TrapExamineSuccess { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Examiner = (NwPlayer) objSelf;
        Examinee = NWScript.StringToObject(EventsPlugin.GetEventData("EXAMINEE_OBJECT_ID")).ToNwObject();
        TrapExamineSuccess = EventsPlugin.GetEventData("TRAP_EXAMINE_SUCCESS").ParseInt().ToBool();
      }
    }
    
    [NWNXEvent("NWNX_ON_EXAMINE_OBJECT_AFTER")]
    public class OnAddAssociateAfter : Event<OnAddAssociateAfter>
    {
      public NwPlayer Examiner { get; private set; }
      public NwObject Examinee { get; private set; }
      public bool TrapExamineSuccess { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Examiner = (NwPlayer) objSelf;
        Examinee = NWScript.StringToObject(EventsPlugin.GetEventData("EXAMINEE_OBJECT_ID")).ToNwObject();
        TrapExamineSuccess = EventsPlugin.GetEventData("TRAP_EXAMINE_SUCCESS").ParseInt().ToBool();
      }
    }
  }
}