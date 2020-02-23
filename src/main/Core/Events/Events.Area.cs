using System;
using NWM.API;
using NWN;

namespace NWM.Core
{
  public sealed partial class EventManager
  {
    public delegate void AreaEvent(NwArea area, NwObject obj);
    public event AreaEvent OnAreaEnter;
    public event AreaEvent OnAreaExit;

    [ScriptHandler("area_enter")]
    private void AreaEntered()
    {
      NwObject entering = NWScript.GetEnteringObject().ToNwObject();
      NwArea area = NWScript.OBJECT_SELF.ToNwObject<NwArea>();
      OnAreaEnter?.Invoke(area, entering);
    }

    [ScriptHandler("area_exit")]
    private void AreaExited()
    {
      NwObject exiting = NWScript.GetExitingObject().ToNwObject();
      NwArea area = NWScript.OBJECT_SELF.ToNwObject<NwArea>();
      OnAreaExit?.Invoke(area, exiting);
    }
  }
}