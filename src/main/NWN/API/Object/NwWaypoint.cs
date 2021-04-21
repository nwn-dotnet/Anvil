using NWN.API.Constants;
using NWN.Native.API;

namespace NWN.API
{
  [NativeObjectInfo(ObjectTypes.Waypoint, ObjectType.Waypoint)]
  public sealed class NwWaypoint : NwGameObject
  {
    internal readonly CNWSWaypoint Waypoint;

    internal NwWaypoint(CNWSWaypoint waypoint) : base(waypoint)
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

    public override byte[] Serialize()
    {
      return NativeUtils.SerializeGff("UTW", (resGff, resStruct) =>
      {
        Waypoint.SaveObjectState(resGff, resStruct);
        return Waypoint.SaveWaypoint(resGff, resStruct).ToBool();
      });
    }

    public static NwWaypoint Deserialize(byte[] serialized)
    {
      CNWSWaypoint waypoint = null;

      NativeUtils.DeserializeGff(serialized, (resGff, resStruct) =>
      {
        if (!resGff.IsValidGff("UTW"))
        {
          return false;
        }

        waypoint = new CNWSWaypoint(INVALID);
        if (waypoint.LoadWaypoint(resGff, resStruct, null).ToBool())
        {
          waypoint.LoadObjectState(resGff, resStruct);
          return true;
        }

        waypoint.Dispose();
        return false;
      });

      return waypoint != null ? waypoint.m_idSelf.ToNwObject<NwWaypoint>() : null;
    }

    public static NwWaypoint Create(string template, Location location, bool useAppearAnim = false, string newTag = "")
    {
      return NwObject.CreateInternal<NwWaypoint>(template, location, useAppearAnim, newTag);
    }
  }
}
