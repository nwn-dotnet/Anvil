using System.Collections.Generic;
using System.Linq;

namespace NWN.Services
{
  [ServiceBinding(typeof(IRunScriptHandler))]
  internal class DispatchServiceManager : IRunScriptHandler
  {
    private readonly List<IScriptDispatcher> dispatchers;

    public DispatchServiceManager(IEnumerable<IScriptDispatcher> dispatchers)
    {
      this.dispatchers = dispatchers.ToList();
    }

    public int OnRunScript(string script, uint oidSelf)
    {
      foreach (IScriptDispatcher dispatcher in dispatchers)
      {
        ScriptHandleResult result = dispatcher.ExecuteScript(script, oidSelf);
        if (result != ScriptHandleResult.NotHandled)
        {
          return (int) result;
        }
      }

      return (int) ScriptHandleResult.NotHandled;
    }
  }
}