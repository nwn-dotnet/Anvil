using NWM.API;
using NWN;

namespace NWM.Core
{
  public abstract class EventHandler
  {
    protected NwObject EnteringObject => NWScript.GetEnteringObject().ToNwObject();
    protected NwObject ExitingObject => NWScript.GetExitingObject().ToNwObject();

    internal string NamePrefix { get; set; }
    internal abstract bool HandleScriptEvent(string scriptName, NwObject objSelf);
  }
}