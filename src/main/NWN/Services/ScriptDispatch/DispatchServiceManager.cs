using System;
using System.Collections.Generic;
using System.Linq;

namespace NWN.Services
{
  [ServiceBinding(typeof(IRunScriptHandler))]
  internal class DispatchServiceManager : IRunScriptHandler
  {
    private readonly List<IScriptDispatcher> dispatchers;

    public event Action OnScriptContextBegin;
    public event Action OnScriptContextEnd;

    public DispatchServiceManager(IEnumerable<IScriptDispatcher> dispatchers)
    {
      this.dispatchers = dispatchers.ToList();
    }

    public int OnRunScript(string script, uint oidSelf)
    {
      ScriptHandleResult result = ScriptHandleResult.NotHandled;
      OnScriptContextBegin?.Invoke();

      try
      {
        foreach (IScriptDispatcher dispatcher in dispatchers)
        {
          result = dispatcher.ExecuteScript(script, oidSelf);
          if (result != ScriptHandleResult.NotHandled)
          {
            break;
          }
        }
      }
      finally
      {
        OnScriptContextEnd?.Invoke();
      }

      return (int) result;
    }
  }
}