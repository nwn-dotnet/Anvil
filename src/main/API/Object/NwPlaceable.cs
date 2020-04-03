using NWM.API.Constants;
using NWN;

namespace NWM.API
{
  public sealed class NwPlaceable : NwStationary
  {
    internal NwPlaceable(uint objectId) : base(objectId) {}

    public bool Useable
    {
      get => NWScript.GetUseableFlag(this).ToBool();
      set => NWScript.SetUseableFlag(this, value.ToInt());
    }

    public static NwPlaceable Create(string template, Location location, bool useAppearAnim = false, string newTag = "")
    {
      return CreateInternal<NwPlaceable>(ObjectType.Placeable, template, location, useAppearAnim, newTag);
    }
  }
}