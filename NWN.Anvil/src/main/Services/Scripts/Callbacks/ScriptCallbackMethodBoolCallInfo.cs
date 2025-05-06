using System;
using System.Reflection;
using Anvil.API;

namespace Anvil.Services
{
  internal sealed class ScriptCallbackMethodBoolCallInfo(MethodInfo methodInfo, object service) : ScriptCallbackMethod
  {
    private readonly Func<CallInfo, bool> callback = methodInfo.CreateDelegate<Func<CallInfo, bool>>(service);

    public override bool RequiresCallInfo => true;

    public override ScriptHandleResult Execute(CallInfo? callInfo)
    {
      bool result = callback(callInfo!);
      return result ? ScriptHandleResult.True : ScriptHandleResult.False;
    }
  }
}
