using NWM.API.Constants;
using NWMX.API.Constants;

namespace NWM.API
{
  [NativeObjectInfo(ObjectType.Waypoint, InternalObjectType.Waypoint)]
  public sealed class NwWaypoint : NwGameObject
  {
    internal NwWaypoint(uint objectId) : base(objectId) {}

    public static NwWaypoint Create(string template, Location location, bool useAppearAnim = false, string newTag = "")
    {
      return NwObjectFactory.CreateInternal<NwWaypoint>(template, location, useAppearAnim, newTag);
    }
  }
}