using NWN.API;
using NWN.API.Events;
using NWN.Core.NWNX;

namespace NWNX.API.Events
{
  public class ExamineEvents
  {
    [NWNXEvent("NWNX_ON_EXAMINE_OBJECT_BEFORE")]
    public class OnExamineObjectBefore : Event<OnExamineObjectBefore>
    {
      public NwPlayer Examiner { get; private set; }

      public NwObject Examinee { get; private set; }

      public bool TrapExamineSuccess { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Examiner = (NwPlayer) objSelf;
        Examinee = EventsPlugin.GetEventData("EXAMINEE_OBJECT_ID").ParseObject();

        if (Examinee is NwTrappable)
        {
          TrapExamineSuccess = EventsPlugin.GetEventData("TRAP_EXAMINE_SUCCESS").ParseIntBool();
        }
      }
    }

    [NWNXEvent("NWNX_ON_EXAMINE_OBJECT_AFTER")]
    public class OnExamineObjectAfter : Event<OnExamineObjectAfter>
    {
      public NwPlayer Examiner { get; private set; }

      public NwObject Examinee { get; private set; }

      public bool TrapExamineSuccess { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Examiner = (NwPlayer) objSelf;
        Examinee = EventsPlugin.GetEventData("EXAMINEE_OBJECT_ID").ParseObject();

        if (Examinee is NwTrappable)
        {
          TrapExamineSuccess = EventsPlugin.GetEventData("TRAP_EXAMINE_SUCCESS").ParseIntBool();
        }
      }
    }
  }
}
