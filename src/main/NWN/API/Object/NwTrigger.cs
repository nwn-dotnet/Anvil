using System.Collections.Generic;
using NWN.API.Constants;
using NWN.Core;
using NWN.Native.API;

namespace NWN.API
{
  [NativeObjectInfo(ObjectTypes.Trigger, ObjectType.Trigger)]
  public sealed class NwTrigger : NwTrappable
  {
    private readonly CNWSTrigger trigger;

    internal NwTrigger(uint objectId, CNWSTrigger trigger) : base(objectId, trigger)
    {
      this.trigger = trigger;
    }

    public static implicit operator CNWSTrigger(NwTrigger trigger)
    {
      return trigger?.trigger;
    }

    public override Location Location
    {
      set
      {
        trigger.AddToArea(value.Area, value.Position.X, value.Position.Y, value.Position.Z);
        Rotation = value.Rotation;
      }
    }

    /// <summary>
    /// Gets all objects of the given type that are currently in this trigger.
    /// </summary>
    /// <typeparam name="T">The type of objects to return.</typeparam>
    /// <returns>An enumerable containing all objects currently in the trigger.</returns>
    public IEnumerator<T> GetObjectsInTrigger<T>() where T : NwGameObject
    {
      int objType = (int) GetObjectType<T>();
      for (uint obj = NWScript.GetFirstInPersistentObject(this, objType); obj != INVALID; obj = NWScript.GetNextInPersistentObject(this, objType))
      {
        yield return obj.ToNwObject<T>();
      }
    }

    /// <summary>
    /// Gets all objects of the given types that are currently in this trigger.
    /// </summary>
    /// <param name="objectTypes">The types of object to return.</param>
    /// <returns>An enumerable containing all objects currently in the trigger.</returns>
    public IEnumerator<NwGameObject> GetObjectsInTrigger(ObjectTypes objectTypes)
    {
      int objType = (int) objectTypes;
      for (uint obj = NWScript.GetFirstInPersistentObject(this, objType); obj != INVALID; obj = NWScript.GetNextInPersistentObject(this, objType))
      {
        yield return obj.ToNwObject<NwGameObject>();
      }
    }
  }
}
