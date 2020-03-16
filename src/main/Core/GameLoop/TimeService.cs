using System.Collections.Generic;
using System.Diagnostics;
using NLog;

namespace NWM.Core
{
  [Service]
  public class TimeService
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private readonly List<IUpdateable> updateables;
    private readonly Stopwatch stopwatch = new Stopwatch();

    public double Time { get; private set; }
    public double DeltaTime { get; private set; }

    public TimeService(List<IUpdateable> updateables)
    {
      this.updateables = updateables;
      Log.Debug(Stopwatch.IsHighResolution ? "Using high resolution loop timer for loop operations..." : "Using system time for loop operations...");
    }

    internal void Update()
    {
      DeltaTime = stopwatch.Elapsed.TotalSeconds;
      Time += DeltaTime;
      stopwatch.Restart();

      for (int i = 0; i < updateables.Count; i++)
      {
        updateables[i].Update(this);
      }
    }
  }
}