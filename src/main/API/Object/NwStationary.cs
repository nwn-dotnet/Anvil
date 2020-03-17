using NWN;

namespace NWM.API
{
  public class NwStationary : NwTrappable
  {
    protected NwStationary(uint objectId) : base(objectId) {}

    // TODO - Test trigger rotation.
    public override float Rotation
    {
      get => (NWScript.GetFacing(this) + 180) % 360;
      set => base.Rotation = value;
    }
  }
}