using NWN;

namespace NWM.API
{
  public class NwPlaceable : NwStationary
  {
    protected internal NwPlaceable(uint objectId) : base(objectId) {}
    public static NwPlaceable Create(string template, Location location, bool useAppearAnim = false, string newTag = "")
    {
      return CreateInternal<NwPlaceable>(ObjectType.Placeable, template, location, useAppearAnim, newTag);
    }
  }
}