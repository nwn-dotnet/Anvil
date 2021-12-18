using System;

namespace Anvil.API
{
  [Flags]
  public enum ClassRestrictions : byte
  {
    None = 0,
    Neutral = 1 << 0,
    Lawful = 1 << 1,
    Chaotic = 1 << 2,
    Good = 1 << 3,
    Evil = 1 << 4,
  }
}
