using System;
using System.Collections.Generic;
using System.Linq;
using Anvil.API;
using Anvil.Plugins;
using LightInject;
using NLog;

namespace Anvil.Services
{
  /// <summary>
  /// A service for registering C# functions as script handlers dynamically.
  /// </summary>
  [ServiceBinding(typeof(ScriptHandleFactory))]
  [ServiceBinding(typeof(IScriptDispatcher))]
  public sealed class ScriptHandleFactory : IScriptDispatcher, IDisposable
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private readonly IServiceManager serviceManager;
    private readonly Dictionary<string, ScriptCallbackHandle> activeHandlers = new Dictionary<string, ScriptCallbackHandle>();
    public int ExecutionOrder => 0;

    public ScriptHandleFactory(IServiceManager serviceManager)
    {
      this.serviceManager = serviceManager;
      serviceManager.OnContainerPostDispose += OnContainerPostDispose;
    }

    void IDisposable.Dispose()
    {
      serviceManager.OnContainerPostDispose -= OnContainerPostDispose;
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
    /// Gets if the specified script name has a script handler already defined.
    /// </summary>
    /// <param name="scriptName">The script name to query.</param>
    /// <returns>True if a handler already exists, otherwise false.</returns>
    public bool IsScriptRegistered(string scriptName)
    {
      return activeHandlers.ContainsKey(scriptName);
    }

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
      if (!scriptName.IsValidScriptName(false))
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
    /// Unregisters any handler assigned to the specified script.
    /// </summary>
    /// <param name="scriptName">The script name to unregister.</param>
    /// <returns>True if a handler was removed, false if nothing was removed.</returns>
    public bool UnregisterScriptHandler(string scriptName)
    {
      if (activeHandlers.Remove(scriptName, out ScriptCallbackHandle? handle))
      {
        handle.IsValid = false;
        return true;
      }

      return false;
    }

    ScriptHandleResult IScriptDispatcher.ExecuteScript(string scriptName, uint oidSelf)
    {
      if (activeHandlers.TryGetValue(scriptName, out ScriptCallbackHandle? handler))
      {
        CallInfo callInfo = new CallInfo(scriptName, oidSelf.ToNwObject());
        return handler.Invoke(callInfo);
      }

      return ScriptHandleResult.NotHandled;
    }

    // Isolated plugins may leave handlers behind when unloaded. They keep the plugin loaded, keep invoking its old code,
    // and make the next load of the plugin fail when it registers the same script names again.
    private void OnContainerPostDispose(IServiceContainer container, Plugin? plugin)
    {
      if (plugin == null)
      {
        return;
      }

      List<string> scriptNames = activeHandlers.Values.Where(handle => handle.IsOwnedBy(plugin)).Select(handle => handle.ScriptName).ToList();
      foreach (string scriptName in scriptNames)
      {
        UnregisterScriptHandler(scriptName);
      }

      if (scriptNames.Count > 0)
      {
        Log.Debug("Removed {Count} script handlers left by unloaded plugin {PluginName}", scriptNames.Count, plugin.Name.Name);
      }
    }
  }
}
