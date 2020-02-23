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

      switch ((ObjectType)NWScript.GetObjectType(objectId))
      {
        case ObjectType.AreaOrModule:
          if (objectId == moduleObj)
          {
            return moduleObj;
          }
          return new NwArea(objectId);
        case ObjectType.Creature:
          return NWScript.GetIsPC(objectId) == NWScript.TRUE ? new NwPlayer(objectId) : new NwCreature(objectId);
        case ObjectType.Item:
          return new NwItem(objectId);
        case ObjectType.Placeable:
          return new NwPlaceable(objectId);
        default:
          return new NwObject(objectId);
      }
    }

    private enum ObjectType
    {
      AreaOrModule   = 0,
      Creature       = 1,
      Item           = 2,
      Trigger        = 4,
      Door           = 8,
      AreaOfEffect   = 16,
      Waypoint       = 32,
      Placeable      = 64,
      Store          = 128,
      Encounter      = 256,
      All            = 32767,
      Invalid        = 32767,
    }
  }
}