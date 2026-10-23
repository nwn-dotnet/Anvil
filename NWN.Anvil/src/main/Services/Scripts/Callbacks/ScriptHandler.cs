using System.Collections.Generic;
using System.Reflection;
using Anvil.API;
using NLog;

namespace Anvil.Services
{
  internal sealed class ScriptHandler(string scriptName)
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private readonly List<(ScriptCallbackMethod Method, object Service)> callbackMethods = [];

    public bool HasCallbacks => callbackMethods.Count > 0;

    public void AddCallback(MethodInfo method, object service)
    {
      ScriptCallbackMethod? callbackMethod = ScriptCallbackMethod.Create(scriptName, method, service);
      if (callbackMethod != null)
      {
        callbackMethods.Add((callbackMethod, service));
        Log.Info($"Registered Script Handler (#{callbackMethods.Count}){scriptName} -> {method.GetFullName()}");
      }
      else
      {
        Log.Error($"Failed to register script handler, method has invalid parameters or return value: {scriptName} -> {method.GetFullName()}");
      }
    }

    public void RemoveCallbacks(IReadOnlySet<object> services)
    {
      int removed = callbackMethods.RemoveAll(callback => services.Contains(callback.Service));
      if (removed > 0)
      {
        Log.Info($"Unregistered {removed} script handler(s) for {scriptName}");
      }
    }

    public ScriptHandleResult ProcessCallbacks(uint objSelfId)
    {
      ScriptHandleResult result = ScriptHandleResult.NotHandled;
      CallInfo? callInfo = null;

      foreach ((ScriptCallbackMethod callbackMethod, object _) in callbackMethods)
      {
        if (callbackMethod.RequiresCallInfo)
        {
          callInfo ??= new CallInfo(scriptName, objSelfId.ToNwObject());
        }

        result = callbackMethod.Execute(callInfo);
        if (result != ScriptHandleResult.NotHandled)
        {
          break;
        }
      }

      return result;
    }
  }
}
