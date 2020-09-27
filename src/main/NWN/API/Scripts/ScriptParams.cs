using NWN.Core;

namespace NWN.API
{
  public sealed class ScriptParams
  {
    /// <summary>
    /// Gets the specified parameter value.
    /// </summary>
    /// <param name="paramName">The parameter name to resolve the value of.</param>
    public string this[string paramName]
    {
      get => NWScript.GetScriptParam(paramName);
      set => NWScript.SetScriptParam(paramName, value);
    }
  }
}
