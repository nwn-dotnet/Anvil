using System;
using NWN.Native.API;

namespace Anvil.API
{
  /// <summary>
  /// A waypoint entity that uniquely identifies a location in the module.
  /// </summary>
  [NativeObjectInfo(ObjectTypes.Waypoint, ObjectType.Waypoint)]
  public sealed class NwWaypoint : NwGameObject
  {
    internal readonly CNWSWaypoint Waypoint;

    internal NwWaypoint(CNWSWaypoint waypoint) : base(waypoint)
    {
      Waypoint = waypoint;
    }

    public static implicit operator CNWSWaypoint(NwWaypoint waypoint)
    {
      return waypoint?.Waypoint;
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

      bool result = NativeUtils.DeserializeGff(serialized, (resGff, resStruct) =>
      {
        if (!resGff.IsValidGff("UTW"))
        {
          return false;
        }

        waypoint = new CNWSWaypoint(Invalid);
        if (waypoint.LoadWaypoint(resGff, resStruct, null).ToBool())
        {
          waypoint.LoadObjectState(resGff, resStruct);
          GC.SuppressFinalize(waypoint);
          return true;
        }

        waypoint.Dispose();
        return false;
      });

      return result && waypoint != null ? waypoint.ToNwObject<NwWaypoint>() : null;
    }

    public static NwWaypoint Create(string template, Location location, bool useAppearAnim = false, string newTag = "")
    {
      return CreateInternal<NwWaypoint>(template, location, useAppearAnim, newTag);
    }

    private protected override void AddToArea(CNWSArea area, float x, float y, float z)
    {
      Waypoint.AddToArea(area, x, y, z, true.ToInt());
    }

    internal override void RemoveFromArea()
    {
      Waypoint.RemoveFromArea();
    }
  }
}
