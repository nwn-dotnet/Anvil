/*
 * Usage examples for the Scheduler Service.
 */

using System;
using Anvil.Services;

namespace NWN.Anvil.Samples
{
  [ServiceBinding(typeof(ScheduledService))]
  public class ScheduledService
  {
    private IDisposable schedule;
    private IDisposable runLater;
    private int timesRun;

    private readonly SchedulerService schedulerService;

    public ScheduledService(SchedulerService schedulerService)
    {
      this.schedulerService = schedulerService;
      // Schedules a repeating task to run every minute.
      schedule = schedulerService.ScheduleRepeating(OncePerMinute, TimeSpan.FromMinutes(1));

      // Schedules a once-off task to run in 20 minutes.
      runLater = schedulerService.Schedule(RunIn20Minutes, TimeSpan.FromMinutes(20));
    }

    private void OncePerMinute()
    {
      timesRun++;
      if (timesRun > 10)
      {
        // Cancel the repeating task after 10 runs.
        schedule.Dispose();

        // Actually, don't run the "RunLater" task in 20 minutes. Run it in 30 instead.
        runLater.Dispose();
        runLater = schedulerService.Schedule(RunIn20Minutes, TimeSpan.FromMinutes(30));
      }
    }

    private void RunIn20Minutes()
    {
      // Do something
    }
  }
}
