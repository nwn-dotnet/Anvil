using System;
using NWN;

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

    public static NwObject Create(uint objectId)
    {
      // TODO reuse cached objects
      if (!NWScript.GetIsObjectValid(objectId).ToBool())
      {
        return null;
      }

      switch ((ObjectType) Internal.Internal.GetObjectType(objectId))
      {
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

    private enum ObjectType : byte
    {
      GUI = 1,
      Tile = 2,
      Module = 3,
      Area = 4,
      Creature = 5,
      Item = 6,
      Trigger = 7,
      Projectile = 8,
      Placeable = 9,
      Door = 10,
      AreaOfEffect = 11,
      Waypoint = 12,
      Encounter = 13,
      Store = 14,
      Portal = 15,
      Sound = 16,
    }
  }
}