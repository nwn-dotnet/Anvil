using System;
using System.Reflection;
using Anvil.API;

namespace Anvil.Services
{
  internal sealed class ScriptCallbackMethodBool(MethodInfo methodInfo, object service) : ScriptCallbackMethod
  {
    private readonly Func<bool> callback = methodInfo.CreateDelegate<Func<bool>>(service);

    public override bool RequiresCallInfo => false;

    public override ScriptHandleResult Execute(CallInfo? callInfo)
    {
      bool result = callback();
      return result ? ScriptHandleResult.True : ScriptHandleResult.False;
    }
  }
}
