using System.Diagnostics;

namespace Anvil.Services
{
  /// <summary>
  /// Tracks per-loop and total server run time.
  /// </summary>
  [ServiceBinding(typeof(LoopTimeService))]
  public class LoopTimeService
  {
    private readonly Stopwatch stopwatch = new Stopwatch();

    public double DeltaTime { get; private set; }
    public double Time { get; private set; }

    internal void UpdateTime()
    {
      DeltaTime = stopwatch.Elapsed.TotalSeconds;
      Time += DeltaTime;
      stopwatch.Restart();
    }
  }
}
