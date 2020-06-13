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
        int result = dispatcher.ExecuteScript(script, oidSelf);
        if (result != ScriptDispatchConstants.SCRIPT_NOT_HANDLED)
        {
          return result;
        }
      }

      return ScriptDispatchConstants.SCRIPT_NOT_HANDLED;
    }
  }
}