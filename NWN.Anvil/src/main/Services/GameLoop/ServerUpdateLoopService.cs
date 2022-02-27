using System;
using System.Collections.Generic;
using System.Linq;
using Anvil.API;
using NLog;

namespace Anvil.Services
{
  [ServiceBinding(typeof(ServerUpdateLoopService))]
  internal sealed class ServerUpdateLoopService : IDisposable
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private IUpdateable[] updateables;

    public ServerUpdateLoopService(IEnumerable<IUpdateable> updateables)
    {
      this.updateables = updateables.OrderBy(updateable => updateable.GetType().GetServicePriority()).ToArray();
    }

    internal void Update()
    {
      for (int i = 0; i < updateables.Length; i++)
      {
        try
        {
          updateables[i].Update();
        }
        catch (Exception e)
        {
          Log.Error(e);
        }
      }
    }

    public void Dispose()
    {
      updateables = Array.Empty<IUpdateable>();
    }
  }
}
