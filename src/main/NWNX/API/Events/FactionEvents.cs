using NWN.API;
using NWN.API.Events;
using NWN.Core.NWNX;

namespace NWNX.API.Events
{
  public static class FactionEvents
  {
    [NWNXEvent("NWNX_ON_SET_NPC_FACTION_REPUTATION_BEFORE")]
    public sealed class OnSetNPCFactionReputationBefore : IEventSkippable, IEventNWNXResult
    {
      private int newReputation = EventsPlugin.GetEventData("NEW_REPUTATION").ParseInt();

      /// <summary>
      /// Gets the unique faction ID whose reputation value to <see cref="SubjectFactionId"/> is being modified.
      /// </summary>
      public int FactionId { get; } = EventsPlugin.GetEventData("FACTION_ID").ParseInt();

      /// <summary>
      /// Gets the unique faction ID whose reputation value from <see cref="FactionId"/> is being modified.
      /// </summary>
      public int SubjectFactionId { get; } = EventsPlugin.GetEventData("SUBJECT_FACTION_ID").ParseInt();

      /// <summary>
      /// Gets the previous reputation value before this event.
      /// </summary>
      public int PreviousReputation { get; } = EventsPlugin.GetEventData("PREVIOUS_REPUTATION").ParseInt();

      /// <summary>
      /// Gets or sets the new reputation value to be assigned.
      /// </summary>
      public int NewReputation
      {
        get => newReputation;
        set
        {
          if (newReputation == value)
          {
            return;
          }

          newReputation = value;
          Skip = true;
        }
      }

      public bool Skip { get; set; }

      NwObject IEvent.Context => null;

      string IEventNWNXResult.EventResult => Skip ? NewReputation.ToString() : null;
    }

    [NWNXEvent("NWNX_ON_SET_NPC_FACTION_REPUTATION_AFTER")]
    public sealed class OnSetNPCFactionReputationAfter : IEventSkippable
    {
      /// <summary>
      /// Gets the unique faction ID whose reputation value to <see cref="SubjectFactionId"/> is being modified.
      /// </summary>
      public int FactionId { get; } = EventsPlugin.GetEventData("FACTION_ID").ParseInt();

      /// <summary>
      /// Gets the unique faction ID whose reputation value from <see cref="FactionId"/> is being modified.
      /// </summary>
      public int SubjectFactionId { get; } = EventsPlugin.GetEventData("SUBJECT_FACTION_ID").ParseInt();

      /// <summary>
      /// Gets the previous reputation value before this event.
      /// </summary>
      public int PreviousReputation { get; } = EventsPlugin.GetEventData("PREVIOUS_REPUTATION").ParseInt();

      /// <summary>
      /// Gets the new reputation value that was assigned.
      /// </summary>
      public int NewReputation { get; } = EventsPlugin.GetEventData("NEW_REPUTATION").ParseInt();

      public bool Skip { get; set; }

      NwObject IEvent.Context => null;
    }
  }
}
