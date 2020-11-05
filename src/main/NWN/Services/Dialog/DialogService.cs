using System;
using System.Collections.Generic;
using NWN.API;

namespace NWN.Services
{
  [ServiceBinding(typeof(DialogService))]
  [ServiceBinding(typeof(IScriptDispatcher))]
  public sealed class DialogService : IScriptDispatcher
  {
    private readonly Dictionary<string, DialogEvent> callbacks = new Dictionary<string, DialogEvent>();

    public void SetHandler<T>(string scriptName, Action<T> callback) where T : DialogEvent<T>, new()
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

      if (callbacks.TryGetValue(scriptName, out DialogEvent callback))
      {
        result = callback.ProcessEvent(oidSelf.ToNwObject());
      }

      return result;
    }
  }
}
