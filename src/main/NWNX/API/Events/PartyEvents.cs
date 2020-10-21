using NWN.API;
using NWN.Core;
using NWN.Core.NWNX;

namespace NWNX.API.Events
{
  public static class PartyEvents
  {
    [NWNXEvent("NWNX_ON_PARTY_LEAVE_BEFORE")]
    public class OnLeaveBefore : EventSkippable<OnLeaveBefore>
    {
      /// <summary>
      /// Gets the player leaving the party.
      /// </summary>
      public NwPlayer Player { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Player = (NwPlayer)objSelf;
      }
    }

    [NWNXEvent("NWNX_ON_PARTY_LEAVE_AFTER")]
    public class OnLeaveAfter : EventSkippable<OnLeaveAfter>
    {
      /// <summary>
      /// Gets the player leaving the party.
      /// </summary>
      public NwPlayer Player { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Player = (NwPlayer)objSelf;
      }
    }

    [NWNXEvent("NWNX_ON_PARTY_KICK_BEFORE")]
    public class OnKickBefore : EventSkippable<OnKickBefore>
    {
      /// <summary>
      /// Gets the party leader who is kicking the party member.
      /// </summary>
      public NwPlayer PartyLeader { get; private set; }

      /// <summary>
      /// Gets the creature that was kicked.
      /// </summary>
      public NwCreature Kicked { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        PartyLeader = (NwPlayer)objSelf;
        Kicked = EventsPlugin.GetEventData("KICKED").ParseObject<NwCreature>();
      }
    }

    [NWNXEvent("NWNX_ON_PARTY_KICK_AFTER")]
    public class OnKickAfter : EventSkippable<OnKickAfter>
    {
      /// <summary>
      /// Gets the party leader who is kicking the party member.
      /// </summary>
      public NwPlayer PartyLeader { get; private set; }

      /// <summary>
      /// Gets the creature that was kicked.
      /// </summary>
      public NwCreature Kicked { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        PartyLeader = (NwPlayer)objSelf;
        Kicked = EventsPlugin.GetEventData("KICKED").ParseObject<NwCreature>();
      }
    }

    [NWNXEvent("NWNX_ON_PARTY_TRANSFER_LEADERSHIP_BEFORE")]
    public class OnTransferLeadershipBefore : EventSkippable<OnTransferLeadershipBefore>
    {
      /// <summary>
      /// Gets the player who is transferring leadership.
      /// </summary>
      public NwPlayer PreviousLeader { get; private set; }

      /// <summary>
      /// Gets the player who was promoted to party leader.
      /// </summary>
      public NwPlayer NewLeader { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        PreviousLeader = (NwPlayer)objSelf;
        NewLeader = EventsPlugin.GetEventData("NEW_LEADER").ParseObject<NwPlayer>();
      }
    }

    [NWNXEvent("NWNX_ON_PARTY_TRANSFER_LEADERSHIP_AFTER")]
    public class OnTransferLeadershipAfter : EventSkippable<OnTransferLeadershipAfter>
    {
      /// <summary>
      /// Gets the player who is transferring leadership.
      /// </summary>
      public NwPlayer PreviousLeader { get; private set; }

      /// <summary>
      /// Gets the player who was promoted to party leader.
      /// </summary>
      public NwPlayer NewLeader { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        PreviousLeader = (NwPlayer)objSelf;
        NewLeader = EventsPlugin.GetEventData("NEW_LEADER").ParseObject<NwPlayer>();
      }
    }

    [NWNXEvent("NWNX_ON_PARTY_INVITE_BEFORE")]
    public class OnInviteBefore : EventSkippable<OnInviteBefore>
    {
      /// <summary>
      /// Gets the player who sent the invitation.
      /// </summary>
      public NwPlayer Sender { get; private set; }

      /// <summary>
      /// Gets the player invited to the party.
      /// </summary>
      public NwPlayer Invited { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Sender = (NwPlayer)objSelf;
        Invited = EventsPlugin.GetEventData("INVITED").ParseObject<NwPlayer>();
      }
    }

    [NWNXEvent("NWNX_ON_PARTY_INVITE_AFTER")]
    public class OnInviteAfter : EventSkippable<OnInviteAfter>
    {
      /// <summary>
      /// Gets the player who sent the invitation.
      /// </summary>
      public NwPlayer Sender { get; private set; }

      /// <summary>
      /// Gets the player invited to the party.
      /// </summary>
      public NwPlayer Invited { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Sender = (NwPlayer)objSelf;
        Invited = EventsPlugin.GetEventData("INVITED").ParseObject<NwPlayer>();
      }
    }

    [NWNXEvent("NWNX_ON_IGNORE_INVITATION_BEFORE")]
    public class OnIgnoreInvitationBefore : EventSkippable<OnIgnoreInvitationBefore>
    {
      /// <summary>
      /// Gets the player that ignored the request.
      /// </summary>
      public NwPlayer IgnoredBy { get; private set; }

