using NWM.API;

namespace NWM.Core
{
  public sealed partial class EventManager
  {
    public delegate void AreaEvent(NwArea area, NwObject obj);
    public event AreaEvent OnAreaEnter;
    public event AreaEvent OnAreaExit;

    [ScriptHandler("area_enter")]
    private void AreaEntered() => OnAreaEnter?.Invoke((NwArea) EventObjectSelf, EnteringObject);

    [ScriptHandler("area_exit")]
    private void AreaExited() => OnAreaExit?.Invoke((NwArea) EventObjectSelf, ExitingObject);
  }
}