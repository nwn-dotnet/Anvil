using System;
using System.Collections.Generic;
using Anvil.API;

namespace Anvil.Services
{
  /// <summary>
  /// A service for registering C# functions as script handlers dynamically.
  /// </summary>
  [ServiceBinding(typeof(RuntimeScriptDispatchService))]
  [ServiceBinding(typeof(IScriptDispatcher))]
  public sealed class RuntimeScriptDispatchService : IScriptDispatcher
  {
    public int ExecutionOrder { get; } = 0;

    private readonly Dictionary<string, Func<CallInfo, ScriptHandleResult>> activeHandlers = new Dictionary<string, Func<CallInfo, ScriptHandleResult>>();

    /// <summary>
    /// Registers the specified action as a callback for the specified script name.
    /// </summary>
    /// <param name="scriptName">The script name to be handled.</param>
    /// <param name="handler">The function invoked when this script is called by the Virtual Machine.</param>
    /// <returns>A handle that can be disposed to remove the handler.</returns>
    /// <exception cref="ArgumentException">Thrown if the specified script name is internally used by Anvil.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the specified script already has a handler defined.</exception>
    public IDisposable RegisterScriptHandler(string scriptName, Func<CallInfo, ScriptHandleResult> handler)
    {
      if (ScriptConstants.IsReservedScriptName(scriptName) || !scriptName.IsValidScriptName())
      {
        throw new ArgumentException("The specified script name is not valid.", scriptName);
      }

      if (activeHandlers.ContainsKey(scriptName))
      {
        throw new InvalidOperationException($"A handler is already registered for script name {scriptName}");
      }

      activeHandlers.Add(scriptName, handler);
      return new ScriptCallbackHandle(scriptName);
    }

    public (string scriptName, IDisposable handle) CreateUniqueHandler(Func<CallInfo, ScriptHandleResult> handler)
    {
      string scriptName;

      do
      {
        scriptName = ResourceNameGenerator.Create();
      }
      while (IsScriptRegistered(scriptName));

      IDisposable handle = RegisterScriptHandler(scriptName, handler);
      return (scriptName, handle);
    }

    /// <summary>
    /// Unregisters any handler assigned to the specified script.
    /// </summary>
    /// <param name="scriptName">The script name to unregister.</param>
    /// <returns>True if a handler was removed, false if nothing was removed.</returns>
    public bool UnregisterScriptHandler(string scriptName)
    {
      return activeHandlers.Remove(scriptName);
    }

    /// <summary>
    /// Gets if the specified script name has a script handler already defined.
    /// </summary>
    /// <param name="scriptName">The script name to query.</param>
    /// <returns>True if a handler already exists, otherwise false.</returns>
    public bool IsScriptRegistered(string scriptName)
    {
      return activeHandlers.ContainsKey(scriptName);
    }

    ScriptHandleResult IScriptDispatcher.ExecuteScript(string scriptName, uint oidSelf)
    {
      if (activeHandlers.TryGetValue(scriptName, out Func<CallInfo, ScriptHandleResult> handler))
      {
        if (handler != null)
        {
          CallInfo callInfo = new CallInfo(scriptName, oidSelf.ToNwObject());
          return handler.Invoke(callInfo);
        }
        else
        {
          activeHandlers.Remove(scriptName);
        }
      }

      return ScriptHandleResult.NotHandled;
    }

    private sealed class ScriptCallbackHandle : IDisposable
    {
      private readonly string scriptName;

      [Inject]
      private static RuntimeScriptDispatchService RuntimeScriptDispatchService { get; set; }

      public ScriptCallbackHandle(string scriptName)
      {
        this.scriptName = scriptName;
      }

      public void Dispose()
      {
        RuntimeScriptDispatchService.UnregisterScriptHandler(scriptName);
      }
    }
  }
}
