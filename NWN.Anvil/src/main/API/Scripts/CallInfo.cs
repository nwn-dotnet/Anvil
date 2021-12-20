using System.Reflection;
using Anvil.API.Events;
using NWN.Core;

namespace Anvil.API
{
  /// <summary>
  /// Meta information for script calls, consumed by ScriptHandler attributed methods in service classes.
  /// </summary>
  public sealed class CallInfo
  {
    private static readonly ScriptParams CachedScriptParams = new ScriptParams();

    public CallInfo(string scriptName, NwObject objSelf)
    {
      ScriptName = scriptName;
      ObjectSelf = objSelf;
      ScriptType = (EventScriptType)NWScript.GetCurrentlyRunningEvent();
    }

    /// <summary>
    /// Gets the object that is currently running on this script.
    /// </summary>
    public NwObject ObjectSelf { get; }

    /// <summary>
    /// Gets the name of the script that is being executing.
    /// </summary>
    public string ScriptName { get; }

    /// <summary>
    /// Gets the parameters set for this script call.<br/>
    /// NOTE: variable values are NOT guaranteed outside of this script context, and must be read before any async method/lambda is invoked.
    /// </summary>
    public ScriptParams ScriptParams => CachedScriptParams;

    /// <summary>
    /// Gets the event that triggered the execution of this script.
    /// </summary>
    public EventScriptType ScriptType { get; }

    /// <summary>
    /// Attempts to get the current running event.
    /// </summary>
    /// <param name="eventData">When this method returns, contains the created event if the current event is a TEvent. Otherwise, returns the default value for TEvent.</param>
    /// <typeparam name="TEvent">The expected event type. Only events attributed with <see cref="GameEventAttribute"/> are supported.</typeparam>
    /// <returns>true if the current running script is a TEvent, otherwise false.</returns>
    public bool TryGetEvent<TEvent>(out TEvent eventData) where TEvent : IEvent, new()
    {
      GameEventAttribute gameEventAttribute = typeof(TEvent).GetCustomAttribute<GameEventAttribute>();
      if (gameEventAttribute?.EventScriptType == ScriptType)
      {
        eventData = new TEvent();
        return true;
      }

      eventData = default;
      return false;
    }
  }
}
