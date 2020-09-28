using NWN.Core;

namespace NWN.API
{
  public static class NativeScript
  {
    /// <summary>
    /// Executes the specified NWN script.
    /// If scriptName does not specify a compiled script, nothing happens.
    /// </summary>
    public static void Execute(string scriptName, NwObject target, params (string ParamName, string ParamValue)[] scriptParams)
    {
      foreach ((string ParamName, string ParamValue) scriptParam in scriptParams)
      {
        NWScript.SetScriptParam(scriptParam.ParamName, scriptParam.ParamValue);
      }

      NWScript.ExecuteScript(scriptName, target);
    }

    /// <summary>
    /// Executes the specified NWN script.
    /// If scriptName does not specify a compiled script, nothing happens.
    /// </summary>
    public static void Execute(string scriptName, params (string ParamName, string ParamValue)[] scriptParams)
      => Execute(scriptName, null, scriptParams);
  }
}
