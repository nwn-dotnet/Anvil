/*
 * Report the current tick rate every server loop.
 */

using System;
using NWN.Services;

[ServiceBinding(typeof(IUpdateable))]
[ServiceBinding(typeof(PerformanceReportService))]
public class PerformanceReportService : IUpdateable
{
  private readonly LoopTimeService timeService;

  public PerformanceReportService(LoopTimeService timeService)
  {
    this.timeService = timeService;
  }

  public void Update()
  {
    Console.WriteLine($"Current tick rate: {1/timeService.DeltaTime}");
  }
}
