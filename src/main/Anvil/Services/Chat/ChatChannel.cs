namespace NWN.Services
{
  public enum ChatChannel : byte
  {
    PlayerTalk = Native.API.ChatChannel.PlayerTalk,
    PlayerShout = Native.API.ChatChannel.PlayerShout,
    PlayerWhisper = Native.API.ChatChannel.PlayerWhisper,
    PlayerTell = Native.API.ChatChannel.PlayerTell,
    ServerMessage = Native.API.ChatChannel.ServerMessage,
    PlayerParty = Native.API.ChatChannel.PlayerParty,
    PlayerDm = Native.API.ChatChannel.PlayerDm,
    DmTalk = Native.API.ChatChannel.DmTalk,
    DmShout = Native.API.ChatChannel.DmShout,
    DmWhisper = Native.API.ChatChannel.DmWhisper,
    DmTell = Native.API.ChatChannel.DmTell,
    DmParty = Native.API.ChatChannel.DmParty,
    DmDm = Native.API.ChatChannel.DmDm,
  }
}
