using NWN.API.Constants;
using NWN.Core;

namespace NWN.API
{
  /// <summary>
  /// Meta information for script calls, consumed by ScriptHandler attributed methods in service classes.
  /// </summary>
  public class CallInfo
  {
    private static readonly ScriptParams CachedScriptParams = new ScriptParams();

    /// <summary>
    /// Gets the parameters set for this script call.<br/>
    /// NOTE: variable values are NOT guaranteed outside of this script context, and must be read before any async method/lambda is invoked.
    /// </summary>
    public ScriptParams ScriptParams
    {
      get => CachedScriptParams;
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
    /// Gets the event that triggered the execution of this script.
    /// </summary>
    public EventScriptType ScriptType { get; }

    public CallInfo(string scriptName, NwObject objSelf)
    {
      this.ScriptName = scriptName;
      this.ObjectSelf = objSelf;
      this.ScriptType = (EventScriptType) NWScript.GetCurrentlyRunningEvent();
    }
  }
}
