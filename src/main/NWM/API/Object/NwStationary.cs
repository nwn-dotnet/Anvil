using System;
using System.Numerics;
using System.Threading.Tasks;
using NWN;

namespace NWM.API
{
  public abstract class NwStationary : NwTrappable
  {
    internal NwStationary(uint objectId) : base(objectId) {}

    public override async Task FaceToPoint(Vector3 point)
    {
      Vector3 direction = Vector3.Normalize(point - Position);
      await base.FaceToPoint(Position - direction);
    }

    // TODO - Test trigger rotation.
    public override float Rotation => (360 - NWScript.GetFacing(this)) % 360;

    public override Location Location => Location.Create(Area, Position, Rotation);

    public override Task SetLocation(Location value)
    {
      throw new NotSupportedException();
    }

    public override Task SetRotation(float value)
    {
      return base.SetRotation(360 - value);
    }
  }
}