using System.Collections.Generic;
using System.Reflection;
using Anvil.API;
using NLog;

namespace Anvil.Services
{
  internal sealed class ScriptHandler(string scriptName)
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private readonly List<ScriptCallbackMethod> callbackMethods = [];

    public void AddCallback(MethodInfo method, object service)
    {
      ScriptCallbackMethod? callbackMethod = ScriptCallbackMethod.Create(scriptName, method, service);
      if (callbackMethod != null)
      {
        callbackMethods.Add(callbackMethod);
        Log.Info($"Registered Script Handler (#{callbackMethods.Count}){scriptName} -> {method.GetFullName()}");
      }
      else
      {
        Log.Error($"Failed to register script handler, method has invalid parameters or return value: {scriptName} -> {method.GetFullName()}");
      }
    }

    public ScriptHandleResult ProcessCallbacks(uint objSelfId)
    {
      ScriptHandleResult result = ScriptHandleResult.NotHandled;
      CallInfo? callInfo = null;

      foreach (ScriptCallbackMethod callbackMethod in callbackMethods)
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
