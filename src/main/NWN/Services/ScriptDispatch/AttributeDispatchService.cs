using System;
using System.Collections.Generic;
using System.Reflection;
using NLog;
using NWN.API;
using NWN.API.Constants;

namespace NWN.Services
{
  [ServiceBinding(typeof(IScriptDispatcher))]
  internal sealed class AttributeDispatchService : IScriptDispatcher
  {
    private const int START_CAPACITY = 2000;
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private Dictionary<string, ScriptCallback> scriptHandlers = new Dictionary<string, ScriptCallback>(START_CAPACITY);

    public AttributeDispatchService()
    {
      NManager.Instance.OnInitComplete += () => Init(NManager.Instance.ServiceManager.GetRegisteredServices());
    }

    private void Init(IEnumerable<object> services)
    {
      foreach (object service in services)
      {
        RegisterServiceListeners(service);
      }
    }

    private void RegisterServiceListeners(object service)
    {
      Type serviceType = service.GetType();

      foreach (MethodInfo method in serviceType.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
      {
        foreach (ScriptHandlerAttribute handler in method.GetCustomAttributes<ScriptHandlerAttribute>())
        {
          RegisterMethod(service, method, handler.ScriptName);
        }
      }
    }

    private void RegisterMethod(object service, MethodInfo method, string scriptName)
    {
      if (scriptName.Length > ScriptConstants.MaxScriptNameSize)
      {
        Log.Warn($"Script Handler {scriptName} - name exceeds character limit ({ScriptConstants.MaxScriptNameSize}) and will be ignored.\n" +
                 $"Method: \"{method.GetFullName()}\"");
        return;
      }

      ScriptCallback callback;
      if (!scriptHandlers.TryGetValue(scriptName, out callback))
      {
        callback = new ScriptCallback();
        scriptHandlers.Add(scriptName, callback);
      }

      callback.AddCallback(service, method, scriptName);
    }

    ScriptHandleResult IScriptDispatcher.ExecuteScript(string script, uint oidSelf)
    {
      if (scriptHandlers.TryGetValue(script, out ScriptCallback handler))
      {
        return handler.ProcessCallbacks(oidSelf);
      }

      return ScriptHandleResult.NotHandled;
    }
  }
}