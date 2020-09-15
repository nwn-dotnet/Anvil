using NWN.Core;

namespace NWN.API
{
  public static class NativeScript
  {
    /// <summary>
    ///  Executes the specified NWN script.
    ///  If scriptName does not specify a compiled script, nothing happens.
    /// </summary>
    public static void Execute(string scriptName, NwObject target, params (string paramName, string paramValue)[] scriptParams)
    {
      foreach ((string paramName, string paramValue) scriptParam in scriptParams)
      {
        NWScript.SetScriptParam(scriptParam.paramName, scriptParam.paramValue);
      }

      NWScript.ExecuteScript(scriptName, target);
    }

    /// <summary>
    ///  Makes the specified target object execute scriptName and then returns execution to the calling script.
    ///  If scriptName does not specify a compiled script, nothing happens.
    /// </summary>
    public static void Execute(string scriptName, params (string paramName, string paramValue)[] scriptParams)
      => Execute(scriptName, null, scriptParams);
  }
}