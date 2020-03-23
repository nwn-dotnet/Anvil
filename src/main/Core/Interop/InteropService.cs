using System.Collections.Generic;
using System.IO;
using System.Linq;
using NWM.API;
using NWN;

namespace NWM.Core
{
  [Service]
  public class InteropService
  {
    /// <summary>
    ///  Makes the specified target object execute scriptName and then returns execution to the calling script.
    ///  If scriptName does not specify a compiled script, nothing happens.
    /// </summary>
    public void ExecuteNss(string scriptName, NwObject target = null)
    {
      NWScript.ExecuteScript(scriptName, target);
    }

    /// <summary>
    /// Executes the specified NWScript function.<br/>
    /// This interop requires a JIT compile, and is very slow.
    /// </summary>
    /// <param name="scriptName">The script containing the function.</param>
    /// <param name="function">The function to call.</param>
    public void ExecuteNssMethod(string scriptName, string function, NwObject target, params object[] parameters)
    {
      string source = GetExecutionString(scriptName, function, parameters.Select(p => p.ToString()));
      ExecuteNssChunk(source, false, target);
    }

    public void ExecuteNssChunk(string source, bool wrapIntoMain = true, NwObject target = null)
    {
      string error = NWScript.ExecuteScriptChunk(source, target, wrapIntoMain.ToInt());
      if (string.IsNullOrEmpty(error))
      {
        return;
      }

      throw new InvalidDataException(error);
    }

    private string GetExecutionString(string include, string method, IEnumerable<string> parameters)
    {
      string methodParams = string.Join(",", parameters);

      return $"#include \"{include}\"\n" +
             $"void main() {{{method}({methodParams});}}";
    }
  }
}