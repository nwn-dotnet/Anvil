using System;

namespace Anvil.API
{
  [Flags]
  public enum ClassRestrictionTypes : byte
  {
    None = 0,
    LawChaos = 1 << 0,
    GoodEvil = 1 << 1,
    Both = LawChaos | GoodEvil,
  }
}
