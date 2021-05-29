using NWN.Native.API;

namespace NWN.API
{
  public sealed class WorldTimer
  {
    private readonly CWorldTimer worldTimer;

    public WorldTimer(CWorldTimer worldTimer)
    {
      this.worldTimer = worldTimer;
    }

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
