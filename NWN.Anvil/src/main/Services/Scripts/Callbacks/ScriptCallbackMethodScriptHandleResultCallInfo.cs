using System;
using System.Reflection;
using Anvil.API;

namespace Anvil.Services
{
  internal sealed class ScriptCallbackMethodScriptHandleResultCallInfo(MethodInfo methodInfo, object service) : ScriptCallbackMethod
  {
    private readonly Func<CallInfo, ScriptHandleResult> callback = methodInfo.CreateDelegate<Func<CallInfo, ScriptHandleResult>>(service);

    public override bool RequiresCallInfo => true;

    public override ScriptHandleResult Execute(CallInfo? callInfo)
    {
      return callback(callInfo!);
    }
  }
}
