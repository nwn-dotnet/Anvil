using System;
using NWN.API;
using NWN.API.Events;
using NWN.Core;
using NWN.Core.NWNX;

namespace NWNX.API.Events
{
  public static class PartyEvents
  {
    [NWNXEvent("NWNX_ON_PARTY_LEAVE_BEFORE")]
    [Obsolete("Use NwModule/NwPlayer.OnPartyEvent instead.")]
    public sealed class OnLeaveBefore : IEventSkippable
    {
      /// <summary>
      /// Gets the player leaving the party.
      /// </summary>
      public NwPlayer Player { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlayer>();

      public bool Skip { get; set; }

      NwObject IEvent.Context => Player;
    }

    [NWNXEvent("NWNX_ON_PARTY_LEAVE_AFTER")]
    [Obsolete("Use NwModule/NwPlayer.OnPartyEvent instead.")]
    public sealed class OnLeaveAfter : IEventSkippable
    {
      /// <summary>
      /// Gets the player leaving the party.
      /// </summary>
      public NwPlayer Player { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlayer>();

      public bool Skip { get; set; }

      NwObject IEvent.Context => Player;
    }

    [NWNXEvent("NWNX_ON_PARTY_KICK_BEFORE")]
    [Obsolete("Use NwModule/NwPlayer.OnPartyEvent instead.")]
    public sealed class OnKickBefore : IEventSkippable
    {
      /// <summary>
      /// Gets the party leader who is kicking the party member.
      /// </summary>
      public NwPlayer PartyLeader { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlayer>();

      /// <summary>
      /// Gets the creature that was kicked.
      /// </summary>
      public NwCreature Kicked { get; } = EventsPlugin.GetEventData("KICKED").ParseObject<NwCreature>();

      public bool Skip { get; set; }

      NwObject IEvent.Context => PartyLeader;
    }

    [NWNXEvent("NWNX_ON_PARTY_KICK_AFTER")]
    [Obsolete("Use NwModule/NwPlayer.OnPartyEvent instead.")]
    public sealed class OnKickAfter : IEventSkippable
    {
      /// <summary>
      /// Gets the party leader who is kicking the party member.
      /// </summary>
      public NwPlayer PartyLeader { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlayer>();

      /// <summary>
      /// Gets the creature that was kicked.
      /// </summary>
      public NwCreature Kicked { get; } = EventsPlugin.GetEventData("KICKED").ParseObject<NwCreature>();

      public bool Skip { get; set; }

      NwObject IEvent.Context => PartyLeader;
    }

    [NWNXEvent("NWNX_ON_PARTY_TRANSFER_LEADERSHIP_BEFORE")]
    [Obsolete("Use NwModule/NwPlayer.OnPartyEvent instead.")]
    public sealed class OnTransferLeadershipBefore : IEventSkippable
    {
      /// <summary>
      /// Gets the player who is transferring leadership.
      /// </summary>
      public NwPlayer PreviousLeader { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlayer>();

      /// <summary>
      /// Gets the player who was promoted to party leader.
      /// </summary>
      public NwPlayer NewLeader { get; } = EventsPlugin.GetEventData("NEW_LEADER").ParseObject<NwPlayer>();

      public bool Skip { get; set; }

      NwObject IEvent.Context => PreviousLeader;
    }

    [NWNXEvent("NWNX_ON_PARTY_TRANSFER_LEADERSHIP_AFTER")]
    [Obsolete("Use NwModule/NwPlayer.OnPartyEvent instead.")]
    public sealed class OnTransferLeadershipAfter : IEventSkippable
    {
      /// <summary>
      /// Gets the player who is transferring leadership.
      /// </summary>
      public NwPlayer PreviousLeader { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlayer>();

      /// <summary>
      /// Gets the player who was promoted to party leader.
      /// </summary>
      public NwPlayer NewLeader { get; } = EventsPlugin.GetEventData("NEW_LEADER").ParseObject<NwPlayer>();

      public bool Skip { get; set; }

      NwObject IEvent.Context => PreviousLeader;
    }

    [NWNXEvent("NWNX_ON_PARTY_INVITE_BEFORE")]
    [Obsolete("Use NwModule/NwPlayer.OnPartyEvent instead.")]
    public sealed class OnInviteBefore : IEventSkippable
    {
      /// <summary>
      /// Gets the player who sent the invitation.
      /// </summary>
      public NwPlayer Sender { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlayer>();

      /// <summary>
      /// Gets the player invited to the party.
      /// </summary>
      public NwPlayer Invited { get; } = EventsPlugin.GetEventData("INVITED").ParseObject<NwPlayer>();

      public bool Skip { get; set; }

      NwObject IEvent.Context => Sender;
    }

    [NWNXEvent("NWNX_ON_PARTY_INVITE_AFTER")]
    [Obsolete("Use NwModule/NwPlayer.OnPartyEvent instead.")]
    public sealed class OnInviteAfter : IEventSkippable
    {
      /// <summary>
      /// Gets the player who sent the invitation.
      /// </summary>
      public NwPlayer Sender { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlayer>();

      /// <summary>
      /// Gets the player invited to the party.
      /// </summary>
      public NwPlayer Invited { get; } = EventsPlugin.GetEventData("INVITED").ParseObject<NwPlayer>();

      public bool Skip { get; set; }

      NwObject IEvent.Context => Sender;
    }

