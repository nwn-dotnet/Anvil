using NWM.API.Constants;
using NWMX.API.Constants;
using NWN;

namespace NWM.API
{
  [NativeObjectInfo(ObjectType.Placeable, InternalObjectType.Placeable)]
  public sealed class NwPlaceable : NwStationary
  {
    internal NwPlaceable(uint objectId) : base(objectId) {}

    public bool Occupied => NWScript.GetSittingCreature(this) != INVALID;

    public NwCreature SittingCreature => NWScript.GetSittingCreature(this).ToNwObject<NwCreature>();

    /// <summary>
    /// Gets or sets whether this placeable should illuminate
    /// </summary>
    public bool Illumination
    {
      get => NWScript.GetPlaceableIllumination(this).ToBool();
      set => NWScript.SetPlaceableIllumination(this, value.ToInt());
    }

    /// <summary>
    /// Gets or sets whether this placeable should be useable (clickable)
    /// </summary>
    public bool Useable
    {
      get => NWScript.GetUseableFlag(this).ToBool();
      set => NWScript.SetUseableFlag(this, value.ToInt());
    }

    public static NwPlaceable Create(string template, Location location, bool useAppearAnim = false, string newTag = "")
    {
      location = Location.Create(location.Area, location.Position, location.FlippedRotation);
      return NwObjectFactory.CreateInternal<NwPlaceable>(template, location, useAppearAnim, newTag);
    }
  }
}