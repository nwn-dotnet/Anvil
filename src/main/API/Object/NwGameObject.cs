using NWN;

namespace NWM.API
{
  public class NwGameObject : NwObject
  {
    protected NwGameObject(uint objectId) : base(objectId) {}

    public NwArea Area
    {
      get => NWScript.GetArea(this).ToNwObject<NwArea>();
    }

    public bool PlotFlag
    {
      get => NWScript.GetPlotFlag(this).ToBool();
      set => NWScript.SetPlotFlag(this, value.ToInt());
    }
  }
}