using NWN.Core;

namespace Anvil.API
{
  public enum ObjectVisualTransformDataScope
  {
    AllScopes = -1,
    Base = NWScript.OBJECT_VISUAL_TRANSFORM_DATA_SCOPE_BASE,
    CreatureHead = NWScript.OBJECT_VISUAL_TRANSFORM_DATA_SCOPE_CREATURE_HEAD,
    CreatureTail = NWScript.OBJECT_VISUAL_TRANSFORM_DATA_SCOPE_CREATURE_TAIL,
    CreatureWings = NWScript.OBJECT_VISUAL_TRANSFORM_DATA_SCOPE_CREATURE_WINGS,
    CreatureCloak = NWScript.OBJECT_VISUAL_TRANSFORM_DATA_SCOPE_CREATURE_CLOAK,
    ItemPart1 = NWScript.OBJECT_VISUAL_TRANSFORM_DATA_SCOPE_ITEM_PART1,
    ItemPart2 = NWScript.OBJECT_VISUAL_TRANSFORM_DATA_SCOPE_ITEM_PART2,
    ItemPart3 = NWScript.OBJECT_VISUAL_TRANSFORM_DATA_SCOPE_ITEM_PART3,
    ItemPart4 = NWScript.OBJECT_VISUAL_TRANSFORM_DATA_SCOPE_ITEM_PART4,
    ItemPart5 = NWScript.OBJECT_VISUAL_TRANSFORM_DATA_SCOPE_ITEM_PART5,
  }
}
