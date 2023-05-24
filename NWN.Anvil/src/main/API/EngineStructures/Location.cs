using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using NWN.Core;

namespace Anvil.API
{
  /// <summary>
  /// A location in the module, represented by area, position and orientation.
  /// </summary>
  public sealed class Location : EngineStructure
  {
    internal Location(IntPtr handle, bool memoryOwn) : base(handle, memoryOwn) {}

    /// <summary>
    /// Gets the associated Area of this location.
    /// </summary>
    public NwArea Area => NWScript.GetAreaFromLocation(this).ToNwObject<NwArea>()!;

    /// <summary>
    /// Gets the inverted rotation value of this location (placeables).
    /// </summary>
    public float FlippedRotation => (360 - Rotation) % 360;

    /// <summary>
    /// Gets the z-offset for the walkmesh at this location.
    /// </summary>
    public float GroundHeight => NWScript.GetGroundHeight(this);

    /// <summary>
    /// Gets a value indicating whether the location is walkable.
    /// </summary>
    public bool IsWalkable => NWScript.Get2DAString("surfacemat", "Walk", SurfaceMaterial).ParseIntBool();

    /// <summary>
    /// Gets the position Vector of this location.
    /// </summary>
    public Vector3 Position => NWScript.GetPositionFromLocation(this);

    /// <summary>
    /// Gets the rotation value of this location.
    /// </summary>
    public float Rotation => NWScript.GetFacingFromLocation(this);

    /// <summary>
    /// Gets the surface material index at this location.<br/>
    /// Returns 0 if the location is invalid or has no surface type.
    /// </summary>
    public int SurfaceMaterial => NWScript.GetSurfaceMaterial(this);

    /// <summary>
    /// Gets the color of the first main light in the tile containing this location.
    /// </summary>
    /// <returns>A <see cref="TileMainLightColor"/> value representing the main light color of the specified tile.</returns>
    public TileMainLightColor TileMainLightColorOne => (TileMainLightColor)NWScript.GetTileMainLight1Color(this);

    /// <summary>
    /// Gets the color of the second main light in the tile containing this location.
    /// </summary>
    /// <returns>A <see cref="TileMainLightColor"/> value representing the second main light color of the specified tile.</returns>
    public TileMainLightColor TileMainLightColorTwo => (TileMainLightColor)NWScript.GetTileMainLight2Color(this);

    /// <summary>
    /// Gets the color of the first light source in the tile containing this location.
    /// </summary>
    /// <returns>A <see cref="TileSourceLightColor"/> value representing the first light source color of the specified tile.</returns>
    public TileSourceLightColor TileSourceLightColorOne => (TileSourceLightColor)NWScript.GetTileSourceLight1Color(this);

    /// <summary>
    /// Gets the color of the second light source in the tile containing this location.
    /// </summary>
    /// <returns>A <see cref="TileSourceLightColor"/> value representing the second light source color of the specified tile.</returns>
    public TileSourceLightColor TileSourceLightColorTwo => (TileSourceLightColor)NWScript.GetTileSourceLight2Color(this);

    /// <summary>
    /// Gets the id of the tile set at this location.
    /// </summary>
    public int TileId => NWScript.GetTileID(this);

    /// <summary>
    /// Gets the rotation of the tile set at this location.
    /// </summary>
    public TileRotation TileRotation => (TileRotation)NWScript.GetTileOrientation(this);

    /// <summary>
    /// Gets the height of the tile set at this location.
    /// </summary>
    public int TileHeight => NWScript.GetTileHeight(this);

    protected override int StructureId => NWScript.ENGINE_STRUCTURE_LOCATION;

    /// <summary>
    /// Create a new location from the specified area, position and orientation
    /// </summary>
    /// <param name="area">The area of the location.</param>
    /// <param name="position">The position of the location.</param>
    /// <param name="orientation">The rotation of the location.</param>
    /// <returns>The created location.</returns>
    [return: NotNullIfNotNull("area")]
    public static Location? Create(NwArea area, Vector3 position, float orientation)
    {
      return NWScript.Location(area, position, orientation);
    }

