namespace Anvil.Services
{
  public enum ChatChannel : byte
  {
    PlayerTalk = NWN.Native.API.ChatChannel.PlayerTalk,
    PlayerShout = NWN.Native.API.ChatChannel.PlayerShout,
    PlayerWhisper = NWN.Native.API.ChatChannel.PlayerWhisper,
    PlayerTell = NWN.Native.API.ChatChannel.PlayerTell,
    ServerMessage = NWN.Native.API.ChatChannel.ServerMessage,
    PlayerParty = NWN.Native.API.ChatChannel.PlayerParty,
    PlayerDm = NWN.Native.API.ChatChannel.PlayerDm,
    DmTalk = NWN.Native.API.ChatChannel.DmTalk,
    DmShout = NWN.Native.API.ChatChannel.DmShout,
    DmWhisper = NWN.Native.API.ChatChannel.DmWhisper,
    DmTell = NWN.Native.API.ChatChannel.DmTell,
    DmParty = NWN.Native.API.ChatChannel.DmParty,
    DmDm = NWN.Native.API.ChatChannel.DmDm,
  }
}
