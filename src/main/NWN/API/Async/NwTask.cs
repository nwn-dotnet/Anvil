using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace NWN.API
{
  /// <summary>
  /// Awaiters for running NWN code in an async context.
  /// </summary>
  public static partial class NwTask
  {
    /// <summary>
    /// Waits until the specified amount of time has passed.
    /// </summary>
    /// <param name="delay">How long to wait.</param>
    public static async Task Delay(TimeSpan delay)
    {
      Stopwatch stopwatch = Stopwatch.StartNew();
      await RunAndAwait(() => delay < stopwatch.Elapsed);
    }

    /// <summary>
    /// Safely returns to a NWScript context from another thread.
    /// </summary>
    public static async Task SwitchToMainThread() => await DelayFrame(1);

    public static async Task NextFrame() => await DelayFrame(1);

    /// <summary>
    /// Waits until the specified amount of frames have passed.
    /// </summary>
    /// <param name="frames">The number of frames to wait.</param>
    public static async Task DelayFrame(int frames)
    {
      frames++;
      await RunAndAwait(() =>
      {
        frames--;
        return frames <= 0;
      });
    }

    /// <summary>
    /// Waits until the specified expression returns true.
    /// </summary>
    /// <param name="test">The test expression.</param>
    /// <returns></returns>
    public static async Task WaitUntil(Func<bool> test) => await RunAndAwait(test);

    /// <summary>
    /// Waits until the specified value source changes.
    /// </summary>
    /// <param name="valueSource">The watched value source.</param>
    public static async Task WaitUntilValueChanged<T>(Func<T> valueSource)
    {
      T currentVal = valueSource();
      await RunAndAwait(() => !valueSource().Equals(currentVal));
    }
  }
}