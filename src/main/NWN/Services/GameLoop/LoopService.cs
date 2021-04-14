using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Anvil.Internal;
using NLog;

namespace NWN.Services
{
  [ServiceBinding(typeof(ICoreLoopHandler))]
  internal sealed class LoopService : ICoreLoopHandler
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private readonly LoopTimeService loopTimeService;
    private readonly IUpdateable[] updateables;

    public LoopService(LoopTimeService loopTimeService, IEnumerable<IUpdateable> updateables)
    {
      this.loopTimeService = loopTimeService;
      this.updateables = updateables.ToArray();
      Log.Debug(Stopwatch.IsHighResolution ? "Using high resolution loop timer for loop operations..." : "Using system time for loop operations...");
    }

    public void OnLoop()
    {
      loopTimeService.UpdateTime();
      for (int i = 0; i < updateables.Length; i++)
      {
        updateables[i].Update();
      }
    }
  }
}
