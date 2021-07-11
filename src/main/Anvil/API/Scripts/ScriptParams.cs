using NWN.Core;
using NWN.Native.API;

namespace NWN.API
{
  public sealed class ScriptParams
  {
    private static readonly CVirtualMachine VirtualMachine = NWNXLib.VirtualMachine();

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
      if (VirtualMachine.m_nRecursionLevel < 0)
      {
        return false;
      }

      CExoArrayListScriptParam scriptParams = VirtualMachine.m_lScriptParams.GetItem(VirtualMachine.m_nRecursionLevel);
      for (int i = 0; i < scriptParams.num; i++)
      {
        ScriptParam scriptParam = scriptParams._OpIndex(i);
        if (scriptParam.key.ToString() == paramName)
        {
          return true;
        }
      }

      return false;
    }
  }
}
