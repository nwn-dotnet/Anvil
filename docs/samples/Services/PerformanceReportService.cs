/*
 * Report the current tick rate every server loop.
 */

using System;
using Anvil.API;
using Anvil.Services;
using NLog;

namespace NWN.Anvil.Samples
{
  [ServiceBinding(typeof(IUpdateable))]
  [ServiceBinding(typeof(PerformanceReportService))]
  public class PerformanceReportService : IUpdateable
  {
    // Gets the server log. By default, this reports to "anvil.log"
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    public void Update()
    {
      Log.Info($"Current tick rate: {1 / Time.DeltaTime.TotalSeconds}");
    }
  }
}
