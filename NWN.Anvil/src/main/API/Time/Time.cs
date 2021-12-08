using System;
using System.Diagnostics;
using Anvil.Services;

namespace Anvil.API
{
  public static class Time
  {
    public static TimeSpan DeltaTime { get; private set; }
    public static TimeSpan TimeSinceStartup { get; private set; }

    [ServiceBinding(typeof(Service))]
    [ServiceBinding(typeof(IUpdateable))]
    [ServiceBindingOptions(InternalBindingPriority.API)]
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
