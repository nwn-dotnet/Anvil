using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Anvil.API;
using Anvil.Plugins;
using LightInject;
using NLog;

namespace Anvil.Services
{
  [ServiceBinding(typeof(IScriptDispatcher))]
  internal sealed class ScriptHandlerAttributeDispatchService(Lazy<IEnumerable<object>> services, IServiceManager serviceManager) : IScriptDispatcher, IInitializable, IDisposable
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

      serviceManager.OnContainerCreate += OnContainerCreate;
      serviceManager.OnContainerDispose += OnContainerDispose;
    }

    void IDisposable.Dispose()
    {
      serviceManager.OnContainerCreate -= OnContainerCreate;
      serviceManager.OnContainerDispose -= OnContainerDispose;
    }

    ScriptHandleResult IScriptDispatcher.ExecuteScript(string script, uint objectSelf)
    {
      if (scriptHandlers.TryGetValue(script, out ScriptHandler? handler))
      {
        return handler.ProcessCallbacks(objectSelf);
      }

      return ScriptHandleResult.NotHandled;
    }

    // Isolated plugins have their own service container, so their services are not part of the injected services.
    private void OnContainerCreate(IServiceContainer container, Plugin? plugin)
    {
      if (plugin == null)
      {
        return;
      }

      foreach (object service in container.GetAllInstances<object>())
      {
        RegisterServiceListeners(service);
      }
    }

    private void OnContainerDispose(IServiceContainer container, Plugin? plugin, bool immediateDispose)
    {
      if (plugin == null)
      {
        return;
      }

      HashSet<object> disposingServices = new HashSet<object>(container.GetAllInstances<object>(), ReferenceEqualityComparer.Instance);
      foreach ((string scriptName, ScriptHandler handler) in scriptHandlers.ToList())
      {
        handler.RemoveCallbacks(disposingServices);
        if (!handler.HasCallbacks)
        {
          scriptHandlers.Remove(scriptName);
        }
      }
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
