using System.Collections.Generic;
using System.Numerics;
using NWM.API;

// ReSharper disable once CheckNamespace
namespace NWN
{
  public partial class Location
  {
    public Vector3 Position => NWScript.GetPositionFromLocation(this);
    public NwArea Area => NWScript.GetAreaFromLocation(this).ToNwObject<NwArea>();
    public float Rotation => NWScript.GetFacingFromLocation(this);
    public float FlippedRotation => (360 - Rotation) % 360;

    public IEnumerable<T> GetNearestObjectsByType<T>() where T : NwGameObject
    {
      int objType = (int) NwObjectFactory.GetObjectType<T>();
      int i;
      NwGameObject next;
      for (i = 1, next = NWScript.GetNearestObjectToLocation(objType, this, i).ToNwObject<NwGameObject>(); next != NwObject.INVALID; i++, next = NWScript.GetNearestObjectToLocation(objType, this, i).ToNwObject<NwGameObject>())
      {
        if (next is T gameObject)
        {
          yield return gameObject;
        }
      }
    }

    public static Location Create(NwArea area, Vector3 position, float orientation)
    {
      return NWScript.Location(area, position, orientation);
    }
  }
}