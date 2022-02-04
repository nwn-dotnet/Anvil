/*
 * Report the current tick rate every server loop.
 */

using System;
using Anvil.API;
using Anvil.Services;

namespace NWN.Anvil.Samples
{
  [ServiceBinding(typeof(IUpdateable))]
  [ServiceBinding(typeof(PerformanceReportService))]
  public class PerformanceReportService : IUpdateable
  {
    public void Update()
    {
      Console.WriteLine($"Current tick rate: {1 / Time.DeltaTime.TotalSeconds}");
    }
  }
}
