using NWN.Core;

namespace NWM.API.Constants
{
  public enum ObjectType
  {
    Creature = NWScript.OBJECT_TYPE_CREATURE,
    Item = NWScript.OBJECT_TYPE_ITEM,
    Trigger = NWScript.OBJECT_TYPE_TRIGGER,
    Door = NWScript.OBJECT_TYPE_DOOR,
    AreaOfEffect = NWScript.OBJECT_TYPE_AREA_OF_EFFECT,
    Waypoint = NWScript.OBJECT_TYPE_WAYPOINT,
    Placeable = NWScript.OBJECT_TYPE_PLACEABLE,
    Store = NWScript.OBJECT_TYPE_STORE,
    Encounter = NWScript.OBJECT_TYPE_ENCOUNTER,
    All = NWScript.OBJECT_TYPE_ALL,
    Invalid = NWScript.OBJECT_TYPE_INVALID
  }
}