    public static implicit operator Location?(IntPtr intPtr)
    {
      return intPtr != IntPtr.Zero ? new Location(intPtr, true) : null;
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

    /// <summary>
    /// Gets all creatures near this location, ordered by distance.
    /// </summary>
    public IEnumerable<NwCreature> GetNearestCreatures()
    {
      return GetNearestCreatures(CreatureTypeFilter.None, CreatureTypeFilter.None, CreatureTypeFilter.None);
    }

    /// <summary>
    /// Gets all creatures near this location, ordered by distance.
    /// </summary>
    /// <param name="filter1">A filter for the returned creatures.</param>
    public IEnumerable<NwCreature> GetNearestCreatures(CreatureTypeFilter filter1)
    {
      return GetNearestCreatures(filter1, CreatureTypeFilter.None, CreatureTypeFilter.None);
    }

    /// <summary>
    /// Gets all creatures near this location, ordered by distance.
    /// </summary>
    /// <param name="filter1">A filter for the returned creatures.</param>
    /// <param name="filter2">A 2nd filter for the returned creatures.</param>
    public IEnumerable<NwCreature> GetNearestCreatures(CreatureTypeFilter filter1, CreatureTypeFilter filter2)
    {
      return GetNearestCreatures(filter1, filter2, CreatureTypeFilter.None);
    }

    /// <summary>
    /// Gets all creatures near this location, ordered by distance.
    /// </summary>
    /// <param name="filter1">A filter for the returned creatures.</param>
    /// <param name="filter2">A 2nd filter for the returned creatures.</param>
    /// <param name="filter3">A 3rd filter for the returned creatures.</param>
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
        NwCreature? creature = current.ToNwObject<NwCreature>();
        if (creature != null)
        {
          yield return creature;
        }
      }
    }

    /// <summary>
    /// Gets all objects near this location, ordered by distance.
    /// </summary>
    public IEnumerable<T> GetNearestObjectsByType<T>() where T : NwGameObject
    {
      int typeFilter = (int)NwObject.GetObjectFilter<T>();
      int i;
      uint next;
      for (i = 1, next = NWScript.GetNearestObjectToLocation(typeFilter, this, i); next != NwObject.Invalid; i++, next = NWScript.GetNearestObjectToLocation(typeFilter, this, i))
      {
        T? obj = next.ToNwObjectSafe<T>();
        if (obj != null)
        {
          yield return obj;
        }
      }
    }

    /// <summary>
    /// Gets all objects in a shape at this location.
    /// </summary>
    public IEnumerable<NwGameObject> GetObjectsInShape(Shape shape, float size, bool losCheck, ObjectTypes objTypes = ObjectTypes.Creature, Vector3 origin = default)
    {
      int typeFilter = (int)objTypes;
      int nShape = (int)shape;

      for (uint obj = NWScript.GetFirstObjectInShape(nShape, size, this, losCheck.ToInt(), typeFilter, origin);
        obj != NWScript.OBJECT_INVALID;
        obj = NWScript.GetNextObjectInShape(nShape, size, this, losCheck.ToInt(), typeFilter, origin))
      {
        NwGameObject? gameObject = obj.ToNwObject<NwGameObject>();
        if (gameObject != null)
        {
          yield return gameObject;
        }
      }
    }

    /// <summary>
    /// Gets all objects in a shape at this location of the specified type.
    /// </summary>
    public IEnumerable<T> GetObjectsInShapeByType<T>(Shape shape, float size, bool losCheck, Vector3 origin = default) where T : NwGameObject
    {
      int typeFilter = (int)NwObject.GetObjectFilter<T>();
      int nShape = (int)shape;

      for (uint obj = NWScript.GetFirstObjectInShape(nShape, size, this, losCheck.ToInt(), typeFilter, origin);
        obj != NWScript.OBJECT_INVALID;
        obj = NWScript.GetNextObjectInShape(nShape, size, this, losCheck.ToInt(), typeFilter, origin))
      {
        T? gameObject = obj.ToNwObject<T>();
        if (gameObject != null)
        {
          yield return gameObject;
        }
      }
    }

    /// <summary>
    /// Changes the tile at this location to a new tile.
    /// </summary>
    /// <param name="tileId">The new tile to apply.</param>
    /// <param name="rotation">How the tile should be rotated.</param>
    /// <param name="height">The height of the tile.</param>
    /// <param name="flags">Additional flags/behaviours to run after the tile is updated.</param>
    public void SetTile(int tileId, TileRotation rotation, int height = 0, SettleFlags flags = SettleFlags.RecomputeLighting)
    {
      NWScript.SetTile(this, tileId, (int)rotation, height, (int)flags);
    }

    /// <summary>
    /// Sets the state of the animation loops of the tile at this location.
    /// </summary>
    public void SetTileAnimationLoops(bool animLoop1, bool animLoop2, bool animLoop3)
    {
      NWScript.SetTileAnimationLoops(this, animLoop1.ToInt(), animLoop2.ToInt(), animLoop3.ToInt());
    }
  }
}
