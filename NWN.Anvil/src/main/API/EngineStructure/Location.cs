using System;
using System.Collections.Generic;
using System.Numerics;
using NWN.Core;

namespace Anvil.API
{
  public sealed class Location : EngineStructure
  {
    internal Location(IntPtr handle) : base(handle) {}

    /// <summary>
    /// Gets the associated Area of this location.
    /// </summary>
    public NwArea Area
    {
      get => NWScript.GetAreaFromLocation(this).ToNwObject<NwArea>();
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
    /// Gets a value indicating whether the location is walkable.
    /// </summary>
    public bool IsWalkable
    {
      get => NWScript.Get2DAString("surfacemat", "Walk", SurfaceMaterial).ParseIntBool();
    }

    /// <summary>
    /// Gets the position Vector of this location.
    /// </summary>
    public Vector3 Position
    {
      get => NWScript.GetPositionFromLocation(this);
    }

    /// <summary>
    /// Gets the rotation value of this location.
    /// </summary>
    public float Rotation
    {
      get => NWScript.GetFacingFromLocation(this);
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

    protected override int StructureId
    {
      get => NWScript.ENGINE_STRUCTURE_LOCATION;
    }

    public static Location Create(NwArea area, Vector3 position, float orientation)
    {
      return NWScript.Location(area, position, orientation);
    }

    public static implicit operator Location(IntPtr intPtr)
    {
      return new Location(intPtr);
    }

    /// <summary>
    /// Applies the specified effect at this location.
    /// </summary>
    /// <param name="durationType">The duration type to apply with this effect.</param>
    /// <param name="effect">The effect to apply.</param>
    /// <param name="duration">If duration type is <see cref="EffectDuration.Temporary"/>, the duration of this effect.</param>
    public void ApplyEffect(EffectDuration durationType, Effect effect, TimeSpan duration = default)
    {
      NWScript.ApplyEffectAtLocation((int)durationType, effect, this, (float)duration.TotalSeconds);
    }

    /// <summary>
    /// Creates the specified trap.
    /// </summary>
    /// <param name="trap">The base type of trap.</param>
    /// <param name="size">The size of the trap. Minimum size allowed is 1.0f. If no value set, defaults to 2.0f.</param>
    /// <param name="tag">The tag of the trap being created. If no value set, defaults to an empty string.</param>
    /// <param name="disarm">The script that will fire when the trap is disarmed. If no value set, defaults to an empty string and no script will fire.</param>
    /// <param name="triggered">The script that will fire when the trap is triggered. If no value set, defaults to an empty string and the default OnTrapTriggered script for the trap type specified will fire instead (as specified in the traps.2da).</param>
    public void CreateTrap(TrapBaseType trap, float size = 2.0f, string tag = "", string disarm = "", string triggered = "")
    {
      NWScript.CreateTrapAtLocation((int)trap, this, size, tag, sOnDisarmScript: disarm, sOnTrapTriggeredScript: triggered);
    }

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

    public IEnumerable<NwCreature> GetNearestCreatures()
    {
      return GetNearestCreatures(CreatureTypeFilter.None, CreatureTypeFilter.None, CreatureTypeFilter.None);
    }

    public IEnumerable<NwCreature> GetNearestCreatures(CreatureTypeFilter filter1)
    {
      return GetNearestCreatures(filter1, CreatureTypeFilter.None, CreatureTypeFilter.None);
    }

    public IEnumerable<NwCreature> GetNearestCreatures(CreatureTypeFilter filter1, CreatureTypeFilter filter2)
    {
      return GetNearestCreatures(filter1, filter2, CreatureTypeFilter.None);
    }

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

    public IEnumerable<T> GetNearestObjectsByType<T>() where T : NwGameObject
    {
      int objType = (int)NwObject.GetObjectType<T>();
      int i;
      uint next;
      for (i = 1, next = NWScript.GetNearestObjectToLocation(objType, this, i); next != NwObject.Invalid; i++, next = NWScript.GetNearestObjectToLocation(objType, this, i))
      {
        T obj = next.ToNwObjectSafe<T>();
        if (obj != null)
        {
          yield return obj;
        }
      }
    }

    public IEnumerable<NwGameObject> GetObjectsInShape(Shape shape, float size, bool losCheck, ObjectTypes objTypes = ObjectTypes.Creature, Vector3 origin = default)
    {
      int typeFilter = (int)objTypes;
      int nShape = (int)shape;

      for (uint obj = NWScript.GetFirstObjectInShape(nShape, size, this, losCheck.ToInt(), typeFilter, origin);
        obj != NWScript.OBJECT_INVALID;
        obj = NWScript.GetNextObjectInShape(nShape, size, this, losCheck.ToInt(), typeFilter, origin))
      {
        yield return obj.ToNwObject<NwGameObject>();
      }
    }

    public IEnumerable<T> GetObjectsInShapeByType<T>(Shape shape, float size, bool losCheck, Vector3 origin = default) where T : NwGameObject
    {
      int typeFilter = (int)NwObject.GetObjectType<T>();
      int nShape = (int)shape;

      for (uint obj = NWScript.GetFirstObjectInShape(nShape, size, this, losCheck.ToInt(), typeFilter, origin);
        obj != NWScript.OBJECT_INVALID;
        obj = NWScript.GetNextObjectInShape(nShape, size, this, losCheck.ToInt(), typeFilter, origin))
      {
        yield return obj.ToNwObject<T>();
      }
    }
  }
}
