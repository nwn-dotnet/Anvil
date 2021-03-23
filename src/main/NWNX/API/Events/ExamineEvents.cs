using NWN.API;
using NWN.API.Events;
using NWN.Core;
using NWN.Core.NWNX;

namespace NWNX.API.Events
{
  public static class ExamineEvents
  {
    [NWNXEvent("NWNX_ON_EXAMINE_OBJECT_BEFORE")]
    public sealed class OnExamineObjectBefore : IEvent
    {
      public NwPlayer Examiner { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlayer>();

      public NwObject Examinee { get; } = EventsPlugin.GetEventData("EXAMINEE_OBJECT_ID").ParseObject();

      public bool TrapExamineSuccess { get; }

      NwObject IEvent.Context => Examiner;

      public OnExamineObjectBefore()
      {
        if (Examinee is NwTrappable trappable && trappable.IsTrapDetectedBy(Examiner))
        {
          TrapExamineSuccess = EventsPlugin.GetEventData("TRAP_EXAMINE_SUCCESS").ParseIntBool();
        }
      }
    }

    [NWNXEvent("NWNX_ON_EXAMINE_OBJECT_AFTER")]
    public sealed class OnExamineObjectAfter : IEvent
    {
      public NwPlayer Examiner { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlayer>();

      public NwObject Examinee { get; } = EventsPlugin.GetEventData("EXAMINEE_OBJECT_ID").ParseObject();

      NwObject IEvent.Context => Examiner;

      public bool TrapExamineSuccess { get; }

      public OnExamineObjectAfter()
      {
        if (Examinee is NwTrappable trappable && trappable.IsTrapDetectedBy(Examiner))
        {
          TrapExamineSuccess = EventsPlugin.GetEventData("TRAP_EXAMINE_SUCCESS").ParseIntBool();
        }
      }
    }
  }
}
