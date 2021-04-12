using System;
using System.Collections.Generic;
using System.Linq;
using Anvil.Internal;

namespace NWN.Services
{
  [ServiceBinding(typeof(DispatchServiceManager))]
  [ServiceBinding(typeof(ICoreRunScriptHandler))]
  internal class DispatchServiceManager : ICoreRunScriptHandler
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
