using Anvil.Services;
using NWN.Core;
using NWN.Native.API;

namespace Anvil.API
{
  public sealed class ScriptParams
  {
    [Inject]
    private static VirtualMachine VirtualMachine { get; set; }

    /// <summary>
    /// Gets the specified parameter value.
    /// </summary>
    /// <param name="paramName">The parameter name to resolve the value of.</param>
    public string this[string paramName]
    {
      get => NWScript.GetScriptParam(paramName);
      set => NWScript.SetScriptParam(paramName, value);
    }

    /// <summary>
    /// Gets a value indicating whether the specified parameter has an assigned value.
    /// </summary>
    /// <param name="paramName">The parameter name to query.</param>
    /// <returns>true if the specified parameter is set, otherwise false.</returns>
    public bool IsSet(string paramName)
    {
      if (VirtualMachine.RecursionLevel < 0)
      {
        return false;
      }

      foreach (ScriptParam scriptParam in VirtualMachine.GetCurrentContextScriptParams())
      {
        if (scriptParam.key.ToString() == paramName)
        {
          return true;
        }
      }

      return false;
    }
  }
}
