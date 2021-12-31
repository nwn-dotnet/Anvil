using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Anvil.API;
using Anvil.Internal;
using NLog;

namespace Anvil.Services
{
  [ServiceBinding(typeof(ICoreLoopHandler))]
  internal sealed class ServerUpdateLoopService : ICoreLoopHandler, IDisposable
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private IUpdateable[] updateables;

    public ServerUpdateLoopService(IEnumerable<IUpdateable> updateables)
    {
      this.updateables = updateables.OrderBy(updateable => updateable.GetType().GetServicePriority()).ToArray();
      Log.Debug(Stopwatch.IsHighResolution ? "Using high resolution loop timer for loop operations..." : "Using system time for loop operations...");
    }

    public void Dispose()
    {
      updateables = Array.Empty<IUpdateable>();
    }

    public void OnLoop()
    {
      foreach (IUpdateable updateable in updateables)
      {
        try
        {
          updateable.Update();
        }
        catch (Exception e)
        {
          Log.Error(e);
        }
      }
    }
  }
}
