using NWM.API;

namespace NWN
{
  public partial class Location
  {
    public Vector Position => NWScript.GetPositionFromLocation(this);
    public NwArea Area => NWScript.GetAreaFromLocation(this).ToNwObject<NwArea>();
  }
}