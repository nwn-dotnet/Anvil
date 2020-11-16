using NWN.Core;
using NWN.Core.NWNX;

namespace NWN.API
{
  public sealed class ScriptParams
  {
    /// <summary>
    /// Gets the specified parameter value.
    /// </summary>
    /// <param name="paramName">The parameter name to resolve the value of.</param>
    public static string this[string paramName]
    {
      get => NWScript.GetScriptParam(paramName);
      set => NWScript.SetScriptParam(paramName, value);
    }

    /// <summary>
    /// Gets a value indicating whether the specified parameter has an assigned value.
    /// </summary>
    /// <param name="paramName">The parameter name to query.</param>
    /// <returns>true if the specified parameter is set, otherwise false.</returns>
    public static bool IsSet(string paramName)
      => UtilPlugin.GetScriptParamIsSet(paramName).ToBool();
  }
}
