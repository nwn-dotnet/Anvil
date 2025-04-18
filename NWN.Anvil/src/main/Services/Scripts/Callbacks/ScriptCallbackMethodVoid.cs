using System;
using System.Reflection;
using Anvil.API;
using Action = System.Action;

namespace Anvil.Services
{
  internal sealed class ScriptCallbackMethodVoid(MethodInfo methodInfo, object service) : ScriptCallbackMethod
  {
    private readonly Action callback = (Action)Delegate.CreateDelegate(typeof(Action), service, methodInfo);

    public override bool RequiresCallInfo => false;

    public override ScriptHandleResult Execute(CallInfo? callInfo)
    {
      callback();
      return ScriptHandleResult.Handled;
    }
  }
}
