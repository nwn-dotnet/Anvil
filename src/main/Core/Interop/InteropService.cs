using NWM.API;
using NWN;

namespace NWM.Core.Interop
{
  [Service]
  public class InteropService
  {
    /// <summary>
    ///  Makes the specified target object execute scriptName and then returns execution to the calling script.
    ///  If scriptName does not specify a compiled script, nothing happens.
    /// </summary>
    public void ExecuteNwScript(string scriptName, NwObject target = null)
    {
      NWScript.ExecuteScript(scriptName, target);
    }

    public void ExecuteScriptChunk(string source, bool wrapIntoMain = true, NwObject target = null)
    {
      NWScript.ExecuteScriptChunk(source, target, wrapIntoMain.ToInt());
    }
  }
}