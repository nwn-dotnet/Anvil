using NWN;

namespace NWM.API
{
  public class NwGameObject : NwObject
  {
    protected NwGameObject(uint objectId) : base(objectId) {}

    public virtual float Rotation
    {
      get => NWScript.GetFacing(this) % 360;
      set
      {
        ExecuteOnSelf(() => NWScript.SetFacing(value % 360));
      }
    }

    public void FaceTowards(NwGameObject nwObject)
    {
      AssignCommand(() => NWScript.SetFacingPoint(nwObject.Location.Position));
    }

    public NwArea Area
    {
      get => NWScript.GetArea(this).ToNwObject<NwArea>();
    }

    public bool PlotFlag
    {
      get => NWScript.GetPlotFlag(this).ToBool();
      set => NWScript.SetPlotFlag(this, value.ToInt());
    }

    public Location Location
    {
      get => NWScript.GetLocation(this);
    }
  }
}