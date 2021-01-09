using NWN.API.Constants;
using NWN.Native.API;

namespace NWN.API
{
  [NativeObjectInfo(ObjectTypes.Waypoint, ObjectType.Waypoint)]
  public sealed class NwWaypoint : NwGameObject
  {
    private readonly CNWSWaypoint waypoint;

    internal NwWaypoint(uint objectId, CNWSWaypoint waypoint) : base(objectId)
    {
      this.waypoint = waypoint;
    }

    public static implicit operator CNWSWaypoint(NwWaypoint waypoint)
    {
      return waypoint?.waypoint;
    }

    public static NwWaypoint Create(string template, Location location, bool useAppearAnim = false, string newTag = "")
    {
      return NwObject.CreateInternal<NwWaypoint>(template, location, useAppearAnim, newTag);
    }
  }
}
