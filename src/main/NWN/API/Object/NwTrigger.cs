using System.Collections.Generic;
using NWN.API.Constants;
using NWN.Core;
using NWNX.API.Constants;

namespace NWN.API
{
  [NativeObjectInfo(ObjectTypes.Trigger, InternalObjectType.Trigger)]
  public sealed class NwTrigger : NwTrappable
  {
    internal NwTrigger(uint objectId) : base(objectId) {}

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
