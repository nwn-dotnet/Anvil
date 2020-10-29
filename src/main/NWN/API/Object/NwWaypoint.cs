using NWN.API.Constants;
using NWNX.API.Constants;

namespace NWN.API
{
  [NativeObjectInfo(ObjectTypes.Waypoint, InternalObjectType.Waypoint)]
  public sealed class NwWaypoint : NwGameObject
  {
    internal NwWaypoint(uint objectId) : base(objectId) {}

    public static NwWaypoint Create(string template, Location location, bool useAppearAnim = false, string newTag = "")
    {
      return NwObject.CreateInternal<NwWaypoint>(template, location, useAppearAnim, newTag);
    }
  }
}
