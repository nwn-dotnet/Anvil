using NWN.Native.API;

namespace Anvil.API.Events
{
  /// <summary>
  /// Represents the different party-related actions a player can perform.
  /// </summary>
  public enum PartyEventType
  {
    /// <summary>
    /// Player leaves the party.
    /// </summary>
    Leave = MessagePartyMinor.Leave,

    /// <summary>
    /// Player kicks a member from the party.
    /// </summary>
    Kick = MessagePartyMinor.Kick,

    /// <summary>
    /// Player transfers party leadership to another member.
    /// </summary>
    TransferLeadership = MessagePartyMinor.TransferLeadership,

    /// <summary>
    /// Player invites a player to the party.
    /// </summary>
    Invite = MessagePartyMinor.Invite,

    /// <summary>
    /// Player ignores a party invitation.
    /// </summary>
    IgnoreInvitation = MessagePartyMinor.IgnoreInvitation,

    /// <summary>
    /// Player accepts a party invitation.
    /// </summary>
    AcceptInvitation = MessagePartyMinor.AcceptInvitation,

    /// <summary>
    /// Player rejects a party invitation.
    /// </summary>
    RejectInvitation = MessagePartyMinor.RejectInvitation,

    /// <summary>
    /// Player kicks a controlled henchman from the party.
    /// </summary>
    KickHenchman = MessagePartyMinor.KickHenchman,
  }
}
