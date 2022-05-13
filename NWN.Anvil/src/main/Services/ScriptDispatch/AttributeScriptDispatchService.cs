using System;
using System.Collections.Generic;
using System.Reflection;
using Anvil.API;
using NLog;

namespace Anvil.Services
{
  [ServiceBinding(typeof(IScriptDispatcher))]
  [ServiceBinding(typeof(IInitializable))]
  internal sealed class AttributeScriptDispatchService : IScriptDispatcher, IInitializable
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private const int StartCapacity = 2000;

    // All Services
    [Inject]
    private Lazy<IEnumerable<object>> Services { get; init; } = null!;

    private readonly Dictionary<string, ScriptCallback> scriptHandlers = new Dictionary<string, ScriptCallback>(StartCapacity);

    public int ExecutionOrder { get; } = 10000;

    void IInitializable.Init()
    {
      foreach (object service in Services.Value)
      {
        RegisterServiceListeners(service);
      }
    }

    ScriptHandleResult IScriptDispatcher.ExecuteScript(string script, uint objectSelf)
    {
      if (scriptHandlers.TryGetValue(script, out ScriptCallback? handler))
      {
        return handler.ProcessCallbacks(objectSelf);
      }

      return ScriptHandleResult.NotHandled;
    }

    private void RegisterMethod(object service, MethodInfo method, string scriptName)
    {
      if (!scriptName.IsValidScriptName())
      {
        Log.Warn("Script Handler {ScriptName} - name exceeds character limit ({MaxScriptSize}) and will be ignored\n" +
          "Method: {Method}",
          scriptName,
          ScriptConstants.MaxScriptNameSize,
          method.GetFullName());
        return;
      }

      if (!scriptHandlers.TryGetValue(scriptName, out ScriptCallback? callback))
      {
        callback = new ScriptCallback(scriptName);
        scriptHandlers.Add(scriptName, callback);
      }

      callback.AddCallback(service, method);
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
