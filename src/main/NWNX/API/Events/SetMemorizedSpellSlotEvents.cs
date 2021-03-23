using NWN.API;
using NWN.API.Constants;
using NWN.API.Events;
using NWN.Core;
using NWN.Core.NWNX;

namespace NWNX.API.Events
{
  public static class SetMemorizedSpellSlotEvents
  {
    [NWNXEvent("NWNX_SET_MEMORIZED_SPELL_SLOT_BEFORE")]
    public sealed class OnSetMemorizedSpellSlotBefore : IEventSkippable
    {
      public NwCreature Preparer { get; }

      public ClassType ClassType { get; }

      public Spell Spell { get; }

      public OnSetMemorizedSpellSlotBefore()
      {
        Preparer = NWScript.OBJECT_SELF.ToNwObject<NwPlayer>();

        int classIndex = EventsPlugin.GetEventData("SPELL_CLASS").ParseInt();
        ClassType = (ClassType) NWScript.GetClassByPosition(classIndex + 1, Preparer);
        Spell = (Spell) EventsPlugin.GetEventData("SPELL_ID").ParseInt();
      }

      public bool Skip { get; set; }

      NwObject IEvent.Context => Preparer;
    }
  }
}
