using System;
using Anvil.API;

namespace Anvil.Services
{
  /// <summary>
  /// A handle for a native NSS script callback.
  /// </summary>
  public sealed class ScriptCallbackHandle : IDisposable
  {
    [Inject]
    private static ScriptHandleFactory ScriptHandleFactory { get; set; } = null!;

    public readonly string ScriptName;

    private readonly Func<CallInfo, ScriptHandleResult> callback;

    internal ScriptCallbackHandle(string scriptName, Func<CallInfo, ScriptHandleResult> callback)
    {
      ScriptName = scriptName;
      this.callback = callback;
    }

    public bool IsValid { get; internal set; }

    public void Dispose()
    {
      ScriptHandleFactory.UnregisterScriptHandler(ScriptName);
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
  }
}
