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

    private IUpdateable[] updateItems;

    public ServerUpdateLoopService(IEnumerable<IUpdateable> updateItems)
    {
      this.updateItems = updateItems.OrderBy(updateable => updateable.GetType().GetServicePriority()).ToArray();
    }

    public void Dispose()
    {
      updateItems = [];
    }

    internal void Update()
    {
      for (int i = 0; i < updateItems.Length; i++)
      {
        try
        {
          updateItems[i].Update();
        }
        catch (Exception e)
        {
          Log.Error(e);
        }
      }
    }
  }
}
