using System.Collections.Generic;
using System.Numerics;
using NWM.API.Constants;
using NWN;

namespace NWM.API
{
  public static class LocationExtensions
  {
    public static Vector3 GetPosition(this Location location) => NWScript.GetPositionFromLocation(location).ToVector3();
    public static NwArea GetArea(this Location location) => NWScript.GetAreaFromLocation(location).ToNwObject<NwArea>();
    public static float GetRotation(this Location location) => NWScript.GetFacingFromLocation(location);
    public static float GetFlippedRotation(this Location location) => (360 - location.GetRotation()) % 360;

    private static IEnumerable<T> GetObjectsInShape<T>(this Location location, Shape shape, float size, bool losCheck = false, Vector3? origin = null) where T : NwGameObject
    {
      int typeFilter = (int) NwObjectFactory.GetObjectType<T>();
      int nShape = (int) shape;

      for (NwGameObject obj = NWScript.GetFirstObjectInShape(nShape, size, location, losCheck.ToInt(), typeFilter, origin.ToNativeVector()).ToNwObject<NwGameObject>();
        obj != NwObject.INVALID;
        obj = NWScript.GetNextObjectInShape(nShape, size, location, losCheck.ToInt(), typeFilter, origin.ToNativeVector()).ToNwObject<NwGameObject>())
      {
        yield return (T) obj;
      }
    }

    public static IEnumerable<T> GetNearestObjectsByType<T>(this Location location) where T : NwGameObject
    {
      int objType = (int) NwObjectFactory.GetObjectType<T>();
      int i;
      NwGameObject next;
      for (i = 1, next = NWScript.GetNearestObjectToLocation(objType, location, i).ToNwObject<NwGameObject>(); next != NwObject.INVALID; i++, next = NWScript.GetNearestObjectToLocation(objType, location, i).ToNwObject<NwGameObject>())
      {
        if (next is T gameObject)
        {
          yield return gameObject;
        }
      }
    }

    public static IEnumerable<NwCreature> GetNearestCreatures(this Location location) => GetNearestCreatures(location, CreatureTypeFilter.None, CreatureTypeFilter.None, CreatureTypeFilter.None);
    public static IEnumerable<NwCreature> GetNearestCreatures(this Location location, CreatureTypeFilter filter1) => GetNearestCreatures(location, filter1, CreatureTypeFilter.None, CreatureTypeFilter.None);
    public static IEnumerable<NwCreature> GetNearestCreatures(this Location location, CreatureTypeFilter filter1, CreatureTypeFilter filter2) => GetNearestCreatures(location, filter1, filter2, CreatureTypeFilter.None);

    public static IEnumerable<NwCreature> GetNearestCreatures(this Location location, CreatureTypeFilter filter1, CreatureTypeFilter filter2, CreatureTypeFilter filter3)
    {
      int i;
      NwCreature current;

      for (i = 1, current = NWScript.GetNearestCreatureToLocation(
          filter1.Key,
          filter1.Value,
          location,
          i,
          filter2.Key,
          filter2.Value,
          filter3.Key,
          filter3.Value).ToNwObject<NwCreature>();
        current != NwObject.INVALID;
        i++, current = NWScript.GetNearestCreatureToLocation(
          filter1.Key,
          filter1.Value,
          location,
          i,
          filter2.Key,
          filter2.Value,
          filter3.Key,
          filter3.Value).ToNwObject<NwCreature>())
      {
        yield return current;
      }
    }
  }
}