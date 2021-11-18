using System;
using System.Collections.Generic;
using System.Linq;
using Anvil.Internal;
using NLog;

namespace Anvil.Services
{
  [ServiceBinding(typeof(ScriptDispatchService))]
  [ServiceBinding(typeof(ICoreRunScriptHandler))]
  internal class ScriptDispatchService : ICoreRunScriptHandler
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private readonly List<IScriptDispatcher> dispatchers;

    public ScriptDispatchService(IEnumerable<IScriptDispatcher> dispatchers)
    {
      this.dispatchers = dispatchers.ToList();
      this.dispatchers.Sort((dispatcherA, dispatcherB) => dispatcherA.ExecutionOrder.CompareTo(dispatcherB.ExecutionOrder));
    }

    public int OnRunScript(string script, uint oidSelf)
    {
      try
      {
        ScriptHandleResult result = ScriptHandleResult.NotHandled;
        foreach (IScriptDispatcher dispatcher in dispatchers)
        {
          result = dispatcher.ExecuteScript(script, oidSelf);
          if (result != ScriptHandleResult.NotHandled)
          {
            break;
          }
        }

        return (int)result;
      }
      catch (Exception e)
      {
        Log.Error(e);
        return (int)ScriptHandleResult.Handled;
      }
    }
  }
}