    [NWNXEvent("NWNX_ON_IGNORE_INVITATION_BEFORE")]
    [Obsolete("Use NwModule/NwPlayer.OnPartyEvent instead.")]
    public sealed class OnIgnoreInvitationBefore : IEventSkippable
    {
      /// <summary>
      /// Gets the player that ignored the request.
      /// </summary>
      public NwPlayer IgnoredBy { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlayer>();

      /// <summary>
      /// Gets the player who invited the player.
      /// </summary>
      public NwPlayer InvitedBy { get; } = EventsPlugin.GetEventData("INVITED_BY").ParseObject<NwPlayer>();

      public bool Skip { get; set; }

      NwObject IEvent.Context => IgnoredBy;
    }

    [NWNXEvent("NWNX_ON_IGNORE_INVITATION_AFTER")]
    [Obsolete("Use NwModule/NwPlayer.OnPartyEvent instead.")]
    public sealed class OnIgnoreInvitationAfter : IEventSkippable
    {
      /// <summary>
      /// Gets the player that ignored the request.
      /// </summary>
      public NwPlayer IgnoredBy { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlayer>();

      /// <summary>
      /// Gets the player doing the action.
      /// </summary>
      public NwPlayer InvitedBy { get; } = EventsPlugin.GetEventData("INVITED_BY").ParseObject<NwPlayer>();

      public bool Skip { get; set; }

      NwObject IEvent.Context => IgnoredBy;
    }

    [NWNXEvent("NWNX_ON_ACCEPT_INVITATION_BEFORE")]
    [Obsolete("Use NwModule/NwPlayer.OnPartyEvent instead.")]
    public sealed class OnAcceptInvitationBefore : IEventSkippable
    {
      /// <summary>
      /// Gets the player that accepted the invitation.
      /// </summary>
      public NwPlayer AcceptedBy { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlayer>();

      /// <summary>
      /// Gets the player sending the invitiation.
      /// </summary>
      public NwPlayer InvitedBy { get; } = EventsPlugin.GetEventData("INVITED_BY").ParseObject<NwPlayer>();

      public bool Skip { get; set; }

      NwObject IEvent.Context => AcceptedBy;
    }

    [NWNXEvent("NWNX_ON_ACCEPT_INVITATION_AFTER")]
    [Obsolete("Use NwModule/NwPlayer.OnPartyEvent instead.")]
    public sealed class OnAcceptInvitationAfter : IEventSkippable
    {
      /// <summary>
      /// Gets the player that accepted the invitation.
      /// </summary>
      public NwPlayer AcceptedBy { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlayer>();

      /// <summary>
      /// Gets the player sending the invitiation.
      /// </summary>
      public NwPlayer InvitedBy { get; } = EventsPlugin.GetEventData("INVITED_BY").ParseObject<NwPlayer>();

      public bool Skip { get; set; }

      NwObject IEvent.Context => AcceptedBy;
    }

    [NWNXEvent("NWNX_ON_REJECT_INVITATION_BEFORE")]
    [Obsolete("Use NwModule/NwPlayer.OnPartyEvent instead.")]
    public sealed class OnRejectInvitationBefore : IEventSkippable
    {
      /// <summary>
      /// Gets the player who rejected the invitation.
      /// </summary>
      public NwPlayer RejectedBy { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlayer>();

      /// <summary>
      /// Gets the player who sent the invitation.
      /// </summary>
      public NwPlayer SentBy { get; } = EventsPlugin.GetEventData("INVITED_BY").ParseObject<NwPlayer>();

      public bool Skip { get; set; }

      NwObject IEvent.Context => RejectedBy;
    }

    [NWNXEvent("NWNX_ON_REJECT_INVITATION_AFTER")]
    [Obsolete("Use NwModule/NwPlayer.OnPartyEvent instead.")]
    public sealed class OnRejectInvitationAfter : IEventSkippable
    {
      /// <summary>
      /// Gets the player who rejected the invitation.
      /// </summary>
      public NwPlayer RejectedBy { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlayer>();

      /// <summary>
      /// Gets the player who sent the invitation.
      /// </summary>
      public NwPlayer SentBy { get; } = EventsPlugin.GetEventData("INVITED_BY").ParseObject<NwPlayer>();

      public bool Skip { get; set; }

      NwObject IEvent.Context => RejectedBy;
    }

    [NWNXEvent("NWNX_ON_KICK_HENCHMAN_BEFORE")]
    [Obsolete("Use NwModule/NwPlayer.OnPartyEvent instead.")]
    public sealed class OnKickHenchmanBefore : IEventSkippable
    {
      /// <summary>
      /// Gets the player that kicked the henchmen.
      /// </summary>
      public NwPlayer Player { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlayer>();

      /// <summary>
      /// Gets the henchman that was kicked from party.
      /// </summary>
      public NwCreature Henchman { get; } = EventsPlugin.GetEventData("INVITED_BY").ParseObject<NwCreature>();

      public bool Skip { get; set; }

      NwObject IEvent.Context => Player;
    }

    [NWNXEvent("NWNX_ON_KICK_HENCHMAN_AFTER")]
    [Obsolete("Use NwModule/NwPlayer.OnPartyEvent instead.")]
    public sealed class OnKickHenchmanAfter : IEventSkippable
    {
      /// <summary>
      /// Gets the player that kicked the henchmen.
      /// </summary>
      public NwPlayer Player { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlayer>();

      /// <summary>
      /// Gets the henchman that was kicked from party.
      /// </summary>
      public NwCreature Henchman { get; } = EventsPlugin.GetEventData("INVITED_BY").ParseObject<NwCreature>();

      public bool Skip { get; set; }

      NwObject IEvent.Context => Player;
    }
  }
}
