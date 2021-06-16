using System;
using System.Collections.Generic;
using NWN.API.Constants;
using NWN.Core;
using NWN.Native.API;

namespace NWN.API
{
  [NativeObjectInfo(ObjectTypes.Trigger, ObjectType.Trigger)]
  public sealed partial class NwTrigger : NwTrappable
  {
    internal readonly CNWSTrigger Trigger;

    internal NwTrigger(CNWSTrigger trigger) : base(trigger)
    {
      Trigger = trigger;
    }

    public static implicit operator CNWSTrigger(NwTrigger trigger)
    {
      return trigger?.Trigger;
    }

    public override Location Location
    {
      set
      {
        if (value.Area != Area)
        {
          Trigger.AddToArea(value.Area, value.Position.X, value.Position.Y, value.Position.Z, true.ToInt());

          // If the trigger is trapped it needs to be added to the area's trap list for it to be detectable by players.
          if (IsTrapped)
          {
            value.Area.Area.m_pTrapList.Add(this);
          }
        }
        else
        {
          Position = value.Position;
        }

        Rotation = value.Rotation;
      }
    }

    /// <summary>
    /// Gets all objects of the given type that are currently in this trigger.
    /// </summary>
    /// <typeparam name="T">The type of objects to return.</typeparam>
    /// <returns>An enumerable containing all objects currently in the trigger.</returns>
    public IEnumerable<T> GetObjectsInTrigger<T>() where T : NwGameObject
    {
      int objType = (int)GetObjectType<T>();
      for (uint obj = NWScript.GetFirstInPersistentObject(this, objType); obj != Invalid; obj = NWScript.GetNextInPersistentObject(this, objType))
      {
        yield return obj.ToNwObject<T>();
      }
    }

    /// <summary>
    /// Gets all objects of the given types that are currently in this trigger.
    /// </summary>
    /// <param name="objectTypes">The types of object to return.</param>
    /// <returns>An enumerable containing all objects currently in the trigger.</returns>
    public IEnumerable<NwGameObject> GetObjectsInTrigger(ObjectTypes objectTypes = ObjectTypes.All)
    {
      int objType = (int)objectTypes;
      for (uint obj = NWScript.GetFirstInPersistentObject(this, objType); obj != Invalid; obj = NWScript.GetNextInPersistentObject(this, objType))
      {
        yield return obj.ToNwObject<NwGameObject>();
      }
    }

    public override byte[] Serialize()
    {
      return NativeUtils.SerializeGff("UTT", (resGff, resStruct) =>
      {
        Trigger.SaveObjectState(resGff, resStruct);
        return Trigger.SaveTrigger(resGff, resStruct).ToBool();
      });
    }

    public static NwTrigger Deserialize(byte[] serialized)
    {
      CNWSTrigger trigger = null;

      bool result = NativeUtils.DeserializeGff(serialized, (resGff, resStruct) =>
      {
        if (!resGff.IsValidGff("UTT"))
        {
          return false;
        }

        trigger = new CNWSTrigger(Invalid);
        if (trigger.LoadTrigger(resGff, resStruct).ToBool())
        {
          trigger.LoadObjectState(resGff, resStruct);
          GC.SuppressFinalize(trigger);
          return true;
        }

        trigger.Dispose();
        return false;
      });

      return result && trigger != null ? trigger.ToNwObject<NwTrigger>() : null;
    }
  }
}
