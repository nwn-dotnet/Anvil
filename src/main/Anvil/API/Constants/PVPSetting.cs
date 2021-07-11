namespace Anvil.API
{
  public enum PVPSetting : byte
  {
    None = NWN.Native.API.PvPSetting.None,
    Party = NWN.Native.API.PvPSetting.Party,
    Full = NWN.Native.API.PvPSetting.Full,
    Default = NWN.Native.API.PvPSetting.Default,
  }
}
