using System.Collections.Generic;
using System.Linq;

namespace Anvil.Services
{
  [ServiceBinding(typeof(ScriptDispatchService))]
  internal sealed class ScriptDispatchService
  {
    private readonly List<IScriptDispatcher> dispatchers;

    public ScriptDispatchService(IReadOnlyList<IScriptDispatcher> dispatchers)
    {
      this.dispatchers = dispatchers.ToList();
      this.dispatchers.Sort((dispatcherA, dispatcherB) => dispatcherA.ExecutionOrder.CompareTo(dispatcherB.ExecutionOrder));
    }

    public ScriptHandleResult TryExecuteScript(string script, uint objectSelf)
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
  }
}
