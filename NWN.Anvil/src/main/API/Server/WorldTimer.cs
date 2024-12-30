using NWN.Native.API;

namespace Anvil.API
{
  public sealed class WorldTimer(CWorldTimer worldTimer)
  {
    /// <summary>
    /// Gets or sets the module's real life minutes per in game hour.
    /// </summary>
    public byte MinutesPerHour
    {
      get => worldTimer.m_nMinutesPerHour;
      set => worldTimer.m_nMinutesPerHour = value;
    }
  }
}
