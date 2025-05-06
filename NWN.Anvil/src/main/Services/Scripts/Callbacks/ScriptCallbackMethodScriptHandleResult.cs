using System;
using System.Reflection;
using Anvil.API;

namespace Anvil.Services
{
  internal sealed class ScriptCallbackMethodScriptHandleResult(MethodInfo methodInfo, object service) : ScriptCallbackMethod
  {
    private readonly Func<ScriptHandleResult> callback = methodInfo.CreateDelegate<Func<ScriptHandleResult>>(service);

    public override bool RequiresCallInfo => false;

    public override ScriptHandleResult Execute(CallInfo? callInfo)
    {
      return callback();
    }
  }
}
