using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Anvil.API;
using Anvil.Services;
using NUnit.Framework;

namespace Anvil.Tests.Services
{
  [TestFixture(Category = "Services.Scheduler")]
  public sealed class SchedulerServiceTests
  {
    [Inject]
    private static SchedulerService SchedulerService { get; set; } = null!;

    [Test(Description = "Scheduling a task correctly schedules and runs the task with the specified delay.")]
    [Timeout(10000)]
    [TestCase(500)]
    [TestCase(1000)]
    [TestCase(5000)]
    public async Task ScheduleDelayRunsTaskAfterDelay(int delayMs)
    {
      TimeSpan delay = TimeSpan.FromMilliseconds(delayMs);
      Stopwatch stopwatch = Stopwatch.StartNew();

      bool executed = false;
      SchedulerService.Schedule(() =>
      {
        Assert.That(stopwatch.Elapsed, Is.EqualTo(delay).Within(100).Milliseconds, "Delay was not within the margin of error.");
        executed = true;
      }, delay);

      await NwTask.WaitUntil(() => executed);
      Assert.That(executed, Is.EqualTo(true));
    }

    [Test(Description = "Scheduling a task and cancelling it prevents the schedule from running.")]
    [Timeout(10000)]
    [TestCase(500)]
    [TestCase(1000)]
    [TestCase(5000)]
    public async Task ScheduleAndCancelDoesNotRunTask(int delayMs)
    {
      TimeSpan delay = TimeSpan.FromMilliseconds(delayMs);

      ScheduledTask task = SchedulerService.Schedule(() =>
      {
        Assert.Fail("The task executed when it shouldn't have.");
      }, delay);

      task.Cancel();
      await NwTask.Delay(delay + TimeSpan.FromSeconds(1));
    }

    [Test(Description = "Scheduling a repeating task correctly schedules and runs the task with the specified interval.")]
    [Timeout(10000)]
    [TestCase(500)]
    [TestCase(1000)]
    public async Task RepeatingScheduleRunsRepeatingTask(int delayMs)
    {
      TimeSpan interval = TimeSpan.FromMilliseconds(delayMs);
      Stopwatch stopwatch = Stopwatch.StartNew();

      int executionCount = 0;
      ScheduledTask task = SchedulerService.ScheduleRepeating(() =>
      {
        Assert.That(stopwatch.Elapsed, Is.EqualTo(interval).Within(100).Milliseconds, "Delay was not within the margin of error.");
        executionCount++;
        stopwatch.Restart();
      }, interval);

      await NwTask.WaitUntil(() => executionCount > 5);
      Assert.That(executionCount, Is.GreaterThan(5));

      task.Cancel();
    }

    [Test(Description = "Scheduling a repeating task and cancelling the task after the first run correctly cancels the task.")]
    [Timeout(10000)]
    [TestCase(500)]
    [TestCase(1000)]
    public async Task RepeatingScheduleCancelAfterRunPreventsSubsequentRuns(int delayMs)
    {
      TimeSpan interval = TimeSpan.FromMilliseconds(delayMs);

      ScheduledTask? scheduledTask = null;
      int executionCount = 0;

      scheduledTask = SchedulerService.ScheduleRepeating(() =>
      {
        Assert.That(executionCount, Is.EqualTo(0), "Repeating task ran after being cancelled.");
        executionCount++;

        // ReSharper disable once AccessToModifiedClosure
        scheduledTask!.Cancel();
      }, interval);

      await NwTask.WaitUntil(() => executionCount > 0);
      Assert.That(executionCount, Is.EqualTo(1));
    }
  }
}
