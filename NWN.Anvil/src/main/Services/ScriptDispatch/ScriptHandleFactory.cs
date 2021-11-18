using System;
using System.Collections.Generic;
using Anvil.API;

namespace Anvil.Services
{
  /// <summary>
  /// A service for registering C# functions as script handlers dynamically.
  /// </summary>
  [ServiceBinding(typeof(ScriptHandleFactory))]
  [ServiceBinding(typeof(IScriptDispatcher))]
  public sealed class ScriptHandleFactory : IScriptDispatcher
  {
    public int ExecutionOrder { get; } = 0;

    private readonly Dictionary<string, ScriptCallbackHandle> activeHandlers = new Dictionary<string, ScriptCallbackHandle>();

    /// <summary>
    /// Registers the specified action as a callback for the specified script name.
    /// </summary>
    /// <param name="scriptName">The script name to be handled.</param>
    /// <param name="callback">The function invoked when this script is called by the Virtual Machine.</param>
    /// <returns>A handle that can be disposed to remove the handler.</returns>
    /// <exception cref="ArgumentException">Thrown if the specified script name is internally used by Anvil.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the specified script already has a handler defined.</exception>
    public ScriptCallbackHandle RegisterScriptHandler(string scriptName, Func<CallInfo, ScriptHandleResult> callback)
    {
      if (!scriptName.IsValidScriptName())
      {
        throw new ArgumentException("The specified script name is not valid.", scriptName);
      }

      if (activeHandlers.ContainsKey(scriptName))
      {
        throw new InvalidOperationException($"A handler is already registered for script name {scriptName}");
      }

      ScriptCallbackHandle handle = new ScriptCallbackHandle(scriptName, callback);
      activeHandlers.Add(scriptName, handle);
      handle.IsValid = true;

      return handle;
    }

    /// <summary>
    /// Creates a unique script callback handle for the specified callback method.<br/>
    /// The returned handle can be used for certain API functions that take a script name as a parameter.
    /// </summary>
    /// <param name="handler">The callback function.</param>
    /// <returns>The callback handle.</returns>
    public ScriptCallbackHandle CreateUniqueHandler(Func<CallInfo, ScriptHandleResult> handler)
    {
      string scriptName;

      do
      {
        scriptName = ResourceNameGenerator.Create();
      }
      while (IsScriptRegistered(scriptName));

      ScriptCallbackHandle handle = RegisterScriptHandler(scriptName, handler);
      return handle;
    }

    /// <summary>
    /// Unregisters any handler assigned to the specified script.
    /// </summary>
    /// <param name="scriptName">The script name to unregister.</param>
    /// <returns>True if a handler was removed, false if nothing was removed.</returns>
    public bool UnregisterScriptHandler(string scriptName)
    {
      if (activeHandlers.TryGetValue(scriptName, out ScriptCallbackHandle handle))
      {
        handle.IsValid = false;
      }

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
      if (activeHandlers.TryGetValue(scriptName, out ScriptCallbackHandle handler))
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
  }
}
