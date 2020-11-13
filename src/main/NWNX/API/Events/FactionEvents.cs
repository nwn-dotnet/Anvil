using NWN.API;
using NWN.Core.NWNX;

namespace NWNX.API.Events
{
  public class FactionEvents
  {
    [NWNXEvent("NWNX_ON_SET_NPC_FACTION_REPUTATION_BEFORE")]
    public class OnSetNPCFactionReputationBefore : NWNXEventSkippable<OnSetNPCFactionReputationBefore>
    {
      /// <summary>
      /// Gets the unique faction ID whose reputation value to <see cref="SubjectFactionId"/> is being modified.
      /// </summary>
      public int FactionId { get; private set; }

      /// <summary>
      /// Gets the unique faction ID whose reputation value from <see cref="FactionId"/> is being modified.
      /// </summary>
      public int SubjectFactionId { get; private set; }

      /// <summary>
      /// Gets the previous reputation value before this event.
      /// </summary>
      public int PreviousReputation { get; private set; }

      /// <summary>
      /// Gets or sets the new reputation value to be assigned.
      /// </summary>
      public int NewReputation { get; set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        FactionId = EventsPlugin.GetEventData("FACTION_ID").ParseInt();
        SubjectFactionId = EventsPlugin.GetEventData("SUBJECT_FACTION_ID").ParseInt();
        PreviousReputation = EventsPlugin.GetEventData("PREVIOUS_REPUTATION").ParseInt();
        NewReputation = EventsPlugin.GetEventData("NEW_REPUTATION").ParseInt();
      }

      protected override void ProcessEvent()
      {
        Skip = false;
        int originalValue = NewReputation;

        InvokeCallbacks();

        if (NewReputation != originalValue)
        {
          EventsPlugin.SetEventResult(NewReputation.ToString());
          Skip = true;
        }

        CheckEventSkip();
      }
    }

    [NWNXEvent("NWNX_ON_SET_NPC_FACTION_REPUTATION_AFTER")]
    public class OnSetNPCFactionReputationAfter : NWNXEventSkippable<OnSetNPCFactionReputationAfter>
    {
      /// <summary>
      /// Gets the unique faction ID whose reputation value to <see cref="SubjectFactionId"/> is being modified.
      /// </summary>
      public int FactionId { get; private set; }

      /// <summary>
      /// Gets the unique faction ID whose reputation value from <see cref="FactionId"/> is being modified.
      /// </summary>
      public int SubjectFactionId { get; private set; }

      /// <summary>
      /// Gets the previous reputation value before this event.
      /// </summary>
      public int PreviousReputation { get; private set; }

      /// <summary>
      /// Gets the new reputation value that was assigned.
      /// </summary>
      public int NewReputation { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        FactionId = EventsPlugin.GetEventData("FACTION_ID").ParseInt();
        SubjectFactionId = EventsPlugin.GetEventData("SUBJECT_FACTION_ID").ParseInt();
        PreviousReputation = EventsPlugin.GetEventData("PREVIOUS_REPUTATION").ParseInt();
        NewReputation = EventsPlugin.GetEventData("NEW_REPUTATION").ParseInt();
      }
    }
  }
}
