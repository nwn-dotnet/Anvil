using System;
using NWN;

namespace NWM.API
{
  public class NwPlaceable : NwStationary
  {
    public float Rotation
    {
      get => (NWScript.GetFacing(this) + 180) % 360;
      set => AssignCommand(() => NWScript.SetFacing(value % 360));
    }

    protected internal NwPlaceable(uint objectId) : base(objectId) {}
    public static NwPlaceable Create(string template, Location location, bool useAppearAnim = false, string newTag = "")
    {
      return CreateInternal<NwPlaceable>(ObjectType.Placeable, template, location, useAppearAnim, newTag);
    }

    public void FaceTowards(NwGameObject nwObject)
    {
      AssignCommand(() => NWScript.SetFacingPoint(nwObject.Location.Position));
    }
  }
}