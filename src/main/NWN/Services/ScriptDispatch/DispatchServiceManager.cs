using System;
using System.Collections.Generic;
using System.Linq;
using Anvil.Internal;
using NLog;

namespace NWN.Services
{
  [ServiceBinding(typeof(DispatchServiceManager))]
  [ServiceBinding(typeof(ICoreRunScriptHandler))]
  internal class DispatchServiceManager : ICoreRunScriptHandler
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private readonly List<IScriptDispatcher> dispatchers;

    public DispatchServiceManager(IEnumerable<IScriptDispatcher> dispatchers)
    {
      this.dispatchers = dispatchers.ToList();
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
