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

    /// <summary>
    /// Gets the surface material index at this location.<br/>
    /// Returns 0 if the location is invalid or has no surface type.
    /// </summary>
    public int SurfaceMaterial
    {
      get => NWScript.GetSurfaceMaterial(this);
    }

    /// <summary>
    /// Gets the color of the first main light in the tile containing this location.
    /// </summary>
    /// <returns>A <see cref="TileMainLightColor"/> value representing the main light color of the specified tile.</returns>
    public TileMainLightColor TileMainLightColorOne
    {
      get => (TileMainLightColor)NWScript.GetTileMainLight1Color(this);
    }

    /// <summary>
    /// Gets the color of the second main light in the tile containing this location.
    /// </summary>
    /// <returns>A <see cref="TileMainLightColor"/> value representing the second main light color of the specified tile.</returns>
    public TileMainLightColor TileMainLightColorTwo
    {
      get => (TileMainLightColor)NWScript.GetTileMainLight2Color(this);
    }

    /// <summary>
    /// Gets the color of the first light source in the tile containing this location.
    /// </summary>
    /// <returns>A <see cref="TileSourceLightColor"/> value representing the first light source color of the specified tile.</returns>
    public TileSourceLightColor TileSourceLightColorOne
    {
      get => (TileSourceLightColor)NWScript.GetTileSourceLight1Color(this);
    }

    /// <summary>
    /// Gets the color of the second light source in the tile containing this location.
    /// </summary>
    /// <returns>A <see cref="TileSourceLightColor"/> value representing the second light source color of the specified tile.</returns>
    public TileSourceLightColor TileSourceLightColorTwo
    {
      get => (TileSourceLightColor)NWScript.GetTileSourceLight2Color(this);
    }

    /// <summary>
    /// Sets the main light colors on the tile located at (tileLocation).
    /// (tile) specifies the location of the tile.
    /// You must call RecomputeStaticLighting() after calling this function for changes to occur for the players.
    /// </summary>
    public void TileMainLightColor(Location tileLocation, TileMainLightColor first, TileMainLightColor second)
      => NWScript.SetTileMainLightColor(tileLocation, (int)first, (int)second);

    /// <summary>
    /// Sets the source light colors on the tile located at (tileLocation).
    /// (tile) specifies the location of the tile.
    /// You must call RecomputeStaticLighting() after calling this function for changes to occur for the players.
    /// </summary>
    public void TileSourceLightColor(Location tileLocation, TileSourceLightColor first, TileSourceLightColor second)
      => NWScript.SetTileMainLightColor(tileLocation, (int)first, (int)second);

    /// <summary>
    /// Returns the distance to the target.<br/>
    /// If you only need to compare the distance, you can compare the squared distance using <see cref="DistanceSquared"/> to avoid a costly sqrt operation.
    /// </summary>
    /// <param name="target">The other location to calculate distance between.</param>
    /// <returns>The distance in game units, or -1 if the target is in a different area.</returns>
    public float Distance(Location target)
    {
      if (target.Area != Area)
      {
        return -1.0f;
      }

      return Vector3.Distance(target.Position, Position);
    }

    /// <summary>
    /// Returns the squared distance to the target.
    /// </summary>
    /// <param name="target">The other location to calculate distance between.</param>
    /// <returns>The squared distance in game units, or -1 if the target is in a different area.</returns>
    public float DistanceSquared(Location target)
    {
      if (target.Area != Area)
      {
        return -1.0f;
      }

      return Vector3.DistanceSquared(target.Position, Position);
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
        T obj = next.ToNwObjectSafe<T>();
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
