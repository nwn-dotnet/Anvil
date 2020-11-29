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

    /// <summary>
    /// Gets NwCreature who is the clicking object.
    /// </summary>
    /// <remarks>The reason for it being identical to GetEnteringObject() is that triggers, or course, can be said to be clicked on, or entered, when a transition is used.<br/>
    /// This function is of course not meant to be used outside certain events, as it can return long dead or invalid creatures.</remarks>
    public static NwCreature ClickingObject => NWScript.GetClickingObject().ToNwObject<NwCreature>();
  }
}
