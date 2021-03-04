using NWN.API.Constants;
using NWN.Native.API;

namespace NWN.API
{
  [NativeObjectInfo(ObjectTypes.Waypoint, ObjectType.Waypoint)]
  public sealed class NwWaypoint : NwGameObject
  {
    internal readonly CNWSWaypoint Waypoint;

    internal NwWaypoint(uint objectId, CNWSWaypoint waypoint) : base(objectId, waypoint)
    {
      this.Waypoint = waypoint;
    }

    public static implicit operator CNWSWaypoint(NwWaypoint waypoint)
    {
      return waypoint?.Waypoint;
    }

    public override Location Location
    {
      set
      {
        Waypoint.AddToArea(value.Area, value.Position.X, value.Position.Y, value.Position.Z, true.ToInt());
        Rotation = value.Rotation;
      }
    }

    public static NwWaypoint Create(string template, Location location, bool useAppearAnim = false, string newTag = "")
    {
      return NwObject.CreateInternal<NwWaypoint>(template, location, useAppearAnim, newTag);
    }
  }
}
