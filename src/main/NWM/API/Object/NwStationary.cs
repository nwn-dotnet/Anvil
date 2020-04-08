using System.Numerics;
using NWN;

namespace NWM.API
{
  public abstract class NwStationary : NwTrappable
  {
    internal NwStationary(uint objectId) : base(objectId) {}

    public override void FaceToPoint(Vector3 point)
    {
      Vector3 direction = Vector3.Normalize(point - Position);
      base.FaceToPoint(Position - direction);
    }

    // TODO - Test trigger rotation.
    public override float Rotation
    {
      get => (360 - NWScript.GetFacing(this)) % 360;
      set => base.Rotation = 360 - value;
    }

    public override Location Location
    {
      get { return Location.Create(Area, Position, Rotation); }
      set
      {
        base.Location = Location.Create(value.Area, value.Position, value.FlippedRotation);
      }
    }
  }
}