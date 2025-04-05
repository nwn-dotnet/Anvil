using System;
using System.Reflection;
using Anvil.API;

namespace Anvil.Services
{
  internal abstract class ScriptCallbackMethod
  {
    public abstract bool RequiresCallInfo { get; }

    public abstract ScriptHandleResult Execute(CallInfo? callInfo);

    public static ScriptCallbackMethod? Create(string scriptName, MethodInfo method, object service)
    {
      Type returnType = method.ReturnType;
      ParameterInfo[] parameterTypes = method.GetParameters();

      if (parameterTypes.Length > 1 || parameterTypes.Length == 1 && parameterTypes[0].ParameterType != typeof(CallInfo))
      {
        return null;
      }

      bool hasCallInfo = parameterTypes.Length > 0;
      if (returnType == typeof(void))
      {
        return hasCallInfo ? new ScriptCallbackMethodVoidCallInfo(method, service) : new ScriptCallbackMethodVoid(method, service);
      }

      if (returnType == typeof(bool))
      {
        return hasCallInfo ? new ScriptCallbackMethodBoolCallInfo(method, service) : new ScriptCallbackMethodBool(method, service);
      }

      if (returnType == typeof(ScriptHandleResult))
      {
        return hasCallInfo ? new ScriptCallbackMethodScriptHandleResultCallInfo(method, service) : new ScriptCallbackMethodScriptHandleResult(method, service);
      }

      return null;
    }
  }
}
