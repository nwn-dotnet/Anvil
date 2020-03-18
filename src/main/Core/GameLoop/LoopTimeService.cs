using System.Diagnostics;

namespace NWM.Core
{
  [Service]
  public class LoopTimeService
  {
    public double Time { get; private set; }
    public double DeltaTime { get; private set; }

    private readonly Stopwatch stopwatch = new Stopwatch();

    internal void UpdateTime()
    {
      DeltaTime = stopwatch.Elapsed.TotalSeconds;
      Time += DeltaTime;
      stopwatch.Restart();
    }
  }
}