      /// <summary>
      /// Gets the player who invited the player.
      /// </summary>
      public NwPlayer InvitedBy { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        IgnoredBy = (NwPlayer)objSelf;
        InvitedBy = EventsPlugin.GetEventData("INVITED_BY").ParseObject<NwPlayer>();
      }
    }

    [NWNXEvent("NWNX_ON_IGNORE_INVITATION_AFTER")]
    public class OnIgnoreInvitationAfter : EventSkippable<OnIgnoreInvitationAfter>
    {
      /// <summary>
      /// Gets the player that ignored the request.
      /// </summary>
      public NwPlayer IgnoredBy { get; private set; }

      /// <summary>
      /// Gets the player doing the action.
      /// </summary>
      public NwPlayer InvitedBy { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        IgnoredBy = (NwPlayer)objSelf;
        InvitedBy = EventsPlugin.GetEventData("INVITED_BY").ParseObject<NwPlayer>();
      }
    }

    [NWNXEvent("NWNX_ON_ACCEPT_INVITATION_BEFORE")]
    public class OnAcceptInvitationBefore : EventSkippable<OnAcceptInvitationBefore>
    {
      /// <summary>
      /// Gets the player that accepted the invitation.
      /// </summary>
      public NwPlayer AcceptedBy { get; private set; }

      /// <summary>
      /// Gets the player sending the invitiation.
      /// </summary>
      public NwPlayer InvitedBy { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        AcceptedBy = (NwPlayer)objSelf;
        InvitedBy = EventsPlugin.GetEventData("INVITED_BY").ParseObject<NwPlayer>();
      }
    }

    [NWNXEvent("NWNX_ON_ACCEPT_INVITATION_AFTER")]
    public class OnAcceptInvitationAfter : EventSkippable<OnAcceptInvitationAfter>
    {
      /// <summary>
      /// Gets the player that accepted the invitation.
      /// </summary>
      public NwPlayer AcceptedBy { get; private set; }

      /// <summary>
      /// Gets the player sending the invitiation.
      /// </summary>
      public NwPlayer InvitedBy { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        AcceptedBy = (NwPlayer)objSelf;
        InvitedBy = EventsPlugin.GetEventData("INVITED_BY").ParseObject<NwPlayer>();
      }
    }

    [NWNXEvent("NWNX_ON_REJECT_INVITATION_BEFORE")]
    public class OnRejectInvitationBefore : EventSkippable<OnRejectInvitationBefore>
    {
      /// <summary>
      /// Gets the player who rejected the invitation.
      /// </summary>
      public NwPlayer RejectedBy { get; private set; }

      /// <summary>
      /// Gets the player who sent the invitation.
      /// </summary>
      public NwPlayer SentBy { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        RejectedBy = (NwPlayer)objSelf;
        SentBy = EventsPlugin.GetEventData("INVITED_BY").ParseObject<NwPlayer>();
      }
    }

    [NWNXEvent("NWNX_ON_REJECT_INVITATION_AFTER")]
    public class OnRejectInvitationAfter : EventSkippable<OnRejectInvitationAfter>
    {
      /// <summary>
      /// Gets the player who rejected the invitation.
      /// </summary>
      public NwPlayer RejectedBy { get; private set; }

      /// <summary>
      /// Gets the player who sent the invitation.
      /// </summary>
      public NwPlayer SentBy { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        RejectedBy = (NwPlayer)objSelf;
        SentBy = EventsPlugin.GetEventData("INVITED_BY").ParseObject<NwPlayer>();
      }
    }

    [NWNXEvent("NWNX_ON_KICK_HENCHMAN_BEFORE")]
    public class OnKickHenchmanBefore : EventSkippable<OnKickHenchmanBefore>
    {
      /// <summary>
      /// Gets the player that kicked the henchmen.
      /// </summary>
      public NwPlayer Player { get; private set; }

      /// <summary>
      /// Gets the henchman that was kicked from party.
      /// </summary>
      public NwCreature Henchman { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Player = (NwPlayer)objSelf;
        Henchman = EventsPlugin.GetEventData("INVITED_BY").ParseObject<NwCreature>();
      }
    }

    [NWNXEvent("NWNX_ON_KICK_HENCHMAN_AFTER")]
    public class OnKickHenchmanAfter : EventSkippable<OnKickHenchmanAfter>
    {
      /// <summary>
      /// Gets the player that kicked the henchmen.
      /// </summary>
      public NwPlayer Player { get; private set; }

      /// <summary>
      /// Gets the henchman that was kicked from party.
      /// </summary>
      public NwCreature Henchman { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Player = (NwPlayer)objSelf;
        Henchman = EventsPlugin.GetEventData("INVITED_BY").ParseObject<NwCreature>();
      }
    }
  }
}
