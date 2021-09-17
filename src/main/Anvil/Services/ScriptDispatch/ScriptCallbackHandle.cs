using System;
using Anvil.API;

namespace Anvil.Services
{
  /// <summary>
  /// Represents a handle for a script callback handle.
  /// </summary>
  public sealed class ScriptCallbackHandle : IDisposable
  {
    [Inject]
    private static RuntimeScriptDispatchService RuntimeScriptDispatchService { get; set; }

    public readonly string ScriptName;
    public bool IsValid { get; internal set; }

    private readonly Func<CallInfo, ScriptHandleResult> callback;

    internal ScriptCallbackHandle(string scriptName, Func<CallInfo, ScriptHandleResult> callback)
    {
      ScriptName = scriptName;
      this.callback = callback;
    }

    internal void AssertValid()
    {
      if (!IsValid)
      {
        throw new InvalidOperationException("Attempted to use invalid script callback handle.");
      }
    }

    internal ScriptHandleResult Invoke(CallInfo callInfo)
    {
      return callback(callInfo);
    }

    public void Dispose()
    {
      RuntimeScriptDispatchService.UnregisterScriptHandler(ScriptName);
    }
  }
}
