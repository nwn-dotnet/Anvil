using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Anvil.API;
using Anvil.Services;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  [TestFixture(Category = "API.Async")]
  public sealed class NwTaskTests
  {
    [Inject]
    private static VirtualMachine VirtualMachine { get; set; } = null!;

    [Test(Description = "Starts an async task, then attempts to switch back to the main thread & script context.")]
    [Timeout(2000)]
    public async Task ReturnToMainThreadAfterSwitch()
    {
      await Task.Run(async () =>
      {
        await Task.Delay(TimeSpan.FromSeconds(0.1f));
      });

      Assert.That(VirtualMachine.IsInScriptContext, Is.False, "Expected to be outside of script context.");
      await NwTask.SwitchToMainThread();
      Assert.That(VirtualMachine.IsInScriptContext, Is.True, "Did not return to the main thread after SwitchToMainThread.");
    }

    [Test(Description = "Await a fixed delay then continue execution.")]
    [Timeout(10000)]
    [TestCase(500)]
    [TestCase(1000)]
    [TestCase(5000)]
    public async Task AwaitDelayContinuesExecutionAtExpectedTime(int delayMs)
    {
      TimeSpan delay = TimeSpan.FromMilliseconds(delayMs);
      Stopwatch stopwatch = Stopwatch.StartNew();

      await NwTask.Delay(delay);

      Assert.That(stopwatch.Elapsed, Is.EqualTo(delay).Within(100).Milliseconds, "Delay was not within the margin of error.");
      Assert.That(VirtualMachine.IsInScriptContext, Is.True, "Did not return to the main thread after NwTask.Delay.");
    }
  }
}
