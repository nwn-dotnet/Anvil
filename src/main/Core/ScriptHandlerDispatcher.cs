using System;
using System.Collections.Generic;
using System.Reflection;
using NLog;
using NWM.API;
using NWN;

namespace NWM.Core
{
  [Service]
  internal sealed class ScriptHandlerDispatcher
  {
    private const int MAX_CHARS_IN_SCRIPT_NAME = 16;
    private const int SCRIPT_HANDLED = 0;
    private const int SCRIPT_NOT_HANDLED = -1;

    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private delegate void ScriptHandler();
    private delegate void NwObjectScriptHandler(NwObject nwObject);

    private Dictionary<string, ScriptHandler> scriptHandlers = new Dictionary<string, ScriptHandler>();
    private Dictionary<string, NwObjectScriptHandler> nwObjectScriptHandlers = new Dictionary<string, NwObjectScriptHandler>();

    public void Init(IEnumerable<object> services)
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
          RegisterMethod(serviceType, service, method, handler.ScriptName);
        }
      }
    }

    private void RegisterMethod(Type serviceType, object service, MethodInfo method, string scriptName)
    {
      if (scriptName.Length > MAX_CHARS_IN_SCRIPT_NAME)
      {
        Log.Warn($"Script Handler {scriptName} - name exceeds character limit ({MAX_CHARS_IN_SCRIPT_NAME}) and will be ignored.\n" +
                 $"Method: \"{GetFullMethodName(serviceType, method)}\"");
        return;
      }

      if (scriptHandlers.ContainsKey(scriptName) || scriptHandlers.ContainsKey(scriptName))
      {
        Log.Warn($"Script Handler {scriptName} is already registered by: \"{GetFullMethodName(serviceType, scriptHandlers[scriptName].Method)}\"");
        return;
      }

      ParameterInfo[] parameters = method.GetParameters();
      if (parameters.Length == 0)
      {
        scriptHandlers.Add(scriptName, (ScriptHandler)Delegate.CreateDelegate(typeof(ScriptHandler), service, method));
        Log.Info($"Registered Script Handler: {scriptName} -> {GetFullMethodName(serviceType, method)}");
      }
      else if (parameters[0].ParameterType == typeof(NwObject))
      {
        nwObjectScriptHandlers.Add(scriptName, (NwObjectScriptHandler) Delegate.CreateDelegate(typeof(NwObjectScriptHandler), service, method));
        Log.Info($"Registered Script Handler: {scriptName} -> {GetFullMethodName(serviceType, method)}");
      }
    }

    private string GetFullMethodName(Type classType, MemberInfo method)
    {
      return $"{classType.FullName}.{method.Name}";
    }

    public int ExecuteScript(string script, uint oidSelf)
    {
      if (scriptHandlers.TryGetValue(script, out ScriptHandler handler))
      {
        handler();
        return SCRIPT_HANDLED;
      }
      if (nwObjectScriptHandlers.TryGetValue(script, out NwObjectScriptHandler objHandler))
      {
        objHandler(NWScript.OBJECT_SELF.ToNwObject());
        return SCRIPT_HANDLED;
      }

      return SCRIPT_NOT_HANDLED;
    }
  }
}