using System;

namespace Anvil.API
{
  [Flags]
  public enum SpellTargetTypes : ushort
  {
    Unknown = 0,
    Self = 1 << 0,
    Creature = 1 << 1,
    Area = 1 << 2,
    Item = 1 << 3,
    Door = 1 << 4,
    Placeable = 1 << 5,
    Trigger = 1 << 6,
  }
}
