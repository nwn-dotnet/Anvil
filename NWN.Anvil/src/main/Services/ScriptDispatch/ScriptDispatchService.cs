using System;
using System.Collections.Generic;
using System.Linq;
using NLog;

namespace Anvil.Services
{
  [ServiceBinding(typeof(ScriptDispatchService))]
  internal sealed class ScriptDispatchService : IInitializable
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    [Inject]
    private VirtualMachineFunctionHandler VirtualMachineFunctionHandler { get; init; }

    private readonly List<IScriptDispatcher> dispatchers;

    public ScriptDispatchService(IReadOnlyList<IScriptDispatcher> dispatchers)
    {
      this.dispatchers = dispatchers.ToList();
      this.dispatchers.Sort((dispatcherA, dispatcherB) => dispatcherA.ExecutionOrder.CompareTo(dispatcherB.ExecutionOrder));
    }

    public ScriptHandleResult TryExecuteScript(string script, uint objectSelf)
    {
      try
      {
        ScriptHandleResult result = ScriptHandleResult.NotHandled;
        foreach (IScriptDispatcher dispatcher in dispatchers)
        {
          result = dispatcher.ExecuteScript(script, objectSelf);
          if (result != ScriptHandleResult.NotHandled)
          {
            break;
          }
        }

        return result;
      }
      catch (Exception e)
      {
        Log.Error(e);
        return (int)ScriptHandleResult.Handled;
      }
    }

    void IInitializable.Init()
    {
      VirtualMachineFunctionHandler.Load(this);
    }
  }
}
