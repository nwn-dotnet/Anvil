using System;
using System.Collections.Generic;
using System.Reflection;
using Anvil.API;
using NLog;

namespace Anvil.Services
{
  [ServiceBinding(typeof(IScriptDispatcher))]
  internal sealed class ScriptHandlerAttributeDispatchService(Lazy<IEnumerable<object>> services) : IScriptDispatcher, IInitializable
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private const int StartCapacity = 2000;

    private readonly Dictionary<string, ScriptHandler> scriptHandlers = new Dictionary<string, ScriptHandler>(StartCapacity);

    public int ExecutionOrder => 10000;

    void IInitializable.Init()
    {
      foreach (object service in services.Value)
      {
        RegisterServiceListeners(service);
      }
    }

    ScriptHandleResult IScriptDispatcher.ExecuteScript(string script, uint objectSelf)
    {
      if (scriptHandlers.TryGetValue(script, out ScriptHandler? handler))
      {
        return handler.ProcessCallbacks(objectSelf);
      }

      return ScriptHandleResult.NotHandled;
    }

    private void RegisterMethod(object service, MethodInfo method, string scriptName)
    {
      if (!scriptName.IsValidScriptName(false))
      {
        Log.Warn("Script Handler {ScriptName} - name exceeds character limit ({MaxScriptSize}) and will be ignored\n" +
          "Method: {Method}",
          scriptName,
          ScriptConstants.MaxScriptNameSize,
          method.GetFullName());
        return;
      }

      if (!scriptHandlers.TryGetValue(scriptName, out ScriptHandler? callback))
      {
        callback = new ScriptHandler(scriptName);
        scriptHandlers.Add(scriptName, callback);
      }

      callback.AddCallback(method, service);
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
  }
}
