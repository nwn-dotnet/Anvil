using NWM.API.Constants;
using NWN;

namespace NWM.API
{
  public sealed class NwPlaceable : NwStationary
  {
    internal NwPlaceable(uint objectId) : base(objectId) {}

    public bool Occupied => NWScript.GetSittingCreature(this) != INVALID;
    public NwCreature SittingCreature => NWScript.GetSittingCreature(this).ToNwObject<NwCreature>();

    public bool Useable
    {
      get => NWScript.GetUseableFlag(this).ToBool();
      set => NWScript.SetUseableFlag(this, value.ToInt());
    }

    public static NwPlaceable Create(string template, Location location, bool useAppearAnim = false, string newTag = "")
    {
      location = Location.Create(location.Area, location.Position, location.FlippedRotation);
      return CreateInternal<NwPlaceable>(ObjectType.Placeable, template, location, useAppearAnim, newTag);
    }
  }
}