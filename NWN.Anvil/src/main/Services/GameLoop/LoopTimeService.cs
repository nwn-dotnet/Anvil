using System;

namespace Anvil.Services
{
  /// <summary>
  /// Tracks per-loop and total server run time.
  /// </summary>
  [Obsolete("Use the Anvil.API.Time static members instead.")]
  [ServiceBinding(typeof(LoopTimeService))]
  public class LoopTimeService
  {
    public double DeltaTime
    {
      get => API.Time.DeltaTime.TotalSeconds;
    }

    public double Time
    {
      get => API.Time.TimeSinceStartup.TotalSeconds;
    }
  }
}
