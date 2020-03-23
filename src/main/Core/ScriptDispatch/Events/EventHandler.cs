using NWM.API;
using NWN;

namespace NWM.Core
{
  public abstract class EventHandler
  {
    protected NwGameObject EnteringObject => NWScript.GetEnteringObject().ToNwObject<NwGameObject>();
    protected NwGameObject ExitingObject => NWScript.GetExitingObject().ToNwObject<NwGameObject>();

    internal string NamePrefix { get; set; }
    internal abstract bool HandleScriptEvent(string scriptName, NwObject objSelf);
  }
}