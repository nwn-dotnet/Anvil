using System;
using NWM.API.Constants;
using NWM.API.Events;
using NWN;
using NWNX;

namespace NWM.API.NWNX.Events
{
  public static class SetMemorizedSpellSlotEvents
  {
    [NWNXEvent("NWNX_SET_MEMORIZED_SPELL_SLOT_BEFORE")]
    public class OnSetMemorizedSpellSlotBefore : IEvent<OnSetMemorizedSpellSlotBefore>
    {
      public NwCreature Preparer { get; private set; }
      public ClassType ClassType { get; private set; }
      public Spell Spell { get; private set; }
      public bool Skip { get; set; }

      public void BroadcastEvent(NwObject objSelf)
      {
        Preparer = (NwCreature) objSelf;

        int classIndex = EventsPlugin.GetEventData("SPELL_CLASS").ToInt();
        ClassType = (ClassType) NWScript.GetClassByPosition(classIndex + 1, Preparer);
        Spell = (Spell) EventsPlugin.GetEventData("SPELL_ID").ToInt();
        Skip = false;

        Callbacks?.Invoke(this);

        if (Skip)
        {
          EventsPlugin.SkipEvent();
        }
      }

      public event Action<OnSetMemorizedSpellSlotBefore> Callbacks;
    }
  }
}