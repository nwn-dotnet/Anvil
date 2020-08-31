using NWN.Core.NWNX;

namespace NWNX.API.Constants
{
  internal enum InternalObjectType
  {
    Invalid = ObjectPlugin.NWNX_OBJECT_TYPE_INTERNAL_INVALID,
    Gui = ObjectPlugin.NWNX_OBJECT_TYPE_INTERNAL_GUI,
    Tile = ObjectPlugin.NWNX_OBJECT_TYPE_INTERNAL_TILE,
    Module = ObjectPlugin.NWNX_OBJECT_TYPE_INTERNAL_MODULE,
    Area = ObjectPlugin.NWNX_OBJECT_TYPE_INTERNAL_AREA,
    Creature = ObjectPlugin.NWNX_OBJECT_TYPE_INTERNAL_CREATURE,
    Item = ObjectPlugin.NWNX_OBJECT_TYPE_INTERNAL_ITEM,
    Trigger = ObjectPlugin.NWNX_OBJECT_TYPE_INTERNAL_TRIGGER,
    Projectile = ObjectPlugin.NWNX_OBJECT_TYPE_INTERNAL_PROJECTILE,
    Placeable = ObjectPlugin.NWNX_OBJECT_TYPE_INTERNAL_PLACEABLE,
    Door = ObjectPlugin.NWNX_OBJECT_TYPE_INTERNAL_DOOR,
    AreaOfEffect = ObjectPlugin.NWNX_OBJECT_TYPE_INTERNAL_AREAOFEFFECT,
    Waypoint = ObjectPlugin.NWNX_OBJECT_TYPE_INTERNAL_WAYPOINT,
    Encounter = ObjectPlugin.NWNX_OBJECT_TYPE_INTERNAL_ENCOUNTER,
    Store = ObjectPlugin.NWNX_OBJECT_TYPE_INTERNAL_STORE,
    Portal = ObjectPlugin.NWNX_OBJECT_TYPE_INTERNAL_PORTAL,
    Sound = ObjectPlugin.NWNX_OBJECT_TYPE_INTERNAL_SOUND
  }

  internal enum LocalVarType
  {
    Int = ObjectPlugin.NWNX_OBJECT_LOCALVAR_TYPE_INT,
    Float = ObjectPlugin.NWNX_OBJECT_LOCALVAR_TYPE_FLOAT,
    String = ObjectPlugin.NWNX_OBJECT_LOCALVAR_TYPE_STRING,
    Object = ObjectPlugin.NWNX_OBJECT_LOCALVAR_TYPE_OBJECT,
    Location = ObjectPlugin.NWNX_OBJECT_LOCALVAR_TYPE_LOCATION
  }
}