using System;
using System.Diagnostics;
using Anvil.Services;

namespace Anvil.API
{
  /// <summary>
  /// Interface for querying time information for the current frame or since startup.
  /// </summary>
  public static class Time
  {
    /// <summary>
    /// The time of the last update loop (frame).
    /// </summary>
    public static TimeSpan DeltaTime { get; private set; }

    /// <summary>
    /// The time at the beginning of this update loop (frame).<br/>
    /// This is the total time since Anvil was started up.
    /// </summary>
    public static TimeSpan TimeSinceStartup { get; private set; }

    [ServiceBinding(typeof(Service))]
    [ServiceBinding(typeof(IUpdateable))]
    [ServiceBindingOptions(InternalBindingPriority.Highest)] // Highest, as we always want this to execute first in the frame.
    internal sealed class Service : IUpdateable
    {
      private readonly Stopwatch deltaTimeStopwatch = Stopwatch.StartNew();
      private readonly Stopwatch startupTimeStopwatch = Stopwatch.StartNew();

      public void Update()
      {
        TimeSinceStartup = startupTimeStopwatch.Elapsed;
        DeltaTime = deltaTimeStopwatch.Elapsed;
        deltaTimeStopwatch.Restart();
      }
    }
  }
}
