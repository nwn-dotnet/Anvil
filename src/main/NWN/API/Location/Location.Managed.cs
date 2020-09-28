using System.Collections.Generic;
using System.Numerics;
using NWN.API.Constants;
using NWN.Core;

namespace NWN.API
{
  public partial class Location
  {
    /// <summary>
    /// Gets the position Vector of this location.
    /// </summary>
    public Vector3 Position
    {
      get => NWScript.GetPositionFromLocation(this);
    }

    /// <summary>
    /// Gets the associated Area of this location.
    /// </summary>
    public NwArea Area
    {
      get => NWScript.GetAreaFromLocation(this).ToNwObject<NwArea>();
    }

    /// <summary>
    /// Gets the rotation value of this location.
    /// </summary>
    public float Rotation
    {
      get => NWScript.GetFacingFromLocation(this);
    }

    /// <summary>
    /// Gets the inverted rotation value of this location (placeables).
    /// </summary>
    public float FlippedRotation
    {
      get => (360 - Rotation) % 360;
    }

    /// <summary>
    /// Gets the z-offset for the walkmesh at this location.
    /// </summary>
    public float GroundHeight
    {
      get => NWScript.GetGroundHeight(this);
    }

    public IEnumerable<T> GetObjectsInShape<T>(Shape shape, float size, bool losCheck, Vector3 origin = default) where T : NwGameObject
    {
      int typeFilter = (int) NwObject.GetObjectType<T>();
      int nShape = (int) shape;

      for (uint obj = NWScript.GetFirstObjectInShape(nShape, size, this, losCheck.ToInt(), typeFilter, origin);
        obj != NWScript.OBJECT_INVALID;
        obj = NWScript.GetNextObjectInShape(nShape, size, this, losCheck.ToInt(), typeFilter, origin))
      {
        yield return obj.ToNwObject<T>();
      }
    }

    public IEnumerable<T> GetNearestObjectsByType<T>() where T : NwGameObject
    {
      int objType = (int) NwObject.GetObjectType<T>();
      int i;
      uint next;
      for (i = 1, next = NWScript.GetNearestObjectToLocation(objType, this, i); next != NwObject.INVALID; i++, next = NWScript.GetNearestObjectToLocation(objType, this, i))
      {
        T obj = next.ToNwObject<T>();
        if (obj != null)
        {
          yield return obj;
        }
      }
    }

    public IEnumerable<NwCreature> GetNearestCreatures() => GetNearestCreatures(CreatureTypeFilter.None, CreatureTypeFilter.None, CreatureTypeFilter.None);

    public IEnumerable<NwCreature> GetNearestCreatures(CreatureTypeFilter filter1) => GetNearestCreatures(filter1, CreatureTypeFilter.None, CreatureTypeFilter.None);

    public IEnumerable<NwCreature> GetNearestCreatures(CreatureTypeFilter filter1, CreatureTypeFilter filter2) => GetNearestCreatures(filter1, filter2, CreatureTypeFilter.None);

    public IEnumerable<NwCreature> GetNearestCreatures(CreatureTypeFilter filter1, CreatureTypeFilter filter2, CreatureTypeFilter filter3)
    {
      int i;
      uint current;

      for (i = 1, current = NWScript.GetNearestCreatureToLocation(
          filter1.Key,
          filter1.Value,
          this,
          i,
          filter2.Key,
          filter2.Value,
          filter3.Key,
          filter3.Value);
        current != NWScript.OBJECT_INVALID;
        i++, current = NWScript.GetNearestCreatureToLocation(
          filter1.Key,
          filter1.Value,
          this,
          i,
          filter2.Key,
          filter2.Value,
          filter3.Key,
          filter3.Value))
      {
        yield return current.ToNwObject<NwCreature>();
      }
    }

    public static Location Create(NwArea area, Vector3 position, float orientation)
    {
      return NWScript.Location(area, position, orientation);
    }
  }
}
