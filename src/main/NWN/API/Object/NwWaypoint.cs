using NWN.API.Constants;
using NWN.Native.API;

namespace NWN.API
{
  [NativeObjectInfo(ObjectTypes.Waypoint, ObjectType.Waypoint)]
  public sealed class NwWaypoint : NwGameObject
  {
    internal NwWaypoint(uint objectId) : base(objectId) {}

    public static NwWaypoint Create(string template, Location location, bool useAppearAnim = false, string newTag = "")
    {
      return NwObject.CreateInternal<NwWaypoint>(template, location, useAppearAnim, newTag);
    }
  }
}
