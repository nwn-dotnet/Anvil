using NWN.Core;

namespace Anvil.API
{
  public enum Subfeat
  {
    None = 0,
    CalledShotLeg = NWScript.SUBFEAT_CALLED_SHOT_LEG,
    CalledShotArms = NWScript.SUBFEAT_CALLED_SHOT_ARMS,
    ElementalShapeEarth = NWScript.SUBFEAT_ELEMENTAL_SHAPE_EARTH,
    ElementalShapeWater = NWScript.SUBFEAT_ELEMENTAL_SHAPE_WATER,
    ElementalShapeFire = NWScript.SUBFEAT_ELEMENTAL_SHAPE_FIRE,
    ElementalShapeAir = NWScript.SUBFEAT_ELEMENTAL_SHAPE_AIR,
    WildShapeBrownBear = NWScript.SUBFEAT_WILD_SHAPE_BROWN_BEAR,
    WildShapePanther = NWScript.SUBFEAT_WILD_SHAPE_PANTHER,
    WildShapeWolf = NWScript.SUBFEAT_WILD_SHAPE_WOLF,
    WildShapeBoar = NWScript.SUBFEAT_WILD_SHAPE_BOAR,
    WildShapeBadger = NWScript.SUBFEAT_WILD_SHAPE_BADGER,
  }
}
