using NWN.Native.API;

namespace NWN.API.Events
{
  public enum PartyEventType
  {
    Leave =  MessagePartyMinor.Leave,
    Kick = MessagePartyMinor.Kick,
    TransferLeadership = MessagePartyMinor.TransferLeadership,
    Invite = MessagePartyMinor.Invite,
    IgnoreInvitation = MessagePartyMinor.IgnoreInvitation,
    AcceptInvitation = MessagePartyMinor.AcceptInvitation,
    RejectInvitation = MessagePartyMinor.RejectInvitation,
    KickHenchman = MessagePartyMinor.KickHenchman
  }
}
