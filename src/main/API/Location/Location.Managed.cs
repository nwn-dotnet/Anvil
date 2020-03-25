using System.Numerics;
using NWM.API;

// ReSharper disable once CheckNamespace
namespace NWN
{
  public partial class Location
  {
    public Vector3 Position => NWScript.GetPositionFromLocation(this);
    public NwArea Area => NWScript.GetAreaFromLocation(this).ToNwObject<NwArea>();
  }
}