using System;
using NWM.Internal;
using NWN;
using Object = NWM.Internal.Object;

namespace NWM.API
{
  public partial class NwObject
  {
    private static NwModule cachedModule;

    private static NwModule moduleObj
    {
      get
      {
        if (cachedModule != null)
        {
          return cachedModule;
        }

        cachedModule = new NwModule(NWScript.GetModule());
        return cachedModule;
      }
    }

    public static NwObject Deserialize(string serializedObject)
    {
      return Create(Object.Deserialize(serializedObject));
    }

    public static NwObject GetByUUID(Guid uuid)
    {
      return Create(NWScript.GetObjectByUUID(uuid.ToString("N")));
    }

    public static NwObject Create(uint objectId)
    {
      switch (NWMInterop.GetObjectType(objectId))
      {
        case ObjectType.Invalid:
          return null;
        case ObjectType.Creature:
          return NWScript.GetIsPC(objectId) == NWScript.TRUE ? new NwPlayer(objectId) : new NwCreature(objectId);
        case ObjectType.Item:
          return new NwItem(objectId);
        case ObjectType.Placeable:
          return new NwPlaceable(objectId);
        case ObjectType.Module:
          return moduleObj;
        case ObjectType.Area:
          return new NwArea(objectId);
        case ObjectType.Trigger:
          return new NwTrigger(objectId);
        case ObjectType.Door:
          return new NwDoor(objectId);
        case ObjectType.Waypoint:
          return new NwWaypoint(objectId);
        case ObjectType.Encounter:
          return new NwEncounter(objectId);
        case ObjectType.Store:
          return new NwStore(objectId);
        default:
          return new NwObject(objectId);
      }
    }
  }
}