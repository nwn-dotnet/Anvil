using NWN.API;
using NWN.API.Constants;
using NWN.Core;
using NWN.Core.NWNX;

namespace NWNX.API.Events
{
  public static class SetMemorizedSpellSlotEvents
  {
    [NWNXEvent("NWNX_SET_MEMORIZED_SPELL_SLOT_BEFORE")]
    public class OnSetMemorizedSpellSlotBefore : EventSkippable<OnSetMemorizedSpellSlotBefore>
    {
      public NwCreature Preparer { get; private set; }

      public ClassType ClassType { get; private set; }

      public Spell Spell { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Preparer = (NwCreature) objSelf;

        int classIndex = EventsPlugin.GetEventData("SPELL_CLASS").ParseInt();
        ClassType = (ClassType) NWScript.GetClassByPosition(classIndex + 1, Preparer);
        Spell = (Spell) EventsPlugin.GetEventData("SPELL_ID").ParseInt();
      }
    }
  }
}
