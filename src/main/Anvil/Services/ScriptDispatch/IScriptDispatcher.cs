namespace Anvil.Services
{
  /// <summary>
  /// Implement in a service to add a custom handler for event scripts.
  /// </summary>
  public interface IScriptDispatcher
  {
    public int ExecutionOrder { get; }

    /// <summary>
    /// Called when the game would execute the specified script name.
    /// </summary>
    /// <param name="scriptName">The script being called.</param>
    /// <param name="oidSelf">The current object context (OBJECT_SELF).</param>
    /// <returns>If this is a conditional script, return True or False. Otherwise return Handled if this script should not be called by other dispatchers or the game.</returns>
    ScriptHandleResult ExecuteScript(string scriptName, uint oidSelf);
  }
}
