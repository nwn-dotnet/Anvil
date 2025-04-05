using System;
using System.Reflection;
using Anvil.API;

namespace Anvil.Services
{
  internal sealed class ScriptCallbackMethodVoidCallInfo(MethodInfo methodInfo, object service) : ScriptCallbackMethod
  {
    private readonly Action<CallInfo> callback = (Action<CallInfo>)Delegate.CreateDelegate(typeof(Action<CallInfo>), service, methodInfo);

    public override bool RequiresCallInfo => false;

    public override ScriptHandleResult Execute(CallInfo? callInfo)
    {
      callback(callInfo!);
      return ScriptHandleResult.Handled;
    }
  }
}
