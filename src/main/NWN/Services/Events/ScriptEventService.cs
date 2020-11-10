using System;
using System.Collections.Generic;
using NWN.API;
using NWN.API.Events;

namespace NWN.Services
{
  [ServiceBinding(typeof(ScriptEventService))]
  [ServiceBinding(typeof(IScriptDispatcher))]
  public sealed class ScriptEventService : IScriptDispatcher
  {
    private readonly Dictionary<string, ScriptEvent> callbacks = new Dictionary<string, ScriptEvent>();

    public void SetHandler<T>(string scriptName, Action<T> callback) where T : ScriptEvent<T>, new()
    {
      T dialogEvent = new T
      {
        Callback = callback
      };

      callbacks[scriptName] = dialogEvent;
    }

    ScriptHandleResult IScriptDispatcher.ExecuteScript(string scriptName, uint oidSelf)
    {
      ScriptHandleResult result = ScriptHandleResult.NotHandled;

      if (callbacks.TryGetValue(scriptName, out ScriptEvent callback))
      {
        result = callback.ProcessEvent(oidSelf.ToNwObject());
      }

      return result;
    }
  }
}
